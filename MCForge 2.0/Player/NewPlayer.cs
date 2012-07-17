using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using MCForge.Networking;
using System.Net;
using MCForge.Interface.Command;
using MCForge.World;
using MCForge.Core;
using MCForge.Utils;
using MCForge.Groups;
using MCForge.Networking.Packets;
using MCForge.API.Events;
using System.IO;
using MCForge.Utils.Settings;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace MCForge.Entity {
    public class NewPlayer : IDisposable {

        #region Properties

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public TcpClient Client { get; set; }

        /// <summary>
        /// Gets or sets the packet queue.
        /// </summary>
        /// <value>
        /// The packet queue.
        /// </value>
        public PacketQueue PacketQueue { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="NewPlayer"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }



        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public char Color { get; set; }

        private string _displayName = "";
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName {
            get {
                return _displayName;
            }
            set {
                _displayName = value;
                //if (IsLoggedIn) {
                //    this.GlobalDie();
                //     SpawnThisPlayerToOtherPlayers();
                //  }
            }
        }

        private Level _level;
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public Level Level {
            get {
                if ( _level == null )
                    _level = Server.Mainlevel;
                return _level;
            }
            set {
                _level = value;
                //  if (IsLoggedIn)
                //    SendMap();
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the color of the title.
        /// </summary>
        /// <value>
        /// The color of the title.
        /// </value>
        public char TitleColor { get; set; }

        /// <summary>
        /// Gets or sets the IP.
        /// </summary>
        /// <value>
        /// The IP.
        /// </value>
        public IPAddress IP { get; set; }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        public PlayerGroup Group { get; set; }



        /// <summary>
        /// Gets or sets a value indicating whether this instance is logged in.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is logged in; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoggedIn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is validated.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is validated; otherwise, <c>false</c>.
        /// </value>
        public bool IsValidated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is loading.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is loading; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoading { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is being kicked.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is being kicked; otherwise, <c>false</c>.
        /// </value>
        public bool IsBeingKicked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is an admin.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is an admin; otherwise, <c>false</c>.
        /// </value>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is an operator.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is an operator; otherwise, <c>false</c>.
        /// </value>
        public bool IsOperator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is hidden; otherwise, <c>false</c>.
        /// </value>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected { get; set; }


        /// <summary>
        /// Gets the data base ID.
        /// </summary>
        public long DataBaseID { get; private set; }

        /// <summary>
        /// Gets the first login.
        /// </summary>
        public DateTime FirstLogin { get; private set; }

        /// <summary>
        /// Gets the last login.
        /// </summary>
        public DateTime LastLogin { get; private set; }




        /// <summary>
        /// Gets or sets a value indicating whether static commands are enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if static commands are enabled; otherwise, <c>false</c>.
        /// </value>
        public bool StaticCommandsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the last command used.
        /// </summary>
        /// <value>
        /// The last command used.
        /// </value>
        public ICommand LastCommandUsed { get; set; }



        /// <summary>
        /// Gets or sets the block change history.
        /// </summary>
        /// <value>
        /// The block change history.
        /// </value>
        public BlockChangeHistory BlockChangeHistory { get; set; }

        /// <summary>
        /// Gets the player ID.
        /// </summary>
        public byte PlayerID { get; private set; }



        /// <summary>
        /// Gets or sets the current position.
        /// </summary>
        /// <value>
        /// The current position.
        /// </value>
        public Vector3S CurrentPosition { get; set; }

        /// <summary>
        /// Gets or sets the old position.
        /// </summary>
        /// <value>
        /// The old position.
        /// </value>
        public Vector3S OldPosition { get; set; }

        /// <summary>
        /// Gets or sets the current pitch.
        /// </summary>
        /// <value>
        /// The current pitch.
        /// </value>
        public byte CurrentPitch { get; set; }

        /// <summary>
        /// Gets or sets the current yaw.
        /// </summary>
        /// <value>
        /// The current yaw.
        /// </value>
        public byte CurrentYaw { get; set; }

        /// <summary>
        /// Gets or sets the old pitch.
        /// </summary>
        /// <value>
        /// The old pitch.
        /// </value>
        public byte OldPitch { get; set; }

        /// <summary>
        /// Gets or sets the old yaw.
        /// </summary>
        /// <value>
        /// The old yaw.
        /// </value>
        public byte OldYaw { get; set; }


        /// <summary>
        /// Gets the extra data.
        /// </summary>
        public ExtraData<object, object> ExtraData { get; private set; }

        private readonly Random Random = new Random();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NewPlayer"/> class.
        /// </summary>
        public NewPlayer() {

            ExtraData = new ExtraData<object, object>();
            this._displayName = string.Empty;
            this.Username = string.Empty;
            this.Title = string.Empty;
            this.Group = PlayerGroup.Default;
            // BlockChangeHistory = new BlockChangeHistory(this);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewPlayer"/> class.
        /// </summary>
        /// <param name="Client">The client.</param>
        public NewPlayer(TcpClient Client)
            : this() {

            IP = IPAddress.Parse(Client.Client.RemoteEndPoint.ToString());
            this.Client = Client;
            PacketQueue = new PacketQueue(Client);
            IsConnected = true;

            ProcessPackets();
        }

        #endregion

        #region Packet Processors / Mutators

        void ProcessPackets() {

            while ( IsConnected ) {

                try {
                    MCForge.Networking.Packet packet = PacketQueue.InQueue.Dequeue();
                    // PacketEventArgs args = new PacketEventArgs(packet.WritePacket(), true, )

                    switch ( packet.PacketID ) {

                        case PacketIDs.Identification:
                            ProcessLogin(packet as PacketIdentification);
                            break;

                        case PacketIDs.PlayerSetBlock:
                            ProcessBlockChange(packet as PacketPlayerSetBlock);
                            break;

                        case PacketIDs.PosAndRot:
                            ProcessPosAndRot(packet as PacketPositionAndOrientation);
                            break;

                        case PacketIDs.Message:
                            ProcessMessage(packet as PacketMessage);
                            break;

                    }

                }
                catch {
                    break;
                }

            }

            PacketQueue.CloseConnection();
        }


        void ProcessLogin(PacketIdentification packet) {

            if ( IsLoggedIn ) {
                return;
            }

            this.Username = packet.Username;

            if ( Server.IPBans.ToArray().ContainsIgnoreCase(IP.ToString()) ||
                  Server.UsernameBans.ToArray().ContainsIgnoreCase(packet.Username) ) {
                Kick("You are banned : " + GetBanReason(packet.Username) ?? "");
                return;
            }

            //TODO: temp bans

            if ( !IsValidAccount(this, packet.VerificationKey) ) {
                Kick("Account could not be verified.");
                return;
            }
            IsValidated = true;

            if ( !IsValidUsername(packet.Username) ) {
                Kick("Invalid Username");
            }

            if ( Server.Players.Count >= ServerSettings.GetSettingInt("maxplayers") ) {
                Kick("Server is full. Please try again later.");
                return;
            }

            Player p;
            if ( ( p = Find(Username) ) != null ) {
                p.Kick("Logged in somewhere else");
                return;
            }


            IsLoggedIn = true;

            // --------- Player is logged in and validated --------------//

            Logger.Log(string.Format("{0} logged in as {1}.", IP, Username));
            SendMessageToAllPlayersF("&3{0} has joined the server", Username);
            try { Server.IRC.SendMessage(string.Format("{0} Joined the server", Username)); }
            catch { }

            foreach ( var group in PlayerGroup.Groups ) {
                if ( group.Players.Contains(Username) ) {
                    this.Group = group;
                    break;
                }
            }

            SendMOTD();
            PlayerID = GetNextFreeID();
            Level = Server.Mainlevel;


            // ------------- Player is Connected, and map is loaded -----------//

            CurrentPosition = new Vector3S() {
                x = (short)( ( .5 + Level.SpawnPos.x ) * PlayerConstants.PIXEL_TO_METER_RATIO ),
                y = (short)( ( 1 + Level.SpawnPos.y ) * PlayerConstants.PIXEL_TO_METER_RATIO ),
                z = (short)( ( .5 + Level.SpawnPos.z ) * PlayerConstants.PIXEL_TO_METER_RATIO ),
            };
            CurrentPitch = Level.SpawnRot[0];
            CurrentYaw = Level.SpawnRot[1];

            OldPosition = CurrentPosition;
            OldPitch = CurrentPitch;
            OldYaw = CurrentYaw;

            SpawnEntity(this);
            GlobalSpawnEntity(this);
            UpdateEntities();
        }

        void ProcessBlockChange(PacketPlayerSetBlock packet) {

        }

        void ProcessPosAndRot(PacketPositionAndOrientation packet) {

        }

        void ProcessMessage(PacketMessage packet) {

        }


        #endregion

        #region Public methods

        /// <summary>
        /// Kicks the player with the specified reason.
        /// </summary>
        /// <param name="reason">The reason.</param>
        public void Kick(string reason) {
            IsBeingKicked = true;

            PacketQueue.WritePacketNowAndFlush(new PacketDisconnectPlayer(0xFF, reason));

            PacketQueue.Stop();

            PacketQueue.CloseConnection();

            Dispose();
        }


        /// <summary>
        /// Sends a formatted message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="message">The message.</param>
        public void SendMessageF(string format, string message) {

        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SendMessage(string message) {

        }

        /// <summary>
        /// Updates the entities.
        /// </summary>
        public void UpdateEntities() {

        }

        /// <summary>
        /// Spawns the entity.
        /// </summary>
        /// <param name="p">The player.</param>
        public void SpawnEntity(NewPlayer p) {

            byte ID = 255;

            if( p != this )
                   ID = p.PlayerID;

            PacketQueue.WritePacket(new PacketSpawnPlayer(ID, p.DisplayName, p.CurrentPosition, p.CurrentPitch, p.CurrentYaw));
            UpdatePos();

        }

        /// <summary>
        /// Updates the player position.
        /// </summary>
        public void UpdatePos() {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Sends the MOTD.
        /// </summary>
        /// <param name="firstLine">The first line.</param>
        /// <param name="secondLine">The second line.</param>
        public void SendMOTD(string firstLine, string secondLine) {
            PacketQueue.WritePacket(new PacketIdentification(IsOperator) {
                FirstLine = firstLine,
                SecondLine = secondLine
            });
        }

        /// <summary>
        /// Sends the MOTD.
        /// </summary>
        public void SendMOTD() {
            PacketQueue.WritePacket(new PacketIdentification(IsOperator));
        }


        /// <summary>
        /// Sends the map.
        /// </summary>
        public void SendMap() {

            IsLoading = true;

            PacketQueue.WritePacket(new PacketLevelInitialize());

            byte[] data;
            using ( var stream = new MemoryStream() ) {
                using ( var zipper = new GZipStream(stream, CompressionMode.Compress, true) ) {
                    zipper.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Level.Data.Length)), 0, 4);
                    zipper.Write(Level.Data, 0, Level.Data.Length);
                }
                data = stream.ToArray();
            }

            int sentBytes = 0;
            byte[] buffer = new byte[1024];

            while ( sentBytes < data.Length ) {
                int size = data.Length - sentBytes;
                if ( size > 1024 ) {
                    size = 1024;
                }

                Array.Copy(data, sentBytes, buffer, 0, size);
                byte prog = (byte)( 100 * sentBytes / data.Length );
                PacketQueue.WritePacket(new PacketLevelDataChunk((short)size, buffer, prog));

                sentBytes += size;
            }

            PacketQueue.WritePacket(new PacketLevelFinalize(Level.Size));
            IsLoading = false;

        }

        #endregion

        #region Utils

        /// <summary>
        /// Gets a ban reason, if there is any.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>A ban reason</returns>
        public static string GetBanReason(string username) {
            foreach ( var line in File.ReadAllLines("bans/BanInfo.txt") ) {
                string[] split = line.Split('`');

                if ( split.Length == 2 && string.Equals(split[0], username, StringComparison.OrdinalIgnoreCase) ) {
                    return split[1];
                }
            }
            return null;
        }

        /// <summary>
        /// Determines whether the player has a valid account
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="playerKey">The player key.</param>
        /// <returns>
        ///   <c>true</c> if the player has a valid account; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidAccount(NewPlayer player, string playerKey) {

            if ( ServerSettings.GetSettingBoolean("offline") || InetUtils.IsLocalIP(player.IP) )
                return true;

            while ( playerKey.Length < 32 )
                playerKey = "0" + playerKey;

            MD5 hash = MD5.Create();
            StringBuilder builder = new StringBuilder(32);
            foreach ( byte b in hash.ComputeHash(Encoding.ASCII.GetBytes(ServerSettings.Salt + player.Username)) )
                builder.AppendFormat("{0:x2}", b);

            return string.Equals(builder.ToString(), playerKey, StringComparison.OrdinalIgnoreCase);

        }

        /// <summary>
        /// Determines whether the specified username is a valid username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        ///   <c>true</c> if the specified username is a valid username; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidUsername(string username) {
            return Regex.IsMatch(username, "[A-Za-z0-9\\_\\.]{1,64}") && !Regex.IsMatch(username, "[\\s+]{1,64}");
        }

        public static void SendMessageToAllPlayers(string message, ChatType chatType = ChatType.Player) {

            //TODO: handle chat type

            foreach ( var player in Server.Players )
                player.SendMessage(message);
        }

        /// <summary>
        /// Sends a message to all of the players using a formatted method.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="message">The message.</param>
        /// <param name="chatType">Type of the chat.</param>
        public static void SendMessageToAllPlayersF(string format, params string[] message) {
            SendMessageToAllPlayers(string.Format(format, message), ChatType.Player);
        }

        /// <summary>
        /// Attempts to find the player in the list of online players. Returns null if no players are found.
        /// </summary>
        /// <param name="name">The player name to find</param>
        /// <remarks>Can be a partial name</remarks>
        public static Player Find(string name) {
            foreach ( var p in Server.Players.ToArray() )
                if ( p.Username.Equals(name, StringComparison.OrdinalIgnoreCase) )
                    return p;
            return null;
        }

        /// <summary>
        /// Gets the next free ID.
        /// </summary>
        /// <returns>a byte ID</returns>
        public static byte GetNextFreeID() {

            List<byte> usedIds = new List<byte>();

            Server.Players.ForEach(p => usedIds.Add(p.ID));
            Server.Bots.ForEach(p => usedIds.Add(p.Player.ID));

            for ( byte i = 1; i < ServerSettings.GetSettingInt("maxplayers"); ++i ) {
                if ( usedIds.Contains(i) ) continue;
                return i;
            }

            return 254;
        }

        /// <summary>
        /// Spawns the specified player for everybody.
        /// </summary>
        /// <param name="playerToSpawn">The player to spawn.</param>
        public static void GlobalSpawnEntity(NewPlayer playerToSpawn) {
            foreach ( var player in Server.Players ) { }
        }

        #endregion

        #region Enumerations


        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {

            if ( Disposed )
                return;

            Disposed = true;

            try {
                if ( Client != null ) {
                    Client.Close();
                    Client = null;
                }
            }
            catch ( ObjectDisposedException ) { }

            IsLoggedIn = false;
            IsConnected = false;
            IsValidated = false;

        }

        #endregion
    }
}
