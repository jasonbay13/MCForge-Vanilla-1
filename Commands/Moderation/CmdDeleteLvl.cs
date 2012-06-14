using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Groups;
using MCForge.World;
using MCForge.Core;
using System.IO;
using MCForge.Entity;
using MCForge.Utils;

namespace MCForge.Commands {
    public class CmdDeleteLvl : ICommand {
        #region ICommand Members

        public string Name {
            get { return "DeleteLvl"; }
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
            get { return null; }
        }

        public byte Permission {
            get { return (byte)PermissionLevel.Operator; }
        }

        public void Use(Entity.Player p, string[] args) {
            if (args.Length != 1) {
                Help(p);
                return;
            }

            var lvl = Level.FindLevel(args[0]);
            if (lvl != null) {
                foreach (var pe in lvl.Players)
                    pe.Level = Server.Mainlevel;
                lvl.Unload();
            }

            Backup.BackupLevel(args[0], "-deleted");

            foreach (var trashCan in Level.UnloadedLevels)
                if (trashCan.ToLower() == args[0].ToLower())
                    File.Delete(FileUtils.LevelsPath + trashCan + ".lvl");

            Player.UniversalChat("Level \"" + args[0] + "\" was deleted");
        }

        public void Help(Entity.Player p) {
            p.SendMessage("/deletelvl <lvl name> - deletes the specified <level>");
        }

        public void Initialize() {
            Command.AddReference(this, "deletelvl", "deletelevel", "removelevel", "removelvl");
        }

        #endregion
    }
}
