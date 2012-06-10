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
using System.IO;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;

namespace MCForge.Commands
{
    public class CmdJoker : ICommand
    {
        public string Name { get { return "Joker"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            Player who = null;
            if (args.Length == 0) { Help(p); return; }
            if (args.Length == 1) { who = Player.Find(args[0]); }
            else { who = Player.Find(args[1]); }
            if (who == null) { p.SendMessage("Cannot find that player!"); return; }
            if (Server.devs.Contains(who.Username)) { p.SendMessage("You can't joker a MCForge Developer!"); return; }
            CheckEmpty();

            who.ExtraData.CreateIfNotExist("Jokered", false);
            if (args.Length == 1) //normal joker
            {
                if ((bool)who.ExtraData["Jokered"]) { who.ExtraData["Jokered"] = false; Player.UniversalChat(who.Username + " is no longer a &aJ&bo&ck&5e&9r"); return; }
                else { who.ExtraData["Jokered"] = true; Player.UniversalChat(who.Username + " is now a &aJ&bo&ck&5e&9r"); return; }
            }
            else //stealth
            {
                if ((bool)who.ExtraData["Jokered"]) { who.ExtraData["Jokered"] = false; p.SendMessage("Successfully stealth unjokered " + who.Username); return; }
                else { who.ExtraData["Jokered"] = true; p.SendMessage("Successfully stealth jokered " + who.Username); return; }
            }

        }

        public void Help(Player p)
        {
            p.SendMessage("/joker <name> - Causes a player to become a joker!");
            p.SendMessage("/joker # <name> - Makes the player a joker silently.");
        }
        public void Initialize()
        {
            Command.AddReference(this, "joker");
        }
        void CheckEmpty()
        {
            if (Server.jokermessages.Count == 0)
            {
                string text = "I am a pony" + Environment.NewLine + "Rainbow Dash <3" + Environment.NewLine + "I like trains!";
                File.WriteAllText("text/jokermessages.txt", text);
                Server.jokermessages.Add("I am a pony");
                Server.jokermessages.Add("Rainbow Dash <3");
                Server.jokermessages.Add("I like trains!");
            }
        }
    }
}
