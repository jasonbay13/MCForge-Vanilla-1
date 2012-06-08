/*
Copyright 2012 MCForge
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
using System.Linq;
using MCForge.Interface.Command;
using MCForge.Groups;
using MCForge.Utils;
using MCForge.API.Events;
using MCForge.Entity;
using MCForge.World;

namespace CommandDll.Building {
    public class CmdLine : ICommand {
        public string Name {get { return "Line"; }}
        public CommandTypes Type {get {return CommandTypes.Building;}}
        public string Author {get { return "headdetect"; }}
        public int Version {get { return 1; }}
        public string CUD {get { return "com.mcforge.commands.line"; }}
        public byte Permission {
            get {
                return (byte)PermissionLevel.Builder;
            }
        }
        public void Use(Player p, string[] args) {
            if (p == null) {
                Logger.Log("This command can only be used in game");
                return;
            }

            p.SendMessage("Place two blocks to determine the corners.");

            byte block = 0;

            if (args.Length > 0) {
                byte test = Block.NameToBlock(args[0]);
                if (test == 255) {
                    p.SendMessage("That is not a valid block");
                    return;
                }
                block = test;
            }
            else
                block = 255;

            //TODO: Check if user can place block
            //If user can put all of the blocks down

            p.ExtraData.CreateIfNotExist<object, object>("Command.Line", new BlockInfo(255,new Vector3S(0,0,0)));
            p.OnPlayerBlockChange.Normal += new Event<Player, BlockChangeEventArgs>.EventHandler(CatchBlockOne);

        }

        public void Help(Player p) {
            throw new NotImplementedException();
        }

        public void Initialize() {
            Command.AddReference(this, "line");
        }

        void CatchBlockOne(Player sender, BlockChangeEventArgs e) {
            BlockInfo block = (BlockInfo)sender.ExtraData.GetIfExist<object, object>("Command.Line");

            sender.ExtraData.ChangeOrCreate<object, object>("Command.Line", new BlockInfo(block.Block != 255 ? block.Block : e.Holding, new Vector3S(e.X, e.Z, e.Y)));
            e.Cancel();
            sender.OnPlayerBlockChange.Normal -= CatchBlockOne;
            sender.OnPlayerBlockChange.Normal += new Event<Player, BlockChangeEventArgs>.EventHandler(CatchBlockTwo);
        }

        void CatchBlockTwo(Player sender, BlockChangeEventArgs e) {
            e.Cancel();
            sender.OnPlayerBlockChange.Normal -= CatchBlockTwo;

            try {
                BlockInfo raw = (BlockInfo)sender.ExtraData.GetIfExist<object, object>("Command.Line");
                Vector3S from = raw.Pos;
                Vector3S to = new Vector3S(e.X, e.Z, e.Y);
                byte block = raw.Block;

                IEnumerable<Vector3S> path = from.PathTo(to);

                foreach (var pos in path) {
                    if (!sender.Level.IsInBounds(pos))
                        continue;
                    sender.Level.BlockChange(pos, block);
                }


                sender.SendMessage(string.Format("Changed {0} blocks in a line", path.Count()));
            }
            catch (Exception er) {
                sender.SendMessage("An Error occurred while trying to make a pretty line");
                Logger.LogError(er);
            }

            sender.ExtraData.Remove("Command.Line");
        }

        private struct BlockInfo {
            public byte Block;
            public Vector3S Pos;

            public BlockInfo(byte block, Vector3S pos) {
                Block = block;
                Pos = pos;
            }
        }
    }
}
