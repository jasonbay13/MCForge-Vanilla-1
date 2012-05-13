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
using System.Linq;
using MCForge.Interface.Command;
using MCForge.Utils.Settings;
using MCForge.Entity;

namespace CommandDll.Moderation
{
    class CmdSettings : ICommand
    {
        public string Name { get { return "Settings"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "headdetect"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 100; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length < 1)
            {
                Help(p);
                return;
            }
            if (args.Length > 2)
            {
                if (args[0].ToLower() == "help")
                {
                    if (ServerSettings.HasKey(args[1]))
                        p.SendMessage(ServerSettings.GetDescription(args[1]));
                    else
                        p.SendMessage("Key doesn't exist");
                    return;
                }
                else if (ServerSettings.HasKey(args[0]))
                {
                    ServerSettings.SetSetting(args[0], values: String.Join(" ", args, 1, args.Count()));
                    return;
                }
                else
                {
                    Help(p);
                    return;
                }
            }
            if (!ServerSettings.HasKey(args[0]))
                p.SendMessage("Key doesn't exist");
            else
                p.SendMessage(String.Format("Value for {0} is {1}", args[0], ServerSettings.GetSetting(args[0])));
        }

        public void Help(Player p)
        {
            p.SendMessage("Usage: /settings <key> [value]");
            p.SendMessage("To get a value, do not add a value at the end of the command.");
            p.SendMessage("To set a value, add a value at the end of the command.");
            p.SendMessage("ex: /settings motd Welcome $user");
            p.SendMessage("To get a description of a setting, type /settings help <key>.");
        }

        public void Initialize()
        {
            Command.AddReference(this, "settings");
        }
    }
}
