using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Groups;
using MCForge.Core;

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
            Player who = p;


            //undo <seconds>
            if (args.Length == 1) {
                try {
                    _time = int.Parse(args[0]);
                    if (_time < 1) {
                        p.SendMessage("The time must be greater than 1");
                        return;
                    }
                }
                catch {
                    p.SendMessage("That not a number silly head");
                    return;
                }
            }


            if (args.Length == 2) {
                who = Player.Find(args[0]);

                if (who == null) {
                    p.SendMessage("Player doesn't exist");
                    return;
                }

                if (args[1].ToLower() == "all") {

                    _time = int.MaxValue;
                }
                else {
                    try {
                        _time = int.Parse(args[1]);
                        if (_time < 1) {
                            p.SendMessage("The time must be greater than 1");
                            return;
                        }
                    }
                    catch {
                        p.SendMessage("That is not a valid numba");
                    }
                }
            }

            if (p.Group.Permission < (byte)PermissionLevel.Operator) {
                if (who != p) {
                    p.SendMessage("You cannot undo other peoples stuff");
                    return;
                }
            }
            Undo(who, _time);
            p.SendMessage(Server.DefaultColor + "Undid " + who.Color + who.Username + Server.DefaultColor + " for &c" + _time + Server.DefaultColor + " seconds");
        }

        void Undo(Player p, int time = 30) {
            if (p == null || p.BlockChanges.Count < 0)
                return;

            long timeToLook = p.BlockChanges[p.BlockChanges.Count - 1].Time.Second - time;

            if (timeToLook < 0)
                timeToLook = p.BlockChanges[0].Time.Second;
            else if (timeToLook > int.MaxValue)
                timeToLook = int.MaxValue;


            for (int i = p.BlockChanges.Count - 1; i > 0; i++) {
                var bChange = p.BlockChanges[i];

                if (bChange.Time.Second < timeToLook)
                    continue;

                for (int j = 0; j < i; j++) {
                    p.Level.BlockChange(bChange.Position, bChange.BlockFrom);
                }
                return;
            }
        }

        public void Help(Player p) {
            p.SendMessage(Server.DefaultColor + "/undo <player> <seconds> - Undoes the block changes for <player> in the past <seconds>");
            p.SendMessage(Server.DefaultColor + "/undo <player> all - Undoes as much as it can.");
            p.SendMessage(Server.DefaultColor + "/undo <seconds> - Unodes the block changes for yourself in the past <seconds>");
            p.SendMessage(Server.DefaultColor + "/undo - Undoes the block changes for yourself in the past&c 30 " + Server.DefaultColor + " seconds");
        }

        public void Initialize() {
            Command.AddReference(this, "undo");
        }

        #endregion
    }
}
