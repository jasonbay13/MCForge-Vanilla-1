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
using System.Linq;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.World;
using MCForge.Core;
using MCForge.API.PlayerEvent;

namespace CommandDll
{
    public class CmdMapInfo : ICommand
    {
        public string Name { get { return "MapInfo"; } }
        public CommandTypes Type { get { return CommandTypes.Information; } }
        public string Author { get { return "jasonbay13"; } }
        public Version Version { get { return new Version(1, 0, 0, 0); } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            Level l = args.Length != 0
                ? Level.levels.Find((lev) => { if (lev.Name.IndexOf(String.Join(" ", args)) != -1) return true; return false; })
                : p.Level;

            p.SendMessage(String.Concat(Colors.yellow, "Map Name: ", Colors.white, l.Name));
            p.SendMessage(String.Concat(Colors.yellow, "Map Size: ", Colors.white, l.Size));
            p.SendMessage(String.Concat(Colors.yellow, "Total Blocks: ", Colors.white, l.TotalBlocks));
            p.SendMessage(String.Concat(Colors.yellow, "Spawn Pos: ", Colors.white, l.SpawnPos));
            p.SendMessage(String.Concat(Colors.yellow, "Physics Tick: ", Colors.white, l.PhysicsTick));
            p.SendMessage(String.Concat("To see a list of players currently on ", l.Name, ", type \"yes\"."));
            OnPlayerChat.Register(plist, MCForge.API.Priority.Normal, l, p);
        }

        private void plist(OnPlayerChat eventargs)
        {
            eventargs.Unregister(true);
            if (eventargs.Message.ToLower() != "yes") return;
            eventargs.IsCanceled = true;
            Server.Players.FindAll((p) => { if (p.Level == (Level)eventargs.GetData()) return true; return false; }).ForEach((p) =>
            {
                eventargs.Player.SendMessage(String.Concat(p.titleColor, p.title, p.color, p.Username));
            });
        }
        public void Help(Player p)
        {
            p.SendMessage("/mapinfo (mapname) - Shows info of the map your currently on or mapname.");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "mapinfo", "mi" });
        }
    }
}
