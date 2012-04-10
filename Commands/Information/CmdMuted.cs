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
using System.Collections.Generic;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;

namespace CommandDll
{
    public class CmdMuted : ICommand
    {
        public string Name { get { return "Muted"; } }
        public CommandTypes Type { get { return CommandTypes.information; } }
        public string Author { get { return "Givo"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public static List<string> mutedlist = new List<string>();

        public void Use(Player p, string[] args)
        {
            mutedlist.Clear();

            if (args.Length > 0) { Help(p); }

			Server.ForeachPlayer(delegate(Player pl)
			{
				if (pl.muted)
				{
					mutedlist.Add(pl.Username);
				}
			});
            p.SendMessage("Muted: ");
            foreach (string muted in mutedlist)
            {
                p.SendMessage(muted);
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/mute - Displays muted players");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "muted" });
        }
    }
}

