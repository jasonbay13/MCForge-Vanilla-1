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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace MCForge_
{
    public static class SrvProperties
    {
    	public static void ConvertSettings()
    	{
    		try {
    			List<string> lines = new List<string>(); 
    			//TODO As more default options are added, we must convert them here
    			lines.Add("servername=" + Server.name);
    			lines.Add("wom-alternate_name=" + Server.Server_ALT);
    			lines.Add("port=" + Server.port);
    			lines.Add("use-upnp=false");
    			lines.Add("motd=" + Server.motd);
    			lines.Add("maxplayers=" + Server.players);
    			lines.Add("public=" + Server.pub);
    			lines.Add("verifynames=" + Server.verify);
    			lines.Add("wom-server_description=" + Server.Server_Disc);
    			lines.Add("wom-server_flags=" + Server.Server_Flag);
    			lines.Add("moneyname=" + Server.moneys);
    			lines.Add("serverowner=" + Server.server_owner);
    			lines.Add("verifying=" + Server.verifyadmins);
    			lines.Add("verifygroup=operator");
    			lines.Add("Enable-Remote=false");
    			lines.Add("Remote-IP=0.0.0.0");
    			lines.Add("Remote-Port=5050");
    			lines.Add("showfirstrunscreen=true");
    			lines.Add("usingconsole=" + Server.mono);
    			lines.Add("shutdownmessage=" + Server.customShutdownMessage);
    			lines.Add("welcomemessage=Welcome $name to $server<br>Enjoy your stay");
    			lines.Add("configpath=config/");
    			lines.Add("messageappending=true");
    			lines.Add("defaultgroup=" + Server.defaultRank);
    			lines.Add("allowhigherranktp=" + Server.higherranktp);
    			lines.Add("main-level=" + Server.level);
    			lines.Add("databasetype=" + ((Server.useMySQL) ? "mysql" : "sqlite"));
    			lines.Add("mysql-ip=" + Server.MySQLHost);
    			lines.Add("mysql-port=" + Server.MySQLPort);
    			lines.Add("mysql-username=" + Server.MySQLUsername);
    			lines.Add("mysql-password=" + Server.MySQLPassword);
    			lines.Add("mysql-pooling=" + Server.DatabasePooling);
    			lines.Add("mysql-dbname=" + Server.MySQLDatabaseName);
    			lines.Add("sqlite-inmemory=True");
    			lines.Add("sqlite-filepath=_mcforge.db");
    			lines.Add("sqlite-pooling=True");
    			lines.Add("database-queuing=False");
    			lines.Add("database-flush_interval=20");
    			lines.Add("irc-enabled=" + Server.irc);
    			lines.Add("irc-server=" + Server.ircServer);
    			lines.Add("irc-port=" + Server.ircPort);
    			lines.Add("irc-nickname=" + Server.ircNick);
    			lines.Add("irc-channel=" + Server.ircChannel);
    			lines.Add("irc-opchannel=" + Server.ircOpChannel);
    			lines.Add("irc-nickserv=" + Server.ircPassword);
    			lines.Add("agreeingtorules=" + Server.agreetorulesonentry);
    			lines.Add("$before$name=" + Server.dollardollardollar);
    			lines.Add("treesgothrough=false");
    			lines.Add("reviewmoderatorperm=80");
    			lines.Add("opchatpermission=80");
    			lines.Add("adminchatpermission=100");
    			lines.Add("defaultcolor=" + Server.DefaultColor);
    			lines.Add("backupfiles=true");
    			lines.Add("backupinterval=" + Server.backupInterval);
    			lines.Add("physicsinterval=100");
    			File.WriteAllLines("properties/server.properties", lines.ToArray());
    			lines.Clear();
    		}
    		catch (Exception e)
    		{
    			Console.ForegroundColor = ConsoleColor.Red;
    			Console.WriteLine("Failed to convert properties!");
    			Console.WriteLine(e.ToString());
    			Console.ForegroundColor = ConsoleColor.Green;
    		}
    		
    	}
    	public static void Load(string givenPath) {
    		Load(givenPath, false);
    	}
        public static void Load(string givenPath, bool skipsalt)
        {
            if (File.Exists(givenPath))
            {
                string[] lines = File.ReadAllLines(givenPath);

                foreach (string line in lines)
                {
                    if (line != "" && line[0] != '#')
                    {
                        //int index = line.IndexOf('=') + 1; // not needed if we use Split('=')
                        string key = line.Split('=')[0].Trim();
                        string value = "";
                        if (line.IndexOf('=') >= 0)
                            value = line.Substring(line.IndexOf('=') + 1).Trim(); // allowing = in the values
                        string color = "";

                        switch (key.ToLower())
                        {
                            case "server-name":
                                if (ValidString(value, "![]:.,{}~-+()?_/\\' "))
                                {
                                    Server.name = value;
                                }
                                else { Server.s.Log("server-name invalid! setting to default."); }
                                break;
                            case "motd":
                                if (ValidString(value, "=![]&:.,{}~-+()?_/\\' ")) // allow = in the motd
                                {
                                    Server.motd = value;
                                }
                                else { Server.s.Log("motd invalid! setting to default."); }
                                break;
                            case "port":
                                try { Server.port = Convert.ToInt32(value); }
                                catch { Server.s.Log("port invalid! setting to default."); }
                                break;
                            case "verify-names":
                                Server.verify = (value.ToLower() == "true") ? true : false;
                                break;
                            case "public":
                                Server.pub = (value.ToLower() == "true") ? true : false;
                                break;
                            case "world-chat":
                                Server.worldChat = (value.ToLower() == "true") ? true : false;
                                break;
                            //case "guest-goto":
                            //    Server.guestGoto = (value.ToLower() == "true") ? true : false;
                            //    break;
                            case "max-players":
                                try
                                {
                                    if (Convert.ToByte(value) > 128)
                                    {
                                        value = "128"; Server.s.Log("Max players has been lowered to 128.");
                                    }
                                    else if (Convert.ToByte(value) < 1)
                                    {
                                        value = "1"; Server.s.Log("Max players has been increased to 1.");
                                    }
                                    Server.players = Convert.ToByte(value);
                                }
                                catch { Server.s.Log("max-players invalid! setting to default."); }
                                break;
                            case "max-guests":
                                try
                                {
                                    if (Convert.ToByte(value) > Server.players)
                                    {
                                        value = Server.players.ToString(); Server.s.Log("Max guests has been lowered to " + Server.players.ToString());
                                    }
                                    else if (Convert.ToByte(value) < 0)
                                    {
                                        value = "0"; Server.s.Log("Max guests has been increased to 0.");
                                    }
                                    Server.maxGuests = Convert.ToByte(value);
                                }
                                catch { Server.s.Log("max-guests invalid! setting to default."); }
                                break;
                            case "max-maps":
                                try
                                {
                                    if (Convert.ToByte(value) > 100)
                                    {
                                        value = "100";
                                        Server.s.Log("Max maps has been lowered to 100.");
                                    }
                                    else if (Convert.ToByte(value) < 1)
                                    {
                                        value = "1";
                                        Server.s.Log("Max maps has been increased to 1.");
                                    }
                                    Server.maps = Convert.ToByte(value);
                                }
                                catch
                                {
                                    Server.s.Log("max-maps invalid! setting to default.");
                                }
                                break;
                            case "irc":
                                Server.irc = (value.ToLower() == "true") ? true : false;
                                break;
                            case "irc-colorsenable":
                                Server.ircColorsEnable = (value.ToLower() == "true") ? true : false;
                                break;
                            case "irc-server":
                                Server.ircServer = value;
                                break;
                            case "irc-nick":
                                Server.ircNick = value;
                                break;
                            case "irc-channel":
                                Server.ircChannel = value;
                                break;
                            case "irc-opchannel":
                                Server.ircOpChannel = value;
                                break;
                            case "irc-port":
                                try
                                {
                                    Server.ircPort = Convert.ToInt32(value);
                                }
                                catch
                                {
                                    Server.s.Log("irc-port invalid! setting to default.");
                                }
                                break;
                            case "irc-identify":
                                try
                                {
                                    Server.ircIdentify = Convert.ToBoolean(value);
                                }
                                catch
                                {
                                    Server.s.Log("irc-identify boolean value invalid! Setting to the default of: " + Server.ircIdentify + ".");
                                }
                                break;
                            case "irc-password":
                                Server.ircPassword = value;
                                break;
                            case "anti-tunnels":
                                Server.antiTunnel = (value.ToLower() == "true") ? true : false;
                                break;
                            case "max-depth":
                                try
                                {
                                    Server.maxDepth = Convert.ToByte(value);
                                }
                                catch
                                {
                                    Server.s.Log("maxDepth invalid! setting to default.");
                                }
                                break;

                            case "rplimit":
                                try { Server.rpLimit = Convert.ToInt16(value); }
                                catch { Server.s.Log("rpLimit invalid! setting to default."); }
                                break;
                            case "rplimit-norm":
                                try { Server.rpNormLimit = Convert.ToInt16(value); }
                                catch { Server.s.Log("rpLimit-norm invalid! setting to default."); }
                                break;


                            case "report-back":
                                Server.reportBack = (value.ToLower() == "true") ? true : false;
                                break;
                            case "backup-time":
                                if (Convert.ToInt32(value) > 1) { Server.backupInterval = Convert.ToInt32(value); }
                                break;
                            case "backup-location":
                                if (!value.Contains("System.Windows.Forms.TextBox, Text:"))
                                    Server.backupLocation = value;
                                break;

                            //case "console-only": // Never used
                            //    Server.console = (value.ToLower() == "true") ? true : false;
                            //    break;

                            case "physicsrestart":
                                Server.physicsRestart = (value.ToLower() == "true") ? true : false;
                                break;
                            case "deathcount":
                                Server.deathcount = (value.ToLower() == "true") ? true : false;
                                break;

                            case "usemysql":
                                Server.useMySQL = (value.ToLower() == "true") ? true : false;
                                break;
                            case "host":
                                Server.MySQLHost = value;
                                break;
                            case "sqlport":
                                Server.MySQLPort = value;
                                break;
                            case "username":
                                Server.MySQLUsername = value;
                                break;
                            case "password":
                                Server.MySQLPassword = value;
                                break;
                            case "databasename":
                                Server.MySQLDatabaseName = value;
                                break;
                            case "pooling":
                                try { Server.DatabasePooling = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "defaultcolor":
                                color = c.Parse(value);
                                if (color == "")
                                {
                                    color = c.Name(value); if (color != "") color = value; else { Server.s.Log("Could not find " + value); return; }
                                }
                                //Server.DefaultColor = color;
                                break;
                            case "irc-color":
                                color = c.Parse(value);
                                if (color == "")
                                {
                                    color = c.Name(value); if (color != "") color = value; else { Server.s.Log("Could not find " + value); return; }
                                }
                                Server.IRCColour = color;
                                break;
                            case "old-help":
                                try { Server.oldHelp = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "log-heartbeat":
                                try { Server.logbeat = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ".  Using default."); break; }
                                break;
                            case "force-cuboid":
                                try { Server.forceCuboid = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ".  Using default."); break; }
                                break;
                            case "profanity-filter":
                                try { Server.profanityFilter = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "notify-on-join-leave":
                                try { Server.notifyOnJoinLeave = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "cheapmessage":
                                try { Server.cheapMessage = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "cheap-message-given":
                                if (value != "") Server.cheapMessageGiven = value;
                                break;
                            case "custom-ban":
                                try { Server.customBan = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "custom-ban-message":
                                if (value != "") Server.customBanMessage = value;
                                break;
                            case "custom-shutdown":
                                try { Server.customShutdown = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "custom-shutdown-message":
                                if (value != "") Server.customShutdownMessage = value;
                                break;
                            case "custom-griefer-stone":
                                try { Server.customGrieferStone = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "custom-griefer-stone-message":
                                if (value != "") Server.customGrieferStoneMessage = value;
                                break;
                            case "custom-promote-message":
                                if (value != "") Server.customPromoteMessage = value;
                                break;
                            case "custom-demote-message":
                                if (value != "") Server.customDemoteMessage = value;
                                break;
                            case "rank-super":
                                try { Server.rankSuper = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "default-rank":
                                try { Server.defaultRank = value.ToLower(); }
                                catch { }
                                break;
                            case "afk-minutes":
                                try
                                {
                                    Server.afkminutes = Convert.ToInt32(value);
                                }
                                catch
                                {
                                    Server.s.Log("irc-port invalid! setting to default.");
                                }
                                break;
                            case "afk-kick":
                                try { Server.afkkick = Convert.ToInt32(value); }
                                catch { Server.s.Log("irc-port invalid! setting to default."); }
                                break;
                            case "check-updates":
                                try { Server.autonotify = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "auto-update":
                                Server.autoupdate = (value.ToLower() == "true") ? true : false;
                                break;
                            case "in-game-update-notify":
                                Server.notifyPlayers = (value.ToLower() == "true") ? true : false;
                                break;
                            case "update-countdown":
                                try { Server.restartcountdown = Convert.ToInt32(value).ToString(); }
                                catch { Server.restartcountdown = "10"; }
                                break;
                            case "autoload":
                                try { Server.AutoLoad = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "auto-restart":
                                try { Server.autorestart = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "restarttime":
                                try { Server.restarttime = DateTime.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using defualt."); break; }
                                break;
                            case "parse-emotes":
                                try { Server.parseSmiley = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "use-whitelist":
                                Server.useWhitelist = (value.ToLower() == "true") ? true : false;
                                break;
                            case "premium-only":
                                Server.PremiumPlayersOnly = (value.ToLower() == "true") ? true : false;
                                break;
                            case "allow-tp-to-higher-ranks":
                                Server.higherranktp = (value.ToLower() == "true") ? true : false;
                                break;
                            case "agree-to-rules-on-entry":
                                try { Server.agreetorulesonentry = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "admins-join-silent":
                                try { Server.adminsjoinsilent = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "main-name":
                                Server.level = value;
                                break;
                            case "dollar-before-dollar":
                                try { Server.dollardollardollar = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "money-name":
                                if (value != "") Server.moneys = value;
                                break;
                            /*case "mono":
                                try { Server.mono = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;*/
                            case "restart-on-error":
                                try { Server.restartOnError = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "repeat-messages":
                                try { Server.repeatMessage = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "host-state":
                                if (value != "")
                                    Server.ZallState = value;
                                break;
                            case "kick-on-hackrank":
                                try { Server.hackrank_kick = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "hackrank-kick-time":
                                try { Server.hackrank_kick_time = int.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "server-owner":
                                if (value != "")
                                    Server.server_owner = value;
                                break;
                            case "zombie-on-server-start":
                                try { Server.startZombieModeOnStartup = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "no-respawning-during-zombie":
                                try { Server.noRespawn = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "no-level-saving-during-zombie":
                                try { Server.noLevelSaving = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "no-pillaring-during-zombie":
                                try { Server.noPillaring = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "zombie-name-while-infected":
                                if (value != "")
                                    Server.ZombieName = value;
                                break;
                            case "enable-changing-levels":
                                try { Server.ChangeLevels = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "zombie-survival-only-server":
                                try { Server.ZombieOnlyServer = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "use-level-list":
                                try { Server.UseLevelList = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "zombie-level-list":
                                if (value != "")
                                {

                                    string input = value.Replace(" ", "").ToString();
                                        int itndex = input.IndexOf("#");
                                    if (itndex > 0)
                                        input = input.Substring(0, itndex);

                                    Server.LevelList = input.Split(',').ToList<string>();
                                }
                                break;
                            case "guest-limit-notify":
                                try { Server.guestLimitNotify = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "guest-join-notify":
                                try { Server.guestJoinNotify = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "guest-leave-notify":
                                try { Server.guestLeaveNotify = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "ignore-ops":
                                try { Server.globalignoreops = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "admin-verification":
                                try { Server.verifyadmins = bool.Parse(value); }
                                catch { Server.s.Log("invalid " + key + ". Using default"); }
                                break;
                            case "mute-on-spam":
                                try { Server.checkspam = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "spam-messages":
                                try { Server.spamcounter = int.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "spam-mute-time":
                                try { Server.mutespamtime = int.Parse(value); } catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "spam-counter-reset-time":
                                try { Server.spamcountreset = int.Parse(value); } catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "show-empty-ranks":
                                try { Server.showEmptyRanks = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "global-chat-enabled":
                                try { Server.UseGlobalChat = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;

                            case "global-chat-nick":
                                if (value != "")
                                    Server.GlobalChatNick = value;
                                break;

                            case "global-chat-color":
                                color = c.Parse(value);
                                if (color == "")
                                {
                                    color = c.Name(value); if (color != "") color = value; else { Server.s.Log("Could not find " + value); return; }
                                }
                                Server.GlobalChatColor = color;
                                break;

                            case "total-undo":
                                try { Server.totalUndo = int.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;

                            case "griefer-stone-tempban":
                                try { Server.grieferStoneBan = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;

                            case "wom-direct":
                                try { Server.WomDirect = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "wom-serveralt":
                                Server.Server_ALT = value;
                                break;
                            case "wom-serverdis":
                                Server.Server_Disc = value;
                                break;
                            case "wom-serverflag":
                                Server.Server_Flag = value;
                                break;
                            case "wom-textures":
                                Server.UseTextures = bool.Parse(value);
                                break;
                            case "bufferblocks":
                                try
                                {
                                    Server.bufferblocks = bool.Parse(value);
                                }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "mcforge-user":
                                Server.mcforgeUser = value;
                                break;
                            case "mcforge-pass":
                                Server.mcforgePass = value;
                                break;
                            case "translation-enabled":
                                Server.transenabled = (value.ToLower() == "true") ? true : false;
                                break;
                            case "translation-language":
                                string langcode = value;
                            switch (langcode)
                                {
                                    case "af":
                                        langcode = "Afrikaans";
                                        break;
                                    case "ar-sa":
                                        langcode = "Arabic (Saudi Arabia)";
                                        break;
                                    case "ar-eg":
                                        langcode = "Arabic (Egypt)";
                                        break;
                                    case "ar-dz":
                                        langcode = "Arabic (Algeria)";
                                        break;
                                    case "ar-tn":
                                        langcode = "Arabic (Tunisia)";
                                        break;
                                    case "ar-ye":
                                        langcode = "Arabic (Yemen)";
                                        break;
                                    case "ar-jo":
                                        langcode = "Arabic (Jordan)";
                                        break;
                                    case "ar-kw":
                                        langcode = "Arabic (Kuwait)";
                                        break;
                                    case "ar-bh":
                                        langcode = "Arabic (Bahrain)";
                                        break;
                                    case "eu":
                                        langcode = "Basque";
                                        break;
                                    case "be":
                                        langcode = "Belarusian";
                                        break;
                                    case "zh-tw":
                                        langcode = "Chinese (Taiwan)";
                                        break;
                                    case "zh-hk":
                                        langcode = "Chinese (Hong Kong SAR)";
                                        break;
                                    case "hr":
                                        langcode = "Croatian";
                                        break;
                                    case "da":
                                        langcode = "Danish";
                                        break;
                                    case "nl-be":
                                        langcode = "Dutch (Belgium)";
                                        break;
                                    case "en-us":
                                        langcode = "English (United States)";
                                        break;
                                    case "en-au":
                                        langcode = "English (Australia)";
                                        break;
                                    case "en-nz":
                                        langcode = "English (New Zealand)";
                                        break;
                                    case "en-za":
                                        langcode = "English (South Africa)";
                                        break;
                                    case "en-tt":
                                        langcode = "English (Trinidad)";
                                        break;
                                    case "fo":
                                        langcode = "Faeroese";
                                        break;
                                    case "fi":
                                        langcode = "Finnish";
                                        break;
                                    case "fr-be":
                                        langcode = "French (Belgium)";
                                        break;
                                    case "fr-ch":
                                        langcode = "French (Switzerland)";
                                        break;
                                    case "gd":
                                        langcode = "Gaelic (Scotland)";
                                        break;
                                    case "de":
                                        langcode = "German (Standard)";
                                        break;
                                    case "de-at":
                                        langcode = "German (Austria)";
                                        break;
                                    case "de-li":
                                        langcode = "German (Liechtenstein)";
                                        break;
                                    case "he":
                                        langcode = "Hebrew";
                                        break;
                                    case "hu":
                                        langcode = "Hungarian";
                                        break;
                                    case "id":
                                        langcode = "Indonesian";
                                        break;
                                    case "it-ch":
                                        langcode = "Italian (Switzerland)";
                                        break;
                                    case "ko":
                                        langcode = "Korean";
                                        break;
                                    case "lv":
                                        langcode = "Latvian";
                                        break;
                                    case "mk":
                                        langcode = "Macedonian (FYROM)";
                                        break;
                                    case "mt":
                                        langcode = "Maltese";
                                        break;
                                    case "no":
                                        langcode = "Norwegian (Nynorsk)";
                                        break;
                                    case "pt-br":
                                        langcode = "Portuguese (Brazil)";
                                        break;
                                    case "rm":
                                        langcode = "Rhaeto-Romanic";
                                        break;
                                    case "ro-mo":
                                        langcode = "Romanian (Republic of Moldova)";
                                        break;
                                    case "ru-mo":
                                        langcode = "Russian (Republic of Moldova)";
                                        break;
                                    case "sr":
                                        langcode = "Serbian (Cyrillic)";
                                        break;
                                    case "sk":
                                        langcode = "Slovak";
                                        break;
                                    case "sb":
                                        langcode = "Sorbian";
                                        break;
                                    case "es-mx":
                                        langcode = "Spanish (Mexico)";
                                        break;
                                    case "es-cr":
                                        langcode = "Spanish (Costa Rica)";
                                        break;
                                    case "es-do":
                                        langcode = "Spanish (Dominican Republic)";
                                        break;
                                    case "es-co":
                                        langcode = "Spanish (Colombia)";
                                        break;
                                    case "es-ar":
                                        langcode = "Spanish (Argentina)";
                                        break;
                                    case "es-cl":
                                        langcode = "Spanish (Chile)";
                                        break;
                                    case "es-py":
                                        langcode = "Spanish (Paraguay)";
                                        break;
                                    case "es-sv":
                                        langcode = "Spanish (El Salvador)";
                                        break;
                                    case "es-ni":
                                        langcode = "Spanish (Nicaragua)";
                                        break;
                                    case "sx":
                                        langcode = "Sutu";
                                        break;
                                    case "sv-fi":
                                        langcode = "Swedish (Finland)";
                                        break;
                                    case "ts":
                                        langcode = "Tsonga";
                                        break;
                                    case "tr":
                                        langcode = "Turkish";
                                        break;
                                    case "ur":
                                        langcode = "Urdu";
                                        break;
                                    case "vi":
                                        langcode = "Vietnamese";
                                        break;
                                    case "ji":
                                        langcode = "Yiddish";
                                        break;
                                    case "sq":
                                        langcode = "Albanian";
                                        break;
                                    case "ar-iq":
                                        langcode = "Arabic (Iraq)";
                                        break;
                                    case "ar-ly":
                                        langcode = "Arabic (Libya)";
                                        break;
                                    case "ar-ma":
                                        langcode = "Arabic (Morocco)";
                                        break;
                                    case "ar-om":
                                        langcode = "Arabic (Oman)";
                                        break;
                                    case "ar-sy":
                                        langcode = "Arabic (Syria)";
                                        break;
                                    case "ar-lb":
                                        langcode = "Arabic (Lebanon)";
                                        break;
                                    case "ar-ae":
                                        langcode = "Arabic (U.A.E.)";
                                        break;
                                    case "ar-qa":
                                        langcode = "Arabic (Qatar)";
                                        break;
                                    case "bg":
                                        langcode = "Bulgarian";
                                        break;
                                    case "ca":
                                        langcode = "Catalan";
                                        break;
                                    case "zh-cn":
                                        langcode = "Chinese (PRC)";
                                        break;
                                    case "zh-sg":
                                        langcode = "Chinese (Singapore)";
                                        break;
                                    case "cs":
                                        langcode = "Czech";
                                        break;
                                    case "nl":
                                        langcode = "Dutch (Standard)";
                                        break;
                                    case "en":
                                        langcode = "English";
                                        break;
                                    case "en-gb":
                                        langcode = "English (United Kingdom)";
                                        break;
                                    case "en-ca":
                                        langcode = "English (Canada)";
                                        break;
                                    case "en-ie":
                                        langcode = "English (Ireland)";
                                        break;
                                    case "en-jm":
                                        langcode = "English (Jamaica)";
                                        break;
                                    case "en-bz":
                                        langcode = "English (Belize)";
                                        break;
                                    case "et":
                                        langcode = "Farsi";
                                        break;
                                    case "fr":
                                        langcode = "French (Standard)";
                                        break;
                                    case "ga":
                                        langcode = "Irish";
                                        break;
                                    case "el":
                                        langcode = "Greek";
                                        break;
                                    case "hi":
                                        langcode = "Hindi";
                                        break;
                                    case "it":
                                        langcode = "Italian (Standard)";
                                        break;
                                    case "is":
                                        langcode = "Icelandic";
                                        break;
                                    case "ja":
                                        langcode = "Japanese";
                                        break;
                                    case "lt":
                                        langcode = "Lithuanian";
                                        break;
                                    case "ms":
                                        langcode = "Malaysian";
                                        break;
                                    case "pl":
                                        langcode = "Polish";
                                        break;
                                    case "pt":
                                        langcode = "Portuguese";
                                        break;
                                    case "ro":
                                        langcode = "Romanian";
                                        break;
                                    case "ru":
                                        langcode = "Russian";
                                        break;
                                    case "sz":
                                        langcode = "Sami (Lappish) ";
                                        break;
                                    case "sl":
                                        langcode = "Slovenian ";
                                        break;
                                    case "es":
                                        langcode = "Spanish";
                                        break;
                                    case "sv":
                                        langcode = "Swedish";
                                        break;
                                    case "th":
                                        langcode = "Thai";
                                        break;
                                    case "tn":
                                        langcode = "Tswana";
                                        break;
                                    case "uk":
                                        langcode = "Ukrainian";
                                        break;
                                    case "ve":
                                        langcode = "Venda";
                                        break;
                                    case "xh":
                                        langcode = "Xhosa";
                                        break;
                                    case "zu":
                                        langcode = "Zulu";
                                        break;
                                    default:
                                        langcode = "!ERROR!";
                                        break;
                                }
                            if (langcode != "!ERROR!")
                            {
                                Server.translang = value.ToString().ToLower();
                            }
                            else
                            {
                                Server.translang = "en";
                            }
                                break;

                        }
                    }
                }
            }
        }
        public static bool ValidString(string str, string allowed)
        {
            string allowedchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890" + allowed;
            foreach (char ch in str)
            {
                if (allowedchars.IndexOf(ch) == -1)
                {
                    return false;
                }
            } return true;
        }
    }
}