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
using MCForge.Utils;
namespace CommandDll
{
    public class CmdMe : ICommand
    {
        public string Name { get { return "Me"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            p.ExtraData.CreateIfNotExist("Muted", false);

            if (Server.voting) { 
                p.SendMessage("Cannot use /me while voting is in progress!"); 
                return; 
            }
            if ((bool)p.ExtraData["Muted"]) {
                p.SendMessage("Cannot use /me while muted!"); 
                return; 
            }
            if (args.Length == 0) { 
                p.SendMessage("You!"); 
                return; 
            }
            string message = null;
            foreach (string s in args) { message += s + " "; }
            Player.UniversalChat("*" + p.Username + " " + message);
        }
        public void Help(Player p)
        {
            p.SendMessage("What do you need help with, m'boy? Are you stuck down a well?");
        }
        public void Initialize()
        {
            Command.AddReference(this, "me");
        }
    }
}

