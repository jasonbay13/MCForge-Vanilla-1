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

namespace MCForge.Commands
{
    public class CmdAbort : ICommand
    {
        public string Name { get { return "Abort"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            if (p.ExtraData.ContainsKey("Mode"))
                p.ExtraData["Mode"] = false;
            if (p.ExtraData.ContainsKey("OpChat"))
                p.ExtraData["OpChat"] = false;
            if (p.ExtraData.ContainsKey("AdminChat"))
                p.ExtraData["Admin"] = false;
            if (p.ExtraData.ContainsKey("IsWhispering"))
                p.ExtraData["IsWhispering"] = false;
            if (p.ExtraData.ContainsKey("WhisperingTo"))
                p.ExtraData["WhisperingTo"] = null;
            if (p.ExtraData.ContainsKey("ModeBlock"))
                p.ExtraData["ModeBlock"] = null;
        }

        public void Help(Player p)
        {
            p.SendMessage("/abort - cancels every toggled action");
            p.SendMessage("Shortcut: /a");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[2] {"abort", "a"});
        }
    }
}

