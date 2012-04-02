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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MCForge;
using System.IO;

namespace CommandDll
{
    public class CmdModerate : ICommand
    {
        public string Name { get { return "Moderate"; } }
        public CommandTypes Type { get { return CommandTypes.mod; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }

        public void Use(Player p, string[] args)
        {
            if (Server.moderation)
            {
                Server.moderation = false;
                Player.UniversalChat("Chat moderation has been disabled!"); return;
            }
            else
            {
                Server.moderation = true;
                Player.UniversalChat("Chat moderation has been enabled!"); return;
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/moderate - Moderates the chat. Only voiced players will be able to speak!");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "moderate" });
        }
    }
}
