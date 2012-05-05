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
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using MCForge.API.PlayerEvent;
using MCForge.Core;
using MCForge.Groups;
using MCForge.Interface.Command;
using MCForge.Utilities.Settings;
using MCForge.World;
using MCForge.Utilities;
using MCForge.Utils;
using System.Linq;
using MCForge.SQL;
using System.Data;

namespace MCForge.Entity {
    /// <summary>
    /// The player class, this contains all player information.
    /// </summary>
    public sealed partial class Player {
        #region Variables

        //TODO: Change all o dis
        internal static readonly System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        internal static readonly MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        protected static readonly packet pingPacket = new packet(new byte[1] { (byte)packet.types.SendPing });
        protected static readonly packet mapSendStartPacket = new packet(new byte[1] { (byte)packet.types.MapStart });
        private static byte ForceTpCounter = 0;

        protected static packet MOTD_NonAdmin = new packet();
        protected static packet MOTD_Admin = new packet();
        protected static void CheckMotdPackets() {
            if (MOTD_NonAdmin.bytes == null) {
                MOTD_NonAdmin.Add(packet.types.MOTD);
                MOTD_NonAdmin.Add(ServerSettings.Version);
                MOTD_NonAdmin.Add(ServerSettings.GetSetting("ServerName"), 64);
                MOTD_NonAdmin.Add(ServerSettings.GetSetting("motd"), 64);
                MOTD_NonAdmin.Add((byte)0);
                MOTD_Admin = MOTD_NonAdmin;
                MOTD_Admin.bytes[130] = 100;
            }
        }

        public Socket Socket { get; protected set; }
        public TcpClient Client { get; protected set; }
        protected packet.types lastPacket = packet.types.SendPing;

        /// <summary>
        /// The player's money.
        /// </summary>
        //public int money = 0;

        /// <summary>
        /// Checks if the player is the server owner.
        /// </summary>
        public bool IsOwner { get { return Username == Server.owner; } }
        
        /// <summary>
        /// This is the player's username
        /// </summary>       
        public string Username
        {
            get;
            set;
        }
        
        /// <summary>
        /// This is the UID for the player in the database
        /// </summary>
        internal int UID;
        /// <summary>
        /// This is the player's IP Address
        /// </summary>
        public string Ip;
        /// <summary>
        /// The player's stored message (For appending)
        /// </summary>
        private string _storedMessage = "";

        private byte[] buffer = new byte[0];
        private byte[] tempBuffer = new byte[0xFFF];

        /// <summary>
        /// True if the player is currently loading a map
        /// </summary>
        public bool IsLoading { get; set; }
        /// <summary>
        /// True if the player has completed the login process
        /// </summary>
        public bool IsLoggedIn { get; set; }

        /// <summary>
        /// Get or set if the player is currently being kicked
        /// </summary>
        public bool IsBeingKicked { get; set; }

        private Level _level;
        /// <summary>
        /// This is the players current level
        /// When the value of the level is changed, the user is sent the new map.
        /// </summary>
        public Level Level {
            get {
                if (_level == null)
                    _level = Server.Mainlevel;
                return _level;
            }
            set {
                _level = value;
                SendMap();
            }
        }
        /// <summary>
        /// The players MC Id, this changes each time the player logs in
        /// </summary>
        public byte id { get; set; }
        /// <summary>
        /// The players current position
        /// </summary>
        public Vector3 Pos;
        /// <summary>
        /// The players last known position
        /// </summary>
        public Vector3 oldPos;
        /// <summary>
        /// The players current rotation
        /// </summary>
        public byte[] Rot { get; set; }
        /// <summary>
        /// The players last known rotation
        /// </summary>
        public byte[] oldRot { get; set; }
        /// <summary>
        /// The players last known click
        /// </summary>
        public Vector3 LastClick;
        /// <summary>
        /// Get or set if the player is hidden from other players
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// True if this player is an admin
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Get or set if the player is verified in the AdminPen
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Holds replacement messages for profan filter
        /// </summary>
        public static readonly List<string> replacement = new List<string>();

        /// <summary>
        /// Dictionary for housing extra data, great for giving player objects to pass
        /// </summary>
        public readonly Dictionary<object, object> ExtraData = new Dictionary<object, object>();
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
        private BlockChangeDelegate blockChange;
        private NextChatDelegate nextChat;

        /// <summary>
        /// The current Group of the player
        /// </summary>
        public PlayerGroup group = PlayerGroup.Find(ServerSettings.GetSetting("defaultgroup"));

        private Random playerRandom;


        #endregion

        internal Player(TcpClient TcpClient) {
            CheckMotdPackets();
            try {

                Socket = TcpClient.Client;
                Client = TcpClient;
                Server.Connections.Add(this);
                Ip = Socket.RemoteEndPoint.ToString().Split(':')[0];
                Server.Log("[System]: " + Ip + " connected", ConsoleColor.Gray, ConsoleColor.Black);

                CheckMultipleConnections();
                if (CheckIfBanned()) return;

                Socket.BeginReceive(tempBuffer, 0, tempBuffer.Length, SocketFlags.None, new AsyncCallback(Incoming), this);

                playerRandom = new Random();

            }
            catch (Exception e) {
                SKick("There has been an Error.");
                Server.Log(e);
            }
        }

        #region Special Chat Handlers
        protected void HandleCommand(string[] args) {
            string[] sendArgs = new string[0];
            if (args.Length > 1) {
                sendArgs = new string[args.Length - 1];
                for (int i = 1; i < args.Length; i++) {
                    sendArgs[i - 1] = args[i];
                }
            }

            string name = args[0].ToLower().Trim();
            bool canceled = OnPlayerCommand.Call(this, name, args);
            if (canceled) // If any event canceled us
                return;
            if (Command.Commands.ContainsKey(name)) {
                ThreadPool.QueueUserWorkItem(delegate {
                    ICommand cmd = Command.Commands[name];
                    if (!Server.agreed.Contains(Username) && group.permission < 80 && name != "rules" && name != "agree" && name != "disagree") {
                        SendMessage("You need to /agree to the /rules before you can use commands!"); return;
                    }
                    if (!group.CanExecute(cmd)) {
                        SendMessage(Colors.red + "You cannot use /" + name + "!");
                        return;
                    }
                    try { cmd.Use(this, sendArgs); } //Just so it doesn't crash the server if custom command makers release broken commands!
                    catch (Exception ex) {
                        Server.Log("[Error] An error occured when " + Username + " tried to use " + name + "!", ConsoleColor.Red, ConsoleColor.Black);
                        Server.Log(ex);
                    }
                    if (ExtraData.ContainsKey("LastCmd"))
                        ExtraData["LastCmd"] = name;
                    else
                        ExtraData.Add("LastCmd", name);
                });
            }
            else {
                SendMessage("Unknown command \"" + name + "\"!");
            }

        }
        #endregion


        internal static void GlobalUpdate() {
            ForceTpCounter++;

            Server.ForeachPlayer(delegate(Player p) {
                if (ForceTpCounter == 100) { if (!p.IsHidden) p.UpdatePosition(true); }
                else { if (!p.IsHidden) p.UpdatePosition(false); }
            });
        }
        
        #region Extra Data Saving/Loading
        /// <summary>
        /// Load all the players extra data from the database
        /// </summary>
        public static void LoadAllExtra()
        {
        	Server.Players.ForEach(p =>
        	                       {
        	                       	p.LoadExtra();
        	                       });
        }
        /// <summary>
        /// Load the players extra data from the database
        /// </summary>
        public void LoadExtra()
        {
        	DataTable tbl = Database.fillData("SELECT * WHERE UID=" + UID);
        	for (int i = 0; i < tbl.Rows.Count; i++)
        	{
        		ExtraData.Add(tbl.Rows[i]["key"], tbl.Rows[i]["value"]);
        	}
        	tbl.Dispose();
        }
        /// <summary>
        /// Save all the players extra data
        /// </summary>
        public static void SaveAllExtra()
        {
        	Server.Players.ForEach(p =>
        	                       {
        	                       	p.SaveExtra();
        	                       });
        }
        /// <summary>
        /// Save the players extra data
        /// </summary>
        public void SaveExtra()
        {
        	List<string> commands = new List<string>();
        	foreach (object obj in ExtraData.Keys)
        	{
        		if (!IsInTable(obj))
        			commands.Add("INSERT INTO extra (key, value, UID) VALUES ('" + obj.ToString() + "', '" + ExtraData[obj].ToString() + "', " + UID + ")");
        		else
        			commands.Add("UPDATE extra SET value='" + ExtraData[obj].ToString() + "' WHERE key='" + obj.ToString() + "' AND UID=" + UID);
        	}
        	Database.executeQuery(commands.ToArray());
        	commands.Clear();
        }
        /// <summary>
        /// Check to see if the key is in the table already
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>If true, then they key is in the table and doesnt need to be added, if false, then the key needs to be added</returns>
        internal bool IsInTable(object key)
        {
        	DataTable temp = Database.fillData("SELECT * WHERE key='" + key.ToString() + "' AND UID=" + UID);
        	bool return1 = false;
        	if (temp.Rows.Count >= 1)
        		return1 = true;
        	temp.Dispose();
        	return return1;
        }
        #endregion

        #region PluginStuff
        /// <summary>
        /// This void catches the new blockchange a player does.
        /// </summary>
        /// <param name="change">The BlockChangeDelegate that will be executed on blockchange.</param>
        /// <param name="data">A passback object that can be used for a command to send data back to itself for use</param>
        [Obsolete("Please use OnPlayerBlockChange event (will be removed before release)")]
        public void CatchNextBlockchange(BlockChangeDelegate change, object data) {
            if (!ExtraData.ContainsKey("PassBackData"))
                ExtraData.Add("PassBackData", null);
            ExtraData["PassBackData"] = data;
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
        public void Click(ushort x, ushort z, ushort y, byte type) {
            bool canceled = OnPlayerBlockChange.Call(x, y, z, ActionType.Place, this, type);
            if (canceled) // If any event canceled us
                return;
            if (blockChange != null) {
                bool placing = true;
                BlockChangeDelegate tempBlockChange = blockChange;
                if (!ExtraData.ContainsKey("PassBackData"))
                    ExtraData.Add("PassBackData", null);
                object tempPassBack = ExtraData["PassBackData"];

                blockChange = null;
                ExtraData["PassBackData"] = null;

                ThreadPool.QueueUserWorkItem(delegate { tempBlockChange.Invoke(this, x, z, y, type, placing, tempPassBack); });
                return;
            }
        }
        #endregion

        protected static List<string> LineWrapping(string message) {
            List<string> lines = new List<string>();
            message = Regex.Replace(message, @"(&[0-9a-f])+(&[0-9a-f])", "$2");
            message = Regex.Replace(message, @"(&[0-9a-f])+$", "");
            int limit = 64; string color = "";
            while (message.Length > 0) {
                if (lines.Count > 0) { message = "> " + color + message.Trim(); }
                if (message.Length <= limit) { lines.Add(message); break; }
                for (int i = limit - 1; i > limit - 9; --i) {
                    if (message[i] == ' ') {
                        lines.Add(message.Substring(0, i)); goto Next;
                    }
                }
                lines.Add(message.Substring(0, limit));
            Next: message = message.Substring(lines[lines.Count - 1].Length);
                if (lines.Count == 1) {
                    limit = 60;
                }
                int index = lines[lines.Count - 1].LastIndexOf('&');
                if (index != -1) {
                    if (index < lines[lines.Count - 1].Length - 1) {
                        char next = lines[lines.Count - 1][index + 1];
                        if ("0123456789abcdef".IndexOf(next) != -1) { color = "&" + next; }
                        if (index == lines[lines.Count - 1].Length - 1) {
                            lines[lines.Count - 1] = lines[lines.Count - 1].
                                Substring(0, lines[lines.Count - 1].Length - 2);
                        }
                    }
                    else if (message.Length != 0) {
                        char next = message[0];
                        if ("0123456789abcdef".IndexOf(next) != -1) {
                            color = "&" + next;
                        }
                        lines[lines.Count - 1] = lines[lines.Count - 1].
                            Substring(0, lines[lines.Count - 1].Length - 1);
                        message = message.Substring(1);

                    }
                }
            } return lines;
        }

        protected byte FreeId() {
            List<byte> usedIds = new List<byte>();

            Server.ForeachPlayer(p => usedIds.Add(p.id));

            for (byte i = 0; i < ServerSettings.GetSettingInt("maxplayers"); ++i) {
                if (usedIds.Contains(i)) continue;
                return i;
            }

            Logger.Log("Too many players O_O");
            return 254;
        }
        protected void UpgradeConnectionToPlayer() {
            Server.UpgradeConnectionToPlayer(this);

            //TODO Update form list
        }

        #region Verification Stuffs
        protected void CheckMultipleConnections() {

            //Not a good idea, wom uses 2 connections when getting textures
            if (Server.Connections.Count < 2)
                return;
            foreach (Player p in Server.Connections.ToArray()) {
                if (p.Ip == Ip && p != this) {
                    //p.Kick("Only one half open connection is allowed per IP address.");
                }
            }
        }
        protected static void CheckDuplicatePlayers(string username) {
            Server.ForeachPlayer(delegate(Player p) {
                if (p.Username.ToLower() == username.ToLower()) {
                    p.Kick("You have logged in elsewhere!");
                }
            });
        }
        protected bool CheckIfBanned() {
            if (Server.IPBans.Contains(Ip)) {
                Kick("You're Banned!");
                return true;
            }
            return false;
        }
        protected bool VerifyAccount(string name, string verify) {
            if (!ServerSettings.GetSettingBoolean("offline") && Ip != "127.0.0.1") {
                if (Server.PlayerCount >= ServerSettings.GetSettingInt("maxplayers")) {
                    SKick("Server is full, please try again later!");
                    return false;
                }

                if (verify == null || verify == "" || verify == "--" || (verify != BitConverter.ToString(md5.ComputeHash(enc.GetBytes(ServerSettings.Salt + name))).Replace("-", "").ToLower().TrimStart('0') && verify != BitConverter.ToString(md5.ComputeHash(enc.GetBytes(ServerSettings.Salt + name))).Replace("-", "").ToLower().TrimStart('0'))) {
                    SKick("Account could not be verified, try again.");
                    //Server.Log("'" + verify + "' != '" + BitConverter.ToString(md5.ComputeHash(enc.GetBytes(ServerSettings.salt + name))).Replace("-", "").ToLower().TrimStart('0') + "'");
                    return false;
                }
            }
            if (name.Length > 16 || !ValidName(name)) {
                SKick("Illegal name!");
                return false;
            }

            return true;
        }
        /// <summary>
        /// Check to see is a given name is valid
        /// </summary>
        /// <param name="name">the name to check</param>
        /// <returns>returns true if name is valid</returns>
        public static bool ValidName(string name) {
            const string allowedchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890._";
            foreach (char ch in name) { if (allowedchars.IndexOf(ch) == -1) { return false; } } return true;
        }

        /// <summary>
        /// Attempts to find the player in the list of online players
        /// </summary>
        /// <param name="name">The player name to find</param>
        /// <remarks>Can be a partial name</remarks>
        public static Player Find(string name) {
            foreach (var p in Server.Players)
                if (p.Username.ToLower().StartsWith(name.ToLower()))
                    return p;
            return null;
        }
        #endregion

    }


}
