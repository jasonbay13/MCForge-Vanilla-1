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
using System.Timers;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Threading;
using MCForge;
using MCForge.API;
using MCForge.API.Events;
using MCForge.World;
using MCForge.Interface.Plugin;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Utils.Settings;
using MCForge.Utils;
namespace Plugins.WomPlugin
{
    public class PluginWomTextures : IPlugin
    {

        public string Name
        {
            get { return "WomTextures"; }
        }

        public string Author
        {
            get { return "headdetect and Gamemakergm"; }
        }

        public int Version
        {
            get { return 1; }
        }

        public string CUD
        {
            get { return "com.headdetect.womtextures"; }
        }

        private WomSettings WomSettings { get; set; }
        public void OnLoad(string[] args1)
        {
            Server.OnServerFinishSetup += new Server.ServerFinishSetup(OnLoadDone);
        }
        void OnLoadDone()
        {
            MCForge.Utils.Logger.Log("[WomTextures] Succesfully initiated!");
            WomSettings = new WomSettings();
            WomSettings.OnLoad();

            //Player.OnReceivePacket.Normal += new Event<Player,ReceivePacketEventArgs>.EventHandler(Magic);
            Player.OnAllPlayersReceivePacket.Normal += new Event<Player, PacketEventArgs>.EventHandler(OnData);
            //Player.OnAllPlayersReceiveUnknownPacket.Normal += new Event<Player, PacketEventArgs>.EventHandler(OnUnknown);

            Player.OnAllPlayersChat.Normal += ((sender, args) =>
                SendDetailToPlayer(sender, "This is a detail, deal &4With &3It"));
        }

        void OnUnknown(Player p, PacketEventArgs args)
        {
            Logger.Log("[UnknownWoM] " + args.Type);
            string meep = "";
            foreach (var data in args.Data)
            {
                meep += data + ", ";
            }
            ASCIIEncoding enc = new ASCIIEncoding();
            string incomingText = enc.GetString(args.Data);
            meep += (Environment.NewLine + "[UnknownWoM]IncomingText: " + incomingText);
            Logger.Log(meep);
        }

        private readonly Regex Parser = new Regex("GET /([a-zA-Z0-9_]{1,16})(~motd)? .+", RegexOptions.Compiled);

        void OnData(Player p, PacketEventArgs args)
        {
            if (args.Data.Length < 0)
            {
                return;
            }
            else if (args.Data[0] != (byte)'G')
            {
                return;
            }
            else
            {

                args.Cancel();
                ServeCfg(p, args.Data);
            }
            if (args.Type == Packet.Types.MOTD)
            {
                string ip = "";
                using (System.Net.WebClient wb = new System.Net.WebClient())
                {
                    ip = wb.DownloadString("http://www.mcforge.net/serverdata/ip.php");
                }
                args.Cancel();
                Packet pa = new Packet();
                pa.Add(Packet.Types.MOTD);
                pa.Add((byte)7);
                pa.Add("har har", 64);
                pa.Add(ServerSettings.GetSetting("MOTD") + "&0cfg=" + ip + ":" + ServerSettings.GetSetting("Port") + "/" + p.Level.Name + "~motd", 64);
                pa.Add((byte)0);
                p.SendPacket(pa);
            }

        }
        
        void ServeCfg(Player p, byte[] buffer)
        {
            var netStream = new NetworkStream(p.Socket);
            using (var Reader = new StreamReader(netStream)) //Not used but it likes it...
            using (var Writer = new StreamWriter(netStream))
            {
                var line = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Split('\n')[0];
                var match = Parser.Match(line);

                if (match.Success)
                {
                    Logger.Log("[WoM] Match!");
                    var lvl = Level.FindLevel(match.Groups[1].Value);
                    var userNameLine = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Split('\n')[3];
                    var username = userNameLine.Remove(0, "X-WoM-Username: ".Length).Replace("\r", "");
                    Thread.CurrentThread.Join(2000);
                    var player = Player.Find(username);
                    if (player != null)
                        player.ExtraData.Add("UsingWom", true);

                    if (lvl == null)
                    {
                        Writer.Write("HTTP/1.1 404 Not Found");
                        Writer.Flush();
                    }
                    else
                    {
                        if (!lvl.ExtraData.ContainsKey("WomConfig"))
                        {
                            Writer.Write("HTTP/1.1 Internal Server Error");
                            Writer.Flush();
                        }
                        else
                        {
                            var Config = (string)lvl.ExtraData["WomConfig"];
                            var bytes = Encoding.UTF8.GetBytes(Config);
                            Writer.WriteLine("HTTP/1.1 200 OK");
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

                else
                {
                    Writer.Write("HTTP/1.1 400 Bad Request");
                    Writer.Flush();
                }
            }
            //p.Kick("");
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }

        #region Static Helper Methods


        /// <summary>
        /// Send a detail message to any player with wom.
        /// If user is not using wom, message will be ignored
        /// </summary>
        /// <param name="player">Player to send detail to</param>
        /// <param name="detail">Detail message to send </param>
        public static void SendDetailToPlayer(Player player, string detail)
        {
            if (!player.ExtraData.ContainsKey("UsingWom"))
                return;
            player.SendMessage(String.Format("^detail.user={0}", detail));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detail"></param>
        public static void SendDetailToAll(string detail)
        {
            Server.Players.ForEach(p => SendDetailToPlayer(p, detail));
        }
        #endregion
    }
}