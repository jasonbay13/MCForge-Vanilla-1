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
using System.Linq;
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
using MCForge_.SQL;

namespace MCForge_
{
	
	
    public class Server
    {
    	public static Server s;
    	
        public static bool cancelcommand = false;
        public static bool canceladmin = false;
        public static bool cancellog = false;
        public static bool canceloplog = false;
        public static string apppath = Application.StartupPath;
        public static Thread locationChecker;
        public static bool UseTextures = false;
        public static Thread blockThread;
        //public static List<MySql.Data.MySqlClient.MySqlCommand> mySQLCommands = new List<MySql.Data.MySqlClient.MySqlCommand>();

        public static int speedPhysics = 250;

        public static Version Version { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; } }

        // URL hash for connecting to the server
        public static string Hash = String.Empty;
        public static string URL = String.Empty;

        public static Socket listen;
        public static System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
        public static System.Timers.Timer updateTimer = new System.Timers.Timer(100);
        //static System.Timers.Timer heartbeatTimer = new System.Timers.Timer(60000); //Every 45 seconds
        static System.Timers.Timer messageTimer = new System.Timers.Timer(60000 * 5); //Every 5 mins
        public static System.Timers.Timer cloneTimer = new System.Timers.Timer(5000);

        //public static Thread physThread;
        //public static bool physPause;
        //public static DateTime physResume = DateTime.Now;
        //public static System.Timers.Timer physTimer = new System.Timers.Timer(1000);
        // static Thread botsThread;
        //Chatrooms
        public static List<string> Chatrooms = new List<string>();
        //Other
        public static bool higherranktp = true;
        public static bool agreetorulesonentry = false;
        public static bool UseCTF = false;
        public static bool ServerSetupFinished = false;
        // The MCForge Developer List
        internal static readonly List<string> devs = new List<string>(new string[] { "dmitchell", "shade2010", "erickilla", "fredlllll", "jasonbay13", "cazzar", "snowl", "techjar", "nerketur", "lavoaster", "meinigeshandwerk", "hirsty1989" });
        public static List<string> Devs { get { return new List<string>(devs); } }

        public static List<TempBan> tempBans = new List<TempBan>();
        public struct TempBan { public string name; public DateTime allowedJoin; }

        public static PerformanceCounter PCCounter = null;
        public static PerformanceCounter ProcessCounter = null;

        //reviewlist intitialize
        public static List<string> reviewlist = new List<string>();
        //Translate settings initialize
        public static bool transenabled = false;
        public static string translang = "en";
        public static List<string> transignore = new List<string>();
        //Global Chat Rules Accepted list
        public static List<string> gcaccepted = new List<string>();
        //public static List<levelID> allLevels = new List<levelID>();
        public struct levelID { public int ID; public string name; }

        public static List<string> afkset = new List<string>();
        public static List<string> ircafkset = new List<string>();
        public static List<string> afkmessages = new List<string>();
        public static List<string> messages = new List<string>();

        public static List<string> gcmods = new List<string>();
        public static List<string> gcmodprotection = new List<string>();
        public static List<string> gcnamebans = new List<string>();
        public static List<string> gcipbans = new List<string>();

        public static DateTime timeOnline;
        public static string IP;
        //auto updater stuff
        public static bool autoupdate;
        public static bool autonotify;
        public static bool notifyPlayers;
        public static string restartcountdown = "";
        public static string selectedrevision = "";
        public static bool autorestart;
        public static DateTime restarttime;

        public static bool chatmod = false;

        //Global VoteKick In Progress Flag
        public static bool voteKickInProgress = false;
        public static int voteKickVotesNeeded = 0;


        //WoM Direct
        public static string Server_ALT = "";
        public static string Server_Disc = "";
        public static string Server_Flag = "";

        public static Dictionary<string, string> customdollars = new Dictionary<string, string>();


        //Color list as a char array
        public static Char[] ColourCodesNoPercent = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        public static bool ZombieModeOn = false;
        public static bool startZombieModeOnStartup = false;
        public static bool noRespawn = true;
        public static bool noLevelSaving = true;
        public static bool noPillaring = true;
        public static string ZombieName = "";
        public static int gameStatus = 0; //0 = not started, 1 = always on, 2 = one time, 3 = certain amount of rounds, 4 = stop game next round
        public static bool queLevel = false;
        public static bool queZombie = false;
        public static string nextZombie = "";
        public static string nextLevel = "";
        public static bool zombieRound = false;
        public static string lastPlayerToInfect = "";
        public static int infectCombo = 0;
        public static int YesVotes = 0;
        public static int NoVotes = 0;
        public static bool voting = false;
        public static bool votingforlevel = false;
        public static int Level1Vote = 0;
        public static int Level2Vote = 0;
        public static int Level3Vote = 0;
        public static bool ChangeLevels = true;
        public static bool UseLevelList = false;
        public static bool ZombieOnlyServer = true;
        public static List<String> LevelList = new List<String>();
        public static string lastLevelVote1 = "";
        public static string lastLevelVote2 = "";
        public static bool bufferblocks = true;
        public static string mcforgeUser = "";
        public static string mcforgePass = "";

        public static System.Timers.Timer omnibanCheckTimer = new System.Timers.Timer(60000 * 120);

        //Settings
        #region Server Settings
        public const byte version = 7;
        public static string salt = "";

        public static string name = "[MCForge] Default";
        public static string motd = "Welcome!";
        public static byte players = 12;
        //for the limiting no. of guests:
        public static byte maxGuests = 10;

        public static byte maps = 5;
        public static int port = 25565;
        public static bool pub = true;
        public static bool verify = true;
        public static bool worldChat = true;
        //        public static bool guestGoto = false;

        //Spam Prevention
        public static bool checkspam = false;
        public static int spamcounter = 8;
        public static int mutespamtime = 60;
        public static int spamcountreset = 5;

        public static string ZallState = "Alive";

        //public static string[] userMOTD;

        public static string level = "main";
        public static string errlog = "error.log";

        //        public static bool console = false; // never used
        public static bool reportBack = true;

        public static bool irc = false;
        public static bool ircColorsEnable = false;
        //        public static bool safemode = false; //Never used
        public static int ircPort = 6667;
        public static string ircNick = "ForgeBot";
        public static string ircServer = "irc.esper.net";
        public static string ircChannel = "#changethis";
        public static string ircOpChannel = "#changethistoo";
        public static bool ircIdentify = false;
        public static string ircPassword = "";
        public static bool verifyadmins = true;

        public static bool restartOnError = true;

        public static bool antiTunnel = true;
        public static byte maxDepth = 4;
        public static int Overload = 1500;
        public static int rpLimit = 500;
        public static int rpNormLimit = 10000;

        public static int backupInterval = 300;
        public static int blockInterval = 60;
        public static string backupLocation = Application.StartupPath + "/levels/backups";

        public static bool physicsRestart = true;
        public static bool deathcount = true;
        public static bool AutoLoad = false;
        public static int physUndo = 20000;
        public static int totalUndo = 200;
        public static bool rankSuper = true;
        public static bool oldHelp = false;
        public static bool parseSmiley = true;
        public static bool useWhitelist = false;
        public static bool PremiumPlayersOnly = false;
        public static bool forceCuboid = false;
        public static bool profanityFilter = false;
        public static bool notifyOnJoinLeave = false;
        public static bool repeatMessage = false;
        public static bool globalignoreops = false;

        public static bool checkUpdates = true;

        public static bool useMySQL = false;
        public static string MySQLHost = "127.0.0.1";
        public static string MySQLPort = "3306";
        public static string MySQLUsername = "root";
        public static string MySQLPassword = "password";
        public static string MySQLDatabaseName = "MCZallDB";
        public static bool DatabasePooling = true;

        public static string DefaultColor = "&e";
        public static string IRCColour = "&5";

        public static bool UseGlobalChat = true;
        public static string GlobalChatNick = "MCF" + new Random().Next();
        public static string GlobalChatColor = "&6";

        public static int afkminutes = 10;
        public static int afkkick = 45;

        public static string defaultRank = "guest";

        public static bool dollardollardollar = true;
        public static bool unsafe_plugin = true;
        public static bool cheapMessage = true;
        public static string cheapMessageGiven = " is now being cheap and being immortal";
        public static bool customBan = false;
        public static string customBanMessage = "You're banned!";
        public static bool customShutdown = false;
        public static string customShutdownMessage = "Server shutdown. Rejoin in 10 seconds.";
        public static bool customGrieferStone = false;
        public static string customGrieferStoneMessage = "Oh noes! You were caught griefing!";
        public static string customPromoteMessage = "&6Congratulations for working hard and getting &2PROMOTED!";
        public static string customDemoteMessage = "&4DEMOTED! &6We're sorry for your loss. Good luck on your future endeavors! &1:'(";
        public static string moneys = "moneys";
        public static bool logbeat = false;
        public static bool adminsjoinsilent = false;
        public static bool mono { get { return (Type.GetType("Mono.Runtime") != null); } }
        public static string server_owner = "Notch";
        public static bool WomDirect = false;
        public static bool UseSeasons = false;
        public static bool guestLimitNotify = false;
        public static bool guestJoinNotify = true;
        public static bool guestLeaveNotify = true;

        public static bool flipHead = false;

        public static bool shuttingDown = false;
        public static bool restarting = false;

        //hackrank stuff
        public static bool hackrank_kick = true;
        public static int hackrank_kick_time = 5; //seconds, it converts it to milliseconds in the command.

        // lol useless junk here lolololasdf poop
        public static bool showEmptyRanks = false;
        public static byte grieferStoneType = 1;
        public static bool grieferStoneBan = true;

        #endregion
        
        public Server() { }
        
        //To many errors...
        //Dont kill me ._.
        public void Log(string message) {
        	Console.WriteLine(message);
        }
        
        public static void ErrorLog(Exception e) {
        	Console.WriteLine(e.ToString());
        }

        public static void LoadAllSettings()
        {
            SrvProperties.Load("properties/server.properties");
            /*Updater.Load("properties/update.properties");
            Group.InitAll();
            Command.InitAll();
            GrpCommands.fillRanks();
            Block.SetBlocks();
            Awards.Load();
            Economy.Load();
            Warp.LOAD();
            CommandOtherPerms.Load();
            ProfanityFilter.Init();*/
        }
    }
}