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
                      leaf = Block.BlockList.LEAVES;

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
                    p.Level.BlockChange(x, z, y, trunk);
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
                    if (gothrough || tile == air || (yyy == y && tile == sap)) { p.Level.BlockChange(x, z, yyy, trunk); }
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
                                if (MathUtils.Random.Next(2) == 0) { p.Level.BlockChange(xxx, zzz, yyy, leaf); }
                            }
                            else { p.Level.BlockChange(xxx, zzz, yyy, leaf); }
                        }
                    }
                }
                p.Level.BlockChange(x, z, y, trunk);
            }
            #endregion

        }
    }

    public enum Direction {
        East,
        West,
        North,
        Sourth,
        Up,
        Down
    }

  public enum TreeType { 
      Classic, 
      Notch, 
      Swamp, 
      Cactus
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
