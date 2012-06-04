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
using System.Collections.Generic;
using System.IO;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;

namespace CommandDll.Moderation
{
    class CmdUnIPBan : ICommand
    {
        public string Name { get { return "UnIPBan"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Initialize() { Command.AddReference(this, new string[1] { "unbanip" }); }
        public void Use(Player p, string[] args)
        {
            bool Stealth = false;
            if (args[0] == "#") Stealth = true;
            List<string> lines = new List<string>(File.ReadAllLines("bans/IPBans.txt"));
            if (!Stealth)
            {
                foreach (string line in lines)
                {
                    if (line == args[0])
                    {
                        for (int i = 1; i <= lines.Count; i++)
                            if (lines[i] == args[0]) lines.Remove(lines[i]);
                        File.WriteAllLines("bans/IPBans.txt", lines.ToArray());
                        Player.UniversalChat("&3" + args[0] + Server.DefaultColor + " is now unbanned!");
                        return;
                    }
                    p.SendMessage("&3" + args[0] + Server.DefaultColor + " is not banned.");
                }
            }
            if (Stealth)
            {
                foreach (string line in File.ReadAllLines("bans/IPBans.txt"))
                {
                    if (line == args[0])
                    {
                        for (int i = 1; i <= lines.Count; i++)
                            if (lines[i] == args[0]) lines.Remove(lines[i]);
                        File.WriteAllLines("bans/IPBans.txt", lines.ToArray());
                        Player.UniversalChatOps("&3" + args[0] + Server.DefaultColor + " is now unbanned!");
                        return;
                    }
                    p.SendMessage("&3" + args[0] + Server.DefaultColor + " is not banned.");
                }
            }
        }
        public void Help(Player p)
        {
            p.SendMessage("/unipban <IP> - Unban <IP>.");
            p.SendMessage("/unipban # <IP> - Stealth unban <IP>. (Unban message sent to ops+ only.)");
        }
    }
}
