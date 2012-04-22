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

namespace CommandDll.Moderation
{
    class CmdBanKick : ICommand
    {
        public string Name { get { return "BanKick"; } }
        public CommandTypes Type { get { return CommandTypes.mod; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Initialize() { Command.AddReference(this, new string[2] { "bankick", "kickban" }); }
        public void Use(Player p, string[] args)
        {
            string _reason = "";
            bool Stealth = false;
            if (args[0] == "#") Stealth = true;
            if (!Stealth)
            {
                Player who = Player.Find(args[0]);
                string reason = _reason.Substring(args[0].Length + 1);
                if (reason == "")
                {
                    if (who != null) { Command.Find("ban").Use(p, new string[1] { who.Username }); who.Kick("Banned by " + p.Username + "!"); }
                    else { Command.Find("ban").Use(p, new string[1] { args[0] }); p.SendMessage("Could not kick \"" + args[0] + "\" because they are not online."); }
                }
                else
                {
                    if (who != null) { Command.Find("ban").Use(p, new string[2] { who.Username, reason }); who.Kick("Banned by " + p.Username + " because " + reason); }
                    else { Command.Find("ban").Use(p, new string[2] { args[0], reason }); p.SendMessage("Could not kick \"" + args[0] + "\" because they are not online."); }
                }
            }
            if (Stealth)
            {
                Player who = Player.Find(args[1]);
                string reason = _reason.Substring(args[0].Length + args[1].Length + 2);
                if (reason == "")
                {
                    if (who != null) { Command.Find("ban").Use(p, new string[2] { "#", who.Username }); who.Kick("Banned by " + p.Username + "!"); }
                    else { Command.Find("ban").Use(p, new string[2] { "#", args[1] }); p.SendMessage("Could not kick \"" + args[1] + "\" because they are not online."); }
                }
                else
                {
                    if (who != null) { Command.Find("ban").Use(p, new string[3] { "#", who.Username, reason }); who.Kick("Banned by " + p.Username + " because " + reason); }
                    else { Command.Find("ban").Use(p, new string[3] { "#", args[1], reason }); p.SendMessage("Could not kick \"" + args[1] + "\" because they are not online."); }
                }
            }
        }
        public void Help(Player p)
        {
            p.SendMessage("/bankick <player> [reason] - Ban and kick <player> with [reason] as the given reason.");
            p.SendMessage("/bankick # <player> [reason] - Same as normal, but a stealth ban/kick instead. (Banned and kicked messages sent to ops+ only.");
            p.SendMessage("[reason] is optional");
        }
    }
}
