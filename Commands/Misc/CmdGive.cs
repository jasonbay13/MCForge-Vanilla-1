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

namespace CommandDll.Misc
{
    class CmdGive : ICommand
    {
        public string Name { get { return "Give"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Use(Player p, string[] args)
        {
            if (args.Length != 2) { Help(p); return; }
            Player who = Player.Find(args[0]);
            if (who == null) { p.SendMessage("Could not find \"" + args[0] + "\"!"); return; }
            if (who == p && !p.IsOwner) { p.SendMessage("You cannot give yourself money!"); return; }
            int amt;
            try { amt = int.Parse(args[1]); }
            catch { p.SendMessage("Invalid amount!"); return; }
            who.ExtraData.CreateIfNotExist("Money", 0);
            p.ExtraData.CreateIfNotExist("Money", 0);
            if ((int)who.ExtraData["Money"] + amt > 16777215) { p.SendMessage("Players cannot have more than 16777215 " + Server.Moneys + "."); return; }
            if (amt < 0) { p.SendMessage("Cannot take negative amounts of " + Server.Moneys + "."); return; }
            who.ExtraData["Money"] = (int)who.ExtraData["Money"] + amt;
            Player.UniversalChat(who.Color + who.Username + Server.DefaultColor + " was given &3" + amt + Server.DefaultColor + " " + Server.Moneys + ".");
        }
        public void Help(Player p)
        {
            p.SendMessage("/give <player> <amount> - Give <player> <amount> amount of " + Server.Moneys + ".");
        }
        public void Initialize()
        {
            Command.AddReference(this, "give");
        }
    }
}
