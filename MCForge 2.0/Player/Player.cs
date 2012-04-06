/*
Copyright 2011 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/

using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Windows.Forms;
using MCForge.API.PlayerEvent;
using MCForge.Core;
using MCForge.World;
using MCForge.Interface.Command;
using MCForge.Groups;

namespace MCForge.Entity
{
    /// <summary>
    /// The player class, this contains all player information.
    /// </summary>
    public class Player
    {
        #region Variables
        internal static System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        internal static MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        protected static packet pingPacket = new packet(new byte[1] { (byte)packet.types.SendPing });
        protected static packet mapSendStartPacket = new packet(new byte[1] { (byte)packet.types.MapStart });
        private static byte ForceTpCounter = 0;

        protected static packet MOTD_NonAdmin = new packet();
        protected static packet MOTD_Admin = new packet();
        protected void CheckMotdPackets()
        {
            if (MOTD_NonAdmin.bytes == null)
            {
                MOTD_NonAdmin.Add(packet.types.MOTD);
                MOTD_NonAdmin.Add(ServerSettings.version);
                MOTD_NonAdmin.Add(ServerSettings.NAME, 64);
                MOTD_NonAdmin.Add(ServerSettings.MOTD, 64);
                MOTD_NonAdmin.Add((byte)0);
                MOTD_Admin = MOTD_NonAdmin;
                MOTD_Admin.bytes[130] = 100;
            }
        }

        protected Socket socket;

        protected packet.types lastPacket = packet.types.SendPing;

        /// <summary>
        /// The player's real username
        /// </summary>
        public string USERNAME;
        /// <summary>
        /// Last command the player used.
        /// </summary>
        public string lastcmd;
        /// <summary>
        /// Has the player voted?
        /// </summary>
        public bool voted;
        /// <summary>
        /// Is the player being kicked ?
        /// </summary>
        public bool beingkicked;
        /// <summary>
        /// Has the player read the rules?
        /// </summary>
        public bool readrules = false;
        /// <summary>
        /// Is the player muted
        /// </summary>
        public bool muted = false;
        /// <summary>
        /// Does the player have voice status
        /// </summary>
        public bool voiced = false;
        /// <summary>
        /// Derermines if the player is jokered
        /// </summary>
        public bool jokered = false;
        /// <summary>
        /// Appears in front of player's name if he is voiced
        /// </summary>
        public string voicestring = "";
        /// <summary>
        /// Used for player's LOWERCASE username.
        /// </summary>
        protected string _username; //Lowercase Username
        /// <summary>
        /// This is the player's LOWERCASE username, use this for comparison instead of calling USERNAME.ToLower()
        /// </summary>       
        public string username //Lowercase Username feild
        {
            get
            {
                if (_username == null) _username = USERNAME.ToLower();
                return _username;
            }
        }
        /// <summary>
        /// This is the player's IP Address
        /// </summary>
        public string ip;
        /// <summary>
        /// The player's stored message (For appending)
        /// </summary>
        public string storedMessage = "";

        protected System.Timers.Timer loginTimer = new System.Timers.Timer(30000);
        /// <summary>
        /// This sends the ping packet to the player. This is how we know when a player disconnects.
        /// </summary>
        protected System.Timers.Timer pingTimer = new System.Timers.Timer(1000);
        protected byte[] buffer = new byte[0];
        protected byte[] tempBuffer = new byte[0xFF];
        protected string tempString = null;
        protected byte tempByte = 0xFF;

        /// <summary>
        /// True if the player is currently loading a map
        /// </summary>
        public bool isLoading = true;
        /// <summary>
        /// True if the player is Online (false if the player has disconnected)
        /// </summary>
        public bool isOnline = true;
        /// <summary>
        /// True if the player has completed the login process
        /// </summary>
        public bool isLoggedIn = false;
        /// <summary>
        /// True if player is using static commands
        /// </summary>
        public bool staticCommands = false;
        /// <summary>
        /// True is the player is flying
        /// </summary>
        public bool isFlying = false;
        /// <summary>
        /// This players current level
        /// </summary>
        public Level level = Server.Mainlevel;
        /// <summary>
        /// The players MC Id, this changes each time the player logs in
        /// </summary>
        public byte id = 255;
        /// <summary>
        /// The players current position
        /// </summary>
        public Point3 Pos;
        /// <summary>
        /// The players last known position
        /// </summary>
        public Point3 oldPos;
        /// <summary>
        /// The players current rotation
        /// </summary>
        public byte[] Rot;
        /// <summary>
        /// The players last known rotation
        /// </summary>
        public byte[] oldRot;
        /// <summary>
        /// The players last known click
        /// </summary>
        public Point3 lastClick;
        /// <summary>
        /// The players COLOR
        /// </summary>
        public string color = Colors.navy;
        /// <summary>
        /// True if this player is hidden
        /// </summary>
        public bool isHidden = false;

        /// <summary>
        /// True if this player is an admin
        /// </summary>
        public bool isAdmin = true;
        /// <summary>
        /// Holds replacement messages for profan filter
        /// </summary>
        public static List<string> replacement = new List<string>();

        object PassBackData;
        /// <summary>
        /// This delegate is used for when a command wants to be activated the first time a player places a block
        /// </summary>
        /// <param name="p">This is a player object</param>
        /// <param name="x">The position of the block that was changed (x)</param>
        /// <param name="z">The position of the block that was changed (z)</param>
        /// <param name="y">The position of the block that was changed (y)</param>
        /// <param name="newType">The type of block the user places (air if user is deleting)</param>
        /// <param name="placing">True if the player is placing a block</param>
        /// <param name="PassBack">A passback object that can be used for a command to send data back to itself for use</param>
        public delegate void BlockChangeDelegate(Player p, ushort x, ushort z, ushort y, byte newType, bool placing, object PassBack);
        /// <summary>
        /// This delegate is used for when a command wants to be activated the next time the player sends a message.
        /// </summary>
        /// <param name="p">The player object</param>
        /// <param name="message">The string the player sent</param>
        /// <param name="PassBack">A passback object that can be used for a command to send data back to itself for use</param>
        public delegate void NextChatDelegate(Player p, string message, object PassBack);
        protected BlockChangeDelegate blockChange;
        protected NextChatDelegate nextChat;

        /// <summary>
        /// The current Group of the player
        /// </summary>
        public PlayerGroup group = ServerSettings.DefaultGroup;


        #endregion

        internal Player(TcpClient TcpClient)
        {
            CheckMotdPackets();
            try
            {

                socket = TcpClient.Client;

                ip = socket.RemoteEndPoint.ToString().Split(':')[0];
                Server.Log("[System]: " + ip + " connected", ConsoleColor.Gray, ConsoleColor.Black);

                CheckMultipleConnections();
                if (CheckIfBanned()) return;

                socket.BeginReceive(tempBuffer, 0, tempBuffer.Length, SocketFlags.None, new AsyncCallback(Incoming), this);

                loginTimer.Elapsed += delegate { HandleConnect(); };
                loginTimer.Start();

                pingTimer.Elapsed += delegate { SendPing(); };
                pingTimer.Start();

                Server.Connections.Add(this);
            }
            catch (Exception e)
            {
                SKick("There has been an Error.");
                Server.Log(e);
            }
        }

        #region Incoming Data
        protected void HandleConnect()
        {
            if (!isLoading)
            {
                loginTimer.Stop();
                foreach (string w in ServerSettings.WelcomeText) SendMessage(w);
            }
        }
        protected static void Incoming(IAsyncResult result)
        {
            while (!Server.Started)
                Thread.Sleep(100);

            Player p = (Player)result.AsyncState;

            if (!p.isOnline)                return;

            try
            {
                int length = p.socket.EndReceive(result);
                if (length == 0)
                {
                    p.CloseConnection();
                    if (!p.beingkicked)
                    {
                        UniversalChat(p.color + p.USERNAME + " has disconnected.");
                    }
                    return;
                }
                byte[] b = new byte[p.buffer.Length + length];
                Buffer.BlockCopy(p.buffer, 0, b, 0, p.buffer.Length);
                Buffer.BlockCopy(p.tempBuffer, 0, b, p.buffer.Length, length);
                p.buffer = p.HandlePacket(b);
                p.socket.BeginReceive(p.tempBuffer, 0, p.tempBuffer.Length, SocketFlags.None, new AsyncCallback(Incoming), p);
            }
            catch (SocketException e)
            {
                SocketException rawr = e;
                p.CloseConnection();
                return;
            }
            catch (Exception e)
            {
                Exception rawr = e;
                p.Kick("Error!");
                Server.Log(e);
                return;
            }
        }
        protected byte[] HandlePacket(byte[] buffer)
        {
            try
            {
                int length = 0; byte msg = buffer[0];
                // Get the length of the message by checking the first byte
                switch (msg)
                {
                    case 0: length = 130; break; // login
                    case 2: SMPKick("This is not an SMP Server!"); break; // login??
                    case 5: length = 8; break; // blockchange
                    case 8: length = 9; break; // input
                    case 13: length = 65; break; // chat
                    default: Kick("Unhandled message id \"" + msg + "\"!"); return new byte[0];
                }
                if (buffer.Length > length)
                {
                    byte[] message = new byte[length];
                    Buffer.BlockCopy(buffer, 1, message, 0, length);

                    byte[] tempbuffer = new byte[buffer.Length - length - 1];
                    Buffer.BlockCopy(buffer, length + 1, tempbuffer, 0, buffer.Length - length - 1);

                    buffer = tempbuffer;

                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        switch (msg)
                        {
                            case 0: HandleLogin(message); break;
                            case 5: HandleBlockchange(message); break;
                            case 8: HandleIncomingPos(message); break;
                            case 13: HandleChat(message); break;
                        }
                    });

                    if (buffer.Length > 0)
                        buffer = HandlePacket(buffer);
                    else
                        return new byte[0];


                }
            }
            catch (Exception e)
            {
                Kick("CONNECTION ERROR: (0x03)");
                Server.Log("[ERROR]: PLAYER MESSAGE RECIEVE ERROR (0x03)", ConsoleColor.Red, ConsoleColor.Black);
                Server.Log(e);
            }
            return buffer;
        }

        protected void HandleLogin(byte[] message)
        {
            try
            {
                if (isLoggedIn) return;
                byte version = message[0];
                USERNAME = enc.GetString(message, 1, 64).Trim();
                string verify = enc.GetString(message, 65, 32).Trim();
                byte type = message[129];
                if (!VerifyAccount(USERNAME, verify)) return;
                if (version != ServerSettings.version) { SKick("Wrong Version!."); return; }

                OnPlayerConnect e = new OnPlayerConnect(this);
                e.Call();
                if (e.IsCanceled)
                    return;

                //TODO Database Stuff

                Server.Log("[System]: " + ip + " logging in as " + USERNAME + ".", ConsoleColor.Green, ConsoleColor.Black);
                UniversalChat(USERNAME + " joined the game!");

                CheckDuplicatePlayers(USERNAME);

                foreach (PlayerGroup g in PlayerGroup.groups)
                    if (g.players.Contains(USERNAME.ToLower()))
                        group = g;

                SendMotd();

                isLoading = true;
                SendMap();
                if (!isOnline) return;
                isLoggedIn = true;

                id = FreeId();
                UpgradeConnectionToPlayer();

                //Do we want the same-ip-new-account code?

                //ushort x = (ushort)((0.5 + level.SpawnPos.x) * 32);
                //ushort y = (ushort)((1 + level.SpawnPos.y) * 32);
                //ushort z = (ushort)((0.5 + level.SpawnPos.z) * 32);

                short x = (short)((0.5 + level.SpawnPos.x) * 32);
                short y = (short)((1 + level.SpawnPos.y) * 32);
                short z = (short)((0.5 + level.SpawnPos.z) * 32);

                //x = (ushort)Math.Abs(x);
                //y = (ushort)Math.Abs(y);
                //z = (ushort)Math.Abs(z);

                Pos = new Point3(x, z, y);
                Rot = level.SpawnRot;
                oldPos = Pos;
                oldRot = Rot;

                SpawnThisPlayerToOtherPlayers();
                SpawnOtherPlayersForThisPlayer();
                SendSpawn(this);

                isLoading = false;

            }
            catch (Exception e)
            {
                Server.Log(e);
            }
        }
        protected void HandleBlockchange(byte[] message)
        {
            if (!isLoggedIn) return;

            ushort x = packet.NTHO(message, 0);
            ushort y = packet.NTHO(message, 2);
            ushort z = packet.NTHO(message, 4);
            byte action = message[6];
            byte newType = message[7];

            lastClick = new Point3(x, z, y);

            if (newType > 49 || (newType == 7 && !isAdmin))
            {
                Kick("HACKED CLIENT!");
                //TODO Send message to op's for adminium hack
                return;
            }

            byte currentType = level.GetBlock(x, z, y);
            if (currentType == (byte)Blocks.Types.zero)
            {
                Kick("HACKED CLIENT!");
                return;
            }

            //TODO Check for permissions to build and distance > max
            bool placing = false;
            if (action == 1) placing = true;
            OnPlayerBlockChange b = new OnPlayerBlockChange(x, y, z, (placing ? ActionType.Place : ActionType.Delete), this, newType);
            b.Call();
            if (b.IsCanceled)
                return;
            if (blockChange != null)
            {
                SendBlockChange(x, z, y, currentType);

                BlockChangeDelegate tempBlockChange = blockChange;
                object tempPassBack = PassBackData;

                blockChange = null;
                PassBackData = null;

                ThreadPool.QueueUserWorkItem(delegate { tempBlockChange.Invoke(this, x, z, y, newType, placing, tempPassBack); });
                return;
            }

            if (action == 0) //Deleting
            {
                level.BlockChange(x, z, y, (byte)Blocks.Types.air);
            }
            else //Placing
            {
                level.BlockChange(x, z, y, newType);
            }
        }
        protected void HandleIncomingPos(byte[] message)
        {
            if (!isLoggedIn)
                return;

            byte thisid = message[0];

            if (thisid != 0xFF && thisid != id && thisid != 0)
            {
                //TODO Player.GlobalMessageOps("Player sent a malformed packet!");
                Kick("Hacked Client!");
                return;
            }

            ushort x = packet.NTHO(message, 1);
            ushort y = packet.NTHO(message, 3);
            ushort z = packet.NTHO(message, 5);
            byte rotx = message[7];
            byte roty = message[8];
            Pos.x = (short)x;
            Pos.y = (short)y;
            Pos.z = (short)z;
            Rot = new byte[2] { rotx, roty };
        }
        protected void HandleChat(byte[] message)
        {
            if (!isLoggedIn) return;

            string incomingText = enc.GetString(message, 1, 64).Trim();

            byte incomingID = message[0];
            if (incomingID != 0xFF && incomingID != id && incomingID != 0)
            {
                //TODO Player.GlobalMessageOps("Player sent a malformed packet!");
                Kick("Hacked Client!");
                return;
            }

            incomingText = Regex.Replace(incomingText, @"\s\s+", " ");
            foreach (char ch in incomingText)
            {
                if (ch < 32 || ch >= 127 || ch == '&')
                {
                    Kick("Illegal character in chat message!");
                    return;
                }
            }
            if (incomingText.Length == 0)
                return;
            //Fixes crash
            if (incomingText[0] == '/' && incomingText.Length == 1)
            {
                SendMessage("You didn't specify a command!");
                return;
            }
            //Get rid of whitespace
            while (incomingText.Contains("  "))
                incomingText.Replace("  ", " ");

            //This allows people to use //Command and have it appear as /Command in the chat.
            if (incomingText[0] == '/' && incomingText[1] == '/')
            {
                incomingText = incomingText.Remove(0, 1);
                goto Meep;
            }
            if (incomingText[0] == '/')
            {
                incomingText = incomingText.Remove(0, 1);

                string[] args = incomingText.Split(' ');
                HandleCommand(args);
                return;
            }

            //Meep is used above for //Command
        Meep:

            if (!File.Exists("text/badwords.txt")) { File.Create("text/badwords.txt").Close(); }
            if (!File.Exists("text/replacementwords.txt")) { File.Create("text/replacementwords.txt").Close(); }

            string textz = File.ReadAllText("text/replacementwords.txt");
            if (textz == "") { File.WriteAllText("text/replacementwords.txt", "Pepper"); }
            StreamReader w = File.OpenText("text/replacementwords.txt");
            while (!w.EndOfStream) replacement.Add(w.ReadLine());
            w.Dispose();

        string[] badwords = File.ReadAllLines("text/badwords.txt");
        string[] replacementwords = File.ReadAllLines("text/replacementwords.txt");

        foreach (string word in badwords)
        {
            string text = incomingText;
            if (text.Contains(word))
            {
                incomingText = Regex.Replace(text, word, replacement[new Random().Next(0, replacement.Count)]);
            }
        }
            if (muted) { SendMessage("You are muted!"); return; }
            if (Server.moderation && !voiced && !Server.devs.Contains(USERNAME)) { SendMessage("You can't talk during chat moderation!"); return; }
            if (jokered)
            {
                Random r = new Random();
                int a = r.Next(0, Server.jokermessages.Count);
                incomingText = Server.jokermessages[a];
            }
            //Message appending stuff.
            if (ServerSettings.Appending == true)
            {
                if (storedMessage != "")
                {
                    if (!incomingText.EndsWith(">") && !incomingText.EndsWith("<"))
                    {
                        incomingText = storedMessage.Replace("|>|", " ").Replace("|<|", "") + incomingText;
                        storedMessage = "";
                    }
                }
                if (incomingText.EndsWith(">"))
                {
                    storedMessage += incomingText.Replace(">", "|>|");
                    SendMessage("Message appended!");
                    return;
                }
                else if (incomingText.EndsWith("<"))
                {
                    storedMessage += incomingText.Replace("<", "|<|");
                    SendMessage("Message appended!");
                    return;
                }
            }

            /*if (nextChat != null) AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH
            {
                NextChatDelegate tempNextChat = nextChat;
                object tempPassBack = PassBackData;

                nextChat = null;
                PassBackData = null;

                ThreadPool.QueueUserWorkItem(delegate { tempNextChat.Invoke(this, incomingText, tempPassBack); });

                return;
            }*/

            OnPlayerChat e = new OnPlayerChat(this, incomingText);
            e.Call();
            if (e.IsCanceled)
                return;
            incomingText = e.GetMessage();

            if (Server.voting)
            {
                if (Server.kickvote && Server.kicker == this) { SendMessage("You're not allowed to vote!"); return; }
                if (voted) { SendMessage("You have already voted..."); return; }
                string vote = incomingText.ToLower();
                if (vote == "yes" || vote == "y") { Server.YesVotes++; voted = true; SendMessage("Thanks for voting!"); return; }
                else if (vote == "no" || vote == "n") { Server.NoVotes++; voted = true; SendMessage("Thanks for voting!"); return; }
                else { SendMessage("Use either %aYes " + Server.DefaultColor + "or %cNo " + Server.DefaultColor + " to vote!"); }
            }
            Server.Log("<" + USERNAME + "> " + incomingText);
            UniversalChat(voicestring + group.colour + USERNAME + ": &f" + incomingText);
        }

        #endregion
        #region Outgoing Packets
        protected void SendPacket(packet pa)
        {
            try
            {
                lastPacket = (packet.types)pa.bytes[0];
            }
            catch (Exception e) { Server.Log(e); }
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    socket.BeginSend(pa.bytes, 0, pa.bytes.Length, SocketFlags.None, delegate(IAsyncResult result) { }, null);
                    return;
                }
                catch
                {
                    continue;
                }
            }
            CloseConnection();
        }
        protected void SendMessage(byte PlayerID, string message)
        {
            packet pa = new packet();


            for (int i = 0; i < 10; i++)
            {
                message = message.Replace("%" + i, "&" + i);
                message = message.Replace("&" + i + " &", "&");
            }
            for (char ch = 'a'; ch <= 'f'; ch++)
            {
                message = message.Replace("%" + ch, "&" + ch);
                message = message.Replace("&" + ch + " &", "&");
            }
			message = Server.DefaultColor + message;

            pa.Add(packet.types.Message);
            pa.Add(PlayerID);

            try
            {
                foreach (string line in LineWrapping(message))
                {
                    if (pa.bytes.Length < 64)
                        pa.Add(line, 64);
                    else
                        pa.Set(2, line, 64);

                    SendPacket(pa);
                }
            }
            catch (Exception e)
            {
                Server.Log(e);
            }

        }
        protected void SendMotd()
        {
            if (isAdmin) SendPacket(MOTD_Admin);
            else SendPacket(MOTD_NonAdmin);
        }
        protected void SendMap()
        {
            try
            {
                SendPacket(mapSendStartPacket); //Send the pre-fab map start packet

                packet pa = new packet(); //Create a packet to handle the data for the map
                pa.Add(level.TotalBlocks); //Add the total amount of blocks to the packet
                byte[] blocks = new byte[level.TotalBlocks]; //Temporary byte array so we dont have to keep modding the packet array

                byte block; //A block byte outside the loop, we save cycles by not making this for every loop iteration
                level.ForEachBlock(delegate(int pos)
                {
                    //Here we loop through the whole map and check/convert the blocks as necesary
                    //We then add them to our blocks array so we can send them to the player
                    block = level.data[pos];
                    if (block < 50) blocks[pos] = block;
                    else blocks[pos] = Blocks.CustomBlocks[block].VisibleType;
                });

                pa.Add(blocks); //add the blocks to the packet
                pa.GZip(); //GZip the packet

                int number = (int)Math.Ceiling(((double)(pa.bytes.Length)) / 1024); //The magic number for this packet

                for (int i = 1; pa.bytes.Length > 0; ++i)
                {
                    short length = (short)Math.Min(pa.bytes.Length, 1024);
                    byte[] send = new byte[1027];
                    packet.HTNO(length).CopyTo(send, 0);
                    Buffer.BlockCopy(pa.bytes, 0, send, 2, length);
                    byte[] tempbuffer = new byte[pa.bytes.Length - length];
                    Buffer.BlockCopy(pa.bytes, length, tempbuffer, 0, pa.bytes.Length - length);
                    pa.bytes = tempbuffer;
                    send[1026] = (byte)(i * 100 / number);

                    packet Send = new packet(send);
                    Send.AddStart(new byte[1] { (byte)packet.types.MapData });

                    SendPacket(Send);
                }

                pa = new packet();
                pa.Add(packet.types.MapEnd);
                pa.Add((short)level.Size.x);
                pa.Add((short)level.Size.y);
                pa.Add((short)level.Size.z);
                SendPacket(pa);

                isLoading = false;
            }
            catch (Exception e)
            {
                Server.Log(e);
            }
        }
        protected void SendSpawn(Player p)
        {
            byte ID = 0xFF;
            if (p != this)
                ID = p.id;

            packet pa = new packet();
            pa.Add(packet.types.SendSpawn);
            pa.Add((byte)ID);
            pa.Add(p.USERNAME, 64);
            pa.Add(p.Pos.x);
            pa.Add(p.Pos.y);
            pa.Add(p.Pos.z);
            pa.Add(p.Rot);
            SendPacket(pa);
        }
        /// <summary>
        /// This send a blockchange to the player only. (Not other players)
        /// </summary>
        /// <param name="x"></param> The position the block will be placed in (x)
        /// <param name="z"></param> The position the block will be placed in (z)
        /// <param name="y"></param> The position the block will be placed in (y)
        /// <param name="type"></param> The type of block that will be placed.
        public void SendBlockChange(ushort x, ushort z, ushort y, byte type)
        {
            if (x < 0 || y < 0 || z < 0 || x >= level.Size.x || y >= level.Size.y || z >= level.Size.z) return;

            packet pa = new packet();
            pa.Add(packet.types.SendBlockchange);
            pa.Add(x);
            pa.Add(y);
            pa.Add(z);

            if (type > 49) type = Blocks.CustomBlocks[type].VisibleType;
            pa.Add(type);

            SendPacket(pa);
        }
        protected void SendKick(string message)
        {
            packet pa = new packet();
            pa.Add(packet.types.SendKick);
            pa.Add(message, 64);
            SendPacket(pa);
        }
        protected void SMPKick(string a)
        {
            //TODO SMPKICK
        }
        protected void SendPing()
        {
            SendPacket(pingPacket);
        }

        /// <summary>
        /// Send this player a message
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendMessage(string message)
        {
            SendMessage(id, message); // 0xFF is NOT a valid player ID
        }
        /// <summary>
        /// Exactly what the function name is, it might be useful to change this players pos first ;)
        /// </summary>
        public void SendThisPlayerTheirOwnPos()
        {
            packet pa = new packet();
            pa.Add((byte)8);
            pa.Add(Pos.x);
            pa.Add(Pos.y);
            pa.Add(Pos.z);
            pa.Add(Rot);
            SendPacket(pa);
        }
        /// <summary>
        /// Kick this player with the specified message, the message broadcasts across the server
        /// </summary>
        /// <param name="message">The message to send</param>
        public void Kick(string message)
        {
            //GlobalMessage(message);
			beingkicked = true;
            SKick(message);
        }
        /// <summary>
        /// Kick this player with a specified message, this message will only get sent to op's
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SKick(string message)
        {
            Server.Log("[Info]: Kicked: *" + USERNAME + "* " + message, ConsoleColor.Yellow, ConsoleColor.Black);
            SendKick(message);
            //CloseConnection();
        }
        /// <summary>
        /// Sends the specified player to the specified coordinates.
        /// </summary>
        /// <param name="_pos"></param>Point3 coordinate to send to.
        /// <param name="_rot"></param>Rot to send to.
        public void SendToPos(Point3 _pos, byte[] _rot)
        {
            oldPos = Pos; oldRot = Rot;

            packet pa = new packet();
            pa.Add(packet.types.SendTeleport);
            pa.Add(unchecked((byte)-1)); //If the ID is not greater than one it doesn't work :c
            pa.Add(_pos.x);
            pa.Add(_pos.y);
            pa.Add(_pos.z);
            pa.Add(Rot);
            foreach (Player p in Server.Players.ToArray())
            {
                if (p.level == level && p.isLoggedIn && !p.isLoading)
                {
                    p.SendPacket(pa);
                }
            }
        }

        protected void UpdatePosition(bool ForceTp)
        {
            byte changed = 0;   //Denotes what has changed (x,y,z, rotation-x, rotation-y)

            Point3 tempOldPos = oldPos;
            Point3 tempPos = Pos;
            byte[] tempRot = Rot;
            byte[] tempOldRot = oldRot;

            oldPos = Pos; oldRot = Rot;

            int diffX = tempPos.x - tempOldPos.x;
            int diffZ = tempPos.z - tempOldPos.z;
            int diffY = tempPos.y - tempOldPos.y;
            int diffR0 = tempRot[0] - tempRot[0];
            int diffR1 = tempRot[1] - tempRot[1];

            if (ForceTp) changed = 4;
            else
            {
                //TODO rewrite local pos change code
                if (diffX == 0 && diffY == 0 && diffZ == 0 && diffR0 == 0 && diffR1 == 0)
                {
                    return; //No changes
                }
                if (Math.Abs(diffX) > 100 || Math.Abs(diffY) > 100 || Math.Abs(diffZ) > 100)
                {
                    changed = 4; //Teleport Required
                }
                else if (diffR0 == 0 && diffR1 == 0)
                {
                    changed = 1; //Pos Update Required
                }
                else
                {
                    changed += 2; //Rot Update Required

                    if (diffX != 0 || diffY != 0 || diffZ != 0)
                    {
                        changed += 1;
                    }
                }
            }

            packet pa = new packet();

            switch (changed)
            {
                case 1: //Pos Change
                    pa.Add(packet.types.SendPosChange);
                    pa.Add(id);
                    pa.Add((sbyte)(diffX));
                    pa.Add((sbyte)(diffY));
                    pa.Add((sbyte)(diffZ));
                    break;
                case 2: //Rot Change
                    pa.Add(packet.types.SendRotChange);
                    pa.Add(id);
                    pa.Add(new byte[2] { (byte)diffR0, (byte)diffR1 });
                    break;
                case 3: //Pos AND Rot Change
                    pa.Add(packet.types.SendPosANDRotChange);
                    pa.Add(id);
                    pa.Add(diffX);
                    pa.Add(diffY);
                    pa.Add(diffZ);
                    pa.Add(new byte[2] { (byte)diffR0, (byte)diffR1 });
                    break;
                case 4: //Teleport Required
                    pa.Add(packet.types.SendTeleport);
                    pa.Add(id);
                    pa.Add(tempPos.x);
                    pa.Add(tempPos.y);
                    pa.Add(tempPos.z);
                    pa.Add(Rot);
                    break;
            }


            foreach (Player p in Server.Players.ToArray())
            {
                if (p != this && p.level == level && p.isLoggedIn && !p.isLoading)
                {
                    p.SendPacket(pa);
                }
            }
        }
        #endregion

        #region Special Chat Handlers
        protected void HandleCommand(string[] args)
        {
            string[] sendArgs = new string[0];
            if (args.Length > 1)
            {
                sendArgs = new string[args.Length - 1];
                for (int i = 1; i < args.Length; i++)
                {
                    sendArgs[i - 1] = args[i];
                }
            }

            string name = args[0].ToLower().Trim();
            if (Command.Commands.ContainsKey(name))
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    ICommand cmd = Command.Commands[name];
                    if (!Server.agreed.Contains(USERNAME) && name != "rules" && name != "agree" && name != "disagree")
                    {
                        SendMessage("You need to /agree to the /rules before you can use commands!"); return;
                    }
                    if (!group.CanExecute(cmd))
                    {
                        SendMessage(Colors.red + "You cannot use /" + name + "!");
                        return;
                    }
                    try { cmd.Use(this, sendArgs); } //Just so it doesn't crash the server if custom command makers release broken commands!
                    catch (Exception ex)
                    {
                        Server.Log("[Error] An error occured when " + USERNAME + " tried to use " + name + "!", ConsoleColor.Red, ConsoleColor.Black);
                        Server.Log(ex);
                    }
                    lastcmd = name;
                });
            }
            else
            {
                SendMessage("Unknown command \"" + name + "\"!");
            }

            foreach (string s in Command.Commands.Keys)
            {
                Console.WriteLine(args[0]);
                Console.WriteLine("'" + s + "'");
            }
        }
        #endregion

        #region Global and Universal shit
        internal static void GlobalUpdate()
        {
            //Player update code
            ForceTpCounter++;
            foreach (Player p in Server.Players.ToArray())
            {
                if (ForceTpCounter == 100) { if (!p.isHidden) p.UpdatePosition(true); }
                else { if (!p.isHidden) p.UpdatePosition(false); }

            }
        }
        internal static void GlobalBlockchange(Level l, ushort x, ushort z, ushort y, byte block)
        {
            foreach (Player p in Server.Players.ToArray())
            {
                if (p.level == l)
                    p.SendBlockChange(x, z, y, block);
            }
        }
        /// <summary>
        /// Kill this player for everyone.
        /// </summary>
        public void GlobalDie()
        {
            packet pa = new packet(new byte[2] { (byte)packet.types.SendDie, id });
            foreach (Player p in Server.Players.ToArray())
            {
                if (p != this)
                {
                    p.SendPacket(pa);
                }
            }
        }
        /// <summary>
        /// Send a message to everyone, on every world
        /// </summary>
        /// <param name="text">The message to send.</param>
        public static void UniversalChat(string text)
        {
            foreach (Player p in Server.Players.ToArray())
            {
                p.SendMessage(text);
            }
        }
        #endregion

        #region PluginStuff
        /// <summary>
        /// This void catches the new blockchange a player does.
        /// </summary>
        /// <param name="change">The BlockChangeDelegate that will be executed on blockchange.</param>
        /// <param name="data">A passback object that can be used for a command to send data back to itself for use</param>
        [Obsolete("Please use OnPlayerBlockChange event (will be removed before release)")]
        public void CatchNextBlockchange(BlockChangeDelegate change, object data)
        {
            PassBackData = data;
            nextChat = null;
            blockChange = change;
        }
        /// <summary>
        /// This delegate is used for when a command wants to be activated the next time the player sends a message.
        /// </summary>
        /// <param name="chat">The NextChatDelegate that will be executed on the next chat.</param>
        /// <param name="data">A passback object that can be used for a command to send data back to itself for use</param>
        /*public void CatchNextChat(NextChatDelegate chat, object data)
        {
            PassBackData = data;
            blockChange = null;
            nextChat = chat;
        }*/
        /// <summary>
        /// Fakes a click by invoking a blockchange event.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        public void Click(ushort x, ushort z, ushort y, byte type)
        {
            OnPlayerBlockChange b = new OnPlayerBlockChange(x, y, z, ActionType.Place, this, type);
            b.Call();
            if (blockChange != null)
            {
                bool placing = true;
                BlockChangeDelegate tempBlockChange = blockChange;
                object tempPassBack = PassBackData;

                blockChange = null;
                PassBackData = null;

                ThreadPool.QueueUserWorkItem(delegate { tempBlockChange.Invoke(this, x, z, y, type, placing, tempPassBack); });
                return;
            }
        }
        #endregion

        protected static List<string> LineWrapping(string message)
        {
            List<string> lines = new List<string>();
            message = Regex.Replace(message, @"(&[0-9a-f])+(&[0-9a-f])", "$2");
            message = Regex.Replace(message, @"(&[0-9a-f])+$", "");
            int limit = 64; string color = "";
            while (message.Length > 0)
            {
                if (lines.Count > 0) { message = "> " + color + message.Trim(); }
                if (message.Length <= limit) { lines.Add(message); break; }
                for (int i = limit - 1; i > limit - 9; --i)
                {
                    if (message[i] == ' ')
                    {
                        lines.Add(message.Substring(0, i)); goto Next;
                    }
                }
                lines.Add(message.Substring(0, limit));
            Next: message = message.Substring(lines[lines.Count - 1].Length);
                if (lines.Count == 1)
                {
                    limit = 60;
                }
                int index = lines[lines.Count - 1].LastIndexOf('&');
                if (index != -1)
                {
                    if (index < lines[lines.Count - 1].Length - 1)
                    {
                        char next = lines[lines.Count - 1][index + 1];
                        if ("0123456789abcdef".IndexOf(next) != -1) { color = "&" + next; }
                        if (index == lines[lines.Count - 1].Length - 1)
                        {
                            lines[lines.Count - 1] = lines[lines.Count - 1].
                                Substring(0, lines[lines.Count - 1].Length - 2);
                        }
                    }
                    else if (message.Length != 0)
                    {
                        char next = message[0];
                        if ("0123456789abcdef".IndexOf(next) != -1)
                        {
                            color = "&" + next;
                        }
                        lines[lines.Count - 1] = lines[lines.Count - 1].
                            Substring(0, lines[lines.Count - 1].Length - 1);
                        message = message.Substring(1);
                    }
                }
            } return lines;
        }
        /// <summary>
        /// Spawns this player to all other players in the server.
        /// </summary>
        protected void SpawnThisPlayerToOtherPlayers()
        {
            foreach (Player p in Server.Players.ToArray())
            {
                if (p == this) continue;
                p.SendSpawn(this);
            }
        }
        /// <summary>
        /// Spawns all other players of the server to this player.
        /// </summary>
        protected void SpawnOtherPlayersForThisPlayer()
        {
            foreach (Player p in Server.Players)
            {
                if (p == this) continue;
                SendSpawn(p);
            }
        }

        protected void CloseConnection()
        {
            isLoggedIn = false;
            isOnline = false;

            GlobalDie();
            Server.Log("[System]: " + USERNAME + " Has DC'ed (" + lastPacket + ")", ConsoleColor.Gray, ConsoleColor.Black);

            pingTimer.Stop();

            Server.Players.Remove(this);
            Server.Connections.Remove(this);

            socket.Close();
        }

        protected byte FreeId()
        {
            List<byte> usedIds = new List<byte>();

            foreach (Player p in Server.Players.ToArray())
            {
                usedIds.Add(p.id);
            }

            for (byte i = 0; i < ServerSettings.MaxPlayers; ++i)
            {
                if (usedIds.Contains(i)) continue;
                return i;
            }

            Server.Log("Too many players O_O", ConsoleColor.Red, ConsoleColor.Black);
            return 254;
        }
        protected void UpgradeConnectionToPlayer()
        {
            try
            {
                Server.Connections.Remove(this);
                Server.Players.Add(this);
            }
            catch (Exception e)
            {
                Server.Log(e);
            }
            //TODO Update form list
        }

        #region Verification Stuffs
        protected void CheckMultipleConnections()
        {
            foreach (Player p in Server.Connections.ToArray())
            {
                if (p.ip == ip && p != this)
                {
                    p.Kick("Only one half open connection is allowed per IP address.");
                }
            }
        }
        protected static void CheckDuplicatePlayers(string username)
        {
            foreach (Player p in Server.Players.ToArray())
            {
                if (p.username == username)
                {
                    p.Kick("You have logged in elsewhere!");
                }
            }
        }
        protected bool CheckIfBanned()
        {
            if (Server.BannedIP.Contains(ip)) { Kick("You're Banned!"); return true; }
            return false;
        }
        protected bool VerifyAccount(string name, string verify)
        {
            if (ServerSettings.VerifyAccounts && ip != "127.0.0.1")
            {
                if (Server.Players.Count >= ServerSettings.MaxPlayers) { SKick("Server is full, please try again later!"); return false; }

                if (verify == null || verify == "" || verify == "--" || (verify != BitConverter.ToString(md5.ComputeHash(enc.GetBytes(ServerSettings.salt + name))).Replace("-", "").ToLower().TrimStart('0') && verify != BitConverter.ToString(md5.ComputeHash(enc.GetBytes(ServerSettings.password + name))).Replace("-", "").ToLower().TrimStart('0')))
                {
                    SKick("Account could not be verified, try again.");
                    //Server.Log("'" + verify + "' != '" + BitConverter.ToString(md5.ComputeHash(enc.GetBytes(ServerSettings.salt + name))).Replace("-", "").ToLower().TrimStart('0') + "'");
                    return false;
                }
            }
            if (name.Length > 16 || !ValidName(name)) { SKick("Illegal name!"); return false; } //Illegal Name Kick
            return true;
        }
        /// <summary>
        /// Check to see is a given name is valid
        /// </summary>
        /// <param name="name">the name to check</param>
        /// <returns>returns true if name is valid</returns>
        public static bool ValidName(string name)
        {
            string allowedchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890._";
            foreach (char ch in name) { if (allowedchars.IndexOf(ch) == -1) { return false; } } return true;
        }

        /// <summary>
        /// Attempts to find the player in the list of online players
        /// </summary>
        /// <param name="name">The player name to find</param>
        /// <remarks>Can be a partial name</remarks>
        public static Player Find(string name)
        {
            List<Player> players = new List<Player>();
            foreach (Player p in Server.Players.ToArray())
            {
                if (p.username.StartsWith(name.ToLower()))
                    players.Add(p);
            }
            if (players.Count == 1)
                return players[0];
            return null;
        }
        #endregion

    }

    public struct packet
    {
        public byte[] bytes;

        #region Constructors
        public packet(byte[] data)
        {
            bytes = data;
        }
        public packet(packet p)
        {
            bytes = p.bytes;
        }
        #endregion
        #region Adds
        public void AddStart(byte[] data)
        {
            byte[] temp = bytes;

            bytes = new byte[temp.Length + data.Length];

            data.CopyTo(bytes, 0);
            temp.CopyTo(bytes, data.Length);
        }

        public void Add(byte[] data)
        {
            if (bytes == null)
            {
                bytes = data;
            }
            else
            {
                byte[] temp = bytes;

                bytes = new byte[temp.Length + data.Length];

                temp.CopyTo(bytes, 0);
                data.CopyTo(bytes, temp.Length);
            }
        }
        public void Add(sbyte a)
        {
            Add(new byte[1] { (byte)a });
        }
        public void Add(byte a)
        {
            Add(new byte[1] { a });
        }
        public void Add(types a)
        {
            Add((byte)a);
        }
        public void Add(short a)
        {
            Add(HTNO(a));
        }
        public void Add(ushort a)
        {
            Add(HTNO(a));
        }
        public void Add(int a)
        {
            Add(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(a)));
        }
        public void Add(string a)
        {
            Add(a, a.Length);
        }
        public void Add(string a, int size)
        {
            Add(Player.enc.GetBytes(a.PadRight(size).Substring(0, size)));
        }
        #endregion
        #region Sets
        public void Set(int offset, short a)
        {
            HTNO(a).CopyTo(bytes, offset);
        }
        public void Set(int offset, ushort a)
        {
            HTNO(a).CopyTo(bytes, offset);
        }
        public void Set(int offset, string a, int length)
        {
            Player.enc.GetBytes(a.PadRight(length).Substring(0, length)).CopyTo(bytes, offset);
        }
        #endregion

        public void GZip()
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            GZipStream gs = new GZipStream(ms, CompressionMode.Compress, true);
            gs.Write(bytes, 0, bytes.Length);
            gs.Close();
            gs.Dispose();

            ms.Position = 0;
            bytes = new byte[ms.Length];
            ms.Read(bytes, 0, (int)ms.Length);
            ms.Close();
            ms.Dispose();
        }

        #region == Host <> Network ==
        public static byte[] HTNO(ushort x)
        {
            byte[] y = BitConverter.GetBytes(x); Array.Reverse(y); return y;
        }
        public static ushort NTHO(byte[] x, int offset)
        {
            byte[] y = new byte[2];
            Buffer.BlockCopy(x, offset, y, 0, 2); Array.Reverse(y);
            return BitConverter.ToUInt16(y, 0);
        }
        public static byte[] HTNO(short x)
        {
            byte[] y = BitConverter.GetBytes(x); Array.Reverse(y); return y;
        }
        #endregion

        public enum types
        {
            Message = 13,
            MOTD = 0,
            MapStart = 2,
            MapData = 3,
            MapEnd = 4,
            SendSpawn = 7,
            SendDie = 12,
            SendBlockchange = 6,
            SendKick = 14,
            SendPing = 1,

            SendPosChange = 10,
            SendRotChange = 11,
            SendPosANDRotChange = 9,
            SendTeleport = 8,

        }
    }
}
