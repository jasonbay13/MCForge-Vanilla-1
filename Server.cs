/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
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
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Data;
using System.Security.Cryptography;
//using MySql.Data.MySqlClient;
//using MySql.Data.Types;

using MonoTorrent.Client;
using MCForge.SQL;

namespace MCForge
{
    public class Server : IDisposable
    {
        internal static bool cancelcommand/* = false*/;
        internal static bool canceladmin/* = false*/;
        internal static bool cancellog/* = false*/;
        internal static bool canceloplog/* = false*/;
        internal static string apppath = Application.StartupPath;
        /// <summary>
        /// This is called when the console uses a command
        /// </summary>
        /// <param name="cmd">The command</param>
        /// <param name="message">The message</param>
        public delegate void OnConsoleCommand(string cmd, string message);
        [Obsolete("Please use OnConsoleCommandEvent.Register()")]
        public static event OnConsoleCommand ConsoleCommand;
        /// <summary>
        /// This is called when the server errors
        /// </summary>
        /// <param name="error">The error</param>
        public delegate void OnServerError(Exception error);
        [Obsolete("Please use OnServerErrorEvent.Register()")]
        public static event OnServerError ServerError = null;
        /// <summary>
        /// This is called when the server logs something to the console
        /// </summary>
        /// <param name="message">The string that was logged</param>
        public delegate void OnServerLog(string message, LogType type);
        [Obsolete("Please use OnServerLogEvent.Register()")]
        public static event OnServerLog ServerLog;
        [Obsolete("Please use OnServerLogEvent.Register()")]
        public static event OnServerLog ServerAdminLog;
        [Obsolete("Please use OnServerLogEvent.Register()")]
        public static event OnServerLog ServerOpLog;
        //TODO:
        //change these to work with current event system
        //INTERNAL FOR GUI/CLI ==============================================
        internal delegate void HeartBeatHandler();
        internal delegate void MessageEventHandler(string message);
        internal delegate void PlayerListHandler(List<Player> playerList);
        internal delegate void VoidHandler();
        internal delegate void LogHandler(string message);
        internal event LogHandler OnLog;
        internal event LogHandler OnSystem;
        internal event LogHandler OnCommand;
        internal event LogHandler OnError;
        internal event LogHandler OnOp;
        internal event LogHandler OnAdmin;
        internal event HeartBeatHandler HeartBeatFail;
        internal event MessageEventHandler OnURLChange;
        internal event PlayerListHandler OnPlayerListChange;
        internal event VoidHandler OnSettingsUpdate;
        //INTERNAL FOR GUI/CLI ==============================================
        public static ForgeBot IRC;
        public static GlobalChatBot GlobalChat;
        public static Thread locationChecker;
        /// <summary>
        /// Is the server using WoM textures?
        /// </summary>
        public static bool UseTextures/* = false*/;
        internal static Thread blockThread;
        //public static List<MySql.Data.MySqlClient.MySqlCommand> mySQLCommands = new List<MySql.Data.MySqlClient.MySqlCommand>();

        //public static int speedPhysics = 250; derp

        /// <summary>
        /// The current server version
        /// </summary>
        public static Version Version { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; } }

        // URL hash for connecting to the server
        /// <summary>
        /// The server hash for connecting to the server
        /// </summary>
        public static string Hash = String.Empty;
        /// <summary>
        /// The server URL for connecting to the server
        /// </summary>
        public static string URL = String.Empty;
        
        /// <summary>
        /// The listening socket for listening to incoming connections
        /// </summary>
        public static Socket listen;
        internal static System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
        /// <summary>
        /// The update checker timer
        /// </summary>
        public static System.Timers.Timer updateTimer = new System.Timers.Timer(100);
        //static System.Timers.Timer heartbeatTimer = new System.Timers.Timer(60000); //Every 45 seconds
        /// <summary>
        /// The server message timer
        /// </summary>
        public static System.Timers.Timer messageTimer = new System.Timers.Timer(60000 * 5); //Every 5 mins
        //public static System.Timers.Timer cloneTimer = new System.Timers.Timer(5000);

        //public static Thread physThread;
        //public static bool physPause;
        //public static DateTime physResume = DateTime.Now;
        //public static System.Timers.Timer physTimer = new System.Timers.Timer(1000);
        // static Thread botsThread;
        //Chatrooms
        public static List<string> Chatrooms = new List<string>();
        //Other
        /// <summary>
        /// Can people TP to higher ranks
        /// </summary>
        public static bool higherranktp = true;
        /// <summary>
        /// Do guest have to agree to rules when they first join
        /// </summary>
        public static bool agreetorulesonentry/* = false*/;
        /// <summary>
        /// Is the server using CTF
        /// </summary>
        public static bool UseCTF/* = false*/;
        /// <summary>
        /// Is the server done setting up
        /// </summary>
        public static bool ServerSetupFinished/* = false*/;
        /// <summary>
        /// The server CTF object
        /// </summary>
        public static Auto_CTF ctf = null;
        /// <summary>
        /// All banned IP's
        /// </summary>
        public static PlayerList bannedIP;
        /// <summary>
        /// Players on the WhiteList
        /// </summary>
        public static PlayerList whiteList;
        /// <summary>
        /// 
        /// </summary>
        public static PlayerList ircControllers;
        /// <summary>
        /// All players who are muted
        /// </summary>
        public static PlayerList muted;
        /// <summary>
        /// All players who are ignored by globalchat
        /// </summary>
        public static PlayerList ignored;
        // The MCForge Developer List
        internal static readonly List<string> devs = new List<string>(new string[] { "dmitchell94", "501st_commander", "edh649", "shade2010", "hypereddie10", "erickilla", "fredlllll", "soccer101nic", "headdetect", "merlin33069", "jasonbay13", "cazzar", "snowl", "techjar", "nerketur", "anthonyani", "wouto1997", "lavoaster", "bemacized", "meinigeshandwerk" });
        /// <summary>
        /// A list of MCForge devs
        /// </summary>
        public static List<string> Devs { get { return new List<string>(devs); } }

        /// <summary>
        /// A list of temp bans (Players who are banned temporally)
        /// </summary>
        public static List<TempBan> tempBans = new List<TempBan>();
        /// <summary>
        /// Temp Ban object
        /// </summary>
        public struct TempBan { 
            /// <summary>
            /// The player name who is banned
            /// </summary>
            public string name; 
            /// <summary>
            /// When this player can join back in
            /// </summary>
            public DateTime allowedJoin; 
        }

        internal static MapGenerator MapGen;

        internal static PerformanceCounter PCCounter = null;
        internal static PerformanceCounter ProcessCounter = null;

        /// <summary>
        /// The server's main level
        /// </summary>
        public static Level mainLevel;
        /// <summary>
        /// A list of levels that are loaded on the server
        /// </summary>
        public static List<Level> levels;
        //reviewlist intitialize
        /// <summary>
        /// A list of players the OP's of the server has to review <seealso cref="CmdReview"/>
        /// </summary>
        public static List<string> reviewlist = new List<string>();
        //Translate settings initialize
        /// <summary>
        /// Will the server translate messages
        /// </summary>
        public static bool transenabled/* = false*/;
        /// <summary>
        /// The default server lang.
        /// </summary>
        public static string translang = "en";
        /// <summary>
        /// List of playernames who will not be translated
        /// </summary>
        public static List<string> transignore = new List<string>();
        //Global Chat Rules Accepted list
        /// <summary>
        /// A list of playernames who agreed to the Global Chat Rules
        /// </summary>
        public static List<string> gcaccepted = new List<string>();
        //public static List<levelID> allLevels = new List<levelID>();
        //public struct levelID { public int ID; public string name; }

        /// <summary>
        /// A list of player names who are AFK
        /// </summary>
        public static List<string> afkset = new List<string>();
        /// <summary>
        /// A list of nick's who are AFK on IRC
        /// </summary>
        public static List<string> ircafkset = new List<string>();

        /// <summary>
        /// A list of AFK messages (This is currently not used)
        /// </summary>
        public static List<string> afkmessages = new List<string>();

        /// <summary>
        /// A list of messages the server will make every 5 minutes <seealso cref="Server.messageTimer"/>
        /// </summary>
        public static List<string> messages = new List<string>();

        /// <summary>
        /// The Date and Time when the server went online
        /// </summary>
        public static DateTime timeOnline;
        /// <summary>
        /// The server's IP
        /// </summary>
        public static string IP;
        //auto updater stuff
        /// <summary>
        /// Will the server autoupdate when an update is found
        /// </summary>
        public static bool autoupdate;
        /// <summary>
        /// Will the server auto notify when an update is found
        /// </summary>
        public static bool autonotify;
        /// <summary>
        /// Will the server notify players when an update is found
        /// </summary>
        public static bool notifyPlayers;
        internal static string restartcountdown = "";
        internal static string selectedrevision = "";
        /// <summary>
        /// Will the server auto restart when the server errors
        /// </summary>
        public static bool autorestart;
        /// <summary>
        /// The time the server will restart
        /// </summary>
        public static DateTime restarttime;

        /// <summary>
        /// Is the server using /moderate <seealso cref="CmdModerate"/>
        /// </summary>
        public static bool chatmod/* = false*/;

        //Global VoteKick In Progress Flag
        /// <summary>
        /// Is there a votekick in progress? <seealso cref="Server.voteKickVotesNeeded"/>
        /// </summary>
        public static bool voteKickInProgress/* = false*/;
        /// <summary>
        /// How many votes are needed <seealso cref="Server.voteKickInProgress"/>
        /// </summary>
        public static int voteKickVotesNeeded/* = 0*/;


        //WoM Direct
        /// <summary>
        /// The server Alternate name for WoM Direct
        /// </summary>
        public static string Server_ALT = "";
        /// <summary>
        /// The server discription for WoM Direct
        /// </summary>
        public static string Server_Disc = "";
        /// <summary>
        /// The server flag for WoM Direct
        /// </summary>
        public static string Server_Flag = "";


        internal static Dictionary<string, string> customdollars = new Dictionary<string, string>();

        // Extra storage for custom commands
        /// <summary>
        /// Extra storage for custom commands
        /// Information that is put in here will be kept by the server
        /// </summary>
        public ExtrasCollection Extras = new ExtrasCollection();

        //Color list as a char array
        internal static Char[] ColourCodesNoPercent = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        /// <summary>
        /// Is there a vote currently going on
        /// This can also be used to start a vote
        /// </summary>
        public static bool voting;
        /// <summary>
        /// How many yes votes are there <seealso cref="Server.voting"/>
        /// </summary>
        public static int YesVotes;
        /// <summary>
        /// How many no votes are there <seealso cref=" Server.voting"/>
        /// </summary>
        public static int NoVotes;
        /// <summary>
        /// Will the server buffer blocks before sending them
        /// </summary>
        public static bool bufferblocks = true;
        /// <summary>
        /// The server's MCForge username
        /// </summary>
        public static string mcforgeUser = "";
        internal static string mcforgePass = ""; //We dont want this public >_>

        //Zombie
        public static int ZombieMode;
        public static bool ZombieRound;
        /// <summary>
        /// Server's Zombie Survival gamemode
        /// </summary>
        public static ZombieSurvival zombie;
        //Zombie

        // Lava Survival
        /// <summary>
        /// Server's Lava Survival gamemode
        /// </summary>
        public static LavaSurvival lava;

        // OmniBan
        /// <summary>
        /// MCForge OmniBan system
        /// </summary>
        public static OmniBan omniban;
        /// <summary>
        /// The OmniBan updater <seealso cref="OmniBan"/>
        /// </summary>
        public static System.Timers.Timer omnibanCheckTimer = new System.Timers.Timer(60000 * 120);

        //Settings
        #region Server Settings
        /// <summary>
        /// Minecraft Server Protocall version
        /// </summary>
        public const byte version = 7;
        /// <summary>
        /// The server salt
        /// </summary>
        public static string salt = "";

        /// <summary>
        /// The server name
        /// </summary>
        public static string name = "[MCForge] Default";
        /// <summary>
        /// The server motd
        /// </summary>
        public static string motd = "Welcome!";
        /// <summary>
        /// The max amount of players that can join the server
        /// </summary>
        public static byte players = 12;
        //for the limiting no. of guests:
        /// <summary>
        /// The max amount of guests that can join the server
        /// </summary>
        public static byte maxGuests = 10;

        /// <summary>
        /// The max amount of maps that can be loaded
        /// </summary>
        public static byte maps = 5;
        /// <summary>
        /// The port the server is using for the listening socket <seealso cref="Server.listen"/>
        /// </summary>
        public static int port = 25565;
        /// <summary>
        /// Is the server public
        /// </summary>
        public static bool pub = true;
        /// <summary>
        /// Will the server verify the usernames that join the server
        /// </summary>
        public static bool verify = true;
        /// <summary>
        /// Can players talk to other players on different levels
        /// </summary>
        public static bool worldChat = true;
//        public static bool guestGoto = false;

        //Spam Prevention
        /// <summary>
        /// Will the server check for chat spam
        /// </summary>
        public static bool checkspam/* = false*/;
        /// <summary>
        /// Amount of messages that count as spamming
        /// </summary>
        public static int spamcounter = 8;
        /// <summary>
        /// How long the player will be muted
        /// </summary>
        public static int mutespamtime = 60;
        /// <summary>
        /// How long should the server wait before resetting the player spam count
        /// </summary>
        public static int spamcountreset = 5;

        /// <summary>
        /// The console status
        /// </summary>
        public static string ZallState = "Alive";

        //public static string[] userMOTD;

        /// <summary>
        /// The main level <seealso cref="Server.mainLevel"/>
        /// </summary>
        public static string level = "main";
        public static string errlog = "error.log";

//        public static bool console = false; // never used
        public static bool reportBack = true;

        /// <summary>
        /// Will the server use IRC
        /// </summary>
        public static bool irc/* = false*/;
        /// <summary>
        /// Will the server use IRC Colors
        /// </summary>
        public static bool ircColorsEnable/* = false*/;
//        public static bool safemode = false; //Never used
        /// <summary>
        /// The port of the IRC Server
        /// </summary>
        public static int ircPort = 6667;
        /// <summary>
        /// The server's nick
        /// </summary>
        public static string ircNick = "ForgeBot";
        /// <summary>
        /// The IRC server
        /// </summary>
        public static string ircServer = "irc.esper.net";
        /// <summary>
        /// The IRC channel (main)
        /// </summary>
        public static string ircChannel = "#changethis";
        /// <summary>
        /// The IRC channel (OP)
        /// </summary>
        public static string ircOpChannel = "#changethistoo";
        /// <summary>
        /// Will the server login to NickServ
        /// </summary>
        public static bool ircIdentify/* = false*/;
        /// <summary>
        /// The server's NickServ password
        /// </summary>
        public static string ircPassword = "";
        /// <summary>
        /// Do admins have to login to the server
        /// </summary>
        public static bool verifyadmins = true;
        public static LevelPermission verifyadminsrank = LevelPermission.Operator;

        /// <summary>
        /// Will the server restart on error
        /// </summary>
        public static bool restartOnError = true;
        /// <summary>
        /// Prevent tunneling
        /// </summary>
        public static bool antiTunnel = true;
        /// <summary>
        /// The max depth guests can dig to
        /// </summary>
        public static byte maxDepth = 4;
        public static int Overload = 1500;
        public static int rpLimit = 500;
        public static int rpNormLimit = 10000;

        public static int backupInterval = 300;
        public static int blockInterval = 60;
        /// <summary>
        /// Level backup location
        /// </summary>
        public static string backupLocation = Application.StartupPath + "/levels/backups";


        public static bool physicsRestart = true;
        /// <summary>
        /// Have the server keep track of how many times a player dies?
        /// </summary>
        public static bool deathcount = true;
        /// <summary>
        /// Autoload a level if a player tries to join and the level is unloaded?
        /// </summary>
        public static bool AutoLoad/* = false*/;
        /// <summary>
        /// Max physics undo <seealso cref="CmdUndo"/>
        /// </summary>
        public static int physUndo = 20000;
        /// <summary>
        /// Max undo <seealso cref="CmdUndo"/>
        /// </summary>
        public static int totalUndo = 200;
        public static bool rankSuper = true;
        /// <summary>
        /// Use the old help from MCSharp
        /// </summary>
        public static bool oldHelp/* = false*/;
        public static bool parseSmiley = true;
        /// <summary>
        /// Use a whitelist
        /// </summary>
        public static bool useWhitelist/* = false*/;
        /// <summary>
        /// Only players who bought minecraft can join the server
        /// </summary>
        public static bool PremiumPlayersOnly/* = false*/;
        public static bool forceCuboid/* = false*/;
        /// <summary>
        /// Add a profanity filter?
        /// </summary>
        public static bool profanityFilter/* = false*/;
        /// <summary>
        /// Have the server announce a player has left or joined the server
        /// </summary>
        public static bool notifyOnJoinLeave/* = false*/;
        public static bool repeatMessage/* = false*/;
        public static bool globalignoreops/* = false*/;

        /// <summary>
        /// Have the server check for updates?
        /// </summary>
        public static bool checkUpdates = true;

        /// <summary>
        /// Use mysql (if set to false sqlite will be used)
        /// </summary>
        public static bool useMySQL/* = false*/;
        /// <summary>
        /// The MySQL hostname
        /// </summary>
        public static string MySQLHost = "127.0.0.1";
        /// <summary>
        /// The MySQL port
        /// </summary>
        public static string MySQLPort = "3306";
        /// <summary>
        /// MySQL username
        /// </summary>
        public static string MySQLUsername = "root";
        /// <summary>
        /// The MySQL password
        /// </summary>
        public static string MySQLPassword = "password";
        /// <summary>
        /// The MySQL database name where the data will be stored
        /// </summary>
        public static string MySQLDatabaseName = "MCZallDB";
        public static bool DatabasePooling = true;

        /// <summary>
        /// The default chat color for when a server makes an announcement
        /// </summary>
        public static string DefaultColor = "&e";
        /// <summary>
        /// The default IRC Color
        /// </summary>
        public static string IRCColour = "&5";

        /// <summary>
        /// Use globalchat
        /// GlobalChat allows you to talk to other MCForge servers!
        /// </summary>
        public static bool UseGlobalChat = true;
        /// <summary>
        /// The server's GlobalChat username
        /// </summary>
        public static string GlobalChatNick = "MCF" + new Random().Next();
        /// <summary>
        /// The server's GlobalChat chat color
        /// </summary>
        public static string GlobalChatColor = "&6";

        /// <summary>
        /// The number of minutes a player has to be not moving to be afk
        /// </summary>
        public static int afkminutes = 10;
        /// <summary>
        /// The number of minutes a player has to be afk to be kicked by the server
        /// </summary>
        public static int afkkick = 45;
        public static LevelPermission afkkickperm = LevelPermission.AdvBuilder;
        //public static int RemotePort = 1337; // Never used

        public static string defaultRank = "guest";

        public static bool dollardollardollar = true;
        public static bool unsafe_plugin = true;
        public static bool cheapMessage = true;
        public static string cheapMessageGiven = " is now being cheap and being immortal";
        /// <summary>
        /// Use custom ban message
        /// </summary>
        public static bool customBan/* = false*/;
        /// <summary>
        /// The kick message players get when they join a server that they are banned from
        /// </summary>
        public static string customBanMessage = "You're banned!";
        /// <summary>
        /// Use custom shutdown message
        /// </summary>
        public static bool customShutdown/* = false*/;
        /// <summary>
        /// The kick message players get when the server is about to shutdown
        /// </summary>
        public static string customShutdownMessage = "Server shutdown. Rejoin in 10 seconds.";
        public static bool customGrieferStone/* = false*/;
        /// <summary>
        /// The message players get when they are caught griefing
        /// </summary>
        public static string customGrieferStoneMessage = "Oh noes! You were caught griefing!";
        /// <summary>
        /// The message players get when they get promoted
        /// </summary>
        public static string customPromoteMessage = "&6Congratulations for working hard and getting &2PROMOTED!";
        /// <summary>
        /// The message players get when they get demoted
        /// </summary>
        public static string customDemoteMessage = "&4DEMOTED! &6We're sorry for your loss. Good luck on your future endeavors! &1:'(";
        /// <summary>
        /// What is the server currency
        /// </summary>
        public static string moneys = "moneys";
        /// <summary>
        /// What ranks can see opchat
        /// </summary>
        public static LevelPermission opchatperm = LevelPermission.Operator;
        /// <summary>
        /// What ranks can see adminchat
        /// </summary>
        public static LevelPermission adminchatperm = LevelPermission.Admin;
        /// <summary>
        /// Log the heartbeart (Useful for debugging)
        /// </summary>
        public static bool logbeat/* = false*/;
        /// <summary>
        /// Will the server not notify if an admin joins the server
        /// </summary>
        public static bool adminsjoinsilent/* = false*/;
        /// <summary>
        /// Is the server using mono (This can't be set)
        /// </summary>
        public static bool mono { get { return (Type.GetType("Mono.Runtime") != null); } }
        /// <summary>
        /// The server owner username
        /// </summary>
        public static string server_owner = "Notch";
        /// <summary>
        /// Will the server pump to Wom Direct heartbeat
        /// </summary>
        public static bool WomDirect/* = false*/;
        //public static bool UseSeasons/* = false*/;
        public static bool guestLimitNotify/* = false*/;
        /// <summary>
        /// Will the server announce if a guest joins the server
        /// </summary>
        public static bool guestJoinNotify = true;
        /// <summary>
        /// Will the server announce if a guest leaves the server
        /// </summary>
        public static bool guestLeaveNotify = true;

        /// <summary>
        /// Is the server flipping everyones head?
        /// </summary>
        public static bool flipHead/* = false*/;

        /// <summary>
        /// Is the server shutting down
        /// </summary>
        public static bool shuttingDown/* = false*/;
        /// <summary>
        /// Is the server restarting
        /// </summary>
        public static bool restarting/* = false*/;

        //hackrank stuff
        /// <summary>
        /// Kick the player if they use /hackrank
        /// </summary>
        public static bool hackrank_kick = true;
        /// <summary>
        /// How many seconds it should wait before kicking the player when using hackrank
        /// </summary>
        public static int hackrank_kick_time = 5; //seconds, it converts it to milliseconds in the command.

        // lol useless junk here lolololasdf poop
        public static bool showEmptyRanks/* = false*/;
        public static byte grieferStoneType = 1;
        public static bool grieferStoneBan = true;
        public static LevelPermission grieferStoneRank = LevelPermission.Guest;

        //reviewoptions intitialize
        public static int reviewcooldown = 600;
        public static LevelPermission reviewenter = LevelPermission.Guest;
        public static LevelPermission reviewleave = LevelPermission.Guest;
        public static LevelPermission reviewview = LevelPermission.Operator;
        public static LevelPermission reviewnext = LevelPermission.Operator;
        public static LevelPermission reviewclear = LevelPermission.Operator;

        #endregion

        internal static MainLoop ml;
        /// <summary>
        /// The server
        /// </summary>
        public static Server s;
        public Server()
        {
            ml = new MainLoop("server");
            Server.s = this;
        }
        //True = cancel event
        //Fale = dont cacnel event
        internal static bool Check(string cmd, string message)
        {
            if (ConsoleCommand != null)
                ConsoleCommand(cmd, message);
            return cancelcommand;
        }
        public void Start()
        {
           
            shuttingDown = false;
            Log("Starting Server");
            {
                try
                {
                    if (File.Exists("Restarter.exe"))
                    {
                        File.Delete("Restarter.exe");
                    }
                }
                catch { }
                try
                {
                    if (File.Exists("Restarter.pdb"))
                    {
                        File.Delete("Restarter.pdb");
                    }
                }
                catch { }
                //dl restarter stuff [Restarter is no longer needed]
                //if (!File.Exists("Restarter.exe"))
                //{
                //    Log("Restarter.exe doesn't exist, Downloading");
                //    try
                //    {
                //        using (WebClient WEB = new WebClient())
                //        {
                //            WEB.DownloadFile("http://mcforge.net/uploads/Restarter.exe", "Restarter.exe");
                //        }
                //        if (File.Exists("Restarter.exe"))
                //        {
                //            Log("Restarter.exe download succesful!");
                //        }
                //    }
                //    catch
                //    {
                //        Log("Downloading Restarter.exe failed, please try again later");
                //    }
                //}
                //if (!File.Exists("Restarter.pdb"))
                //{
                //    Log("Restarter.pdb doesn't exist, Downloading");
                //    try
                //    {
                //        using (WebClient WEB = new WebClient())
                //        {
                //            WEB.DownloadFile("http://mcforge.net/uploads/Restarter.pdb", "Restarter.pdb");
                //        }
                //        if (File.Exists("Restarter.pdb"))
                //        {
                //            Log("Restarter.pdb download succesful!");
                //        }
                //    }
                //    catch
                //    {
                //        Log("Downloading Restarter.pdb failed, please try again later");
                //    }
                //}
                if (!File.Exists("MySql.Data.dll"))
                {
                    Log("MySql.Data.dll doesn't exist, Downloading");
                    try
                    {
                        using (WebClient WEB = new WebClient())
                        {
                            WEB.DownloadFile("http://mcforge.net/uploads/MySql.Data.dll", "MySql.Data.dll");
                        }
                        if (File.Exists("MySql.Data.dll"))
                        {
                            Log("MySql.Data.dll download succesful!");
                        }
                    }
                    catch
                    {
                        Log("Downloading MySql.Data.dll failed, please try again later");
                    }
                }
                if (!File.Exists("System.Data.SQLite.dll"))
                {
                    Log("System.Data.SQLite.dll doesn't exist, Downloading");
                    try
                    {
                        using (WebClient WEB = new WebClient())
                        {
                            WEB.DownloadFile("http://mcforge.net/uploads/System.Data.SQLite.dll", "System.Data.SQLite.dll");
                        }
                        if (File.Exists("System.Data.SQLite.dll"))
                        {
                            Log("System.Data.SQLite.dll download succesful!");
                        }
                    }
                    catch
                    {
                        Log("Downloading System.Data.SQLite.dll failed, please try again later");
                    }
                }
                if (!File.Exists("sqlite3.dll"))
                {
                    Log("sqlite3.dll doesn't exist, Downloading");
                    try
                    {
                        using (WebClient WEB = new WebClient())
                        {
                            WEB.DownloadFile("http://www.mcforge.net/sqlite3.dll", "sqlite3.dll");
                        }
                        if (File.Exists("sqlite3.dll"))
                        {
                            Log("sqlite3.dll download succesful!");
                        }
                    }
                    catch
                    {
                        Log("Downloading sqlite3.dll failed, please try again later");
                    }
                }
                if (!File.Exists("Sharkbite.Thresher.dll"))
                {
                    Log("Sharkbite.Thresher.dll doesn't exist, Downloading");
                    try
                    {
                        using (WebClient WEB = new WebClient())
                        {
                            //WEB.DownloadFile("http://www.mediafire.com/?4rkpqvcji3va8rp", "Sharkbite.Thresher.dll");
                            WEB.DownloadFile("http://mcforge.net/uploads/Sharkbite.Thresher.dll", "Sharkbite.Thresher.dll");
                        }
                        if (File.Exists("Sharkbite.Thresher.dll"))
                        {
                            Log("Sharkbite.Thresher.dll download succesful!");
                        }
                    }
                    catch
                    {
                        Log("Downloading Sharkbite.Thresher.dll failed, please try again later");
                    }
                }
            }
            if (!Directory.Exists("properties")) Directory.CreateDirectory("properties");
            if (!Directory.Exists("levels")) Directory.CreateDirectory("levels");
            if (!Directory.Exists("bots")) Directory.CreateDirectory("bots");
            if (!Directory.Exists("text")) Directory.CreateDirectory("text");
            if (!File.Exists("text/tempranks.txt")) File.CreateText("text/tempranks.txt").Dispose();
            if (!File.Exists("text/rankinfo.txt")) File.CreateText("text/rankinfo.txt").Dispose();
            if (!File.Exists("text/transexceptions.txt")) File.CreateText("text/transexceptions.txt").Dispose();
            if (!File.Exists("text/gcaccepted.txt")) File.CreateText("text/gcaccepted.txt").Dispose();
            if (!File.Exists("text/bans.txt")) File.CreateText("text/bans.txt").Dispose();
            // DO NOT STICK ANYTHING IN BETWEEN HERE!!!!!!!!!!!!!!!
            else
            {
                string bantext = File.ReadAllText("text/bans.txt");
                if (!bantext.Contains("%20") && bantext != "")
                {
                    bantext = bantext.Replace("~", "%20");
                    bantext = bantext.Replace("-", "%20");
                    File.WriteAllText("text/bans.txt", bantext);
                }
            }



            if (!Directory.Exists("extra")) Directory.CreateDirectory("extra");
            if (!Directory.Exists("extra/undo")) Directory.CreateDirectory("extra/undo");
            if (!Directory.Exists("extra/undoPrevious")) Directory.CreateDirectory("extra/undoPrevious");
            if (!Directory.Exists("extra/copy/")) { Directory.CreateDirectory("extra/copy/"); }
            if (!Directory.Exists("extra/copyBackup/")) { Directory.CreateDirectory("extra/copyBackup/"); }
            if (!Directory.Exists("extra/Waypoints")) { Directory.CreateDirectory("extra/Waypoints"); }

            try
            {
                if (File.Exists("server.properties")) File.Move("server.properties", "properties/server.properties");
                if (File.Exists("rules.txt")) File.Move("rules.txt", "text/rules.txt");
                if (File.Exists("welcome.txt")) File.Move("welcome.txt", "text/welcome.txt");
                if (File.Exists("messages.txt")) File.Move("messages.txt", "text/messages.txt");
                if (File.Exists("externalurl.txt")) File.Move("externalurl.txt", "text/externalurl.txt");
                if (File.Exists("autoload.txt")) File.Move("autoload.txt", "text/autoload.txt");
                if (File.Exists("IRC_Controllers.txt")) File.Move("IRC_Controllers.txt", "ranks/IRC_Controllers.txt");
                if (Server.useWhitelist) if (File.Exists("whitelist.txt")) File.Move("whitelist.txt", "ranks/whitelist.txt");
            }
            catch { }

            if (File.Exists("text/custom$s.txt"))
            {
                using (StreamReader r = new StreamReader("text/custom$s.txt"))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        if (!line.StartsWith("//"))
                        {
                            var split = line.Split(new char[] { ':' }, 2);
                            if (split.Length == 2 && !String.IsNullOrEmpty(split[0]))
                            {
                                customdollars.Add(split[0], split[1]);
                            }
                        }
                    }
                }
            }
            else
            {
                s.Log("custom$s.txt does not exist, creating");
                using (StreamWriter SW = File.CreateText("text/custom$s.txt"))
                {
                    SW.WriteLine("// This is used to create custom $s");
                    SW.WriteLine("// If you start the line with a // it wont be used");
                    SW.WriteLine("// It should be formatted like this:");
                    SW.WriteLine("// $website:mcforge.net");
                    SW.WriteLine("// That would replace '$website' in any message to 'mcforge.net'");
                    SW.WriteLine("// It must not start with a // and it must not have a space between the 2 sides and the colon (:)");
                    SW.Close();
                }
            }

            LoadAllSettings();

            if (File.Exists("text/emotelist.txt"))
            {
                foreach (string s in File.ReadAllLines("text/emotelist.txt"))
                {
                    Player.emoteList.Add(s);
                }
            }
            else
            {
                File.Create("text/emotelist.txt").Dispose();
            }


            // LavaSurvival constructed here...
            lava = new LavaSurvival();

            zombie = new ZombieSurvival();

            // OmniBan
            omniban = new OmniBan();

            timeOnline = DateTime.Now;
            {//MYSQL stuff
                try
                {
                    Database.executeQuery("CREATE DATABASE if not exists `" + MySQLDatabaseName + "`", true); // works in both now, SQLite simply ignores this.
                }
                //catch (MySql.Data.MySqlClient.MySqlException e)
                //{
                //    Server.s.Log("MySQL settings have not been set! Many features will not be available if MySQL is not enabled");
                //  //  Server.ErrorLog(e);
                //}
                catch (Exception e)
                {
                    Server.ErrorLog(e);
                    Server.s.Log("MySQL settings have not been set! Please Setup using the properties window.");
                    //process.Kill();
                    return;
                }
                Database.executeQuery("CREATE TABLE if not exists Players (ID INTEGER " + (Server.useMySQL ? "" : "PRIMARY KEY ") + "AUTO" + (Server.useMySQL ? "_" : "") + "INCREMENT NOT NULL, Name VARCHAR(20), IP CHAR(15), FirstLogin DATETIME, LastLogin DATETIME, totalLogin MEDIUMINT, Title CHAR(20), TotalDeaths SMALLINT, Money MEDIUMINT UNSIGNED, totalBlocks BIGINT, totalCuboided BIGINT, totalKicked MEDIUMINT, TimeSpent VARCHAR(20), color VARCHAR(6), title_color VARCHAR(6)" + (Server.useMySQL ? ", PRIMARY KEY (ID)" : "") + ");");
				Database.executeQuery("CREATE TABLE if not exists Playercmds (ID INTEGER " + (Server.useMySQL ? "" : "PRIMARY KEY ") + "AUTO" + (Server.useMySQL ? "_" : "") + "INCREMENT NOT NULL, Time DATETIME, Name VARCHAR(20), Rank VARCHAR(20), Mapname VARCHAR(40), Cmd VARCHAR(40), Cmdmsg VARCHAR(40)" + (Server.useMySQL ? ", PRIMARY KEY (ID)" : "") + ");");

                // Here, since SQLite is a NEW thing from 5.3.0.0, we do not have to check for existing tables in SQLite.
                if (Server.useMySQL) {
                    // Check if the color column exists.
                    DataTable colorExists = MySQL.fillData("SHOW COLUMNS FROM Players WHERE `Field`='color'");

                    if (colorExists.Rows.Count == 0)
                    {
                        MySQL.executeQuery("ALTER TABLE Players ADD COLUMN color VARCHAR(6) AFTER totalKicked");
                        //else SQLite.executeQuery("ALTER TABLE Players ADD COLUMN color VARCHAR(6) AFTER totalKicked");
                    }
                    colorExists.Dispose();

                    // Check if the title color column exists.
                    DataTable tcolorExists = MySQL.fillData("SHOW COLUMNS FROM Players WHERE `Field`='title_color'");

                    if (tcolorExists.Rows.Count == 0)
                    {
                        MySQL.executeQuery("ALTER TABLE Players ADD COLUMN title_color VARCHAR(6) AFTER color");
                        // else SQLite.executeQuery("ALTER TABLE Players ADD COLUMN title_color VARCHAR(6) AFTER color");
                    }
                    tcolorExists.Dispose();

                    DataTable timespent = MySQL.fillData("SHOW COLUMNS FROM Players WHERE `Field`='TimeSpent'");

                    if (timespent.Rows.Count == 0)
                        MySQL.executeQuery("ALTER TABLE Players ADD COLUMN TimeSpent VARCHAR(20) AFTER totalKicked"); //else SQLite.executeQuery("ALTER TABLE Players ADD COLUMN TimeSpent VARCHAR(20) AFTER totalKicked");
                    timespent.Dispose();

                    DataTable totalCuboided = MySQL.fillData("SHOW COLUMNS FROM Players WHERE `Field`='totalCuboided'");

                    if (totalCuboided.Rows.Count == 0)
                        MySQL.executeQuery("ALTER TABLE Players ADD COLUMN totalCuboided BIGINT AFTER totalBlocks"); //else SQLite.executeQuery("ALTER TABLE Players ADD COLUMN totalCuboided BIGINT AFTER totalBlocks");
                    totalCuboided.Dispose();
                }
            }

            if (levels != null)
                foreach (Level l in levels) { l.Unload(); }
            ml.Queue(delegate
            {
                try
                {
                    levels = new List<Level>(Server.maps);
                    MapGen = new MapGenerator();

                    if (File.Exists("levels/" + Server.level + ".lvl"))
                    {
                        mainLevel = Level.Load(Server.level);
                        mainLevel.unload = false;
                        if (mainLevel == null)
                        {
                            if (File.Exists("levels/" + Server.level + ".lvl.backup"))
                            {
                                Log("Attempting to load backup of " + Server.level + ".");
                                File.Copy("levels/" + Server.level + ".lvl.backup", "levels/" + Server.level + ".lvl", true);
                                mainLevel = Level.Load(Server.level);
                                if (mainLevel == null)
                                {
                                    Log("BACKUP FAILED!");
                                    Console.ReadLine(); return;
                                }
                            }
                            else
                            {
                                Log("mainlevel not found");
                                mainLevel = new Level(Server.level, 128, 64, 128, "flat") { permissionvisit = LevelPermission.Guest, permissionbuild = LevelPermission.Guest };
                                mainLevel.Save();
                                Level.CreateLeveldb(Server.level);
                            }
                        }
                        //Wom Textures
                        /*if (Server.UseTextures)
                        {
                            mainLevel.textures.sendwomid = true;
                            mainLevel.textures.MOTD = Server.motd;
                            mainLevel.textures.CreateCFG();
                        }*/
                    }
                    else
                    {
                        Log("mainlevel not found");
                        mainLevel = new Level(Server.level, 128, 64, 128, "flat") { permissionvisit = LevelPermission.Guest, permissionbuild = LevelPermission.Guest };
                        mainLevel.Save();
                        Level.CreateLeveldb(Server.level);
                    }

                    addLevel(mainLevel);

                    // fenderrock - Make sure the level does have a physics thread
                    if (mainLevel.physThread == null)
                        mainLevel.StartPhysics();
                }
                catch (Exception e) { Server.ErrorLog(e); }
            });
            Plugin.Load();
            ml.Queue(delegate
            {
                bannedIP = PlayerList.Load("banned-ip.txt", null);
                ircControllers = PlayerList.Load("IRC_Controllers.txt", null);
                muted = PlayerList.Load("muted.txt", null);

                foreach (Group grp in Group.GroupList)
                    grp.playerList = PlayerList.Load(grp.fileName, grp);
                if (Server.useWhitelist)
                    whiteList = PlayerList.Load("whitelist.txt", null);
            });

            ml.Queue(delegate
            {
                transignore.AddRange(File.ReadAllLines("text/transexceptions.txt"));
                if (File.Exists("text/autoload.txt"))
                {
                    try
                    {
                        string[] lines = File.ReadAllLines("text/autoload.txt");
                        foreach (string line in lines)
                        {
                            //int temp = 0;
                            string _line = line.Trim();
                            try
                            {
                                if (_line == "") { continue; }
                                if (_line[0] == '#') { continue; }
                                int index = _line.IndexOf("=");

                                string key = _line.Split('=')[0].Trim();
                                string value;
                                try
                                {
                                    value = _line.Split('=')[1].Trim();
                                }
                                catch
                                {
                                    value = "0";
                                }

                                if (!key.Equals(mainLevel.name))
                                {
                                    Command.all.Find("load").Use(null, key + " " + value);
                                    Level l = Level.FindExact(key);
                                    //Not needed, as load does it for us.
                                    //try
                                    //{
                                    //        Gui.Window.thisWindow.UpdateMapList("'");
                                    //        Gui.Window.thisWindow.UnloadedlistUpdate();
                                    //}
                                    //catch { }
                                }
                                else
                                {
                                    try
                                    {
                                        int temp = int.Parse(value);
                                        if (temp >= 0 && temp <= 3)
                                        {
                                            mainLevel.setPhysics(temp);
                                        }
                                    }
                                    catch
                                    {
                                        Server.s.Log("Physics variable invalid");
                                    }
                                }


                            }
                            catch
                            {
                                Server.s.Log(_line + " failed.");
                            }
                        }
                    }
                    catch
                    {
                        Server.s.Log("autoload.txt error");
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                else
                {
                    Log("autoload.txt does not exist");
                }
            });

            ml.Queue(delegate
            {
                Translate.Init();
                foreach (string line in File.ReadAllLines("text/transexceptions.txt"))
                {
                    transignore.Add(line); //loading all playernames of people who turned off translation
                }
                foreach (string line in File.ReadAllLines("text/gcaccepted.txt"))
                {
                    gcaccepted.Add(line); //loading all playernames of people who turned off translation
                }
                Log("Creating listening socket on port " + Server.port + "... ");
                if (Setup())
                {
                    s.Log("Done.");
                }
                else
                {
                    s.Log("Could not create socket connection. Shutting down.");
                    return;
                }
            });
            ml.Queue(delegate
            {
                Remote.RemoteServer webServer;
                Remote.RemoteProperties.Load();
                (webServer = new Remote.RemoteServer()).Start();
            });
            
            ml.Queue(delegate
            {
                updateTimer.Elapsed += delegate
                {
                    Player.GlobalUpdate();
                    PlayerBot.GlobalUpdatePosition();
                };

                updateTimer.Start();
            });


            // Heartbeat code here:

            ml.Queue(delegate
            {
                try
                {
                    Heart.Init();
                }
                catch (Exception e)
                {
                    Server.ErrorLog(e);
                }
            });

            ml.Queue(delegate
            {
                if (mcforgeUser != String.Empty && mcforgePass != String.Empty)
                {
                    new Thread(new ThreadStart(delegate { MCForgeAccount.Login(); })).Start();
                }
            });

            // END Heartbeat code

            /*
Thread processThread = new Thread(new ThreadStart(delegate
{
try
{
PCCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
ProcessCounter = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
PCCounter.BeginInit();
ProcessCounter.BeginInit();
PCCounter.NextValue();
ProcessCounter.NextValue();
}
catch { }
}));
processThread.Start();
*/

            ml.Queue(delegate
            {
                messageTimer.Elapsed += delegate
                {
                    RandomMessage();
                };
                messageTimer.Start();

                process = System.Diagnostics.Process.GetCurrentProcess();

                if (File.Exists("text/messages.txt"))
                {
                    using (StreamReader r = File.OpenText("text/messages.txt"))
                    {
                        while (!r.EndOfStream)
                            messages.Add(r.ReadLine());
                    }
                }
                else File.Create("text/messages.txt").Close();

                // We always construct this to prevent errors...
                IRC = new ForgeBot(Server.ircChannel, Server.ircOpChannel, Server.ircNick, Server.ircServer);
                GlobalChat = new GlobalChatBot(GlobalChatNick);
                
                if (Server.irc) IRC.Connect();
                if (Server.UseGlobalChat) GlobalChat.Connect();

                // OmniBan stuff!
                new Thread(new ThreadStart(delegate
                {
                    omniban.Load(true);
                })).Start();

                omnibanCheckTimer.Elapsed += delegate
                {
                    omniban.Load(true);
                    omniban.KickAll();
                };
                omnibanCheckTimer.Start();


                // string CheckName = "FROSTEDBUTTS";

                // if (Server.name.IndexOf(CheckName.ToLower())!= -1){ Server.s.Log("FROSTEDBUTTS DETECTED");}
                new AutoSaver(Server.backupInterval); //2 and a half mins

                blockThread = new Thread(new ThreadStart(delegate
                {
                    while (true)
                    {
                        Thread.Sleep(blockInterval * 1000);
                        levels.ForEach(delegate(Level l)
                        {
                            try
                            {
                                l.saveChanges();
                            }
                            catch (Exception e)
                            {
                                Server.ErrorLog(e);
                            }
                        });
                    }
                }));
                blockThread.Start();

                locationChecker = new Thread(new ThreadStart(delegate
                {
                    Player p, who;
                    ushort x, y, z;
                    int i;
                    while (true)
                    {
                        Thread.Sleep(3);
                        for (i = 0; i < Player.players.Count; i++)
                        {
                            try
                            {
                                p = Player.players[i];

                                if (p.frozen)
                                {
                                    unchecked { p.SendPos((byte)-1, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1]); } continue;
                                }
                                else if (p.following != "")
                                {
                                    who = Player.Find(p.following);
                                    if (who == null || who.level != p.level)
                                    {
                                        p.following = "";
                                        if (!p.canBuild)
                                        {
                                            p.canBuild = true;
                                        }
                                        if (who != null && who.possess == p.name)
                                        {
                                            who.possess = "";
                                        }
                                        continue;
                                    }
                                    if (p.canBuild)
                                    {
                                        unchecked { p.SendPos((byte)-1, who.pos[0], (ushort)(who.pos[1] - 16), who.pos[2], who.rot[0], who.rot[1]); }
                                    }
                                    else
                                    {
                                        unchecked { p.SendPos((byte)-1, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1]); }
                                    }
                                }
                                else if (p.possess != "")
                                {
                                    who = Player.Find(p.possess);
                                    if (who == null || who.level != p.level)
                                        p.possess = "";
                                }

                                x = (ushort)(p.pos[0] / 32);
                                y = (ushort)(p.pos[1] / 32);
                                z = (ushort)(p.pos[2] / 32);

                                if (p.level.Death)
                                    p.RealDeath(x, y, z);
                                p.CheckBlock(x, y, z);

                                p.oldBlock = (ushort)(x + y + z);
                            }
                            catch (Exception e) { Server.ErrorLog(e); }
                        }
                    }
                }));

                locationChecker.Start();
                try
                {
                    using (WebClient web = new WebClient())
                        IP = web.DownloadString("http://www.mcforge.net/serverdata/ip.php");
                }
                catch { }
                try
                {
                    Gui.Window.thisWindow.UpdateMapList("'");
                    Thread.Sleep(100);
                    Gui.Window.thisWindow.UnloadedlistUpdate();
                }
                catch { }
#if DEBUG
      UseTextures = true;          
#endif
                Log("Finished setting up server");
                ServerSetupFinished = true;
                Checktimer.StartTimer();
                Commands.CommandKeywords.SetKeyWords();
                try
                {
                    if (Server.lava.startOnStartup)
                        Server.lava.Start();
                    if (Server.zombie.StartOnStartup)
                        //Server.zombie.Start(0);
                    //This doesnt use the main map
                    if (Server.UseCTF)
                        ctf = new Auto_CTF();
                }
                catch (Exception e) { Server.ErrorLog(e); }
                BlockQueue.Start();
            });
        }

        public static void LoadAllSettings() {
            SrvProperties.Load("properties/server.properties");
            Updater.Load("properties/update.properties");
            Group.InitAll();
            Command.InitAll();
            GrpCommands.fillRanks();
            Block.SetBlocks();
            Awards.Load();
            Economy.Load();
            Warp.LOAD();
            CommandOtherPerms.Load();
            ProfanityFilter.Init();
        }
        
        public static bool Setup()
        {
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, Server.port);
                listen = new Socket(endpoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listen.Bind(endpoint);
                listen.Listen((int)SocketOptionName.MaxConnections);

                listen.BeginAccept(new AsyncCallback(Accept), null);
                return true;
            }
            catch (SocketException e) { ErrorLog(e); return false; }
            catch (Exception e) { ErrorLog(e); return false; }
        }

        static void Accept(IAsyncResult result)
        {
            if (!shuttingDown)
            {
                // found information: http://www.codeguru.com/csharp/csharp/cs_network/sockets/article.php/c7695
                // -Descention
                Player p = null;
                bool begin = false;
                try
                {
					p = new Player(listen.EndAccept(result));
                    listen.BeginAccept(new AsyncCallback(Accept), null);
                    begin = true;
                }
                catch (SocketException)
                {
                    if (p != null)
                        p.Disconnect();
                    if (!begin)
                        listen.BeginAccept(new AsyncCallback(Accept), null);
                }
                catch (Exception e)
                {
                    ErrorLog(e);
                    if (p != null)
                        p.Disconnect();
                    if (!begin)
                        listen.BeginAccept(new AsyncCallback(Accept), null);
                }
            }
        }

        public static void Exit(bool AutoRestart)
        {
            List<string> players = new List<string>();
            foreach (Player p in Player.players) { p.save(); players.Add(p.name); }
            foreach (string p in players)
            {
                if (!AutoRestart)
                    Player.Find(p).Kick(Server.customShutdown ? Server.customShutdownMessage : "Server shutdown. Rejoin in 10 seconds.");
                else
                    Player.Find(p).Kick("Server restarted! Rejoin!");
            }

            //Player.players.ForEach(delegate(Player p) { p.Kick("Server shutdown. Rejoin in 10 seconds."); });
            Player.connections.ForEach(
            delegate(Player p)
            {
                if (!AutoRestart)
                    p.Kick(Server.customShutdown ? Server.customShutdownMessage : "Server shutdown. Rejoin in 10 seconds.");
                else
                    p.Kick("Server restarted! Rejoin!");
            }
            );
            Plugin.Unload();
            if (listen != null)
            {
                listen.Close();
            }
            try
            {
                Remote.RemoteServer.enableRemote = false;
                Remote.RemoteServer.Close();
            }
            catch { }
            try
            {
                if (GlobalChat.IsConnected())
                {
                    if (!AutoRestart)
                        GlobalChat.Disconnect("Server is shutting down.");
                    else
                        GlobalChat.Disconnect("Server is restarting.");
                }
            }
            catch { }
            try
            {
                if (IRC.IsConnected())
                {
                    if (!AutoRestart)
                        IRC.Disconnect("Server is shutting down.");
                    else
                        IRC.Disconnect("Server is restarting.");
                }
            }
            catch { }
        }

        public static void addLevel(Level level)
        {
            levels.Add(level);
        }

        public void PlayerListUpdate()
        {
            if (Server.s.OnPlayerListChange != null) Server.s.OnPlayerListChange(Player.players);
        }

        public void FailBeat()
        {
            if (HeartBeatFail != null) HeartBeatFail();
        }

        public void UpdateUrl(string url)
        {
            if (OnURLChange != null) OnURLChange(url);
        }

        public void Log(string message, bool systemMsg = false)
        {
            if (ServerLog != null)
            {
                OnServerLogEvent.Call(message, LogType.Normal);
                ServerLog(message, LogType.Normal);
                if (cancellog)
                {
                    cancellog = false;
                    return;
                }
            }
            if (OnLog != null)
            {
                if (!systemMsg)
                {
                    OnLog(DateTime.Now.ToString("(HH:mm:ss) ") + message);
                }
                else
                {
                    OnSystem(DateTime.Now.ToString("(HH:mm:ss) ") + message);
                }
            }
            
            Logger.Write(DateTime.Now.ToString("(HH:mm:ss) ") + message + Environment.NewLine);
        }

        public void OpLog(string message, bool systemMsg = false)
        {
            if (ServerOpLog != null)
            {
                OnServerLogEvent.Call(message, LogType.OpLog);
                ServerOpLog(message, LogType.OpLog);
                if (canceloplog)
                {
                    canceloplog = false;
                    return;
                }
            }
            if (OnOp != null)
            {
                if (!systemMsg)
                {
                    OnOp(DateTime.Now.ToString("(HH:mm:ss) ") + message);
                }
                else
                {
                    OnSystem(DateTime.Now.ToString("(HH:mm:ss) ") + message);
                }
            }

            Logger.Write(DateTime.Now.ToString("(HH:mm:ss) ") + message + Environment.NewLine);
        }

        public void AdminLog(string message, bool systemMsg = false)
        {
            if (ServerAdminLog != null)
            {
                OnServerLogEvent.Call(message, LogType.AdminLog);
                ServerAdminLog(message, LogType.AdminLog);
                if (canceladmin)
                {
                    canceladmin = false;
                    return;
                }
            }
            if (OnAdmin != null)
            {
                if (!systemMsg)
                {
                    OnAdmin(DateTime.Now.ToString("(HH:mm:ss) ") + message);
                }
                else
                {
                    OnSystem(DateTime.Now.ToString("(HH:mm:ss) ") + message);
                }
            }

            Logger.Write(DateTime.Now.ToString("(HH:mm:ss) ") + message + Environment.NewLine);
        }

        public void ErrorCase(string message)
        {
            if (OnError != null)
                OnError(message);
        }

        public void CommandUsed(string message)
        {
            if (OnCommand != null) OnCommand(DateTime.Now.ToString("(HH:mm:ss) ") + message);
            Logger.Write(DateTime.Now.ToString("(HH:mm:ss) ") + message + Environment.NewLine);
        }

        public static void ErrorLog(Exception ex)
        {
			if (ServerError != null)
				ServerError(ex);
            Logger.WriteError(ex);
            try
            {
                s.Log("!!!Error! See " + Logger.ErrorLogPath + " for more information.");
            } catch { }
        }

        public static void RandomMessage()
        {
            if (Player.number != 0 && messages.Count > 0)
                Player.GlobalMessage(messages[new Random().Next(0, messages.Count)]);
        }

        internal void SettingsUpdate()
        {
            if (OnSettingsUpdate != null) OnSettingsUpdate();
        }

        public static string FindColor(string Username)
        {
            foreach (Group grp in Group.GroupList)
            {
                if (grp.playerList.Contains(Username)) return grp.color;
            }
            return Group.standard.color;
        }

        public static void PopupNotify(string message)
        {
            PopupNotify(message, System.Windows.Forms.ToolTipIcon.Info);
        }
        public static void PopupNotify(string message, System.Windows.Forms.ToolTipIcon icon)
        {
            try
            {
                Gui.Window.thisWindow.notifyIcon1.ShowBalloonTip(3000, Server.name, message, icon);
            }
            catch { }
        }

        #region IDisposable Implementation

        protected bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                // Do nothing if the object has already been disposed of.
                if (disposed)
                    return;

                if (disposing)
                {
                    // Release diposable objects used by this instance here.

                    if (process != null)
                        process.Dispose();
                    if (updateTimer != null)
                        updateTimer.Dispose();
                    if (messageTimer != null)
                        messageTimer.Dispose();
                    //if (cloneTimer != null)
                    //    cloneTimer.Dispose();
                    if (ctf != null)
                        ctf.Dispose();
                    if (PCCounter != null)
                        PCCounter.Dispose();
                    if (ProcessCounter != null)
                        ProcessCounter.Dispose();
                    if (mainLevel != null)
                        mainLevel.Dispose();
                    if (Extras != null)
                        Extras.Dispose();
                    if (lava != null)
                        lava.Dispose();
                    if (omnibanCheckTimer != null)
                        omnibanCheckTimer.Dispose();
                    if (s != null)
                        s.Dispose();
                }

                // Release unmanaged resources here. Don't access reference type fields.

                // Remember that the object has been disposed of.
                disposed = true;
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            // Unregister object for finalization.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}