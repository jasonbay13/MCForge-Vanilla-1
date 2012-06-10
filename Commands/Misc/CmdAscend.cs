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
using System.Collections.Generic;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.World;
using MCForge.Utils;

namespace MCForge.Commands
{
    public class CmdAscend : ICommand
    {
        public string Name { get { return "Ascend"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            List<Block> blocks = new List<Block>(new Block[] { Block.BlockList.AIR, Block.BlockList.RED_MUSHROOM, Block.BlockList.BROWN_MUSHROOM, Block.BlockList.RED_FLOWER, Block.BlockList.YELLOW_FLOWER, Block.BlockList.ACTIVE_LAVA, Block.BlockList.ACTIVE_WATER, Block.BlockList.WATER, Block.BlockList.LAVA });
            ushort top = (ushort)(p.Level.Size.y), x = (ushort)(p.Pos.x / 32), y = (ushort)(p.Pos.y / 32), z = (ushort)(p.Pos.z / 32); ;
            bool tpd = false;
            while (y < top) { y++;
                if (p.Level.GetBlock(x, z, y) == Block.BlockList.AIR && p.Level.GetBlock(x, z, (ushort)(y + 1)) == Block.BlockList.AIR && !blocks.Contains(p.Level.GetBlock(x, z, (ushort)(y - 1)))) {
                    try { p.SendToPos(new Vector3S((ushort)(p.Pos.x), (ushort)(p.Pos.z), (ushort)((y + 1) * 32)), p.Rot); }
                    catch { p.SendMessage("An error has occured while trying to ascend!"); return; }
                    p.SendMessage("You have ascended!"); tpd = true;
                    break;
                }
            }
            if (!tpd) { p.SendMessage("No free spaces found above you!"); }
        }
        public void Help(Player p)
        {
            p.SendMessage("/ascend - Teleports you to the first free space above you");
        }

        public void Initialize()
        {
            Command.AddReference(this, "ascend");
        }
    }
}