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
using System.IO;
using System;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;

namespace CommandDll
{
    public class CmdCaps : ICommand
    {
        public string Name { get { return "Caps"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Givo"; } }
        public Version Version { get { return new Version(1,0); } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            Player who;
            string reason;
            who = Player.Find(args[0]);
            if (who == null) { p.SendMessage("Player not found"); return; }

            string message = null;
            foreach (string s in args)
            {
                message += s + " ";
            }
            reason = message.Remove(0, message.Split(' ')[0].Length + 1);

            if (who.decaps)
            {
                who.decaps = false;
                p.SendMessage(who.Username+ " is now allowed to use caps.");
                who.SendMessage("You're now allowed to use Caps.");
            return;
            }
            if (!who.decaps)
            {
                who.decaps = true;
                p.SendMessage(who.Username +" cannot use Caps anymore.");
                who.SendMessage("Your Caps permission has taken away.");
                if (reason.Length != 0) who.SendMessage("Reason: " + reason);
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/caps <Player> <Reason> - Take away cap permissions");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "caps" });
        }
    }
}

