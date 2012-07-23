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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Threading;
using MCForge.API.Events;
using MCForge.World;
using MCForge.Interface.Plugin;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Utils.Settings;
using MCForge.Utils;

namespace Plugins.WoMPlugin
{
    public class PluginWoMTextures : IPlugin
    {
        #region IPlugin members
        public string Name { get { return "WoMTextures"; } }
        public string Author { get { return "headdetect and Gamemakergm"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }

        public void OnLoad(string[] args1)
        {
            Server.OnServerFinishSetup += OnLoadDone;
        }
        public void OnUnload()
        {
            Player.OnAllPlayersSendPacket.Normal -= OnOutgoingData;
            Player.OnAllPlayersReceivePacket.Normal -= OnIncomingData;
        }
        #endregion
        #region Plugin Variables
        public static WoMPluginSettings PluginSettings { get; set; }
        public static Dictionary<Level, CFGSettings> CFGDict = new Dictionary<Level, CFGSettings>();
        private string compass = " -NW- | -N- | -NE- | -E- | -SE- | -S- | -SW- | -W- |";
        #endregion
        #region WoM Handling
        //Wait for server to finish so we make sure all of the levels are loaded
        void OnLoadDone()
        {
            Logger.Log("[WoMTextures] Succesfully initiated!");
            FileUtils.CreateDirIfNotExist(ServerSettings.GetSetting("configpath") + "WoMTexturing/");

            foreach (Level l in Level.Levels)
            {
                //This allows us to get a setting from any level cfg.
                CFGSettings s = new CFGSettings(l);
                CFGDict.CreateIfNotExist<Level, CFGSettings>(l, s);
                s.OnLoad();
            }
            PluginSettings = new WoMPluginSettings();
            PluginSettings.OnLoad();

            Server.OnServerFinishSetup -= OnLoadDone;
            //Need on level load event to add to dictionary and serve cfg.
            Player.OnAllPlayersReceiveUnknownPacket.Normal += new Event<Player, PacketEventArgs>.EventHandler(OnIncomingData);
            Player.OnAllPlayersSendPacket.Normal += new Event<Player, PacketEventArgs>.EventHandler(OnOutgoingData);
            Player.OnAllPlayersRotate.Normal += new Event<Player, RotateEventArgs>.EventHandler(OnRotate);
            Level.OnAllLevelsLoad.Normal += new Event<Level, LevelLoadEventArgs>.EventHandler(OnLevelLoad);
        }

        private readonly Regex Parser = new Regex("GET /([a-zA-Z0-9_]{1,16})(~motd)? .+", RegexOptions.Compiled);

        void OnLevelLoad(Level l, LevelLoadEventArgs args)
        {
            //The level loaded does not have a texture file, create it. TODO: Test
            if (CFGDict.GetIfExist<Level, CFGSettings>(l) == null)
            {
                CFGSettings s = new CFGSettings(l);
                CFGDict.CreateIfNotExist<Level, CFGSettings>(l, s);
            }
        }
        void OnIncomingData(Player p, PacketEventArgs args)
        {
            if (args.Data.Length < 0)
                return;

            if (args.Data[0] != (byte)'G')
                return;

            args.Cancel();
            var netStream = p.Client.GetStream();
            using (var Writer = new StreamWriter(netStream))
            {
                var line = Encoding.UTF8.GetString(args.Data, 0, args.Data.Length).Split('\n')[0];
                var match = Parser.Match(line);

                if (match.Success)
                {
                    var lvl = Level.FindLevel(match.Groups[1].Value);
                    var versionLine = Encoding.UTF8.GetString(args.Data, 0, args.Data.Length).Split('\n')[2];
                    var userNameLine = Encoding.UTF8.GetString(args.Data, 0, args.Data.Length).Split('\n')[3];
                    var version = versionLine.Remove(0, "X-WoM-Version: ".Length).Replace("\r", "");
                    var username = userNameLine.Remove(0, "X-WoM-Username: ".Length).Replace("\r", "");
                    Thread.Sleep(1500); //Trying to find player before it loads so wait.
                    var player = Player.Find(username);
                    if (player != null)
                    {
                        player.ExtraData.ChangeOrCreate<object, object>("UsingWoM", true);
                        if (!String.IsNullOrWhiteSpace(version))
                        {
                            player.ExtraData.ChangeOrCreate<object, object>("WoMVersion", version);
                            if (PluginSettings.GetSettingBoolean("notify-ops") == true)
                            {
                                Player.UniversalChatOps(username + " joined using " + version);
                                Logger.Log(username + " joined using " + version);
                            }
                        }
                    }
                    if (lvl == null)
                    {
                        Writer.Write("HTTP/1.1 404 Not Found");
                        Writer.Flush();
                    }
                    else
                    {
                        if (!lvl.ExtraData.ContainsKey("WoMConfig"))
                        {
                            Writer.Write("HTTP/1.1 500 Internal Server Error");
                            Writer.Flush();
                        }
                        else
                        {
                            var config = (string[])lvl.ExtraData["WoMConfig"];
                            var bytes = Encoding.UTF8.GetBytes(config.ToString<string>());
                            Writer.WriteLine("HTTP/1.1 200 OK");
                            Writer.WriteLine("Date: " + DateTime.UtcNow.ToString("R"));
                            Writer.WriteLine("Server: Apache/2.2.21 (CentOS)");
                            Writer.WriteLine("Last-Modified: " + DateTime.UtcNow.ToString("R"));
                            Writer.WriteLine("Accept-Ranges: bytes");
                            Writer.WriteLine("Content-Length: " + bytes.Length);
                            Writer.WriteLine("Connection: close");
                            Writer.WriteLine("Content-Type: text/plain");
                            Writer.WriteLine();
                            foreach (var entry in config)
                                Writer.WriteLine(entry);
                        }
                        CFGSettings a = (CFGSettings)CFGDict.GetIfExist<Level, CFGSettings>(p.Level);
                        WOM.SendDetail(player, a.GetSetting("detail.user")); //Send the detail with parsed variables.
                    }
                }

                else
                {
                    Writer.Write("HTTP/1.1 400 Bad Request");
                    Writer.Flush();
                }
            }
        }

        void OnOutgoingData(Player p, PacketEventArgs e)
        {
            if (e.Type == Packet.Types.MOTD)
            {
                string ip;
                if (Server.DebugMode)
                {
                    ip = "127.0.0.1";
                }
                else
                {
                    ip = InetUtils.GrabWebpage("http://www.mcforge.net/serverdata/ip.php");
                }
                Packet pa = new Packet();
                pa.Add(Packet.Types.MOTD);
                pa.Add((byte)7);
                pa.Add(ServerSettings.GetSetting("ServerName"), 64);
                pa.Add(ServerSettings.GetSetting("MOTD") + " &0cfg=" + ip + ":" + ServerSettings.GetSetting("Port") + "/" + p.Level.Name, 64);
                pa.Add((byte)0);
                e.Data = pa.bytes;
            }
            //Because this is way more fun and requires no edits to the core ~Gamemakergm
            else if (e.Type == Packet.Types.Message)
            {
                if (PluginSettings.GetSettingBoolean("joinleave-alert"))
                {
                    string incoming = Encoding.ASCII.GetString(e.Data).Trim();
                    //Logger.Log(incoming);
                    if (incoming.Contains("joined the game!"))
                    {
                        e.Cancel();
                        WOM.GlobalSendJoin(incoming.Substring(1, incoming.Length - incoming.IndexOf("joined the game!")));
                    }
                    else if (incoming.Contains("has disconnected"))
                    {
                        e.Cancel();
                        WOM.GlobalSendLeave(incoming.Substring(1, incoming.Length - incoming.IndexOf("has disconnected")));
                    }
                }
            }
            else { return; }
        }

        string SubstringLoop(int start)
        {
            int l = 19; //Length of substring
            if (start + l > compass.Length)
            {
                string sub = compass.Substring(start, compass.Length - start);
                sub += compass.Substring(0, l - (compass.Length - start));
                return sub;
            }
            return compass.Substring(start, l);
        }

        void OnRotate(Player p, RotateEventArgs args)
        {
            if (!(bool)(p.ExtraData.GetIfExist<object, object>("WoMCompass") ?? false)) { return; }
            else
            {
                WOM.SendDetail(p, "(" + SubstringLoop(p.Rot[0] / (int)(255 / (compass.Length - 1))) + ")");
            }
        }

        public static string ConvertVars(Player p, string detail)
        {
            StringBuilder sb = new StringBuilder(detail);
            sb.Replace("$name", ServerSettings.GetSettingBoolean("$Before$Name") ? "$" + p.Username : p.Username);
            sb.Replace("$color", p.Color);
            sb.Replace("$gcolor", p.Group.Color);
            sb.Replace("$server", ServerSettings.GetSetting("ServerName"));
            sb.Replace("$money", p.Money.ToString());
            sb.Replace("$" + Server.Moneys, p.Money.ToString());
            sb.Replace("$rank", p.Group.Name);
            sb.Replace("$ip", p.Ip);
            sb.Replace("$time", DateTime.Now.ToString("HH:mm tt"));
            sb.Replace("$date", DateTime.Today.ToShortDateString());
            sb.Replace("$x", p.Pos.x.ToString());
            sb.Replace("$z", p.Pos.z.ToString());
            sb.Replace("$y", p.Pos.y.ToString());
            sb.Replace("$level", p.Level.Name);
            sb.Replace("$world", p.Level.Name);
            sb.Replace("$yaw", p.Rot[0].ToString());
            sb.Replace("$pitch", p.Rot[1].ToString());
            sb.Replace("$belowPos", ((p.belowBlock).ToString()) ?? "Null.");
            if (detail.Contains("$ExtraData"))
            {
                string toCheck = detail.Substring(detail.IndexOf('[') + 1, detail.IndexOf(']') - detail.IndexOf('[') - 1);
                sb.Replace("$ExtraData[" + toCheck + "]", (string)(p.ExtraData.GetIfExist<object, object>(toCheck)));
            }
            return sb.ToString();
        }
    }
}
        #endregion


