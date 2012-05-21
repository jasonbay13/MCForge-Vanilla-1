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
using MCForge.Utils.Settings;

namespace CommandDll.Misc {
    class CmdResetPass : ICommand {
        public string Name { get { return "ResetPass"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Use(Player p, string[] args) {
            if (args[0] == "") { Help(p); return; }
            Player who = Player.Find(args[0]);
            if (p != null && !p.IsOwner) { p.SendMessage("Only the server owner can reset passwords!"); return; }
            if (who == null) { p.SendMessage("Could not find \"" + args[0] + "\"."); return; }
            if (!File.Exists("extra/passwords/" + who.Username + ".xml")) { p.SendMessage("The player you specified does not have a password!"); return; }
            if (p != null && !p.IsVerified) { p.SendMessage("You cannot reset passwords until you have verified with &a/pass <password>" + Server.DefaultColor + "!"); return; }
            try {
                File.Delete("extra/passwords/" + who.Username + ".xml");
                p.SendMessage((string)who.ExtraData.GetIfExist("Color") ?? "" + who.Username + "'s password has been successfully reset.");
            }
            catch (Exception e) {
                p.SendMessage("Password deletion failed. Please manually delete the file, extra/passwords/" + who.Username + ".xml, to reset " + who.Username + "'s password.");
                Logger.LogError(e);
                //No Server.ErrorLog?
            }
        }
        public void Help(Player p) {
            p.SendMessage("/resetpass <player> - Reset <player>'s password. Can only be used by the server owner.");
            p.SendMessage("Shortcut: /resetpassword");
        }
        public void Initialize() {
            Command.AddReference(this, new string[] { "resetpass", "resetpassword" });
        }
    }
}
