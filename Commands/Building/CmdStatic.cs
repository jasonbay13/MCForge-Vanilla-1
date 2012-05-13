using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Groups;
using MCForge.Entity;
using MCForge.Utils;
using MCForge.API.Events;

namespace CommandDll.Building {
    public class CmdStatic : ICommand {

        private static readonly Dictionary<Player, ICommand> playerDictionary = new Dictionary<Player, ICommand>();

        public string Name {
            get { return "Static"; }
        }

        public CommandTypes Type {
            get { return CommandTypes.Building; }
        }

        public string Author {
            get { return "headdetect"; }
        }

        public int Version {
            get { return 1; }
        }

        public string CUD {
            get { return "com.mcforge.cmdstatic"; }
        }

        public byte Permission {
            get { return (byte)PermissionLevel.Builder; }
        }

        public void Use(MCForge.Entity.Player p, string[] args) {

            if (args.Length < 1) {
                if (playerDictionary.ContainsKey(p)) {
                    p.SendMessage("&cStatic Disabled");
                    p.OnCommandEnd.All -= OnEndCommand;
                    playerDictionary.Remove(p);
                }
                else {
                    Help(p);
                }
                return;
            }

            ICommand cmd = Command.Find(args[0]);

            if (cmd == null) {
                p.SendMessage("&cCan't find the specified command");
                return;
            }

            p.OnCommandEnd.All += OnEndCommand;
            playerDictionary.ChangeOrCreate<Player, ICommand>(p, cmd);

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

        public void Help(MCForge.Entity.Player p) {
            p.SendMessage("static [command] (args <optional>) - Makes every command a toggle.");
            p.SendMessage("If [command] is given, then that command is used");
            p.SendMessage("If (args) is given it will use that command with specified arguments");
        }

        public void Initialize() {
            Command.AddReference(this, new[] { "t", "static" });
        }

        public void OnEndCommand(Player sender, CommandEndEventArgs e) {
            e.Command.Use(sender, e.Args);
        }
    }
}
