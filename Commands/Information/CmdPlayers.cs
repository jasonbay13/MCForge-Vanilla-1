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
using MCForge.Groups;
using MCForge.Interface.Command;

namespace CommandDll
{
    public class CmdPlayers : ICommand
    {
        public string Name { get { return "Players"; } }
        public CommandTypes Type { get { return CommandTypes.information; } }
        public string Author { get { return "Arrem"; } }
        public decimal Version { get { return 1.00m; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            foreach (PlayerGroup group in PlayerGroup.groups)
            {
                string send = group.color + group.name;
                if (!send.EndsWith("ed") && !send.EndsWith("s")) { send += "s: " + Server.DefaultColor; } //Plural
                else { send += ": " + Server.DefaultColor; }
                Server.ForeachPlayer(delegate(Player pl)
                    {
                        if (pl.group.permission == group.permission) { send +=  pl.Username + "&a, " + Server.DefaultColor; }
                    });
                p.SendMessage(send.Trim().Remove(send.Length - 4, 4));
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/players - shows the online players");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "players", "online" });
        }
    }
}
