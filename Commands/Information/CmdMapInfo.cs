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
using MCForge.API.Events;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.World;
using MCForge.Utils;

namespace CommandDll {
    public class CmdMapInfo : ICommand {
        public string Name { get { return "MapInfo"; } }
        public CommandTypes Type { get { return CommandTypes.Information; } }
        public string Author { get { return "jasonbay13"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args) {
            Level l = args.Length != 0
                ? Level.Levels.Find(lev => { return lev.Name.IndexOf(String.Join(" ", args)) != -1; })
                : p.Level;
            if (l == null) { p.SendMessage("Could not find specified level."); return; }

            p.SendMessage(String.Concat(Colors.yellow, "Map Name: ", Colors.white, l.Name));
            p.SendMessage(String.Concat(Colors.yellow, "Map Size: ", Colors.white, l.Size));
            p.SendMessage(String.Concat(Colors.yellow, "Total Blocks: ", Colors.white, l.TotalBlocks));
            p.SendMessage(String.Concat(Colors.yellow, "Spawn Pos: ", Colors.white, l.SpawnPos));
            p.SendMessage(String.Concat(Colors.yellow, "Physics Tick: ", Colors.white, l.PhysicsTick));
            p.SendMessage(String.Concat("To see a list of players currently on ", l.Name, ", type \"yes\"."));
            //OnPlayerChat.Register(plist, MCForge.API.Priority.Normal, l, p);
            p.OnPlayerChat.Normal += new ChatEvent.EventHandler(plist);
            p.SetDatapass("mapinfoLevel", l);
        }

        private void plist(Player sender, ChatEventArgs eventargs) {
            sender.OnPlayerChat.Normal -= new ChatEvent.EventHandler(plist);
            if (eventargs.Message.ToLower() != "yes" || sender.ExtraData.GetIfExist<object, object>("LastCmd") != "mapinfo" && sender.ExtraData.GetIfExist("LastCmd") != "mi")
                return;

            eventargs.Cancel();
            Level l = (Level)sender.GetDatapass("mapinfoLevel");
            List<Player> templist = Server.Players.FindAll((p) => { return p.Level == l; });

            if (templist.Count == 0) {
                sender.SendMessage("No one is on " + l.Name + ".");
                return;
            }
            if (templist.Count == 1 && sender.Level == l) {
                sender.SendMessage("No one besides you is on " + l.Name + ".");
                return;
            }

            templist.ForEach((p) => {
                sender.SendMessage(String.Concat((string)p.ExtraData.GetIfExist("Color"), p.Username));
            });
        }

        public void Help(Player p) {
            p.SendMessage("/mapinfo [mapname] - Shows info of the map your currently on or mapname.");
            p.SendMessage("Shortcut: /mi");
        }

        public void Initialize() {
            Command.AddReference(this, new string[] { "mapinfo", "mi" });
        }
    }
}
