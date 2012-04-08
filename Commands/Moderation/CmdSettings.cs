using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Utilities.Settings;
using MCForge.Entity;

namespace CommandDll.Moderation {
    class CmdSettings : ICommand {
        public string Name {
            get { return "settings"; }
        }

        public CommandTypes Type {
            get { return CommandTypes.Mod; }
        }

        public string Author {
            get { return "headdetect"; }
        }

        public decimal Version {
            get { return 1.00m; }
        }

        public string CUD {
            get { return ""; } //idk wtf this do
        }

        public byte Permission {
            get { return 100; }
        }

        public void Use(Player p, string[] args) {
            if (args.Length < 1) {
                Help(p);
                return;
            }
            if (args.Length > 2) {
                if (args[0].ToLower() == "help") {
                    if (ServerSettings.HasKey(args[1]))
                        p.SendMessage(ServerSettings.GetDescription(args[1]));
                    else
                        p.SendMessage("Key doesn't exist");
                    return;
                }
                else if (ServerSettings.HasKey(args[0])) {
                    ServerSettings.SetSetting(args[0], values:String.Join(" ", args, 1, args.Count()));
                    return;
                }
                else {
                    Help(p);
                    return;
                }
            }
            if (!ServerSettings.HasKey(args[0]))
                p.SendMessage("Key doesn't exist");
            else
                p.SendMessage(String.Format("Value for {0} is {1}", args[0], ServerSettings.GetSetting(args[0])));
        }

        public void Help(Player p) {
            p.SendMessage("Usage: /settings <key> [optional:<value>]");
            p.SendMessage("To get a value, do not add a value at the end of the command");
            p.SendMessage("To set a value, add a value at the end of the command");
            p.SendMessage("ex: /setting motd Welcome $user");
            p.SendMessage("to get a description of a setting, type /setting help <key>");
        }

        public void Initialize() {
            Command.AddReference(this, "settings");
        }
    }
}
