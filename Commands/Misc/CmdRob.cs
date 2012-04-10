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
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;

namespace CommandDll.Misc
{
    class CmdRob : ICommand
    {
        public string Name { get { return "Rob"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Use(Player p, string[] args)
        {
            if (args.Length != 2) { Help(p); return; }
            Player who = Player.Find(args[0]);
            if (p.group.permission < who.group.permission) { p.SendMessage("You cannot rob your superiors!"); return; }
            if (who == null) { p.SendMessage("Could not find \"" + args[0] + "\"!"); return; }
            if (who == p) { p.SendMessage("You cannot rob yourself!"); return; }
            int amt;
            try { amt = int.Parse(args[1]); }
            catch { p.SendMessage("Invalid amount!"); return; }
            if (p.money + amt > 16777215) { p.SendMessage("If you steal that much, you'll be so rich your wallet will burst! You cannot have over 16777215 " + Server.moneys + "."); return; }
            if (who.money - amt < 0) { p.SendMessage("You cannot steal money that " + who.Username + " does not have!"); return; }
            if (amt < 0) { p.SendMessage("Cannot take negative amounts of " + Server.moneys + "."); return; }
            Random rand = new Random();
            int x = rand.Next(1, 101);
            if (InBetween(1, x, 3)) { Rob(p, who, amt); }
            if (InBetween(4, x, 6)) { amt -= amt / 5; Rob(p, who, amt); }
            if (InBetween(7, x, 15)) { amt = amt / 5; Rob(p, who, amt); }
            if (InBetween(16, x, 25)) { amt = amt / 10; Rob(p, who, amt); }
            if (InBetween(26, x, 100)) { Player.UniversalChat(p.color + p.Username + Server.DefaultColor + " tried to rob " + who.color + who.Username + Server.DefaultColor + " but failed!"); p.Kick("Thief!"); }
        }
        public void Help(Player p)
        {
            p.SendMessage("/rob <player> <amount> - Try to take <amount> " + Server.moneys + " from <player>.");
        }
        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "rob" });
        }
        bool InBetween(int min, int num, int max)
        {
            if (num >= min && num <= max) return true;
            else return false;
        }
        void Rob(Player p, Player who, int amount)
        {
            who.money -= amount;
            p.money += amount;
            Player.UniversalChat(p.color + p.Username + Server.DefaultColor + " robbed " + who.color + who.Username + Server.DefaultColor + " of &3" + amount + Server.DefaultColor + " " + Server.moneys + ".");
        }
    }
}