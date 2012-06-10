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

namespace MCForge.Commands
{
    public class CmdDevs : ICommand
    {
        public string Name { get { return "Developers"; } }
        public CommandTypes Type { get { return CommandTypes.Information; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            string send = Colors.yellow + "MCForge Development Team: ";
            foreach (string s in Server.devs)
                send += s + Colors.white + ", ";
            p.SendMessage(send.Remove(send.Length - 2, 2));
        }

        public void Help(Player p)
        {
            p.SendMessage("/devs - Shows the MCForge Development Team");
            p.SendMessage("Shortcut: /developers");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[] { "developers", "devs" });
        }
    }
}
