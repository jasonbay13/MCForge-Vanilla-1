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
using MCForge.Interface.Plugin;
using MCForge.Entity;
using System.Net.Sockets;
using MCForge.API.Events;
using MCForge.Utils;
using Plugins.AntiGriefing;
using MCForge.Groups;

namespace Plugins.AntiGriefingPlugin {
    public class PluginAntiGrief : IPlugin {
        private readonly List<PlayerInfo> players = new List<PlayerInfo>();
        private readonly Dictionary<string, IList<Player>> AllowList = new Dictionary<string, IList<Player>>();
        const string caps = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ";
        const string nocaps = "abcdefghijklmnopqrstuvwxyz ";
        #region IPlugin Members

        public string Name {
            get { return "AntiGrief"; }
        }

        public string Author {
            get { return "headdetect and hypereddie10"; }
        }

        public int Version {
            get { return 1; }
        }

        public string CUD {
            get { throw new NotImplementedException(); }
        }

        public void OnLoad(string[] args) {
            Player.OnAllPlayersConnect.Normal += new MCForge.API.Events.Event<Player, MCForge.API.Events.ConnectionEventArgs>.EventHandler(OnAllPlayersConnect_Normal);
            Player.OnAllPlayersBlockChange.Normal += new Event<Player, BlockChangeEventArgs>.EventHandler(OnAllPlayersBlockChange_Normal);
            //Player.OnAllPlayersChat.Normal += new Event<Player, ChatEventArgs>.EventHandler(OnAllPlayersChat_Normal);
            Player.OnAllPlayersCommand.Normal += new Event<Player, CommandEventArgs>.EventHandler(OnAllPlayersCommand_Normal);
        }

        void OnAllPlayersCommand_Normal(Player sender, CommandEventArgs args) {
            if (args.Command != "ag")
                return;

            args.Cancel();

            if (args.Args.Length < 2) {
                Help(sender);
                return;
            }

            if (args.Args[0].ToLower() == "allow") {
                Player who = Player.Find(args.Args[1]);

                if (who == null || who is ConsolePlayer) {
                    sender.SendMessage("The specified player was not found");
                    return;
                }

                AllowList.AddValue<string, Player>(sender.Username, who);
                return;
            }

            else if (args.Args[0].ToLower() == "disallow") {
                Player who = Player.Find(args.Args[1]);

                if (who == null || who is ConsolePlayer) {
                    sender.SendMessage("The specified player was not found");
                    return;
                }

                AllowList.RemoveValue<string, Player>(sender.Username, who);
                return;
            }
        }

        void Help(Player p) {
            p.SendMessage("/ag allow <player> - allows the specified <player> to modify your blocks");
            p.SendMessage("/ag disallow <player> - disallows the specified <player> to modify your blocks");
        }

        void OnAllPlayersChat_Normal(Player p, ChatEventArgs args) {
            #region Spam Check
            if (hasPlayerInfo(p)) {
                PlayerInfo pi = getPlayerInfo(p);
                if (pi.LastMessage.ToLower() == args.Message.ToLower())
                    pi.offense++;
                else {
                    pi.LastMessage = args.Message;
                    pi.offense--;
                }
            }
            #endregion

            #region Caps Check
            int rage = 0;
            bool skip = false;
        goagain: //EWW LABELS
            string newmessage = "";
            string message = args.Message;
            for (int i = 0; i < message.Length; i++) {
                char c = message[i];
                if (caps.IndexOf(c) != -1)
                    rage++;
                else
                    rage--;
                if (rage >= 5 && caps.IndexOf(c) != -1)
                    c = nocaps[caps.IndexOf(c)];
                newmessage += c;
            }
            if (rage == message.Length) {
                skip = true;
                p.SendMessage("Lay off the caps :/");
                goto goagain;
            }
            else if (rage >= 7 && !skip) {
                p.SendMessage("Lay off the caps :/");
                if (!hasPlayerInfo(p)) {
                    PlayerInfo pi = new PlayerInfo(p);
                    pi.LastMessageSent = DateTime.Now;
                    pi.LastMessage = message;
                    players.Add(pi);
                }
                else {
                    PlayerInfo pi = getPlayerInfo(p);
                    pi.offense++;
                }
            }
            args.Message = newmessage;
            #endregion

            if (hasPlayerInfo(p) && getPlayerInfo(p).kicked)
                args.Cancel();
        }
        void OnAllPlayersBlockChange_Normal(Player sender, BlockChangeEventArgs args) {

            string username = Helper.GetOwner(new Vector3S(args.X, args.Z, args.Y), sender.Level.Name);
            if (username == null) //no owner, ADD ALL THE BLOCKS
                return;

            if ((AllowList.Count == 0 && username != sender.Username) || (AllowList.ContainsKey(username) &&
                !AllowList[username].Contains(sender) &&
                sender.Group.Permission > (byte)PermissionLevel.Operator &&
                args.Action != ActionType.Place)) {

                sender.SendMessage("U silly head, you didnt make this. Ask " + username + " to add you to list");
                args.Cancel();
            }

            if (!hasPlayerInfo(sender)) {
                players.Add(new PlayerInfo(sender));
                return;
            }
            PlayerInfo pi = getPlayerInfo(sender);
            if (pi.LastBlock == args.Holding && args.Action == ActionType.Place) {
                if (pi.LastPlace.AddMilliseconds(1000) > DateTime.Now)
                    pi.offense++;
                else
                    pi.offense--;
                if (getPlayerInfo(sender).kicked)
                    args.Cancel();
            }
        }

        void OnAllPlayersConnect_Normal(Player sender, ConnectionEventArgs e) {
            if (!e.Connected)
                return;

            if (hasPlayerInfo(sender))
                return;

            players.Add(new PlayerInfo(sender));
        }

        public PlayerInfo getPlayerInfo(Player p) {
            PlayerInfo toreturn = null;
            players.ForEach(pi => {
                if (pi.Player == p)
                    toreturn = pi;
            });
            return toreturn;
        }

        public bool hasPlayerInfo(Player p) {
            return getPlayerInfo(p) != null;
        }

        public void OnUnload() {
            players.Clear();

            Player.OnAllPlayersChat.Normal -= OnAllPlayersChat_Normal;
            Player.OnAllPlayersConnect.Normal -= OnAllPlayersConnect_Normal;
            Player.OnAllPlayersBlockChange.Normal -= OnAllPlayersBlockChange_Normal;

        }

        #endregion
    }
}