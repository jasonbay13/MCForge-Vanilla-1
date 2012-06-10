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
using System.Collections.Generic;
using MCForge.Interface.Command;
using MCForge.Groups;
using MCForge.Entity;
using MCForge.Utils;
using MCForge.API.Events;

namespace MCForge.Commands.Building {
    public class CmdStatic : ICommand {
        public string Name { get { return "Static"; } }
        public CommandTypes Type { get { return CommandTypes.Building; } }
        public string Author { get { return "headdetect"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return "com.mcforge.cmdstatic"; } }
        public byte Permission {
            get {
                return (byte)PermissionLevel.Builder;
            }
        }
        public void Use(Player p, string[] args) {
            if (args.Length < 1) {
                if (p.StaticCommandsEnabled) {
                    p.StaticCommandsEnabled = false;
                    p.SendMessage("&cStatic Disabled");
                    return;
                }
                Help(p);
                return;
            }

            ICommand cmd = Command.Find(args[0]);

            if (cmd == null) {
                p.SendMessage("&cCan't find the specified command");
                return;
            }

            if (p.Group.CanExecute(cmd)) {
                p.SendMessage("You can't use this command");
                return;
            }

            string[] newArgs;
            if (args.Length < 2) {
                newArgs = new string[0];
            }
            else {
                newArgs = new string[args.Length - 1];
                args.CopyTo(newArgs, 1);
            }

            p.SendMessage("&aStatic Enabled");
            cmd.Use(p, newArgs);



        }

        public void Help(Player p) {
            p.SendMessage("static [command] (args <optional>) - Makes every command a toggle.");
            p.SendMessage("If [command] is given, then that command is used");
            p.SendMessage("If (args) is given it will use that command with specified arguments");
            p.SendMessage("Shortcut: /t");
        }

        public void Initialize() {
            Command.AddReference(this, new[] { "t", "static" });
        }

    }
}
