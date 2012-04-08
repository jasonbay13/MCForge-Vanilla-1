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
using System.Threading;
using MCForge;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;

namespace CommandDll
{
    public class CmdMute : ICommand
    {
        public string Name { get { return "Mute"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Arrem"; } }
        public Version Version { get { return new Version(1,0); } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0) { Help(p); return; }
            Player who = Player.Find(args[0]);
            int time = 0;
            if (Server.devs.Contains(who.Username)) { p.SendMessage("Cannot mute a MCForge Developer!"); return; }
            if (args.Length == 2) //XMute
            {
                if (who.muted) { who.muted = false; Player.UniversalChat(who.Username + " has been unmuted!"); return; }
                try { time = Int32.Parse(args[1]) * 1000; }
                catch { p.SendMessage("Please use a valid number!"); return; }
                if (time > 600000) { p.SendMessage("Cannot mute for more than 10 minutes"); return; }
                who.muted = true;
                Player.UniversalChat(who.Username + " %chas been muted for " + time / 1000 + " seconds!");
                Thread.Sleep(time);
                who.muted = false;
                Player.UniversalChat(who.Username + " has been unmuted!");
            }
            else //Regular mute
            {
                if (who.muted) { who.muted = false; Player.UniversalChat(who.Username + " has been unmuted!"); return; }
                else { who.muted = true; Player.UniversalChat(who.Username + " has been muted!"); return; }
            }
            
        }

        public void Help(Player p)
        {
            p.SendMessage("/mute <player> [seconds] - mutes a player!");
            p.SendMessage("If you specify a time player will be unmuted after that period of time!");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "mute" });
        }
    }
}

