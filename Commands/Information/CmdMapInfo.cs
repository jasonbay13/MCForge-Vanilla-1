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
using MCForge.API.PlayerEvent;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.World;

namespace CommandDll
{
    public class CmdMapInfo : ICommand
    {
        public string Name { get { return "MapInfo"; } }
        public CommandTypes Type { get { return CommandTypes.information; } }
        public string Author { get { return "jasonbay13"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            Level l = args.Length != 0
                ? Level.Levels.Find(lev => { if (lev.Name.IndexOf(String.Join(" ", args)) != -1) return true; return false; })
                : p.Level;
            if (l == null) { p.SendMessage("Could not find specified level."); return; }

            p.SendMessage(String.Concat(Colors.yellow, "Map Name: ", Colors.white, l.Name));
            p.SendMessage(String.Concat(Colors.yellow, "Map Size: ", Colors.white, l.Size));
            p.SendMessage(String.Concat(Colors.yellow, "Total Blocks: ", Colors.white, l.TotalBlocks));
            p.SendMessage(String.Concat(Colors.yellow, "Spawn Pos: ", Colors.white, l.SpawnPos));
            p.SendMessage(String.Concat(Colors.yellow, "Physics Tick: ", Colors.white, l.PhysicsTick));
            p.SendMessage(String.Concat("To see a list of players currently on ", l.Name, ", type \"yes\"."));
            //OnPlayerChat.Register(plist, MCForge.API.Priority.Normal, l, p);
            OnPlayerChat.Register(plist, p).datapass = l;
        }

        private void plist(OnPlayerChat eventargs)
        {
            eventargs.Unregister();
            if (eventargs.message.ToLower() != "yes" || eventargs.Player.lastcmd != "mapinfo" && eventargs.Player.lastcmd != "mi")
                return;

            eventargs.Cancel();
            List<Player> templist = Server.Players.FindAll((p) => { if (p.Level == (Level)eventargs.datapass) return true; return false; });

            if (templist.Count == 0)
            {
                eventargs.Player.SendMessage("No one is on " + ((Level)eventargs.datapass).Name + ".");
                return;
            }
            if (templist.Count == 1 && eventargs.Player.Level == (Level)eventargs.datapass)
            {
                eventargs.Player.SendMessage("No one besides you is on " + ((Level)eventargs.datapass).Name + ".");
                return;
            }

            templist.ForEach((p) =>
            {
                eventargs.Player.SendMessage(String.Concat(p.titleColor, p.title, p.color, p.Username));
            });
        }

        public void Help(Player p)
        {
            p.SendMessage("/mapinfo [mapname] - Shows info of the map your currently on or mapname.");
            p.SendMessage("Shortcut: /mi");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "mapinfo", "mi" });
        }
    }
}
