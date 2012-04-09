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
using MCForge;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;
using System.Collections.Generic;

namespace CommandDll
{
    public class CmdPunished : ICommand
    {
        public string Name { get { return "Punished"; } }
        public CommandTypes Type { get { return CommandTypes.Information; } }
        public string Author { get { return "Givo"; } }
        public Version Version { get { return new Version(1,0); } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public static List<string> mutedlist = new List<string>();
        public static List<string> jokeredlist = new List<string>();
        public static bool yes = false;

        public void Use(Player p, string[] args)
        {
            if (args.Length < 1) { Help(p); }
            if (args[0] == "muted")
            {
                mutedlist.Clear();

                Server.ForeachPlayer(delegate(Player pl)
                {
                    if (pl.muted)
                    {
                        mutedlist.Add(pl.Username);
                        yes = true;
                    }
                });
                if (yes)
                {
                    p.SendMessage("Muted: ");
                }
                else
                {
                    p.SendMessage("No one is Muted");
                }
                foreach (string muted in mutedlist)
                {
                        p.SendMessage(muted);
                }
                yes = false;
                return;
            }
            else if (args[0] == "jokered")
            {
                Server.ForeachPlayer(delegate(Player pl)
                {
                    if (pl.jokered)
                    {
                        jokeredlist.Add(pl.Username);
                        yes = true;
                    }
                });
                if (yes)
                {
                    p.SendMessage("Jokered: ");
                }
                else
                {
                    p.SendMessage("No one is Jokered");
                    return;
                }
                foreach (string jokered in jokeredlist)
                {
                        p.SendMessage(jokered);
                }
                yes = false;
                return;
            }
            else { Help(p); }
            Help(p);
        }

        public void Help(Player p)
        {
            p.SendMessage("/punished <muted/jokered>- Displays punished players");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "Punished" });
        }
    }
}

