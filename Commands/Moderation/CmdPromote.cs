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
using MCForge.Entity;
using MCForge.Groups;
using MCForge.Interface.Command;

namespace CommandDll
{
    public class CmdPromote : ICommand
    {
        public string Name { get { return "Promote"; } }
        public CommandTypes Type { get { return CommandTypes.mod; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0) { Help(p); return; }
            Player who = Player.Find(args[0]);
            if (who == null) { p.SendMessage("Cannot find player!"); return; }
            if (who == p) { p.SendMessage("Cannot demote yourself!"); return; }
            PlayerGroup current = who.group;
            bool next = false;
            foreach (PlayerGroup rank in PlayerGroup.groups)
            {
                if (rank == current) { next = true; continue; }
                if (next) 
                {
                    string[] info = new string[2] { who.Username, rank.name };
                    Command.Find("setrank").Use(p, info);
                    break;
                }
            }         
        }

        public void Help(Player p)
        {
            p.SendMessage("/promote <player> - promotes a player.");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "promote" });
        }
    }
}

