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
using System.Threading;
using MCForge.Interface;
using MCForge.Utils;
using MCForge.Utils.Settings;
using MCForge.Interfaces;
using MCForge.Entity;
using MCForge.Interface.Command;

namespace MCForge.Core {
    static class Program {
        private static void Logger_OnRecieveErrorLog(object sender, LogEventArgs e) {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] " + e.Message);
            Console.ForegroundColor = prevColor;
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            ServerSettings.Init();
            bool checker = CheckArgs(args);
            Console.Title = ServerSettings.GetSetting("ServerName") + " - MCForge 6"; //Don't know what MCForge version we are using yet.
            if (!checker)
                new Thread(new ThreadStart(Server.Init)).Start();
            else
                Logger.Log("Aborting Setup..", LogType.Critical);
            Logger.OnRecieveLog += new EventHandler<LogEventArgs>(Server.OnLog);
            Logger.OnRecieveErrorLog += new EventHandler<LogEventArgs>(Logger_OnRecieveErrorLog);
            cp = new ConsolePlayer(cio);
            while (true) {
                string input = Console.ReadLine();
                if (input == null) { Server.Stop(); return; }
                if (input.Trim().StartsWith("/")) {
                    if (input.Trim().Length > 1) {
                        string name = input.Trim().Substring(1);
                        ICommand c = Command.Find(name);
                        if (c != null) {
                            int i = input.Trim().IndexOf(" ");
                            if (i > 0) {
                                string[] cargs = input.Trim().Substring(i + 1).Split(' ');
                                c.Use(cp, cargs);
                            }
                            else {
                                c.Use(cp, new string[0]);
                            }
                        }
                    }
                }
                if (input.ToLower() == "!stop") {
                    Console.Write("Would you like to save all? [y/n]:");
                    if (Console.ReadLine().ToLower().StartsWith("y")) {
                        Server.SaveAll();
                        Server.Stop();
                    }
                    else {
                        Server.Stop();
                    }
                    return;
                }
                else if (input.ToLower() == "!copyurl") {
                    System.Windows.Forms.Clipboard.SetDataObject(Server.URL, true);
                }
                else if (input.ToLower().Split(' ')[0] == "!packets") {
                    string[] cargs = input.ToLower().Split(' ');
                    if (cargs.Length == 1) {
                        MCForge.Entity.Player.OnAllPlayersReceivePacket.Important += new API.Events.PacketEvent.EventHandler(OnPacket);
                        MCForge.Entity.Player.OnAllPlayersSendPacket.Important += new API.Events.PacketEvent.EventHandler(OnPacket);
                        MCForge.Entity.Player.OnAllPlayersReceiveUnknownPacket.Important += new API.Events.PacketEvent.EventHandler(OnPacket);
                    }
                    if (cargs.Length >= 2) {
                        switch (cargs[1]) {
                            case "stop":
                                MCForge.Entity.Player.OnAllPlayersReceivePacket.Important -= new API.Events.PacketEvent.EventHandler(OnPacket);
                                MCForge.Entity.Player.OnAllPlayersSendPacket.Important -= new API.Events.PacketEvent.EventHandler(OnPacket);
                                MCForge.Entity.Player.OnAllPlayersReceiveUnknownPacket.Important -= new API.Events.PacketEvent.EventHandler(OnPacket);
                                break;
                            case "hide":
                                if (cargs.Length >= 3) {
                                    byte t;
                                    if (Byte.TryParse(cargs[2], out t)) {
                                        if (!hidePackets.Contains(t)) {
                                            hidePackets.Add(t);
                                        }
                                    }
                                }
                                break;
                            case "unhide":
                                if (cargs.Length >= 3) {
                                    byte t;
                                    if (Byte.TryParse(cargs[2], out t)) {
                                        hidePackets.Remove(t);
                                    }
                                }
                                break;
                            case "cancel":
                                if (cargs.Length >= 3) {
                                    byte t;
                                    if (Byte.TryParse(cargs[2], out t)) {
                                        if (!cancelPackets.Contains(t)) {
                                            cancelPackets.Add(t);
                                        }
                                    }
                                }
                                break;
                            case "allow":
                                if (cargs.Length >= 3) {
                                    byte t;
                                    if (Byte.TryParse(cargs[2], out t)) {
                                        cancelPackets.Remove(t);
                                    }
                                }
                                break;
                            case "xth":
                                if (cargs.Length >= 4) {
                                    byte t;
                                    if (Byte.TryParse(cargs[2], out t)) {
                                        int x;
                                        if (Int32.TryParse(cargs[3], out x)) {
                                            if (reducePackets.ContainsKey(t)) {
                                                reducePackets[t] = x;
                                            }
                                            else reducePackets.Add(t, x);
                                            if (xthPackets.ContainsKey(t)) {
                                                xthPackets[t] = x; //The first packet is allowed
                                            }
                                            else xthPackets.Add(t, x);
                                        }
                                    }
                                }
                                break;
                            default:
                                Console.WriteLine("++ To hide/unhide ++\nhide [type number]\n unhide [type number]\n\n++ To cancel/allow ++\ncancel [type number]\nallow [type number]\n\n++ To handle only xth packet ++\nxth [type number] [xth packet number]");
                                break;
                        }
                    }
                }
                //You can use this to talk to the players, someone is probably going to improve this.
                else {
                    Player.UniversalChat(Colors.white + "[Console] " + Server.DefaultColor + input);
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                    Logger.Log("[Console] " + input, Color.Yellow, Color.Black, LogType.Normal);
                }

            }
        }
        private static ConsolePlayer cp;
        private static CommandIO cio = new CommandIO();
        private static List<byte> hidePackets = new List<byte>();
        private static List<byte> cancelPackets = new List<byte>();
        private static Dictionary<byte, int> reducePackets = new Dictionary<byte, int>();
        private static Dictionary<byte, int> xthPackets = new Dictionary<byte, int>();
        private static void OnPacket(MCForge.Entity.Player sender, API.Events.PacketEventArgs args) {
            if (cancelPackets.Contains((byte)args.Type)) {
                args.Cancel();
                return;
            }
            if (reducePackets.ContainsKey((byte)args.Type)) {
                if (xthPackets[(byte)args.Type] % reducePackets[(byte)args.Type] != 0) {
                    xthPackets[(byte)args.Type]++;
                    args.Cancel();
                    return;
                }
                else {
                    xthPackets[(byte)args.Type] = 1;
                }
            }
            if (hidePackets.Contains((byte)args.Type)) return;
            Logger.Log(((args.Incoming) ? "Incoming" : "Outgoing") + " Packet " + (Packet.Types)(byte)args.Type + "(" + (byte)args.Type + ") for player " + sender.Username, ((args.Incoming) ? Color.Green : Color.Red), Color.Black);
        }

        /// <summary>
        /// Check the args that were passed on program startup
        /// </summary>
        /// <param name="args">The args</param>
        /// <returns>If returns false, run normal setup. If returns true, cancel normal setup. In this case something has already started the server.</returns>
        static bool CheckArgs(string[] args) {
            if (args.Length == 0)
                return false;
            string name = args[0];
            switch (name) {
                case "load-plugin":
                    if (args.Length == 1)
                        return false;
                    string plugin = args[1];
                    string[] pargs = new string[] { "-force" };
                    for (int i = 1; i < args.Length; i++)
                        pargs[i] = args[i];
                    LoadAllDlls.LoadDLL(plugin, pargs);
                    break;
                case "debug":
                    Server.DebugMode = true;
                    return false;
                case "abort-setup":
                    return true;
            }
            return args[args.Length - 1] == "abort-setup";
        }
    }
    class CommandIO : IIOProvider {
        public string ReadLine() {
            if (Console.CursorLeft != 0) Console.WriteLine();
            Console.Write("CIO input: ");
            return Console.ReadLine();
        }

        public void WriteLine(string line) {
            if (Console.CursorLeft != 0) Console.WriteLine();
            Console.WriteLine("CIO output: " + line);
        }

        public void WriteLine(string line, string replyChannel)
        {
            
        }
    }
}
