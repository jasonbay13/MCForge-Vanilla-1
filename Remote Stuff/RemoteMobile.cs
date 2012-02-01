using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Security.Cryptography;
using MCForge.Remote;
using System.Threading;
using System.IO;

namespace MCForge.Remote
{

    class RemoteMobile : Remote
    {
        private string key { get; set; }
        public override int version { get { return 4; } }
        public override System.IO.BinaryReader reader { get; set; }
        public override System.IO.BinaryWriter writer { get; set; }
        public override bool isConnected { get; set; }
        public override bool isLoggedIn { get; set; }
        public override RemoteType remoteType { get { return RemoteType.Mobile; } }
        public override string username { get; set; }
        public TcpClient Client { get; set; }
        public Queue<Packet> packetQ { get; set; }
        public short pingNumb { get; set; }
        public bool recievedPing { get; set; }

        public RemoteMobile(TcpClient e, BinaryReader read, BinaryWriter write)
            : base()
        {

            key = generateKey();
            reader = read;
            writer = write;
            Client = e;
            isConnected = true;
            isLoggedIn = false;
            username = "Default";
            packetQ = new Queue<Packet>();

        }
        private string generateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }


        public override void Start()
        {
            if (!RemoteServer.enableRemote) return;
            try
            {
                try
                {
                    new Thread(IORead).Start();
                    new Thread(IOWrite).Start();
                    //new Thread(Ping).Start();
                }
                catch
                {
                    Disconnect();
                }
                RemoteProperties.Load();
                Player.GlobalMessage(c.navy + "A Remote has connected to the server");
                Server.s.Log("[Remote] connected to the server.");
                isConnected = true;
                reader.BaseStream.ReadTimeout = 60000;
                recievedPing = true;


            }

            catch (Exception e)
            {
                Server.ErrorLog(e);
            }
        }


        #region -=Events=-
        #region Vars
        private Server.LogHandler _logHandler;
        private Player.OnPlayerConnect _onPlayerConnect;
        private Player.OnPlayerDisconnect _onPlayerDisconnect;
        private Level.OnLevelLoaded _levelLoaded;
        private Level.OnLevelUnload _levelUnloaded;
        #endregion
        public override void LoadEvents()
        {
            _logHandler += OnLog;
            _onPlayerConnect += PlayerConnect;
            _onPlayerDisconnect += PlayerDisconnect;
            _levelUnloaded += LevelUnload;
            _levelLoaded += LevelLoad;

            Server.s.OnLog += _logHandler;
            Player.PlayerConnect += _onPlayerConnect;
            Player.PlayerDisconnect += _onPlayerDisconnect;
            Level.LevelLoaded += _levelLoaded;
            Level.LevelUnload += _levelUnloaded;

#if Debug
                        Server.s.Log("Registered Events");
#endif
        }

        public override void UnloadEvents()
        {
            Server.s.OnLog -= _logHandler;
            Player.PlayerConnect -= _onPlayerConnect;
            Player.PlayerDisconnect -= _onPlayerDisconnect;
            Level.LevelLoaded -= _levelLoaded;
            Level.LevelUnload -= _levelUnloaded;
#if Debug
                        Server.s.Log("Unregistered Events");
#endif

        }
        void OnLog(string mesg)
        {
            Packet p = new Packet(Encoding.UTF8, false);
            p.addData((byte)PacketId.SChat);
            p.addData((byte)4); //Logs
            p.addData(mesg);
            packetQ.Enqueue(p);
        }
        void OnPlayerChat(Player p, string message)
        {
            message = "%f[" + p.color + p.name + "%f]" + message;
            Packet e = new Packet(Encoding.UTF8, false);
            e.addData((byte)PacketId.SChat);
            e.addData((byte)2); //Player chat
            e.addData(message);
            packetQ.Enqueue(e);
        }
        void OnAdminChat(string msg)
        {

        }
        void OnOpChat(string msg)
        {

        }
        void PlayerConnect(Player p)
        {
            Thread.Sleep(500);                                           //Wait for stuff to finish
            Packet e = new Packet(Encoding.UTF8, false);
            e.addData((byte)PacketId.SPlayers);
            e.addData((byte)0);                                          //Player connect
            e.addData(p.title);
            e.addData(p.name);
            e.addData(p.group.name);
            packetQ.Enqueue(e);
        }
        void PlayerDisconnect(Player p, string reason)
        {
            Packet e = new Packet(Encoding.UTF8, false);
            e.addData((byte)PacketId.SPlayers);
            e.addData((byte)1);                                          //Player disconnect
            e.addData(p.name);
            packetQ.Enqueue(e);
        }
        void LevelLoad(Level l)
        {
            Packet p = new Packet(Encoding.UTF8, false);
            p.addData((byte)PacketId.SMaps);
            p.addData((byte)0);
            p.addData(l.name);
            packetQ.Enqueue(p);
        }
        void LevelUnload(Level l)
        {
            LevelUnload(l.name);
        }
        void LevelUnload(string s)
        {
            Packet p = new Packet(Encoding.UTF8, false);
            p.addData((byte)PacketId.SMaps);
            p.addData((byte)1);
            p.addData(s);
            packetQ.Enqueue(p);
        }
        void OnGroupChange(Group g)
        {
            Packet p = new Packet(Encoding.UTF8, false);
            p.addData((byte)PacketId.SGroups);
            if (g.color.Length >= 2)
                p.addData(g.color[1] + g.name);
            else
                p.addData('f' + g.name);
            p.addData((short)g.Permission);
            p.addData(g.maxUndo);
            p.addData(g.maxBlocks);
            packetQ.Enqueue(p);
        }
        #endregion

        public override void Disconnect()
        {
            Player.GlobalMessage(username != "Default" ? "[Remote]" + username + " Disconnected" : "Remote Disconnected");
            Server.s.Log(username != "Default" ? "[Remote]" + username + " Disconnected" : "Remote Disconnected");
            if (writer != null)
            {
                writer.Close();
                writer.BaseStream.Close();
                writer.BaseStream.Dispose();
                writer = null;
            }
            if (reader != null)
            {
                reader.Close();
                reader = null;
            }
            if (Client != null)
            {
                Client = null;
            }
            if (Remote.remoteList.Contains(this)) Remote.remoteList.Remove(this);

        }
        #region --==Threaded==--

        private void IORead()
        {
            try
            {
                while (reader != null && Client != null && this.isConnected && Client.Connected && reader.BaseStream.CanRead)
                {
                    try
                    {

                        if (!Client.GetStream().DataAvailable)
                        {
                            Thread.Sleep(20);
                            continue;
                        }
                        PacketId packet = (PacketId)reader.ReadByte();

                        switch (packet)
                        {
                            case PacketId.RLogin:
                                HandleLogin();
                                break;
                            case PacketId.RChat:
                                HandleChat();
                                break;
                            case PacketId.RPing:
                                HandlePing();
                                break;
                            case PacketId.RDisconnect:
                                Disconnect();
                                return;
                            case PacketId.RRequest:
                                HandleRequest();
                                break;
                        }
                        Thread.Sleep(20);
                    }
                    catch (SocketException)
                    {
                        Disconnect();
                        return;
                    }
                    catch (IOException)
                    {
                        Disconnect();
                        return;
                    }
                    catch (TimeoutException)
                    {
                        Disconnect();
                        return;
                    }
                    catch (Exception e)
                    {
                        Server.ErrorLog(e);
                        Disconnect();
                        return;
                    }
                }
                Disconnect();
            }
            catch { Disconnect(); }
        }
        private void IOWrite()
        {
            Thread.CurrentThread.Name = "IOWrite";
            try
            {
                while (this.writer.BaseStream.CanWrite && this.isConnected)
                {
                    if (packetQ.Count > 0)
                    {
                        byte[] array = packetQ.Dequeue().getData();
                        if (array.Length <= 0) continue;
                        writer.Write(array, 0, array.Length);
                        writer.Flush();
                    }
                    Thread.Sleep(20);

                }
            }
            catch { }
        }
        private void Ping()
        {
            if (!recievedPing)
            {
                //Disconnect();
                //return;
            }
            recievedPing = false;
            short numb = (short)new Random().Next();
            pingNumb = numb;

            Packet o = new Packet(Encoding.UTF8, false);
            o.addData((byte)PacketId.SPing);
            o.addData(numb);

            Thread.Sleep(30000);

        }
        #endregion


        #region -=Handlers=-
        private void HandleRequest()
        {
            Request r = (Request)reader.ReadByte();
            switch (r)
            {
                case Request.All:
                    StartUp();
                    return;
                case Request.Groups:
                    foreach (var g in Group.GroupList)
                        this.OnGroupChange(g);
                    return;
                case Request.Maps:
                    foreach (var l in Server.levels)
                        this.LevelLoad(l);
                    foreach (var l in GetUnloadedLevels())
                        this.LevelUnload(l);
                    return;
                case Request.Player:
                    foreach (var p in Player.players)
                        this.PlayerConnect(p);
                    return;
                case Request.Settings:
                    SendSettings();
                    return;
            }

        }
        private void HandlePing()
        {
            recievedPing = true;
            if (reader.ReadInt16() == pingNumb) return;
            Disconnect();
        }
        private void HandleChat()
        {
            Server.s.Log(ReadString(reader.ReadInt16()));
        }

        private void HandleLogin()
        {
            string msg = ReadString(reader.ReadInt16());
            Packet p;
            if (msg.StartsWith(this.version.ToString())) //TODO: make a better checker
                msg = msg.Replace(string.Format("{0}: ", this.version.ToString()), "");
            else
            {
                p = new Packet(Encoding.UTF8, false);
                p.addData((byte)PacketId.SLogin);
                p.addData((byte)3);
                packetQ.Enqueue(p);
                Server.s.Log("[Remote] A remote tried to connect with a different version.");
                Disconnect();
                return;
            }
            if (RemoteServer.tries >= 0x3)
            {
                p = new Packet(Encoding.UTF8, false);
                p.addData((byte)PacketId.SLogin);
                p.addData((byte)4);
                packetQ.Enqueue(p);
                Server.s.Log("[Remote] A remote tried to connect with exceeding incorrect credentials");
                Disconnect();
            }
            if (RemoteServer.tries == 0x6)
            {
                p = new Packet(Encoding.UTF8, false);
                p.addData((byte)PacketId.SLogin);
                p.addData((byte)5);
                packetQ.Enqueue(p);
                Server.s.Log("[Remote] Remote was locked from the console, type \"/remote tryreset\" to reset the try count");
                Disconnect();
            }

            if (msg.Split(':')[0] == RemoteServer.Username && msg.Split(':')[1] == RemoteServer.Password)
            {

                //if (OnRemoteLogin != null) OnRemoteLogin(this);
                p = new Packet(Encoding.UTF8, false);
                p.addData((byte)PacketId.SLogin);
                p.addData((byte)1);
                packetQ.Enqueue(p);
                //SendKey();
                Server.s.Log("[Remote] Remote Verified, passing controls to it!");
                isLoggedIn = true;
                Remote.remoteList.Add(this);
                LoadEvents();
                //StartUp();
                return;

            }
            else
            {
                p = new Packet(Encoding.UTF8, false);
                p.addData((byte)PacketId.SLogin);
                p.addData((byte)2);
                packetQ.Enqueue(p);
                Server.s.Log("[Remote] A Remote with incorrect information attempted to join.");
                Disconnect();
                RemoteServer.tries++;

            }

        }
        #endregion

        private void SendKey()
        {
            //Packet p = new Packet(Encoding.UTF8, PacketId.Key)
            //p.addData(key);
            //packetQ.Enqueue(p);
        }
        private void SendSettings()
        {
            if (File.Exists("properties/server.properties"))
            {
                Packet p = new Packet(Encoding.UTF8, false);
                p.addData((byte)PacketId.SSettings);
                p.addData(File.ReadAllText("properties/server.properties", Encoding.UTF8), false);
                packetQ.Enqueue(p);
            }
            else
            {
                Server.s.Log("Error No server properties found");
            }
        }
        private void StartUp()
        {
            Step(0);
            foreach (var l in Server.levels)
                this.LevelLoad(l);
            Step(1);
            foreach (var l in GetUnloadedLevels())
                this.LevelUnload(l);
            Step(2);
            foreach (var p in Player.players)
                this.PlayerConnect(p);
            Step(3);
            foreach (var g in Group.GroupList)
                this.OnGroupChange(g);
            Step(4);
            SendSettings();
            Step(5);
        }

        private void Step(byte e)
        {
            Packet p = new Packet(Encoding.UTF8, false);
            p.addData((byte)PacketId.SProgress);
            p.addData(e);
            packetQ.Enqueue(p);
        }

        #region -=UTILS=-

        public string ReadString(int count = 128)
        {
            if (reader == null) return "";
            byte[] cars = new byte[count];
            reader.BaseStream.Read(cars, 0, cars.Length);
            return Encoding.UTF8.GetString(cars).TrimEnd().Replace("\0", string.Empty);
        }
        public List<string> GetUnloadedLevels()
        {
            var tmpList = new List<string>(Server.levels.Count);
            var realList = new List<string>();
            try
            {
                DirectoryInfo di = new DirectoryInfo("levels/");
                FileInfo[] fi = di.GetFiles("*.lvl");
                tmpList.AddRange(Server.levels.Select(l => l.name));
                realList.AddRange(from l in fi where !tmpList.Contains(l.Name.Replace(".lvl", "")) select l.Name.Replace(".lvl", ""));
                return realList;
            }
            catch (Exception e)
            {
                Server.ErrorLog(e);
                return null;
            }
        }
        #endregion
    }
    enum PacketId : byte
    {
        SLogin,
        SChat,
        SMaps,
        SSettings,
        SGroups,
        SPing,
        SDisconnect,
        SKey,
        SProgress,
        SPlayers,

        RLogin,
        RDisconnect,
        RChat,
        RSettings,
        RPing,
        RGroups,
        RRequest
    }
    enum Request : byte
    {
        All,
        Player,
        Settings,
        Groups,
        Maps
    }
}
