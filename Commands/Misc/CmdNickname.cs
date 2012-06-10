/*
Copyright 2012 MCForge
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
using MCForge.Interface.Command;
using MCForge.Entity;

namespace MCForge.Commands.Misc {
    public class CmdNickname : ICommand {
        public string Name { get { return "Nickname"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "headdetect, ninedrafted"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission {
            get {
#if DEBUG
                return 0;
#else
                return 30;
#endif
            }
        }

        public void Use(Player p, string[] args) {
            if (args.Length == 0 && p.Username != p.DisplayName) p.DisplayName = p.Username;
            string nick = string.Join(" ", args);
            p.DisplayName = nick;
        }

        public void Help(Player p) {
            p.SendMessage("/nickname [name] to set your nickname");
            p.SendMessage("/nickname to remove your nickname");
            p.SendMessage("Shortcut: /nick");
        }

        public void Initialize() {
            Command.AddReference(this, new string[] { "nickname", "nick" });
        }
    }
}