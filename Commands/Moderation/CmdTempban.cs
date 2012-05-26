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
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;

namespace CommandDll
{
    public class CmdTempban : ICommand
    {
        public string Name { get { return "Tempban"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0) { Help(p); return; }
            Player who = Player.Find(args[0]);
            if (who == null) { p.SendMessage("Cannot find player!"); return; }
            DateTime allowed = DateTime.Now; int num;
            if (Server.devs.Contains(who.Username)) { p.SendMessage("Cannot ban a MCForge Developer!"); return; }
            #region =Name=
            if (args.Length == 1) {
                Server.TempBan tb = new Server.TempBan();
                tb.name = who.Username; tb.allowed = DateTime.Now.AddHours(1);
                Server.tempbans.Add(tb);
                who.Kick("Tempbanned for 1 hour!");
                Player.UniversalChat(who.Username + " has been tempbanned for 1 hour!");
            }
            #endregion
            #region =Name and time=
            else if (args.Length == 2) {
                if (!Int32.TryParse(args[1], out num)) { Help(p); return; }
                Server.TempBan tb = new Server.TempBan();
                tb.name = who.Username; tb.allowed = DateTime.Now.AddMinutes(Int32.Parse(args[1]));
                string sipl1 = Int32.Parse(args[1]) == 1 ? "minute" : "minutes";
                Player.UniversalChat(who.Username + " has been tempbanned for " + args[1] + " " + sipl1 + "!");
                who.Kick("Tempbanned for " + args[1] + " " + sipl1 + "!");
            }
            #endregion
            #region =Name time and value=
            else
            {
                if (!Int32.TryParse(args[1], out num)) { Help(p); return; }
                switch (args[2].ToLower())
                {
                    case "m":
                    case "minutes":
                    case "min":                  
                        Server.TempBan tb1 = new Server.TempBan();
                        tb1.name = who.Username; tb1.allowed = DateTime.Now.AddMinutes(Int32.Parse(args[1]));
                        Server.tempbans.Add(tb1);
                        string sipl1 = Int32.Parse(args[1]) == 1 ? "minute" : "minutes";
                        Player.UniversalChat(who.Username + " has been tempbanned for " + args[1] + " " + sipl1 + "!");
                        who.Kick("Tempbanned for " + args[1] + " " + sipl1 + "!");
                        break;
                    case "h":
                    case "hours":
                    case "hrs":                       
                        Server.TempBan tb2 = new Server.TempBan();
                        tb2.name = who.Username; tb2.allowed = DateTime.Now.AddHours(Int32.Parse(args[1]));
                        Server.tempbans.Add(tb2);
                        string sipl2 = Int32.Parse(args[1]) == 1 ? "hour" : "hours";
                        Player.UniversalChat(who.Username + " has been tempbanned for " + args[1] + " " + sipl2 + "!");
                        who.Kick("Tempbanned for " + args[1] + " " + sipl2 + "!");
                        break;
                    case "d":
                    case "days":
                        allowed = allowed.AddDays(Int32.Parse(args[1]));                                               
                        Server.TempBan tb3 = new Server.TempBan();
                        tb3.name = who.Username; tb3.allowed = DateTime.Now.AddDays(Int32.Parse(args[1]));
                        string sipl3 = Int32.Parse(args[1]) == 1 ? "day" : "days";
                        Player.UniversalChat(who.Username + " has been tempbanned for " + args[1] + " " + sipl3 + "!");
                        who.Kick("Tempbanned for " + args[1] + " " + sipl3 + "!");
                            break;
                    default:
                        p.SendMessage("Invalid type! Use either minutes, hours or days");
                        break;
                }
            }
            #endregion
        }

        public void Help(Player p)
        {
            p.SendMessage("/tempban <player> [amount] [m/h/d] - Bans <player> for [amount] of minutes, hours, or days");
            p.SendMessage("If amount isn't specified, player will be banned for an hour");
            p.SendMessage("m = minutes : h = hours : d = days");
            p.SendMessage("&cTempbans will be reset if the server restarts!");
            p.SendMessage("Shortcut: /tb");

        }

        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "tempban", "tb" });
        }
    }
}