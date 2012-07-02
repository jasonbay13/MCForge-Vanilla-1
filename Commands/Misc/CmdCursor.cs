using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.API.Events;
using MCForge.Utils;
using MCForge.Utils.Settings;

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
                p.OnPlayerBlockChange.Normal -= OnPlayerBlockChange_Normal;
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
                if (p.ExtraData["CursorGlassCenter"] != null) {
                    Vector3S old;
                    if (p.ExtraData["CursorGlassCenter"].GetType() == typeof(Vector3S))
                        old = (Vector3S)p.ExtraData["CursorGlassCenter"];
                    else {
                        old = new Vector3S();
                        old.FromString((string)p.ExtraData["CursorGlassCenter"]);
                    }
                    p.ResendBlockChange(surrounder, old);
                }
                return;
            }
            p.OnPlayerRotate.Normal += OnPlayerRotate_Normal;
            p.OnPlayerMove.Normal += OnPlayerMove_Normal;
            p.OnPlayerBigMove.Normal += OnPlayerBigMove_Normal;
            p.OnPlayerBlockChange.Normal += OnPlayerBlockChange_Normal;
        }

        void OnPlayerBlockChange_Normal(Player sender, BlockChangeEventArgs args) {
            args.Cancel();
            if (args.Current == 0 && args.Action == ActionType.Delete) args.Current = 20;
            sender.OnPlayerBlockChange.Normal -= OnPlayerBlockChange_Normal;
            if (sender.ExtraData["Cursor"] != null) {
                Vector3S cursor;
                if (sender.ExtraData["Cursor"].GetType() == typeof(Vector3S))
                    cursor = (Vector3S)sender.ExtraData["Cursor"];
                else {
                    cursor = new Vector3S();
                    cursor.FromString((string)sender.ExtraData["Cursor"]);
                }
                if (args.Action == ActionType.Place) {
                    sender.Click((ushort)cursor.x, (ushort)cursor.z, (ushort)(cursor.y + 1), args.Holding);
                }
                else {
                    sender.Click((ushort)cursor.x, (ushort)cursor.z, (ushort)(cursor.y), args.Holding, false);
                }
            }
            sender.OnPlayerBlockChange.Normal += OnPlayerBlockChange_Normal;
            Curse(sender);
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
                    sender.ResendBlockChange(surrounder, old);
                }
                Vector3S pos = new Vector3S((ushort)(sender.Pos.x / 32), (ushort)(sender.Pos.z / 32), (ushort)(sender.Pos.y / 32));
                sender.SendBlockChange(surrounder, pos, 20);
                sender.ExtraData["CursorGlassCenter"] = pos;
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
                }
                sender.ExtraData["Cursor"] = cursor;
                sender.ExtraData["cursorlocked"] = false;
            }
        }

        public void Help(Entity.Player p) {
        }
        private Vector3S[] surrounder;
        private int radius;
        public void Initialize() {
            radius = ServerSettings.GetSettingInt("CursorGlassRadius");
            if (radius < 2) radius = 3;
            List<Vector3S> l = new List<Vector3S>();
            foreach (Vector3S v in new Vector3S().GetNearBlocksHollow(radius, radius, radius))
                l.Add(v);
            surrounder = l.ToArray();
            Command.AddReference(this, "cursor");
        }

    }

}