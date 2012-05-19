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
using System;
using System.Collections.Generic;
using MCForge.API.Events;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.World;
using MCForge.Utils;

namespace CommandDll {
    public class CmdMeasure : ICommand {
        public string Name { get { return "Measure"; } }
        public CommandTypes Type { get { return CommandTypes.Information; } }
        public string Author { get { return "Gamemakergm"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 30; } }
        public void Use(Player p, string[] args) {
            CatchPos cpos = new CatchPos();
            if (args.Length != 0) {
                cpos.ignore = new List<byte>();
                for (int i = 0; i < args.Length; i++) {
                    try {
                        cpos.ignore.Add(Block.NameToBlock(args[i]));
                    }
                    catch {
                        p.SendMessage("Could not find the block '" + args[i] + "'");
                        return;
                    }
                }
                string s = "";
                for (int i = 0; i < cpos.ignore.Count; i++) {
                    s += ((Block)cpos.ignore[i]).Name;
                    if (i == cpos.ignore.Count - 2) s += " and ";
                    else if (i != cpos.ignore.Count - 1) s += ", ";
                }
                p.SendMessage("Ignoring " + s + ".");
            }
            //else
            //cpos.ignore.Add(Block.NameToByte("unknown")); //So it doesn't ignore air.
            p.SendMessage("Place two blocks to determine the edges.");
            //p.CatchNextBlockchange(new Player.BlockChangeDelegate(CatchBlock), (object)cpos);
            p.SetDatapass("CmdMeasure_cpos", cpos);
            p.OnPlayerBlockChange.Normal += new BlockChangeEvent.EventHandler(CatchBlock);

        }
        //public void CatchBlock(Player p, ushort x, ushort z, ushort y, byte NewType, bool placed, object DataPass)
        public void CatchBlock(Player sender, BlockChangeEventArgs args) {
            args.Cancel();
            args.Unregister();
            sender.SendBlockChange(args.X, args.Z, args.Y, sender.Level.GetBlock(args.X, args.Z, args.Y));
            CatchPos cpos = (CatchPos)sender.GetDatapass("CmdMeasure_cpos");
            cpos.FirstBlock = new Vector3S(args.X, args.Z, args.Y);
            sender.SetDatapass("CmdMeasure_cpos", cpos);
            sender.OnPlayerBlockChange.Normal += new BlockChangeEvent.EventHandler(CatchBlock2);
            //p.CatchNextBlockchange(new Player.BlockChangeDelegate(CatchBlock2), (object)cpos);
        }
        //public void CatchBlock2(Player p, ushort x, ushort z, ushort y, byte NewType, bool placed, object DataPass)
        public void CatchBlock2(Player sender, BlockChangeEventArgs args) {
            args.Cancel();
            args.Unregister();
            sender.SendBlockChange(args.X, args.Z, args.Y, sender.Level.GetBlock(args.X, args.Z, args.Y));
            CatchPos cpos = (CatchPos)sender.GetDatapass("CmdMeasure_cpos");
            Vector3S FirstBlock = cpos.FirstBlock;
            ushort xx, zz, yy;
            int count = 0;
            for (xx = Math.Min((ushort)(FirstBlock.x), args.X); xx <= Math.Max((ushort)(FirstBlock.x), args.X); ++xx)
                for (zz = Math.Min((ushort)(FirstBlock.z), args.Z); zz <= Math.Max((ushort)(FirstBlock.z), args.Z); ++zz)
                    for (yy = Math.Min((ushort)(FirstBlock.y), args.Y); yy <= Math.Max((ushort)(FirstBlock.y), args.Y); ++yy) {
                        if (cpos.ignore == null || !cpos.ignore.Contains(sender.Level.GetBlock(xx, zz, yy))) {
                            count++;
                        }
                    }
            sender.SendMessage(count + " blocks are between (" + FirstBlock.x + ", " + FirstBlock.z + ", " + FirstBlock.y + ") and (" + args.X + ", " + args.Z + ", " + args.Y + ")");
        }

        public void Help(Player p) {
            p.SendMessage("/measure [ignore] - Measures all the blocks between two points.");
            p.SendMessage("[ignore] - Enter a block to ignore them");
            p.SendMessage("Shortcut: /ms");
        }
        public struct CatchPos {
            public Vector3S FirstBlock;
            public List<byte> ignore;
            public int count;
        }
        public void Initialize() {
            Command.AddReference(this, new string[2] { "measure", "ms" });
        }
    }
}
