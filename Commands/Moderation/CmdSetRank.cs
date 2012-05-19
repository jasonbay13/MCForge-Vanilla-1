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
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Groups;

namespace CommandDll
{
    public class CmdSetRank : ICommand
    {
        public string Name { get { return "SetRank"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "cazzar"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Use(Player p, string[] args)
        {
            if (args.Length != 2)
                Help(p);

            Player who = null;
            who = Player.Find(args[0]);
            if (who == null)
            {
                p.SendMessage("Player not found");
                return;
            }

            PlayerGroup group = null;
            group = PlayerGroup.Find(args[1]);

            if (group == null)
            {
                p.SendMessage("Rank not found");
                return;
            }

           /* if (who.Group.Permission >= p.Group.Permission)
            {
                p.SendMessage("You cannot change the rank of someone of an equal or greater rank!");
                return;
            }
            if (group.Permission >= p.Group.Permission)
            {
                p.SendMessage("You cannot promote someone to an equal or greater rank!");
                return;
            }
            if (who.Group == group)
            {
                p.SendMessage(group.Colour + who.Username + Server.DefaultColor + " is already that rank");
                return;
            }*/
            group.AddPlayer(who);
            Player.UniversalChat(group.Colour + who.Username + Server.DefaultColor + " had their rank set to " + group.Colour + group.Name);

        }

        public void Help(Player p)
        {
            p.SendMessage("/setrank <player> <rank> - changes the rank of the specified player.");
            p.SendMessage("Shortcut: /rank");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "setrank", "rank" });
        }
    }
}
