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
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;
using MCForge.API.Events;
using System.Drawing;
using System.Collections.Generic;

namespace MCForge.Commands {
    public class CmdWrite2 : ICommand {
        public string Name { get { return "Write"; } }
        public CommandTypes Type { get { return CommandTypes.Building; } }
        public string Author { get { return "Arrem, ninedrafted"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args) {
            if (args.Length == 0) { p.SendMessage("Please specify a message to write!"); Help(p); return; }
            CatchPos cpos = new CatchPos();
            cpos.message = string.Join(" ", args);
            p.SetDatapass(Name, cpos);
            p.SendMessage("Place two blocks to determine the direction!");
            p.OnPlayerBlockChange.Normal += BlockChange1;
        }
        void BlockChange1(Player sender, BlockChangeEventArgs args) {
            CatchPos cpos = (CatchPos)sender.GetDatapass(Name);
            cpos.pos = new Vector3S((short)args.X, (short)args.Z, (short)args.Y);
            args.Cancel();
            sender.OnPlayerBlockChange.Normal -= BlockChange1;
            sender.SetDatapass(Name, cpos);
            sender.OnPlayerBlockChange.Normal += BlockChange2;
        }
        void BlockChange2(Player sender, BlockChangeEventArgs args) {
            string direction = null;
            sender.OnPlayerBlockChange.Normal -= BlockChange2;
            CatchPos cpos = (CatchPos)sender.GetDatapass(this.Name);
            foreach (Vector3S v in BlockString(cpos.message, cpos.pos, new Vector3S(args.X, args.Z, args.Y), sender.Level.Size)) {
                sender.Level.BlockChange(v, args.Holding, sender);
            }
            args.Cancel();
            return;
        }
        public void Help(Player p) {
            p.SendMessage("/write <message> - writes a specified message in blocks");
            p.SendMessage("Shortcut: /wt");
        }

        public void Initialize() {
            Command.AddReference(this, new string[2] { "write", "wt" });
        }
        protected struct CatchPos {
            public byte block;
            public Vector3S pos;
            public string message;
        }

        //TODO: fix if target is to close at origin
        IEnumerable<Vector3S> BlockString(string text, Vector3S origin, Vector3S target, Vector3S lvlSize) {
            Font font = new Font("Sans-serief",12);
            Image tmp = new Bitmap(1000, 1000);
            Bitmap img = new Bitmap((int)Graphics.FromImage(tmp).MeasureString(text, font).Width, (int)Graphics.FromImage(tmp).MeasureString(text, font).Height);
            tmp = null;
            Graphics g = Graphics.FromImage(img);
            g.FillRectangle(Brushes.White, 0, 0, img.Width, img.Height);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            g.DrawString(text, font, Brushes.Black, new PointF(0, 0));
            List<Vector3S> path = new List<Vector3S>();
            foreach (Vector3S p in origin.PathTo(target)) {
                path.Add(p);
            }
            if (path.Count < 2) yield break;
            for (int x = 0; x < img.Width; x++) {
                for (int y = 0; y < img.Height; y++) {
                    if (img.GetPixel(x, y).ToArgb() != Color.White.ToArgb()) {
                        Vector3S ret = new Vector3S();
                        ret.x = (short)(origin.x + ((path[x % path.Count].x - origin.x) + (path[path.Count - 1].x - origin.x + path[1].x - origin.x) * (x / path.Count)));
                        ret.y = (short)(origin.y + ((path[x % path.Count].y - origin.y) + (path[path.Count - 1].y - origin.y + path[1].y - origin.y) * (x / path.Count)) + img.Height - y);
                        ret.z = (short)(origin.z + ((path[x % path.Count].z - origin.z) + (path[path.Count - 1].z - origin.z + path[1].z - origin.z) * (x / path.Count)));
                        if (ret.x < lvlSize.x && ret.y < lvlSize.y && ret.z < lvlSize.z && ret.x >= 0 && ret.y >= 0 && ret.z >= 0)
                            yield return ret;
                    }
                }
            }

        }
    }
}