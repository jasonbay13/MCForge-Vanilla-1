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
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using System;
using MCForge.API.Events;
using MCForge.Groups;
using MCForge.World;

namespace MCForge.Commands
{
    public class CmdPerVisitMax : ICommand
    {
        public string Name { get { return "PerVisitMax"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "cazzar"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length < 2)
            {
                Help(p);
                return;
            }

            byte perVisitMax = byte.MaxValue;
            PlayerGroup g = null;
            Level l = null;
            l = Level.FindLevel(args[0]);

            if (l == null)
            {
                p.SendMessage("Level not found!");
                return; //no need to continue (troll)
            }

            try
            {
                perVisitMax = byte.Parse(args[1]);
            }
            catch
            {
                try
                {
                    g = PlayerGroup.Find(args[1]);
                    perVisitMax = g.Permission;
                }
                catch
                {
                    p.SendMessage("Error parsing new build permission");
                    return;
                }
            }

            if (perVisitMax > p.Group.Permission)
            {
                p.SendMessage("You cannot set the build permission to a greater rank or permission than yours");
                return;
            }

            if (l.ExtraData.ContainsKey("pervisitmax"))
            {
                l.ExtraData["pervisitmax"] = perVisitMax;
            }
            else
                l.ExtraData.Add("pervisitmax", perVisitMax);

            if (g != null)
            {
                p.SendMessage("Successfully put " + Colors.gold + l.Name + Server.DefaultColor + "'s max visit permission to " + g.Color + g.Name);
            }
            else
            {
                p.SendMessage("Successfully put " + Colors.gold + l.Name + Server.DefaultColor + "'s max visit permission to " + Colors.red + perVisitMax);
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/pervisit [map] [permission/rankname] - sets the minimum permission to visit map");
        }

        public void Initialize()
        {
            Command.AddReference(this, "pervisit");
            Player.OnAllPlayersCommand.Normal += new Event<Player, CommandEventArgs>.EventHandler(OnAllPlayersCommand_Normal);
        }
        public void OnAllPlayersCommand_Normal(Player sender, CommandEventArgs evt)
        {
            byte PerVisitMax = byte.MaxValue;
            Level l = null;

            ICommand cmdran = null;
            try
            {
                cmdran = Command.All[evt.Command];
            }
            catch { cmdran = null; }
            if (cmdran == null)
                return; // no use running this unless it exists
            else if (cmdran != Command.All["goto"])
                return; //yet again, no use to run this if the command aint /goto or a variant

            l = Level.FindLevel(evt.Args[0]);

            if (l.ExtraData.ContainsKey("pervisitmax"))
            {
                try
                {
                    PerVisitMax = (byte)l.ExtraData["pervisitmax"];
                }
                catch
                {
                    PerVisitMax = byte.MaxValue;
                }
            }

            if (sender.Group.Permission >= PerVisitMax)
            {
                sender.SendMessage("You cannot visit this map!");
                evt.Cancel();
            }
        }
    }
}
