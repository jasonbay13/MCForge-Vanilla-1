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
    public class CmdDisagree : ICommand
    {
        public string Name { get { return "Disagree"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            if (Server.agreed.Contains(p.USERNAME)) { p.SendMessage("You have already agreed to the rules!"); return; }
            if (!p.readrules) { p.SendMessage("You need to read the /rules before you can disagree!"); return; }
            p.Kick("Kicked for disagreeing to the rules!");
        }

        public void Help(Player p)
        {
            p.SendMessage("/disagree - disagree to the rules");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "disagree" });
        }
    }
}

