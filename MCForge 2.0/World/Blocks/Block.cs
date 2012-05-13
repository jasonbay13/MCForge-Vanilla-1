using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Core;
using MCForge.World.Blocks;
using MCForge.World.Physics;
using MCForge.Utilities;

namespace MCForge.World {
    public abstract class Block {
        private static readonly Block UnknownBlock = new UNKNOWN();
        private static Block[] Blocks =  {
             new Air(),
             new Stone(),
             new Grass(),
             new Dirt(),
             new Cobblestone(),
             new Wood(),
             new Shrub(),
             new Adminium(),
             new ActiveWater(),
             new Water(),
             new ActiveLava(),
             new Lava(),
             new Sand(),
             new Gravel(),
             new Gold_Ore(),
             new Iron_Ore(),
             new Coal(),
             new Tree(),
             new Leaves(),
             new Sponge(),
             new Glass(),
             new Red(),
             new Orange(),
             new Yellow(),
             new GreenYellow(),
             new Green(),
             new Cyan(),
             new Blue(),
             new BlueViolet(),
             new Purple(),
             new Magenta(),
             new Pink(),
             new Black(),
             new Gray(),
             new White(),
             new YellowFlower(),
             new RedFlower(),
             new BrownShroom(),
             new RedShroom(),
             new Gold(),
             new Iron(),
             new DoubleStair(),
             new Stair(),
             new Brick(),
             new TNT(),
             new Bookcase(),
             new MossyCobbleStone(),
             new Obsidian()
    };
        private static readonly List<Block> CustomBlockList = new List<Block>();
        public abstract byte VisibleBlock { get; }
        public abstract string Name { get; }
        public abstract byte Permission { get; }
        public virtual void SetBlock(Vector3 pos, Level l) {
            l.SetBlock(pos.x, pos.z, pos.y, VisibleBlock);
        }
        public virtual void SetBlock(int x, int z, int y, Level l) {
            l.SetBlock((ushort)x, (ushort)z, (ushort)y, VisibleBlock);
        }
        public virtual void SetBlock(ushort x, ushort z, ushort y, Level l) {
            l.SetBlock(x, z, y, VisibleBlock);
        }
        public virtual void SetBlock(int pos, Level l) {
            l.SetBlock(pos, VisibleBlock);
        }

        /// <summary>
        /// Add your custom block to the list
        /// </summary>
        /// <param name="b">Block to add</param>
        public static void Add(Block b) {
            if (!CustomBlockList.Contains(b))
                CustomBlockList.Add(b);
        }


        internal static void InIt() //Possibly get rid of this...we might not need it.
        {
            Logger.Log("Loading blocks...");
            //get all custom block info and load it
        }

        /// <summary>
        /// Get a byte based on the name of the block
        /// </summary>
        /// <param name="name">The name of the block</param>
        /// <returns>A byte of the block</returns>
        public static byte NameToBlock(string name) {
            foreach (var block in Blocks)
                if (block.Name == name)
                    return block;
            foreach (var block in CustomBlockList)
                if (block.Name == name)
                    return block;
            return UnknownBlock;

        }

        /// <summary>
        /// Check to see if a block from the specified name exists.
        /// </summary>
        /// <param name="name">Name of the block to check</param>
        /// <returns>If the block exists</returns>
        public static bool ValidBlockName(string name) {
            return !(NameToBlock(name) is UNKNOWN);
        }

        /// <summary>
        /// Converts a Block to a byte
        /// </summary>
        public static implicit operator byte(Block b) {
            return b.VisibleBlock;
        }

        /// <summary>
        /// Converts a byte to a Block
        /// </summary>
        public static implicit operator Block(byte b) {
            if (b > 49)
                return UnknownBlock;
            return Blocks[b];
        }

        /// <summary>
        /// Checks to see if a block is valid (MCProtocol)
        /// </summary>
        /// <param name="blockToCheck">Byte or block to check its validness</param>
        /// <returns>A boolean stating whether its valid</returns>
        public static bool IsValidBlock(byte blockToCheck) {
            return (blockToCheck < 50);
        }

        /// <summary>
        /// List of blocks (in the form of a byte)
        /// </summary>
        public class BlockList {
            /// <summary>
            /// Air Block
            /// </summary>
            public const byte AIR = 0;

            /// <summary>
            /// Air Block
            /// </summary>
            public const byte STONE = 1;

            /// <summary>
            /// Air Block
            /// </summary>
            public const byte GRASS = 2;

            /// <summary>
            /// Air Block
            /// </summary>
            public const byte DIRT = 3;

            /// <summary>
            /// Air Block
            /// </summary>
            public const byte COBBLESTONE = 4;

            /// <summary>
            /// Air Block
            /// </summary>
            public const byte WOOD_PLANK = 5;

            /// <summary>
            /// Air Block
            /// </summary>
            public const byte SAPLING = 6;

            /// <summary>
            /// Air Block
            /// </summary>
            public const byte BEDROCK = 7;

            /// <summary>
            /// Air Block
            /// </summary>
            public const byte ACTIVE_WATER = 8;

            /// <summary>
            /// Air Block
            /// </summary>
            public const byte WATER = 9;

            /// <summary>
            /// Air Block
            /// </summary>
            public const byte ACTIVE_LAVA = 10;

            /// <summary>
            /// Air Block
            /// </summary>
            public const byte LAVA = 11;

            //TODO: finish (badpokerface)
            public const byte SAND = 12;
            public const byte GRAVEL = 13;
            public const byte GOLD_ORE = 14;
            public const byte IRON_ORE = 15;
            public const byte COAL_ORE = 16;
            public const byte WOOD = 17;
            public const byte LEAVES = 18;
            public const byte SPONGE = 19;
            public const byte GLASS = 20;
            public const byte RED_CLOTH = 21;
            public const byte ORANGE_CLOTH = 22;
            public const byte YELLOW_CLOTH = 23;
            public const byte LIME_CLOTH = 24;
            public const byte GREEN_CLOTH = 25;
            public const byte CYAN_GREEN_CLOTH = 26;
            public const byte CYAN_CLOTH = 27;
            public const byte BLUE_CLOTH = 28;
            public const byte PURPLE_CLOTH = 29;
            public const byte INDIGO_CLOTH = 30;
            public const byte VIOLET_CLOTH = 31;
            public const byte MAGENTA_CLOTH = 32;
            public const byte PINK_CLOTH = 33;
            public const byte BLACK_CLOTH = 34;
            public const byte GRAY_CLOTH = 35;
            public const byte WHITE_CLOTH = 36;
            public const byte YELLOW_FLOWER = 37;
            public const byte RED_FLOWER = 38;
            public const byte BROWN_MUSHROOM = 39;
            public const byte RED_MUSHROOM = 40;
            public const byte GOLD_BLOCK = 41;
            public const byte IRON_BLOCK = 42;
            public const byte DOUBLE_STAIR = 43;
            public const byte STAIR = 44;
            public const byte BRICK = 45;
            public const byte TNT = 46;
            public const byte BOOKSHELF = 47;
            public const byte MOSSY_COBBLESTONE = 48;
            public const byte OBSIDIAN = 49;
            public const byte UNKNOWN = 255;
        }

    }
}
