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

namespace CommandDll
{
    public class CmdVoice : ICommand
    {
        public string Name { get { return "Voice"; } }
        public CommandTypes Type { get { return CommandTypes.mod; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            Player who = null;
            if (args.Length == 0) { who = p; }
            else { who = Player.Find(args[0]); }
            if (who == null) { p.SendMessage("Cannot find that player!"); return; }
            if (Server.devs.Contains(who.Username)) { p.SendMessage("Cannot change MCForge Developer's voice status!"); return; }
            if (who.voiced) { who.voiced = false; who.voicestring = ""; Player.UniversalChat(who.Username + " is no longer voiced!"); return; }
            else { who.voiced = true; who.voicestring = "+ "; Player.UniversalChat(who.Username + " is now voiced!"); return; }
        }

        public void Help(Player p)
        {
            p.SendMessage("/voice <player> - Voice a player");
            p.SendMessage("Voiced players will be able to speak during chat moderation!");
        }

        public void Initialize()
        {
            Command.AddReference(this, "voice");
        }
    }
}
