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
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge;

namespace CommandDll
{
    public class CmdBan : ICommand
    {
        string _Name = "ban";
        public string Name { get { return _Name; } }

        CommandTypes _Type = CommandTypes.mod;
        public CommandTypes Type { get { return _Type; } }

        string _Author = "cazzar";
        public string Author { get { return _Author; } }

        int _Version = 1;
        public int Version { get { return _Version; } }

        string _CUD = "";
        public string CUD { get { return _CUD; } }

        string[] CommandStrings = new string[1] { "ban" };

        public void Use(Player p, string[] args)
        {
            if (args.Length >= 1)
            {
                if (args.Length == 1)
                    args[2] = "Banned!";

                Player who = Player.Find(args[0]);
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/ban <player> [reason] - Bans the player by name only");
            p.SendMessage("/ban @<player> [reason] - Bans the player by name, IP and kicks them");
            p.SendMessage("/ban #<player> [reason] - Stealth bans the player by name, IP and kicks them");
        }

        public void Initialize()
        {
            Command.AddReference(this, CommandStrings);
        }
    }
}
