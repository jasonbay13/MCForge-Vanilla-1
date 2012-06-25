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
    public class CmdPerBuildMax : ICommand
    {
        public string Name { get { return "PerBuildMax"; } }
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

            byte perBuildMax = byte.MaxValue;
            PlayerGroup g = null;

            try
            {
                perBuildMax = byte.Parse(args[0]);
            }
            catch
            {
                try
                {
                    g = PlayerGroup.Find(args[0]);
                    perBuildMax = g.Permission;
                }
                catch
                {
                    p.SendMessage("Error parsing new max build permission");
                    return;
                }
            }

            if (perBuildMax > p.Group.Permission)
            {
                p.SendMessage("You cannot set the max build permission to a greater rank or permission than yours");
                return;
            }

            if (p.Level.ExtraData.ContainsKey("perbuild"))
            {
                p.Level.ExtraData["perbuildmax"] = perBuildMax;
            }
            else
                p.Level.ExtraData.Add("perbuildmax", perBuildMax);

            if (g != null)
            {
                p.SendMessage("Successfully put " + Colors.gold + p.Level.Name + Server.DefaultColor + "'s maximum build permission to " + g.Color + g.Name);
            }
            else
            {
                p.SendMessage("Successfully put " + Colors.gold + p.Level.Name + Server.DefaultColor + "'s maximum build permission to " + Colors.red + perBuildMax);
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/perbuildmax [permission/rankname] - sets the maximum permission to build on the current map");
        }

        public void Initialize()
        {
            Command.AddReference(this, "perbuildmax");
            Player.OnAllPlayersBlockChange.Normal += new Event<Player, BlockChangeEventArgs>.EventHandler(OnAllPlayersBlockChange_Normal);
        }
        public void OnAllPlayersBlockChange_Normal(Player sender, BlockChangeEventArgs evt)
        {
            byte perBuildMax = byte.MaxValue;

            if (sender.Level.ExtraData.ContainsKey("perbuildmax"))
            {
                try
                {
                    perBuildMax = (byte)sender.Level.ExtraData["perbuildmax"];
                }
                catch
                {
                    perBuildMax = byte.MaxValue;
                }
            }

            if (sender.Group.Permission > perBuildMax)
            {
                sender.SendMessage("You cannot build here");
                evt.Cancel();
            }
        }
    }
}
