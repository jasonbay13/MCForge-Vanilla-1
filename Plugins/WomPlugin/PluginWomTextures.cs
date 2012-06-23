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
        private WoMSettings WomSettings { get; set; }
        private WoMPluginSettings PluginSettings { get; set; }
        #endregion
        #region WoM Handling
        //Wait for server to finish so we make sure all of the levels are loaded
        void OnLoadDone()
        {
            Logger.Log("[WoMTextures] Succesfully initiated!");
            WomSettings = new WoMSettings();
            WomSettings.OnLoad();

            PluginSettings = new WoMPluginSettings();
            PluginSettings.OnLoad();

            Server.OnServerFinishSetup -= OnLoadDone;
            Player.OnAllPlayersReceiveUnknownPacket.Normal += new Event<Player, PacketEventArgs>.EventHandler(OnIncomingData);
            Player.OnAllPlayersSendPacket.Normal += new Event<Player, PacketEventArgs>.EventHandler(OnOutgoingData);
        }

        private readonly Regex Parser = new Regex("GET /([a-zA-Z0-9_]{1,16})(~motd)? .+", RegexOptions.Compiled);

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
                            var Config = (string[])lvl.ExtraData["WoMConfig"];
                            var bytes = Encoding.UTF8.GetBytes(Config.ToString<string>());
                            Writer.WriteLine("HTTP/1.1 200 OK");
                            Writer.WriteLine("Date: " + DateTime.UtcNow.ToString("R"));
                            Writer.WriteLine("Server: Apache/2.2.21 (CentOS)");
                            Writer.WriteLine("Last-Modified: " + DateTime.UtcNow.ToString("R"));
                            Writer.WriteLine("Accept-Ranges: bytes");
                            Writer.WriteLine("Content-Length: " + bytes.Length);
                            Writer.WriteLine("Connection: close");
                            Writer.WriteLine("Content-Type: text/plain");
                            Writer.WriteLine();
                            foreach (var entry in Config)
                                Writer.WriteLine(entry);
                        }
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
#if DEBUG
                string ip = "127.0.0.1";
#else
            string ip = InetUtils.GrabWebpage("http://www.mcforge.net/serverdata/ip.php");
#endif
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
                    Logger.Log(incoming);
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
        #endregion
    }
}