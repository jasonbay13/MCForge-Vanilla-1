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

namespace MCForge.Commands
{
    public class CmdPerBuild : ICommand
    {
        public string Name { get { return "PerBuild"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "cazzar"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length < 1)
            {
                Help(p);
                return;
            }

            byte perBuild = 0;
            PlayerGroup g = null;

            try
            {
                perBuild = byte.Parse(args[0]);
            }
            catch
            {
                try
                {
                    g = PlayerGroup.Find(args[0]);
                    perBuild = g.Permission;
                }
                catch
                {
                    p.SendMessage("Error parsing new build permission");
                    return;
                }
            }

            if (perBuild > p.Group.Permission)
            {
                p.SendMessage("You cannot set the build permission to a greater rank or permission than yours");
                return;
            }

            if (p.Level.ExtraData.ContainsKey("perbuild"))
            {
                p.Level.ExtraData["perbuild"] = perBuild;
            }
            else
                p.Level.ExtraData.Add("perbuild", perBuild);

            if (g != null)
            {
                p.SendMessage("Successfully put " + Colors.gold + p.Level.Name + Server.DefaultColor + "'s build permission to " + g.Color + g.Name);
            }
            else
            {
                p.SendMessage("Successfully put " + Colors.gold + p.Level.Name + Server.DefaultColor + "'s build permission to " + Colors.red + perBuild);
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/perbuild [permission/rankname] - sets the minimum permission to build on the current map");
        }

        public void Initialize()
        {
            Command.AddReference(this, "perbuild");
            Player.OnAllPlayersBlockChange.Important += new Event<Player, BlockChangeEventArgs>.EventHandler(OnAllPlayersBlockChange);
            Player.OnAllPlayersCommand.Important += new Event<Player, CommandEventArgs>.EventHandler(OnCommand);
        }
        public void OnAllPlayersBlockChange(Player sender, BlockChangeEventArgs evt)
        {
            byte perBuild = 0;

            if (sender.Level.ExtraData.ContainsKey("perbuild"))
            {
                try
                {
                    perBuild = (byte)sender.Level.ExtraData["perbuild"];
                }
                catch
                {
                    perBuild = 0;
                }
            }

            if (sender.Group.Permission < perBuild)
            {
                sender.SendMessage("You cannot build here");
                evt.Cancel();
            }
        }

        public void OnCommand(Player sender, CommandEventArgs e)
        {
            ICommand cmd = null;
            try
            {
                cmd = Command.All[e.Command];
            }
            catch
            {
                return;
            }

            byte perBuild = 0;

            if (sender.Level.ExtraData.ContainsKey("perbuild"))
            {
                try
                {
                    perBuild = (byte)sender.Level.ExtraData["perbuild"];
                }
                catch
                {
                    perBuild = 0;
                }
            }

            if (cmd == null) return;

            if (cmd.Type == CommandTypes.Building && sender.Group.Permission < perBuild)
            {
                sender.SendMessage("You cannot use building commands on this level!");
                e.Cancel();
            }
        }
    }
}
