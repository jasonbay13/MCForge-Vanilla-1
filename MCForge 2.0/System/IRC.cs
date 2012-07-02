/*
Copyright 2012 MCForge
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
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Interfaces;
using MCForge.Utils;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Utils.Settings;

namespace MCForge.Core
{
    /// <summary>
    /// IRC Class to connect to an Internet Relay Chat Server
    /// </summary>
    public class IRC
    {
        string nickname = "";
        string server = "";
        string password = "";
        int port = -1;
        public string channel = "";
        public string opChannel = "";
        StreamWriter swrite;
        StreamReader sread;
        NetworkStream sstream;
        TcpClient irc;
        bool debug = false;
        public bool botOn = false;
        public bool connected = false;
        string line;
        public List<string> ircControllers;
        string[] splitLine;
        private static ConsolePlayer cp;
        private static CommandIO cio = new CommandIO();

        public IRC() { }

        public void Start()
        {
            server = ServerSettings.GetSetting("IRC-Server");
            port = ServerSettings.GetSettingInt("IRC-Port");
            nickname = ServerSettings.GetSetting("IRC-Nickname");
            channel = ServerSettings.GetSetting("IRC-Channel");
            opChannel = ServerSettings.GetSetting("IRC-OPChannel");
            password = ServerSettings.GetSetting("IRC-NickServ");
            if (nickname == "" || server == "" || channel == "#" || channel == "" || port == -1 || !ServerSettings.GetSettingBoolean("IRC-Enabled"))
                return;
            ircControllers = LoadIrcControllers();
            Logger.Log("Connecting to IRC...");
            botOn = true;
            try
            {
                // Connect
                irc = new TcpClient(server, port);
                sstream = irc.GetStream();
                sread = new StreamReader(sstream);
                swrite = new StreamWriter(sstream);
            }
            catch (Exception e)
            {
                Logger.Log("Error connecting to " + server + ": " + e);
                return;
            }

            // Identify
            swrite.WriteLine("USER {0} {0} {0} :{1}", nickname, nickname);
            swrite.Flush();
            swrite.WriteLine("NICK {0}", nickname);
            swrite.Flush();

            cp = new ConsolePlayer(cio);

            while (botOn)
            {
                try
                {
                    if ((line = sread.ReadLine()) != null && botOn)
                    {
                        if (debug) Logger.Log(line);
                        splitLine = line.Split(' ');

                        if (splitLine.Length > 0)
                        {
                            if (splitLine[1] == "376" || splitLine[1] == "422")
                            {
                                swrite.WriteLine("JOIN {0}", channel);
                                swrite.Flush();
                                if (opChannel != "#" || opChannel != "")
                                {
                                    swrite.WriteLine("JOIN {0}", opChannel);
                                    swrite.Flush();
                                }
                                swrite.WriteLine("NS IDENTIFY {0} {1}", nickname, password);
                                swrite.Flush();
                                connected = true;
                                Logger.Log("Connected!");
                            }

                            if (splitLine[0] == "PING")
                            {
                                swrite.WriteLine("PONG {0}", splitLine[1]);
                                swrite.Flush();
                            }
                        }
                        string replyChannel = "";
                        if (line.Split(' ')[2] != channel && line.Split(' ')[2] != opChannel) replyChannel = line.Split('!')[0].Remove(0, 1);
                        else replyChannel = line.Split(' ')[2];
                        line = line.Replace("%", "&");
                        if (GetSpokenLine(line).Equals("!players"))
                        {
                            swrite.WriteLine("PRIVMSG {0} :" + "There are " + Server.Players.Count + " player(s) online:",
                                         replyChannel);
                            swrite.Flush();
                            string playerString = "";
                            Server.Players.ForEach(delegate(Player pl)
                            {
                                playerString = playerString + pl.Username + ", ";
                            });
                            swrite.WriteLine("PRIVMSG {0} :" + playerString.Remove(playerString.Length - 2, 2),
                                         replyChannel);
                            swrite.Flush();
                        }
                        else if (GetSpokenLine(line).Equals("!url"))
                        {
                            swrite.WriteLine("PRIVMSG {0} :" + "The url for the server is: " + Server.URL,
                                         replyChannel);
                            swrite.Flush();
                        }
                        else if (GetSpokenLine(line).StartsWith("!"))
                        {
                            if (GetSpokenLine(line).Trim().Length > 1)
                            {
                                string name = GetSpokenLine(line).Trim().Substring(1);
                                ICommand c = Command.Find(name);
                                if (c != null)
                                {
                                    int i = GetSpokenLine(line).Trim().IndexOf(" ");
                                    if (i > 0)
                                    {
                                        string[] cargs = GetSpokenLine(line).Trim().Substring(i + 1).Split(' ');
                                        cp.replyChannel = replyChannel;
                                        c.Use(cp, cargs);
                                    }
                                    else
                                    {
                                        cp.replyChannel = replyChannel;
                                        c.Use(cp, new string[0]);
                                    }
                                }
                            }
                        }
                        else if (line.ToLower().Contains("privmsg") && splitLine[1] != "005")
                        {
                            if (replyChannel != opChannel)
                            {
                                try
                                {
                                    Player.UniversalChat("[IRC] <" + GetUsernameSpeaking(line) + ">: " + IRCToClassic(GetSpokenLine(line)));
                                    Logger.Log("[IRC] <" + GetUsernameSpeaking(line) + ">: " + IRCToClassic(GetSpokenLine(line)));
                                }
                                catch { }
                            }
                            else if (replyChannel == opChannel)
                            {
                                try
                                {
                                    Player.UniversalChatOps("[OPIRC] <" + GetUsernameSpeaking(line) + ">: " + IRCToClassic(GetSpokenLine(line)));
                                    Logger.Log("[OPIRC] <" + GetUsernameSpeaking(line) + ">: " + IRCToClassic(GetSpokenLine(line)));
                                }
                                catch { }
                            }
                        }
                    }
                }
                catch { }
            }
            // Clean up
            connected = false;
            swrite.Close();
            sread.Close();
            irc.Close();
        }

        private static string GetUsernameSpeaking(string line)
        {
            return line.Split('!')[0].Split(':')[1];
        }

        private static string GetHostSpeaking(string line)
        {
            return line.Split('!')[1].Split(' ')[0];
        }

        private static string GetSpokenLine(string line)
        {
            string tmp = "";
            int loop = 0;
            if (line.Split(':').Length >= 2)
            {
                foreach (string str in line.Split(':'))
                {
                    if (str.Contains("PRIVMSG")) loop = 1;
                    else if (loop == 2) tmp = str;
                    else if (loop > 2) tmp = tmp + ":" + str;
                    loop++;
                }
                return tmp;
            }

            return "";
        }

        public void SendMessage(string str)
        {
            if (!connected) return;
            swrite.WriteLine("PRIVMSG {0} :" + ClassicToIRC(str), channel);
            swrite.Flush();
        }

        public void SendUserMessage(string str, string channel)
        {
            if (!connected) return;
            swrite.WriteLine("PRIVMSG {0} :" + ClassicToIRC(str), channel);
            swrite.Flush();
        }

        private List<string> LoadIrcControllers()
        {
            ircControllers = null;
            List<string> temp = new List<string>();
            System.IO.Directory.CreateDirectory("files");
            if (!File.Exists("text/irccontrollers.txt")) { File.Create("text/irccontrollers.txt", 1, FileOptions.Asynchronous).Close(); }
            string line;
            if (new FileInfo("text/irccontrollers.txt").Length != 0)
            {
                StreamReader ircControllerReader = null;
                try
                {
                    ircControllerReader = new StreamReader("text/irccontrollers.txt");
                    while ((line = ircControllerReader.ReadLine()) != null)
                    {
                        temp.Add(line.ToLower());
                    }
                }
                finally
                {
                    if (ircControllerReader != null)
                        ircControllerReader.Close();
                }
            }
            return temp;
        }

        public void Close()
        {
            if (irc != null)
            {
                botOn = false;
                if (connected) swrite.WriteLine("QUIT");
                swrite.Flush();
                swrite.Close();
                sread.Close();
                irc.Close();
                connected = false;
            }
        }

        public static string ClassicToIRC(string message)
        {
            if (message == null) throw new ArgumentNullException("message");
            StringBuilder sb = new StringBuilder(message);
            ClassicToIRC(sb);
            return sb.ToString();
        }

        public static void ClassicToIRC(StringBuilder sb)
        {
            if (sb == null) throw new ArgumentNullException("sb");
            sb.Replace("&0", '\x03' + "1");
            sb.Replace("&1", '\x03' + "2");
            sb.Replace("&2", '\x03' + "3");
            sb.Replace("&3", '\x03' + "10");
            sb.Replace("&4", '\x03' + "4");
            sb.Replace("&5", '\x03' + "6");
            sb.Replace("&6", '\x03' + "8");
            sb.Replace("&7", '\x03' + "15");
            sb.Replace("&8", '\x03' + "14");
            sb.Replace("&9", '\x03' + "12");
            sb.Replace("&a", '\x03' + "9");
            sb.Replace("&b", '\x03' + "11");
            sb.Replace("&c", '\x03' + "4");
            sb.Replace("&d", '\x03' + "13");
            sb.Replace("&e", '\x03' + "7");
            sb.Replace("&f", '\x03' + "0");
            sb.Replace("&A", '\x03' + "9");
            sb.Replace("&B", '\x03' + "11");
            sb.Replace("&C", '\x03' + "4");
            sb.Replace("&D", '\x03' + "13");
            sb.Replace("&E", '\x03' + "7");
            sb.Replace("&F", '\x03' + "0");
        }

        public static string IRCToClassic(string message)
        {
            if (message == null) throw new ArgumentNullException("message");
            StringBuilder sb = new StringBuilder(message);
            IRCToClassic(sb);
            return sb.ToString();
        }

        public static void IRCToClassic(StringBuilder sb)
        {
            if (sb == null) throw new ArgumentNullException("sb");
            sb.Replace('\x03' + "1", "&0");
            sb.Replace('\x03' + "2", "&1");
            sb.Replace('\x03' + "3", "&2");
            sb.Replace('\x03' + "10", "&3");
            sb.Replace('\x03' + "4", "&4");
            sb.Replace('\x03' + "6", "&5");
            sb.Replace('\x03' + "8", "&6");
            sb.Replace('\x03' + "15", "&7");
            sb.Replace('\x03' + "14","&8");
            sb.Replace('\x03' + "12","&9");
            sb.Replace('\x03' + "9", "&a");
            sb.Replace('\x03' + "11", "&b");
            sb.Replace('\x03' + "4", "&c");
            sb.Replace('\x03' + "13", "&d");
            sb.Replace('\x03' + "7", "&e");
            sb.Replace('\x03' + "0","&f");
            sb.Replace('\x03' + "9", "&A");
            sb.Replace('\x03' + "11","&B");
            sb.Replace('\x03' + "4", "&C");
            sb.Replace('\x03' + "13", "&D");
            sb.Replace('\x03' + "7", "&E");
            sb.Replace('\x03' + "0", "&F");
        }

        class CommandIO : IIOProvider
        {
            public string ReadLine()
            {
                return "";
            }

            public void WriteLine(string s)
            {
                
            }

            public void WriteLine(string line, string replyChannel)
            {
                Server.IRC.SendUserMessage(line, replyChannel);
            }
        }
    }
}