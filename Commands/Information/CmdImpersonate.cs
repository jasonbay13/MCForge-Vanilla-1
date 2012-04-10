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
using MCForge.Interface.Command;

namespace CommandDll
{
    public class CmdImpersonate : ICommand
    {
        public string Name { get { return "Impersonate"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
        public string Author { get { return "Givo"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Use(Player p, string[] args)
        {
            if (args.Length == 0) { Help(p); return; }

           Player who = Player.Find(args[0]);

            string message = null;
            foreach (string s in args)
            {
                message += s + " ";
            }
            string newmessage = message.Remove(0, message.Split(' ')[0].Length + 1);
            if (!newmessage.EndsWith(" ")) { p.SendMessage("Please enter a message"); return; }
            if (who != null)
            {
                Player.UniversalChat(who.color + who.Username + "%f: " + newmessage);
            }
            else
            {
                Player.UniversalChat(args[0] + "%f: " + newmessage);
            }
        }
        public void Help(Player p)
        {
            p.SendMessage("/impersonate <player> <message> - Impersonates <player>.");
            p.SendMessage("Shortcut: /imp");
        }
        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "impersonate", "imp" });
        }
    }
}
