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
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using MCForge.Core.HeartService;
using MCForge.Entity;
using MCForge.Interface;
using MCForge.Interface.Command;
using MCForge.Robot;
using MCForge.SQL;
using MCForge.Utils;
using MCForge.Utils.Settings;
using MCForge.World;
using MCForge.World.Physics;

namespace MCForge.Core {
    public static class Server {

        public static Assembly ServerAssembly { get { return Assembly.GetAssembly(typeof(Server)); } }

        public static bool Running {get; set;}

        /// <summary>
        /// The name of the server currency.
        /// </summary>
        public static string Moneys {
            get { return ServerSettings.GetSetting("MoneyName"); }
            set { ServerSettings.SetSetting("MoneyName", value); }
        }
        /// <summary>
        /// The miniumum rank that needs to verify.
        /// </summary>
        public static Groups.PlayerGroup VerifyGroup;
        /// <summary>
        /// Do people need to use /pass?
        /// </summary>
        public static bool Verifying {
            get { return ServerSettings.GetSettingBoolean("Verifying"); }
            set { ServerSettings.SetSetting("Verifying", value.ToString().ToLower()); }
        }
        /// <summary>
        /// The name of the server owner.
        /// </summary>
        public static string Owner {
            get { return ServerSettings.GetSetting("ServerOwner"); }
            set { ServerSettings.SetSetting("ServerOwner", value); }
        }
        /// <summary>
        /// If the server is using mono
        /// </summary>
        public static bool OnMono { get { return (Type.GetType("Mono.Runtime") != null); } }
        /// <summary>
        /// The rank that can destroy griefer_stone without getting kicked
        /// </summary>
        public static byte GrieferStonePerm = 80;
        /// <summary>
        /// The amount of griefer_stone warns player will recieve before getting kicked
        /// </summary>
        public static int GrieferStoneWarns = 3;
        /// <summary>
        /// Get whether the server is currently shutting down
        /// </summary>
        public static bool ShuttingDown;
        /// <summary>
        /// Get whether the server is currently fully started or not
        /// </summary>
        public static bool Started { get; set; }

        private static System.Threading.Timer UpdateTimer;
        private static int GroupsaveInterval = 3000;
        private static int GroupsaveIntervalCurrent = 0;
        private static int PingInterval = 10;
        private static int PingIntervalCurrent = 0;
        private static int BotInterval = 25;
        private static int BotIntervalCurrent = 0;
        public static bool DebugMode { get; set; }

        //Defaq is this?
        public struct TempBan { public string name; public DateTime allowed; }

        /// <summary>
        /// List of players who are temporarly banned
        /// </summary>
        public static List<TempBan> TempBansList = new List<TempBan>();

        /// <summary>
        /// Start time of the server
        /// </summary>
        public readonly static DateTime StartTime = DateTime.Now;

        internal static List<Player> Connections = new List<Player>();

        /// <summary>
        /// Get the current list of online players, note that if you're doing a foreach on this always add .ToArray() to the end, it solves a LOT of issues
        /// </summary>
        public readonly static List<Player> Players = new List<Player>();

        /// <summary>
        /// Gets the player count.
        /// </summary>
        public static int PlayerCount { get { return Players.Count; } }

        /// <summary>
        /// Get the current list of bots, note that if you're doing a foreach on this always add .ToArray() to the end, it solves a LOT of issues
        /// </summary>
        public static List<Bot> Bots = new List<Bot>();

        /// <summary>
        /// Gets the bot count.
        /// </summary>
        public static int BotCount { get { return Bots.Count; } }

        /// <summary>
        /// The current list of banned IP addresses. Note that if you do a foreach on this (or any other public list) you should always add .ToArray() to the end to avoid errors!
        /// </summary>
        public static List<string> IPBans;
        /// <summary>
        /// The list of banned usernames. Note that if you do a foreach on this (or any other public list) you should always add .ToArray() to the end to avoid errors!
        /// </summary>
        public static List<string> UsernameBans;
        /// <summary>
        /// The list of MCForge developers.
        /// </summary>
        public static readonly string[] Devs = new[] { "EricKilla", "Merlin33069", "Snowl", "gamezgalaxy", "headdetect", "Gamemakergm", "cazzar", "givo", "jasonbay13", "Alem_Zupa", "7imekeeper", "ninedrafted", "Nerketur", "Serado", "501st_Commander" };
        /// <summary>
        /// List of VIPs
        /// </summary>
        public readonly static List<string> VIPs = new List<string>();
        /// <summary>
        /// List of players that need to be reviewed
        /// </summary>
        public readonly static List<Player> ReviewList = new List<Player>();
        /// <summary>
        /// List of players that agreed to the rules
        /// </summary>
        public readonly static List<string> AgreedPlayers = new List<string>();

        /// <summary>
        /// A list of words to replace
        /// </summary>
        public readonly static List<string> ReplacementWordsList = new List<string>();

        /// <summary>
        /// A list of bad words
        /// </summary>
        public readonly static List<string> BadWordsList = new List<string>();

        /// <summary>
        /// A list of joker messages
        /// </summary>
        public static List<string> JokerMessages = new List<string>();
        /// <summary>
        /// The main level of the server, where players spawn when they first join
        /// </summary>
        public static Level Mainlevel { get; set; }
        /// <summary>
        /// Determines if the chat moderation is enabled
        /// </summary>
        public static bool Moderation { get; set; }
        //Voting
        /// <value>
        ///   <c>true</c> if voting mode is active; otherwise, <c>false</c>.
        /// </value>
        public static bool Voting { get; set; }
        /// <summary>
        /// Is it a kickvote?
        /// </summary>
        public static bool KickVote { get; set; }
        /// <summary>
        /// Amount of yes votes.
        /// </summary>
        public static int YesVotes { get; set; }
        /// <summary>
        /// Amount of no votes.
        /// </summary>
        public static int NoVotes { get; set; }
        /// <summary>
        /// The player who's getting, if it's /votekick
        /// </summary>
        //RAAAAAGE
        public static Player Kicker { get; set; }
        /// <summary>
        /// The minecraft.net URL of the server
        /// </summary>
        /// 
        public static string URL { get; set; }
        /// <summary>
        /// Internet utilies
        /// </summary>
        public static InetUtils InternetUtils { get; set; }
        /// <summary>
        /// The IRC client for the server
        /// </summary>
        public static IRC IRC { get; set; }
        /// <summary>
        /// The default color
        /// </summary>
        public static string DefaultColor {
            get {
                var colo = ServerSettings.GetSetting("DefaultColor");
                if (!ColorUtils.IsValidMinecraftColorCode(colo)) {
                    Logger.Log("Color code \"" + colo + "\" is not a valid color code", LogType.Error);
                    colo = "&a";
                }
                return colo;
            }
            set {
                var colo = value;
                if (!ColorUtils.IsValidMinecraftColorCode(colo)) {
                    Logger.Log("Color code \"" + colo + "\" is not a valid color code", LogType.Error);
                    colo = "&a";
                }
                ServerSettings.SetSetting("DefaultColor", null, colo);
            }
        }

        /// <summary>
        /// This delegate is used when a command or plugin needs to call a method after a certain amount of time
        /// </summary>
        /// <param name="dataPass">This delegate passes the object that was passed to it back to the method that is to be invoked</param>
        /// <returns>this delegate returns an updated object for the datapass</returns>
        public delegate object TimedMethodDelegate(object dataPass);
        static List<TimedMethod> TimedMethodList = new List<TimedMethod>();

        /// <summary>
        ///  A silly little iterator for looping over the players in the server
        /// </summary>
        /// <param name="p">The player.</param>
        public delegate void ForeachPlayerDelegate(Player p);

        /// <summary>
        ///  A silly little iterator for looping over the bots in the server
        /// </summary>
        /// <param name="p">The bot.</param>
        public delegate void ForeachBotDelegate(Bot p);

        /// <summary>
        ///  When the server finishes setting up
        /// </summary>
        public delegate void ServerFinishSetup();

        /// <summary>
        /// Occurs when [on server finish setup].
        /// </summary>
        public static event ServerFinishSetup OnServerFinishSetup;

        /// <summary>
        /// Inits this instance.
        /// </summary>
        public static void Init() {

            Running = true;

            Logger.WriteLog("--------- Server Started at " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " ---------");
            Logger.Log("Debug mode started", LogType.Debug);
            //TODO Add debug messages
            //TODO load the level if it exists
            Block.InIt();
            Logger.Log("Starting update timer", LogType.Debug);
            UpdateTimer = new System.Threading.Timer((e) => Update(), null, 0, 100);
            Logger.Log("Log timer started", LogType.Debug);
            Logger.Log("Loading DLL's", LogType.Debug);
            LoadAllDlls.Init();
            Logger.Log("Finished loading DLL's", LogType.Debug);
            Logger.Log("Sending Heartbeat..", LogType.Debug);
            Logger.Log("Auto-Updater Starting..", LogType.Debug);
            Updater.InIt();
            HeartThread = new Thread(new ThreadStart(Heartbeat.ActivateHeartBeat));
            HeartThread.Start();
            Logger.Log("Starting Physics Tick..", LogType.Debug);
            PhysicsBlock.InIt();
            CmdReloadCmds reload = new CmdReloadCmds();
            reload.Initialize();

            Groups.PlayerGroup.Load();
            VerifyGroup = Groups.PlayerGroup.Find(ServerSettings.GetSetting("VerifyGroup"));

            Mainlevel = Level.LoadLevel(ServerSettings.GetSetting("Main-Level"));
            if (Mainlevel == null) {
                Mainlevel = Level.CreateLevel(new Vector3S(256, 128, 64), Level.LevelTypes.Flat);
                ServerSettings.SetSetting("Main-Level", null, "main");
            }
            Level.Levels.Add(Mainlevel);
            if (ServerSettings.GetSettingBoolean("LoadAllLevels"))
                Level.LoadAllLevels();

            Backup.StartBackup();

            Database.init();

            CreateCoreFiles();

            InternetUtils = new InetUtils();
            InetUtils.InternetAvailable = InetUtils.CanConnectToInternet();

            Logger.Log("Loading Bans", LogType.Debug);
            Logger.Log("IPBANS", LogType.Debug);
            IPBans = new List<string>(File.ReadAllLines("bans/IPBans.txt"));
            Logger.Log("IPBANS", LogType.Debug);
            UsernameBans = new List<string>(File.ReadAllLines("bans/NameBans.txt"));
            StartListening();
            Started = true;

            if (OnServerFinishSetup != null)
                OnServerFinishSetup();

            Logger.Log("[Important]: Server Started.", Color.Black, Color.White);
            if (!ServerSettings.GetSettingBoolean("VerifyNames"))
                Logger.Log("[Important]: The server is running with verify names off! This could lead to bad things! Please turn on verify names if you dont know the risk and dont want these bad things to happen!", LogType.Critical);
            IRC = new IRC();
            try {
                IRC.Start();
            }
            catch { }
            PlayerConnectionTimeoutChecker = new Thread(() => {
                int sleep = ServerSettings.GetSettingInt("AutoTimeout");
                if (sleep < 0) sleep = 30;
                if (sleep != 0) {
                    while (Running) {
                        for (int i = 0; i < sleep && Running; i++) {
                            Thread.Sleep(1000);
                        }
                        if (!Running) break;
                        foreach (Player p in Players) {
                            if (p.lastReceived.AddSeconds(sleep) < DateTime.Now) {
#if !DEBUG
                                p.Kick("The connection timed out");
#endif
                            }
                        }
                    }
                }
            });
            PlayerConnectionTimeoutChecker.Start();
        }
        static Thread HeartThread;
        static void Update() {
            GroupsaveIntervalCurrent++;
            PingIntervalCurrent++;
            BotIntervalCurrent++;

            Player.GlobalUpdate();

            if (GroupsaveIntervalCurrent >= GroupsaveInterval) { foreach (Groups.PlayerGroup g in Groups.PlayerGroup.Groups) { g.SaveGroup(); } GroupsaveIntervalCurrent = 0; }
            if (PingIntervalCurrent >= PingInterval) { Player.GlobalPing(); }
            if (BotIntervalCurrent >= BotInterval) { Bot.HandleBots(); }

            foreach (TimedMethod TM in TimedMethodList.ToArray()) {
                TM.time--;
                if (TM.time <= 0) {
                    TM.PassBack = TM.MethodToInvoke.Invoke(TM.PassBack);
                    if (TM.repeat == 0) TimedMethodList.Remove(TM);
                    TM.repeat--;
                    TM.time = TM.consistentTime;
                }
            }
        }

        static Thread PlayerConnectionTimeoutChecker;

        static void CreateCoreFiles() {

            //Directories first
            FileUtils.CreateDirIfNotExist("bans");
            FileUtils.CreateDirIfNotExist("text");
            FileUtils.CreateDirIfNotExist("levels");

            if (!File.Exists("properties/economy.properties")) Economy.CreateFile();

            FileUtils.CreateFileIfNotExist("text/badwords.txt");
            FileUtils.CreateFileIfNotExist("text/replacementwords.txt");
            FileUtils.CreateFileIfNotExist("text/agreed.txt");
            FileUtils.CreateFileIfNotExist("text/hacksmessages.txt");
            FileUtils.CreateFileIfNotExist("text/news.txt");
            FileUtils.CreateFileIfNotExist("text/jokermessages.txt", "I am a pony" + Environment.NewLine + "Rainbow Dash <3" + Environment.NewLine + "I like trains!");

            FileUtils.CreateFileIfNotExist("bans/IPBans.txt");
            FileUtils.CreateFileIfNotExist("bans/NameBans.txt");
            FileUtils.CreateFileIfNotExist("bans/BanInfo.txt");

            FileUtils.CreateFileIfNotExist("ranks/vips.txt", "#Vips are the players who can join the server when it's full!" + Environment.NewLine + "#Each name should be in a new line, whithout # at the start!");

            BadWordsList.AddRange(File.ReadAllLines("text/badwords.txt"));
            ReplacementWordsList.AddRange(File.ReadAllLines("text/replacementwords.txt"));

            try {
                string[] lines = File.ReadAllLines("text/agreed.txt");
                foreach (string pl in lines) { AgreedPlayers.Add(pl); }
                lines = File.ReadAllLines("ranks/vips.txt");
                foreach (string line in lines) { if (line.Trim()[0] != '#') { VIPs.Add(line); } }
            }
            catch { Logger.Log("Error reading agreed players and vips!", LogType.Error); }


        }
        /// <summary>
        /// Loops through online players, it is quite pointless. It would be more effecent to just use Server.Players.ForEach(stuff in here);
        /// </summary>
        /// <param name="a"></param>
        public static void ForeachPlayer(ForeachPlayerDelegate a) {
            for (int i = 0; i < Players.Count; i++) {
                if (Players.Count > i)
                    a.Invoke(Players[i]);
            }
        }
        public static void ForeachBot(ForeachBotDelegate a) {
            for (int i = 0; i < Bots.Count; i++) {
                if (Bots.Count > i)
                    a.Invoke(Bots[i]);
            }
        }
        internal static void AddConnection(Player p) {
            Connections.Add(p);
        }
        internal static void UpgradeConnectionToPlayer(Player p) {
            Connections.Remove(p);
            Players.Add(p);
        }
        internal static void RemovePlayer(Player p) {
            Connections.Remove(p);
            Players.Remove(p);
        }
        /// <summary>
        /// Add a method to be called in a specified time for a specified number of repetitions
        /// </summary>
        /// <param name="d">The delegate representing the method to be called</param>
        /// <param name="time">The amount of milliseconds you want to wait to call this method, and to wait inbetween each calling if it repeats (note that this is rounded to 10ths of a second)</param>
        /// <param name="repeat">the number of times to repeat this call</param>
        /// <param name="PassBack">the object to pass back to the method that is called.</param>
        public static void AddTimedMethod(TimedMethodDelegate d, int time, int repeat, object PassBack) {
            TimedMethod TMS = new TimedMethod(d, time, repeat, PassBack);
            TimedMethodList.Add(TMS);
        }

        #region Socket Stuff
        private static TcpListener listener;
        private static void StartListening() {
            try {
                if (ServerSettings.GetSettingBoolean("Use-UPnP")) {
                    if (!UPnP.CanUseUpnp) {
                        Logger.Log("Your router does not support UPnP. You must port forward.", LogType.Error);
                    }
                    else {

                        UPnP.ForwardPort(ServerSettings.GetSettingInt("port"), ProtocolType.Tcp, "MCForgeServer");
                        Logger.Log("Port forwarded automatically using UPnP");

                    }
                }
            }
            catch {
                Logger.Log("There was a problem setting up upnp, this can be a number of problems. Try again or manually port forward.", LogType.Error);
            }
            try {
                listener = new TcpListener(System.Net.IPAddress.Any, ServerSettings.GetSettingInt("port"));
                listener.Start();
            }
            catch (Exception e) {
                Logger.LogError(e);
                return;
            }
            while (true) {
                try {
                    listener.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), listener);
                    break;
                }
                catch (SocketException E) {
                    Logger.LogError(E);
                    break;
                }
                catch (Exception E) {
                    Logger.LogError(E);
                    continue;
                }
            }
        }
        private static void AcceptCallback(IAsyncResult ar) {
            TcpListener listener2 = (TcpListener)ar.AsyncState;
            try {
                TcpClient clientSocket = listener2.EndAcceptTcpClient(ar);
                new Player(clientSocket);
            }
            catch { }
            if (!ShuttingDown) {
                listener.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), listener);
            }
        }
        #endregion
        #region Log Stuff

        /// <summary>
        /// Write A message to the Console and the GuiLog using default (white on black) colors.
        /// </summary>
        /// <param name="message">The message to show</param>
        [Obsolete("Please use Logger.Log", false)]
        public static void Log(string message) {
            Logger.Log(message, Color.White, Color.Black);
        }
        /// <summary>
        /// Write an error to the Console and the GuiLog using Red on black colors
        /// </summary>
        /// <param name="E">The error exception to write.</param>
        [Obsolete("Please use Logger.Log", false)]
        public static void Log(Exception E) {

            Logger.Log("[ERROR]:", Color.Red, Color.Black, LogType.Error);
            Logger.Log(E.Message, Color.Red, Color.Black);
            Logger.Log(E.StackTrace, Color.Red, Color.Black);
        }

        /// <summary>
        /// Write a message to the console and GuiLog using a specified TextColor and BackGround Color
        /// </summary>
        /// <param name="message">The Message to show</param>
        /// <param name="TextColor">The color of the text to show</param>
        /// <param name="BackgroundColor">The color behind the text.</param>
        [Obsolete("Please use Logger.Log", false)]
        public static void Log(string message, ConsoleColor TextColor, ConsoleColor BackgroundColor) {
            var tColor = ColorUtils.ToColor(TextColor);
            var bColor = ColorUtils.ToColor(BackgroundColor);
            Logger.Log(message, tColor, bColor);
        }

        public static void OnLog(object sender, LogEventArgs args) {
            if (args == null) return;
            if (!DebugMode && args.LogType == LogType.Debug)
                return;
            var tColor = ColorUtils.ToConsoleColor(args.TextColor);
            var bColor = ColorUtils.ToConsoleColor(args.BackgroundColor);
            Console.ForegroundColor = tColor;
            Console.BackgroundColor = bColor;
            Console.WriteLine(args.Message.PadRight(Console.WindowWidth - 1));
            Console.ResetColor();
        }
        #endregion

        class TimedMethod {
            public TimedMethodDelegate MethodToInvoke;
            public int consistentTime;
            public int time;
            public int repeat;
            public object PassBack;

            public TimedMethod(TimedMethodDelegate a, int b, int c, object d) {
                MethodToInvoke = a;
                consistentTime = b / 100;
                time = b / 100;
                repeat = c;
                PassBack = d;
            }
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public static void Stop() {
            foreach (var p in Players)
                p.Kick(ServerSettings.GetSetting("ShutdownMessage"));
            ShuttingDown = true;

            if (UpdateTimer != null)
                UpdateTimer.Dispose();

            if (listener != null)
                listener.Stop();
            HeartThread.Abort();

            Logger.DeInit();

            Running = false;
        }

        /// <summary>
        /// Exits the server
        /// </summary>
        public static void Quit() {
            Environment.Exit(1);
        }

        /// <summary>
        /// Restarts the server (TODO - Mono compatible)
        /// </summary>
        public static void Restart() {
            // Get the parameters/arguments passed to program if any
            string arguments = string.Empty;
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 1; i < args.Length; i++) // args[0] is always exe path/filename
                arguments += args[i] + " ";

            // Restart current application, with same arguments/parameters
            System.Diagnostics.Process.Start(System.Windows.Forms.Application.ExecutablePath, arguments);
            Stop();
            Quit();
        }

        /// <summary>
        /// Saves all of the levels and groups
        /// </summary>
        public static void SaveAll() {
            foreach (Level l in Level.Levels)
                l.SaveToBinary();
            foreach (var g in Groups.PlayerGroup.Groups)
                g.SaveGroup();

            if (ServerSettings.GetSettingBoolean("SQLite-InMemory") && ServerSettings.GetSetting("DatabaseType").ToLower() == "sqlite") {
                ((SQLite)Database.SQL).Save();
            }

        }
    }
}
