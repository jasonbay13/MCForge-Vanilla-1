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
using System;
using MCForge.Entity;
using MCForge.Interface.Command;
using System.IO;

namespace CommandDll.Moderation
{
    class CmdBanInfo : ICommand
    {
        public string Name { get { return "BanInfo"; } }
        public CommandTypes Type { get { return CommandTypes.mod; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Initialize() { Command.AddReference(this, new string[2] { "baninfo", "baninformation" }); }
        public void Use(Player p, string[] args)
        {
            string[] lines = File.ReadAllLines("Bans/Ban Info.txt");
            if (lines.Length < 1) { p.SendMessage("Could not find ban information for \"" + args[0] + "\"."); return; }
            foreach (string line in lines)
            {
                string name = line.Split('`')[0];
                if (args[0] == name)
                {
                    string reason = line.Split('`')[1];
                    string date = line.Split('`')[2];
                    string time = line.Split('`')[3];
                    string banner = line.Split('`')[4];
                    p.SendMessage(name + " was banned at " + time + " on " + date + " by " + banner + ".");
                    p.SendMessage("&4Reason: " + MCForge.Core.Server.DefaultColor + reason);
                    return;
                }
                p.SendMessage("Could not find ban information for \"" + args[0] + "\".");
            }
        }
        public void Help(Player p)
        {
            p.SendMessage("/baninfo <player/IP> - View information about <player/IP>'s ban.");
        }
    }
}