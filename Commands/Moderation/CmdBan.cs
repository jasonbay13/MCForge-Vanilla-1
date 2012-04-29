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
using System.IO;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Utils;

namespace CommandDll.Moderation
{
    class CmdBan : ICommand
    {
        public string Name { get { return "Ban"; } }
        public CommandTypes Type { get { return CommandTypes.mod; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Initialize() { Command.AddReference(this, "ban"); }
        public void Use(Player p, string[] args)
        {
            string _reason = "";
            bool Stealth = false;
            if (args[0] == "#") Stealth = true;
            if (!Stealth)
            {
                Player who = Player.Find(args[0]);
                string reason = _reason.Substring(args[0].Length + 1);
                using (StreamWriter SW = File.AppendText("bans/NameBans.txt"))
                {
                    if (who != null)
                        SW.WriteLine(who.Username);
                    else
                        SW.WriteLine(args[0]);
                    SW.Dispose();
                    SW.Close();
                }
                using (StreamWriter SW = File.AppendText("bans/BanInfo.txt"))
                {
                    if (who != null)
                    {
                        if (reason == "") { SW.WriteLine(who.Username + "`No reason specified.`" + DateTime.Now.Date + "`" + DateTime.Now.TimeOfDay + "`" + p.Username); }
                        else { SW.WriteLine(who.Username + "`" + reason + "`" + DateTime.Now.Date + "`" + DateTime.Now.TimeOfDay + "`" + p.Username); }
                    }
                    else
                    {
                        if (reason == "") { SW.WriteLine(args[0] + "`No reason specified.`" + DateTime.Now.Date + "`" + DateTime.Now.TimeOfDay + "`" + p.Username); }
                        else { SW.WriteLine(args[0] + "`" + reason + "`" + DateTime.Now.Date + "`" + DateTime.Now.TimeOfDay + "`" + p.Username); }
                    }
                }
                if (reason == "")
                {
                    if (who != null) Player.UniversalChat((string)who.ExtraData.GetIfExist("Color") ?? "" + who.Username + Server.DefaultColor + " is now &8banned" + Server.DefaultColor + "!");
                    else Player.UniversalChat("&3" + args[0] + Server.DefaultColor + " is now &8banned" + Server.DefaultColor + "!");
                }
                else
                {
                    if (who != null) { Player.UniversalChat((string)who.ExtraData.GetIfExist("Color") ?? "" + who.Username + Server.DefaultColor + " is now &8banned" + Server.DefaultColor + "!"); Player.UniversalChat("&4Reason: &f" + reason); }
                    else { Player.UniversalChat("&3" + args[0] + Server.DefaultColor + " is now &8banned" + Server.DefaultColor + "!"); Player.UniversalChat("&4Reason: &f" + reason); }
                }
            }
            if (Stealth)
            {
                Player who = Player.Find(args[1]);
                string reason = _reason.Substring(args[0].Length + args[1].Length + 2);
                using (StreamWriter SW = File.AppendText("bans/NameBans.txt"))
                {
                    if (who != null)
                        SW.WriteLine(who.Username);
                    else
                        SW.WriteLine(args[1]);
                }
                using (StreamWriter SW = File.AppendText("bans/BanInfo.txt"))
                {
                    if (who != null)
                    {
                        if (reason == "") { SW.WriteLine(who.Username + "`No reason specified.`" + DateTime.Now.Date + "`" + DateTime.Now.TimeOfDay + "`" + p.Username); }
                        else { SW.WriteLine(who.Username + "`" + reason + "`" + DateTime.Now.Date + "`" + DateTime.Now.TimeOfDay + "`" + p.Username); }
                    }
                    else
                    {
                        if (reason == "") { SW.WriteLine(args[1] + "`No reason specified.`" + DateTime.Now.Date + "`" + DateTime.Now.TimeOfDay + "`" + p.Username); }
                        else { SW.WriteLine(args[1] + "`" + reason + "`" + DateTime.Now.Date + "`" + DateTime.Now.TimeOfDay + "`" + p.Username); }
                    }
                }
                if (reason == "")
                {
                    if (who != null) Player.UniversalChatOps((string)who.ExtraData.GetIfExist("Color") ?? "" + who.Username + Server.DefaultColor + " is now &8banned" + Server.DefaultColor + "!");
                    else Player.UniversalChatOps("&3" + args[0] + Server.DefaultColor + " is now &8banned" + Server.DefaultColor + "!");
                }
                else
                {
                    if (who != null) { Player.UniversalChatOps((string)who.ExtraData.GetIfExist("Color") ?? "" + who.Username + Server.DefaultColor + " is now &8banned" + Server.DefaultColor + "!"); Player.UniversalChatOps("&4Reason: &f" + reason); }
                    else { Player.UniversalChatOps("&3" + args[0] + Server.DefaultColor + " is now &8banned" + Server.DefaultColor + "!"); Player.UniversalChatOps("&4Reason: &f" + reason); }
                }
            }
        }
        public void Help(Player p)
        {
            p.SendMessage("/ban <player> [reason] - Ban <player> with [reason] as the given reason.");
            p.SendMessage("/ban # <player> [reason] - Stealth ban <player> with [reason] as the given reason. (Banned message sent to ops+ only.");
            p.SendMessage("[reason] is optional");
        }
    }
}
