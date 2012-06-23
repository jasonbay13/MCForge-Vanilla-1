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
        }

        void OnPlayerMove_Normal(Player sender, MoveEventArgs args) {
            Curse(sender);
        }

        void OnPlayerRotate_Normal(Player sender, RotateEventArgs args) {
            Curse(sender);
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
                    Player.GlobalBlockchange(sender.Level, (ushort)old.x, (ushort)old.z, (ushort)old.y, sender.Level.GetBlock(old));
                }
                Vector3S cursor = sender.GetBlockFromView();
                if ((object)cursor != null) {
                    Player.GlobalBlockchange(sender.Level, (ushort)cursor.x, (ushort)cursor.z, (ushort)cursor.y, 21);
                    sender.SendMessage(cursor + " : " + sender.Rot[0] + "/" + sender.Rot[1]);
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
