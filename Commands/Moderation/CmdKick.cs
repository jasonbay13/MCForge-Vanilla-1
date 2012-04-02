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
using MCForge;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;

namespace CommandDll
{
    public class CmdKick : ICommand
    {
        public string Name { get { return "Kick"; } }
        public CommandTypes Type { get { return CommandTypes.mod; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0) { Help(p); return; }
            Player who = Player.Find(args[0]); args[0] = "";
            string message = null;
            if (args.Length == 1) { message = "Kicked by " + p.USERNAME; }
            else { foreach (string a in args) { message += a + " "; } }
            if (Server.devs.Contains(who.USERNAME)) { p.SendMessage("You can't kick a MCForge Developer!"); return; }
            who.Kick(message);
            Player.UniversalChat(who.USERNAME + " was kicked by " + p.USERNAME);
        }

        public void Help(Player p)
        {
            p.SendMessage("/kick <player> [message] - kicks a player with an optional message!");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "kick", "k" });
        }
    }
}

