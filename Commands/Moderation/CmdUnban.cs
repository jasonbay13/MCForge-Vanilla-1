/*
Copyright 2012 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http:www.opensource.org/licenses/ecl2.php
http:www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
 */
using System.IO;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using System.Collections.Generic;

namespace CommandDll.Moderation
{
    class CmdUnban : ICommand
    {
        public string Name { get { return "Unban"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Initialize() { Command.AddReference(this, "unban"); }
        public void Use(Player p, string[] args)
        {
            bool Stealth = false;
            if (args[0] == "#") Stealth = true;
            Player who = Player.Find(args[0]);
            foreach (string line in File.ReadAllLines("bans/NameBans.txt"))
            {
                if (who != null)
                {
                    if (line == who.Username)
                    {
                        List<string> l = new List<string>();
                        if (line != who.Username)
                            l.Add(line);
                        File.WriteAllLines("bans/NameBans.txt", l.ToArray());
                        if (!Stealth) Player.UniversalChat(who.Color + who.Username + Server.DefaultColor + " is now unbanned!");
                        else Player.UniversalChatOps(who.Color + who.Username + Server.DefaultColor + " is now unbanned!");
                        return;
                    }
                    p.SendMessage(who.Color + who.Username + Server.DefaultColor + " is not banned.");
                }
                else
                {
                    if (line == args[0])
                    {
                        List<string> l = new List<string>();
                        if (line != args[0])
                            l.Add(line);
                        File.WriteAllLines("bans/NameBans.txt", l.ToArray());
                        if (!Stealth) Player.UniversalChat("&3" + args[1] + Server.DefaultColor + " is now unbanned!");
                        else Player.UniversalChatOps("&3" + args[1] + Server.DefaultColor + " is now unbanned!");
                        return;
                    }
                    p.SendMessage("&3" + args[0] + Server.DefaultColor + " is not banned.");
                }
            }
        }
        public void Help(Player p)
        {
            p.SendMessage("/unban <player> - Unban <player>.");
            p.SendMessage("/unban # <player> - Stealth unban <player>. (Unban message sent to ops+ only.)");
        }
    }
}