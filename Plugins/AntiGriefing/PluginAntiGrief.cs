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
            Player.OnAllPlayersChat.Normal += new Event<Player, ChatEventArgs>.EventHandler(OnAllPlayersChat_Normal);
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

        void OnAllPlayersChat_Normal(Player sender, ChatEventArgs args) {

        }

        void OnAllPlayersBlockChange_Normal(Player sender, BlockChangeEventArgs args) {
            string username = Helper.GetOwner(new Vector3S(args.X, args.Z, args.Y), sender.Level.Name);
            if (username == null) //no owner, ADD ALL THE BLOCKS
                return;

            foreach (var value in AllowList) 
                if ((value.Key == username && value.Value.Contains(sender)) || sender.Group.Permission >= (byte)PermissionLevel.Operator)
                    return;

            sender.SendMessage("U silly head, you didnt make this. Ask " + username + " to add you to list");
            args.Cancel();

        }

        void OnAllPlayersConnect_Normal(Player sender, ConnectionEventArgs e) {
            if (!e.Connected)
                return;

            var pInfo = new PlayerInfo(sender);

            if (players.Contains(pInfo))
                return;

            players.Add(pInfo);
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