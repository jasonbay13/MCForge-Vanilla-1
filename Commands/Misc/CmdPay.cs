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
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;

namespace CommandDll.Misc
{
    class CmdPay : ICommand
    {
        public string Name { get { return "Pay"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Use(Player p, string[] args)
        {
            if (args.Length != 2) { Help(p); return; }
            Player who = Player.Find(args[0]);
            if (who == null) { p.SendMessage("Could not find \"" + args[0] + "\"!"); return; }
            if (who == p) { p.SendMessage("You cannot pay yourself!"); return; }
            int amt;
            try { amt = int.Parse(args[1]); }
            catch { p.SendMessage("Invalid amount!"); return; }
            if (who.money + amt > 16777215) { p.SendMessage("Players cannot have over 16777215 " + Server.moneys + "."); return; }
            if (p.money - amt < 0) { p.SendMessage("You cannot pay with more " + Server.moneys + " than you have!"); return; }
            if (amt < 0) { p.SendMessage("Cannot pay negative amounts of " + Server.moneys + "."); return; }
            who.money += amt;
            p.money -= amt;
            Player.UniversalChat(who.color + who.Username + Server.DefaultColor + " was paid &3" + amt + Server.DefaultColor + " " + Server.moneys + " by " + p.color + p.Username + Server.DefaultColor + ".");
            //TODO: DB save
        }
        public void Help(Player p)
        {
            p.SendMessage("/pay <player> <amount> - Pay <player> <amount> of " + Server.moneys + ".");
        }
        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "pay" });
        }
    }
}