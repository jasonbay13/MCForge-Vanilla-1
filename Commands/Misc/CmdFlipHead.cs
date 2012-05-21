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
using System.IO;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;

namespace CommandDll
{
    public class CmdFlipHead : ICommand
    {
        public string Name { get { return "fliphead"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Snowl"; } }
        public int Version { get { return 1; ; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0)
            {
                foreach (Player z in Server.Players.ToArray())
                {
                    z.IsHeadFlipped = !z.IsHeadFlipped;
                    if (z.IsHeadFlipped)
                        z.SendMessage("Your neck was broken!");
                    else
                        z.SendMessage("Your neck was mended!");
                }
            }
            else
            {
                Player z = Player.Find(args[0]);
                if (z == null)
                {
                    p.SendMessage("This player does not exist!");
                    return;
                }
                z.IsHeadFlipped = !z.IsHeadFlipped;
                if (z.IsHeadFlipped)
                    z.SendMessage("Your neck was broken!");
                else
                    z.SendMessage("Your neck was mended!");
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/fliphead - Flips all players heads (Toggled)");
            p.SendMessage("/fliphead <player> - Flips <player> head  (Toggled)");
            p.SendMessage("Shortcuts: /flipheads");
        }
        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "fliphead", "flipheads" });
        }
    }
}