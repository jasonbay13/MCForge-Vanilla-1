using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.World;
using MCForge.Utils;

namespace MCForge.Commands.Moderation {
    public class CmdRedo :ICommand{
        public string Name {
            get { return "Redo"; }
        }

        public CommandTypes Type {
            get { return CommandTypes.Mod; }
        }

        public string Author {
            get { return "ninedrafted"; }
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


        public void Use(Entity.Player p, string[] args) {
            int time = 30;
            long uid = -1;
            Player who = null;
            Level where = null;
            if (args.Length == 1) {
                try { time = int.Parse(args[0]); }
                catch { p.SendMessage("The time was incorrect, using 30 seconds instead"); }
            }
            if (args.Length == 2) {
                if (args[0].StartsWith("uid:")) {
                    try { uid = long.Parse(args[0].Split(':')[0]); }
                    catch { p.SendMessage("Redo aborted!"); }
                    return;
                }
                else {
                    who = Player.Find(args[0]);
                }
                try { time = int.Parse(args[1]); }
                catch { p.SendMessage("The time was incorrect, using 30 seconds instead"); }
            }
            if (args.Length == 3) {
                if (args[0].StartsWith("uid:")) {
                    try { uid = long.Parse(args[0].Split(':')[0]); }
                    catch { p.SendMessage("Redo aborted!"); }
                    return;
                }
                else {
                    who = Player.Find(args[0]);
                }
                try { time = int.Parse(args[1]); }
                catch { p.SendMessage("The time was incorrect, using 30 seconds instead"); }
                where = Level.FindLevel(args[1]);
                if (where == null) return;

            }
            if (who == null && uid == -1) {
                who = p;
                if (where == null) where = who.Level;
            }
            if (where != null) {
                if (who != null)
                    who.history.Redo(DateTime.Now.AddSeconds(-time).Ticks, where);
                else
                    BlockChangeHistory.Redo(uid, DateTime.Now.AddSeconds(-time).Ticks, where);
            }
            else {
                foreach (Level l in Level.Levels) {
                    if (who != null)
                        who.history.Redo(DateTime.Now.AddSeconds(-time).Ticks, l);
                    else
                        BlockChangeHistory.Redo(uid, DateTime.Now.AddSeconds(-time).Ticks, where);
                }
            }
        }

        public void Help(Entity.Player p) {
            p.SendMessage("/redo [time] redos undid changes whitin the last [time] seconds on the current map for you");
            p.SendMessage("/redo [name] [time] [map] redos changes for another player on selected map");
            p.SendMessage("/redo [name] [time] redos changes for another player on all loaded maps");
            p.SendMessage("/redo [uid:number] [time] [map] redos changes for another player on selected map");
            p.SendMessage("/redo [uid:number] [time] redos changes for another player on all loaded maps");
            p.SendMessage("[time] is the number of seonds, if not parsable 30 seconds are used");
            p.SendMessage("[map] is the name of a level, if no level is found redoing will be aborted");
            p.SendMessage("[uid:number] is the uid of a player preceded by \"uid:\" like uid:1234");
        }

        public void Initialize() {
            Command.AddReference(this, this.Name.ToLower());
        }
    }
}
