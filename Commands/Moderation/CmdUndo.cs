using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Groups;

namespace MCForge.Commands.Moderation {
    public class CmdUndo : ICommand {
        #region ICommand Members

        public string Name {
            get { return "Undo"; }
        }

        public CommandTypes Type {
            get { return CommandTypes.Mod; }
        }

        public string Author {
            get { return "MCForge Devs"; }
        }

        public int Version {
            get { return 1; }
        }

        public string CUD {
            get { return ""; }
        }

        public byte Permission {
            get { return 0; }
        }

        public void Use(Player p, string[] args) {

            int _time = 30;
            Player who = null;

            if (args.Length < 1) {
                Help(p);
                return;
            }

            if (args.Length == 1) {
                try {
                    _time = int.Parse(args[0]);
                    who = p;
                }
                catch {
                    p.SendMessage("That not a number silly head");
                    return;
                }

            }


            if (p.Group.Permission < (byte)PermissionLevel.Operator) {
                if (who != p) {
                    p.SendMessage("You cannot undo other peoples stuff");
                    return;
                }
            }


        }

        void Undo(Player p, int time = 30) {
            if (p == null || p.BlockChanges.Count < 0)
                return;

            int timeToLook = p.BlockChanges[p.BlockChanges.Count - 1].Time.Second - time;

            if (timeToLook < 0)
                timeToLook = p.BlockChanges[0].Time.Second;

            for (int i = 0; i < p.BlockChanges.Count ; i++) {
                var bChange = p.BlockChanges[i];

                if (bChange.Time.Second >= timeToLook) {
                    for (int j = p.BlockChanges.Count; j > i; j--) {
                        p.Level.BlockChange(bChange.Position, bChange.BlockFrom);
                    }
                    //return;
                }
            }
        }

        public void Help(Player p) {
            p.SendMessage("/undo <player> <seconds> - Undoes the block changes for <player> in the past <seconds>");
            p.SendMessage("/undo <player> all - Undoes as much as it can.");
            p.SendMessage("/undo <seconds> - Unodes the block changes for yourself in the past <seconds>");
        }

        public void Initialize() {
            Command.AddReference(this, "undo");
        }

        #endregion
    }
}
