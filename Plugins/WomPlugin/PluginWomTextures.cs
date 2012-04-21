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
*/﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge;
using System.Timers;
using MCForge.Interface.Plugin;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Utilities.Settings;
using System.IO;
using MCForge.API.System;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using MCForge.API.PlayerEvent;
using System.Threading;
using MCForge.API.SystemEvent;
using MCForge.World;

namespace Plugins.WomPlugin {
    public class PluginWomTextures : IPlugin {

        public string Name {
            get { return "WomTextures"; }
        }

        public string Author {
            get { return "headdetect"; }
        }

        public int Version {
            get { return 1; }
        }

        public string CUD {
            get { return "com.headdetect.womtextures"; }
        }

        private WomSettings WomSettings { get; set; }
        public void OnLoad() {
            WomSettings = new WomSettings();
            WomSettings.OnLoad();
            OnReceivePacket.Register(OnData);
            OnPlayerChatRaw.Register((args) => SendDetailToPlayer(args.Player, "This is a detail, deal &4With &3It"));
        }

        private readonly Regex Parser = new Regex("GET /([a-zA-Z0-9_]{1,16})(~motd)? .+", RegexOptions.Compiled);
        void OnData(OnReceivePacket args) {
            if (args.Data.Length < 0)
                return;
            if (args.Data[0] != (byte)'G')
                return;


            args.IsCanceled = true;
            var netStream = new NetworkStream(args.Player.Socket);
            using(var Reader = new StreamReader(netStream)) //Not used but it likes it...
            using (var Writer = new StreamWriter(netStream)) {
                var line = Encoding.UTF8.GetString(args.Data, 0, args.Data.Length).Split('\n')[0];
                
                var match = Parser.Match(line);

                if (match.Success) {
                    var lvl = Level.FindLevel(match.Groups[1].Value);
                    var userNameLine = Encoding.UTF8.GetString(args.Data, 0, args.Data.Length).Split('\n')[3];
                    var username = userNameLine.Remove(0, "X-WoM-Username: ".Length).Replace("\r", "");
                    Thread.CurrentThread.Join(2000);
                    var player = Player.Find(username);
                    if (player != null)
                        player.ExtraData.Add("UsingWom", true);

                    if (lvl == null) {
                        Writer.Write("HTTP/1.1 404 Not Found");
                        Writer.Flush();
                    }
                    else {
                        if (!lvl.ExtraData.ContainsKey("WomConfig")) {
                            Writer.Write("HTTP/1.1 Internal Server Error");
                            Writer.Flush();
                        }
                        else {
                            var Config = (string)lvl.ExtraData["WomConfig"];
                            var bytes = Encoding.UTF8.GetBytes(Config);
                            Writer.Write("HTTP/1.1 200 OK");
                            Writer.WriteLine("Date: " + DateTime.UtcNow.ToString("R"));
                            Writer.WriteLine("Server: Apache/2.2.21 (CentOS)");
                            Writer.WriteLine("Last-Modified: " + DateTime.UtcNow.ToString("R"));
                            Writer.WriteLine("Accept-Ranges: bytes");
                            Writer.WriteLine("Content-Length: " + bytes.Length);
                            Writer.WriteLine("Connection: close");
                            Writer.WriteLine("Content-Type: text/plain");
                            Writer.WriteLine();
                            Writer.WriteLine(Config);
                        }
                    }

                }

                else {
                    Writer.Write("HTTP/1.1 400 Bad Request");
                    Writer.Flush();
                }
            }
            args.Player.Kick("");
        }
        public void OnUnload() {
            throw new NotImplementedException();
        }

        #region Static Helper Methods


        /// <summary>
        /// Send a detail message to any player with wom.
        /// If user is not using wom, message will be ignored
        /// </summary>
        /// <param name="player">Player to send detail to</param>
        /// <param name="detail">Detail message to send </param>
        public static void SendDetailToPlayer(Player player, string detail) {
            if (!player.ExtraData.ContainsKey("UsingWom"))
                return;
            player.SendMessage(String.Format("^detail.user={0}", detail));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detail"></param>
        public static void SendDetailToAll(string detail) {
            Server.Players.ForEach(p => SendDetailToPlayer(p, detail));
        }
        #endregion
    }
}