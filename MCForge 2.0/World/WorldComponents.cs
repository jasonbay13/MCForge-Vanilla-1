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
using System.Text;
using MCForge.Core;
using MCForge.Utils;
using MCForge.Entity;

namespace MCForge.World {
    public class WorldComponent {

        private BlockComponent[] components;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldComponent"/> class.
        /// </summary>
        /// <param name="xyz">The pieces of the component.</param>
        public WorldComponent(params BlockComponent[] components) {
            this.components = components;
        }

        /// <summary>
        /// Draws the on level.
        /// </summary>
        /// <param name="mLevel">The level to draw on.</param>
        /// <param name="startPos">The start pos.</param>
        /// <param name="blockOverlay">A block to set the component as. If it is not already set</param>
        /// <remarks>if the block in the component is set to 255 a block overlay will be used automaticly, no block change will occur if the block is at 254</remarks>
        public void DrawOnLevel(Level mLevel, Vector3S startPos, Direction dir, byte blockOverlay = 1) {
            for (int i = 0; i < components.Length; i++) {
                var comp = components[i];
                if (comp.Block == 254)
                    continue;
                switch (dir) {
                    case Direction.East:
                        mLevel.SetBlock(comp.Pos + startPos, comp.Block == 255 ? blockOverlay : comp.Block);
                        break;
                    case Direction.West:
                        mLevel.SetBlock(comp.Pos - startPos, comp.Block == 255 ? blockOverlay : comp.Block);
                        break;
                }
            }
        }
        public static void GenerateTree(Player p, ushort x, ushort z, ushort y, TreeType type, bool gothrough = false)
        {
            byte air = Block.BlockList.AIR,
                 sap = Block.BlockList.SAPLING,
                 trunk = Block.BlockList.WOOD,
                 leaf = Block.BlockList.LEAVES,
                 green = Block.BlockList.GREEN_CLOTH;

            #region Normal
            if (type == TreeType.Classic) {
                    byte height = (byte)MathUtils.Random.Next(5, 8);
                    short top = (short)(height - MathUtils.Random.Next(2, 4));
                    ushort xxx, yyy, zzz;
                    for (ushort yy = 0; yy < top + height - 1; yy++) { //trunk
                        if (!gothrough && p.Level.GetBlock(x, z, (ushort)(y + yy)) != air && p.Level.GetBlock(x, z, (ushort)(y + yy)) != sap) { continue; }
                        p.Level.BlockChange(new Vector3D(x, z, y + yy), trunk);
                    }
                    for (short xx = (short)-top; xx <= top; ++xx) {
                        for (short yy = (short)-top; yy <= top; ++yy) {
                            for (short zz = (short)-top; zz <= top; ++zz) {
                                short Dist = (short)(Math.Sqrt(xx * xx + yy * yy + zz * zz));
                                if (Dist < top + 1) {
                                    if (MathUtils.Random.Next((int)(Dist)) < 2) {
                                        try {
                                            xxx = (ushort)(x + xx);
                                            yyy = (ushort)(y + yy + height);
                                            zzz = (ushort)(z + zz);
                                            if ((xxx != x || zzz != z || yy >= top - 1) && (gothrough || p.Level.GetBlock(xxx, zzz, yyy) == air)) {
                                                p.Level.BlockChange(new Vector3D(xxx,zzz,yyy), leaf);
                                            }
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }
                    }
                    //p.Level.BlockChange(x, z, y, trunk);
            }
            #endregion
            #region Notchy
            if (type == TreeType.Notch) {
                byte dist, tile, height = (byte)MathUtils.Random.Next(3, 7), top = (byte)(height - 2);
                short xx, yy, zz;
                ushort xxx, yyy, zzz;
                for (yy = 0; yy <= height; yy++) {
                    yyy = (ushort)(y + yy);
                    tile = p.Level.GetBlock(x, z, yyy);
                    if (gothrough || tile == air || (yyy == y && tile == sap)) { p.Level.BlockChange(x, z, yyy, trunk, p); }
                }
                for (yy = top; yy <= height + 1; yy++) {
                    dist = yy > height - 1 ? (byte)1 : (byte)2;
                    for (xx = (short)-dist; xx <= dist; xx++) {
                        for (zz = (short)-dist; zz <= dist; zz++) {
                            xxx = (ushort)(x + xx);
                            zzz = (ushort)(z + zz);
                            yyy = (ushort)(y + yy);
                            tile = p.Level.GetBlock(xxx, zzz, yyy);
                            if ((xxx == x & zzz == z && yy <= height) || (!gothrough && tile != air)) { continue; }
                            if (Math.Abs(xx) == dist && Math.Abs(zz) == dist) {
                                if (yy > height) { continue; }
                                if (MathUtils.Random.Next(2) == 0) { p.Level.BlockChange(xxx, zzz, yyy, leaf, p); }
                            }
                            else { p.Level.BlockChange(xxx, zzz, yyy, leaf, p); }
                        }
                    }
                }
                p.Level.BlockChange(x, z, y, trunk, p);
            }
            #endregion
            #region Swamp
            if (type == TreeType.Swamp) {
                byte dist,tile, height = (byte)MathUtils.Random.Next(4, 8), top = (byte)(height - 2);
                short xx,yy,zz;
                ushort xxx,yyy,zzz;
                for (yy = 0; yy <= height; yy++) {
                    yyy = (ushort)(y + yy);
                    tile = p.Level.GetBlock(x, z, yyy);
                    if (gothrough || tile == air || (yyy == y && tile == sap)) { p.Level.BlockChange(x, z, yyy, trunk, p); }
                }
                for (yy = top; yy <= height + 1; yy++) {
                    dist = yy > height - 1 ? (byte)2 : (byte)3;
                    for (xx = (short)-dist; xx <= dist; xx++) {
                        for (zz = (short)-dist; zz <= dist; zz++) {
                            xxx = (ushort)(x + xx);
                            yyy = (ushort)(y + yy);
                            zzz = (ushort)(z + zz);
                            tile = p.Level.GetBlock(xxx, zzz, yyy);
                            if ((xxx == x && zzz == z && yy <= height) || (!gothrough && tile != air)) { continue; }
                            if (Math.Abs(xx) == dist && Math.Abs(zz) == dist) {
                                if (yy > height) { continue; }
                                if (MathUtils.Random.Next(2) == 0) { p.Level.BlockChange(xxx, zzz, yyy, leaf, p); }
                            }
                            else { p.Level.BlockChange(xxx, zzz, yyy, leaf, p); }
                        }
                    }
                }
            }        
            #endregion
            #region Bush
            if (type == TreeType.Bush) {
                short top = (short)2;
                ushort xxx, yyy, zzz;
                for (ushort yy = 0; yy < 2; yy++) {
                    if (!gothrough && p.Level.GetBlock(x, z, (ushort)(y + yy)) != air && p.Level.GetBlock(x, z, (ushort)(y + yy)) != sap) { continue; }
                    p.Level.BlockChange(new Vector3D(x, z, y + yy), trunk);
                }
                int rand = MathUtils.Random.Next(2);
                for (short xx = (short)-top; xx <= top; ++xx) {
                    for (short yy = rand == 0 ? (short)0 : (short)-top; yy <= top; ++yy) {
                        for (short zz = (short)-top; zz <= top; ++zz) {
                            short Dist = (short)(Math.Sqrt(xx * xx + yy * yy + zz * zz));
                            if (Dist < top + 1) {
                                if (MathUtils.Random.Next((int)(Dist)) < 2) {
                                    try {
                                        xxx = (ushort)(x + xx);
                                        yyy = rand == 0 ? (ushort)(y + yy) : (ushort)(y + yy + 1);
                                        zzz = (ushort)(z + zz);
                                        if ((xxx != x || zzz != z || yy >= top - 1) && (gothrough || p.Level.GetBlock(xxx, zzz, yyy) == air)) { p.Level.BlockChange(new Vector3D(xxx, zzz, yyy), leaf); }
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                p.Level.BlockChange(x, z, y, trunk, p);
            }
            #endregion
            #region Pine
            if (type == TreeType.Pine) {
                byte height = (byte)MathUtils.Random.Next(6, 8), top = (byte)(height - 2), distance = (byte)2, tile;
                short xx, yy, zz;
                ushort xxx, yyy, zzz;
                for (yy = 0; yy <= height; yy++) {
                    yyy = (ushort)(y + yy);
                    tile = p.Level.GetBlock(x, z, yyy);
                    if (gothrough || tile == air || (yyy == y && tile == sap)) { p.Level.BlockChange(x, z, yyy, trunk, p); }
                }
                for (yy = 0; yy <= (short)(height + 2); yy++) {
                    if (yy == 0) { continue; }
                    if (yy == 1 && MathUtils.Random.Next(2) == 1) { continue; }
                    if (yy == (height + 2)) {
                        if (MathUtils.Random.Next(2) == 1) { continue; }
                        p.Level.BlockChange(x, z, (ushort)(y + yy), leaf, p); continue;
                    }
                    distance = distance == 2 ? (byte)1 : (byte)2;
                    if ((ushort)(yy) >= height) { distance = 1; }
                    for (xx = (short)-distance; xx <= (short)distance; xx++) {
                        for (zz = (short)-distance; zz <= (short)distance; zz++) {
                            xxx = (ushort)(x + xx);
                            zzz = (ushort)(z + zz);
                            yyy = (ushort)(y + yy);
                            tile = p.Level.GetBlock(xxx, zzz, yyy);
                            if ((xxx == x & zzz == z && yy <= height) || (!gothrough && tile != air)) { continue; }
                            if (yy == height && height % 2 == 0) { continue; }
                            if (Math.Abs(xx) == (short)distance && Math.Abs(zz) == (short)distance) { continue; }
                            else { p.Level.BlockChange(xxx, zzz, yyy, leaf, p); }
                        }
                    }
                }
            }
            #endregion
            #region Cactus
            if (type == TreeType.Cactus) {
                byte height = (byte)MathUtils.Random.Next(3, 6);
                ushort yy;
                for (yy = 0; yy <= height; yy++) { 
                    if (gothrough || p.Level.GetBlock((ushort)x,(ushort)z, (ushort)(y+yy)) == air) { p.Level.BlockChange(x, z, (ushort)(y + yy), green, p); }
                }
                int ix = 0, iz = 0;
                switch (MathUtils.Random.Next(1, 3)) {
                    case 1: ix = -1; break;
                    case 2: 
                    default: iz = -1; break;
                }
                for (yy = height; yy <= MathUtils.Random.Next(height + 2, height + 5); yy++) {
                    if (gothrough || p.Level.GetBlock(x + ix, z + iz, y + yy) == air) {  p.Level.BlockChange((ushort)(x + ix), (ushort)(z + iz), (ushort)(y + yy), green, p); }
                }
                for (yy = height; yy <= MathUtils.Random.Next(height + 2, height + 5); yy++) {
                    if (gothrough || p.Level.GetBlock(x + ix, z + iz, y + yy) == air) { p.Level.BlockChange((ushort)(x - ix), (ushort)(z - iz), (ushort)(y + yy), green, p); }
                }
            }
            #endregion
        }
    }

    public enum Direction {
        East,
        West,
        North,
        South,
        Up,
        Down
    }

  public enum TreeType { 
      Classic, 
      Notch, 
      Swamp, 
      Cactus,
      Bush,
      Pine
  }

    /// <summary>
    /// A struct containting a Block and position
    /// </summary>
    public struct BlockComponent {
        /// <summary>
        /// The type of block
        /// </summary>
        public byte Block;
        /// <summary>
        /// The position
        /// </summary>
        public Vector3S Pos;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockComponent"/> struct.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="mVec">The position.</param>
        public BlockComponent(byte block, Vector3S mVec) {
            this.Block = block;
            this.Pos = mVec;
        }

    }
}
