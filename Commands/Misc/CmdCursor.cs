using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.API.Events;
using MCForge.Utils;
namespace MCForge.Commands.Misc {
    public class CmdCursor : ICommand {

        public string Name {
            get { return "Cursor"; }
        }

        public CommandTypes Type {
            get { return CommandTypes.Misc; }
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
            if (args.Length == 1 && args[0] == "stop") {
                p.OnPlayerRotate.Normal -= OnPlayerRotate_Normal;
                p.OnPlayerMove.Normal -= OnPlayerMove_Normal;
                p.OnPlayerBigMove.Normal -= OnPlayerBigMove_Normal;
                if (p.ExtraData["Cursor"] != null) {
                    Vector3S old;
                    if (p.ExtraData["Cursor"].GetType() == typeof(Vector3S))
                        old = (Vector3S)p.ExtraData["Cursor"];
                    else {
                        old = new Vector3S();
                        old.FromString((string)p.ExtraData["Cursor"]);
                    }
                    p.SendBlockChange((ushort)old.x, (ushort)old.z, (ushort)old.y, p.Level.GetBlock(old));
                    p.ExtraData["Cursor"] = null;
                }
                return;

            }
            p.OnPlayerRotate.Normal += OnPlayerRotate_Normal;
            p.OnPlayerMove.Normal += OnPlayerMove_Normal;
            p.OnPlayerBigMove.Normal += OnPlayerBigMove_Normal;
        }

        void OnPlayerBigMove_Normal(Player sender, MoveEventArgs args) {
            MoveGlass(sender);
        }


        void OnPlayerMove_Normal(Player sender, MoveEventArgs args) {
            Curse(sender);
        }

        void OnPlayerRotate_Normal(Player sender, RotateEventArgs args) {
            Curse(sender);
        }

        void MoveGlass(Player sender) {
            if (sender.ExtraData["cursormoveglasslocked"] != null && (bool)sender.ExtraData["cursormoveglasslocked"]) return;
            lock (this) {
                sender.ExtraData["cursormovelocked"] = true;
                if (sender.ExtraData["CursorGlassCenter"] != null) {
                    Vector3S old;
                    if (sender.ExtraData["CursorGlassCenter"].GetType() == typeof(Vector3S))
                        old = (Vector3S)sender.ExtraData["CursorGlassCenter"];
                    else {
                        old = new Vector3S();
                        old.FromString((string)sender.ExtraData["CursorGlassCenter"]);
                    }
                    IEnumerable<Vector3S> blocks = old.GetNearBlocksHollow(5,5,5);
                    foreach (Vector3S v in blocks) {
                        sender.SendBlockChange((ushort)v.x, (ushort)v.z, (ushort)v.y, sender.Level.GetBlock(v));
                    }
                }
                Vector3S pos = new Vector3S((ushort)(sender.Pos.x / 32), (ushort)(sender.Pos.z / 32), (ushort)(sender.Pos.y / 32));
                foreach (Vector3S v in pos.GetNearBlocksHollow(5, 5, 5)) {
                    sender.SendBlockChange((ushort)v.x, (ushort)v.z, (ushort)v.y, 20);
                }
                sender.ExtraData["CursorGlassCenter"] = pos; //TODO: store all glass blocks here to speedup undoing
                sender.ExtraData["cursormoveglasslocked"] = false;
            }
        }

        void Curse(Player sender) {
            if (sender.ExtraData["cursorlocked"] != null && (bool)sender.ExtraData["cursorlocked"]) return;
            lock (this) {
                sender.ExtraData["cursorlocked"] = true;
                if (sender.ExtraData["Cursor"] != null) {
                    Vector3S old;
                    if (sender.ExtraData["Cursor"].GetType() == typeof(Vector3S))
                        old = (Vector3S)sender.ExtraData["Cursor"];
                    else {
                        old = new Vector3S();
                        old.FromString((string)sender.ExtraData["Cursor"]);
                    }
                    sender.SendBlockChange((ushort)old.x, (ushort)old.z, (ushort)old.y, sender.Level.GetBlock(old));
                }
                Vector3S cursor = sender.GetBlockFromView();
                if ((object)cursor != null) {
                    sender.SendBlockChange((ushort)cursor.x, (ushort)cursor.z, (ushort)cursor.y, 21);
#if DEBUG
                    sender.SendMessage(cursor + " : " + sender.Rot[0] + "/" + sender.Rot[1]);
#endif
                }
                sender.ExtraData["Cursor"] = cursor;
                sender.ExtraData["cursorlocked"] = false;
            }
        }

        public void Help(Entity.Player p) {
        }

        public void Initialize() {
            Command.AddReference(this, "cursor");
        }

    }
}
