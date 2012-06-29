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
using MCForge.World.Blocks;
using MCForge.World.Physics;
using MCForge.Utils;

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
             new Water(),
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
             new SpringGreen(),
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
             new Obsidian(),
             new Active_Water(),
             new Active_Lava()
    };
        internal static readonly List<Block> CustomBlockList = new List<Block>();
        public abstract byte VisibleBlock { get; }
        public abstract string Name { get; }
        public abstract byte Permission { get; }
        public virtual void SetBlock(Vector3S pos, Level l) {
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
        
        public static Block NameToBlockType(string name) {
            foreach (Block block in Blocks)
                if (block.Name == name)
                    return block;
            foreach (Block block in CustomBlockList)
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
            return !(NameToBlock(name) == Block.BlockList.UNKNOWN);
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
            if (b >= 49)
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

        public static bool CanWalkThrough(byte blockToCheck) {
            return (blockToCheck == BlockList.AIR ||
                blockToCheck == BlockList.WATER ||
                blockToCheck == BlockList.LAVA ||
                blockToCheck == BlockList.ACTIVE_LAVA ||
                blockToCheck == BlockList.ACTIVE_WATER ||
                blockToCheck == BlockList.RED_FLOWER ||
                blockToCheck == BlockList.RED_MUSHROOM ||
                blockToCheck == BlockList.YELLOW_FLOWER ||
                blockToCheck == BlockList.BROWN_MUSHROOM ||
                blockToCheck == BlockList.SAPLING);
        }

        public static bool CanEscalate(byte blockToCheck) {
            return (blockToCheck == BlockList.WATER ||
                blockToCheck == BlockList.LAVA ||
                blockToCheck == BlockList.ACTIVE_LAVA ||
                blockToCheck == BlockList.ACTIVE_WATER);
        }

        public static bool IsOPBlock(byte blockToCheck) {
            return blockToCheck == Block.BlockList.BEDROCK;
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
            /// Stone Block
            /// </summary>
            public const byte STONE = 1;
            /// <summary>
            /// Grass Block
            /// </summary>
            public const byte GRASS = 2;
            /// <summary>
            /// Dirt Block
            /// </summary>
            public const byte DIRT = 3;
            /// <summary>
            /// Cobblestone Block
            /// </summary>
            public const byte COBBLESTONE = 4;
            /// <summary>
            /// Wood_plank Block
            /// </summary>
            public const byte WOOD_PLANK = 5;
            /// <summary>
            /// Sapling Block
            /// </summary>
            public const byte SAPLING = 6;
            /// <summary>
            /// Bedrock Block
            /// </summary>
            public const byte BEDROCK = 7;
            /// <summary>
            /// Active_water Block
            /// </summary>
            public const byte ACTIVE_WATER = 8;
            /// <summary>
            /// Water Block
            /// </summary>
            public const byte WATER = 9;
            /// <summary>
            /// Active_lava Block
            /// </summary>
            public const byte ACTIVE_LAVA = 10;
            /// <summary>
            /// Lava Block
            /// </summary>
            public const byte LAVA = 11;
            /// <summary>
            /// Sand Block
            /// </summary>
            public const byte SAND = 12;
            /// <summary>
            /// Gravel Block
            /// </summary>
            public const byte GRAVEL = 13;
            /// <summary>
            /// Gold_ore Block
            /// </summary>
            public const byte GOLD_ORE = 14;
            /// <summary>
            /// Iron_ore Block
            /// </summary>
            public const byte IRON_ORE = 15;
            /// <summary>
            /// Coal_ore Block
            /// </summary>
            public const byte COAL_ORE = 16;
            /// <summary>
            /// Wood Block
            /// </summary>
            public const byte WOOD = 17;
            /// <summary>
            /// Leaves Block
            /// </summary>
            public const byte LEAVES = 18;
            /// <summary>
            /// Sponge Block
            /// </summary>
            public const byte SPONGE = 19;
            /// <summary>
            /// Glass Block
            /// </summary>
            public const byte GLASS = 20;
            /// <summary>
            /// Red_cloth Block
            /// </summary>
            public const byte RED_CLOTH = 21;
            /// <summary>
            /// Orange_cloth Block
            /// </summary>
            public const byte ORANGE_CLOTH = 22;
            /// <summary>
            /// Yellow_cloth Block
            /// </summary>
            public const byte YELLOW_CLOTH = 23;
            /// <summary>
            /// Lime_cloth Block
            /// </summary>
            public const byte LIME_CLOTH = 24;
            /// <summary>
            /// Green_cloth Block
            /// </summary>
            public const byte GREEN_CLOTH = 25;
            /// <summary>
            /// Cyan_green_cloth Block
            /// </summary>
            public const byte CYAN_GREEN_CLOTH = 26;
            /// <summary>
            /// Cyan_cloth Block
            /// </summary>
            public const byte CYAN_CLOTH = 27;
            /// <summary>
            /// Blue_cloth Block
            /// </summary>
            public const byte BLUE_CLOTH = 28;
            /// <summary>
            /// Purple_cloth Block
            /// </summary>
            public const byte PURPLE_CLOTH = 29;
            /// <summary>
            /// Indigo_cloth Block
            /// </summary>
            public const byte INDIGO_CLOTH = 30;
            /// <summary>
            /// Violet_cloth Block
            /// </summary>
            public const byte VIOLET_CLOTH = 31;
            /// <summary>
            /// Magenta_cloth Block
            /// </summary>
            public const byte MAGENTA_CLOTH = 32;
            /// <summary>
            /// Pink_cloth Block
            /// </summary>
            public const byte PINK_CLOTH = 33;
            /// <summary>
            /// Black_cloth Block
            /// </summary>
            public const byte BLACK_CLOTH = 34;
            /// <summary>
            /// Gray_cloth Block
            /// </summary>
            public const byte GRAY_CLOTH = 35;
            /// <summary>
            /// White_cloth Block
            /// </summary>
            public const byte WHITE_CLOTH = 36;
            /// <summary>
            /// Yellow_flower Block
            /// </summary>
            public const byte YELLOW_FLOWER = 37;
            /// <summary>
            /// Red_flower Block
            /// </summary>
            public const byte RED_FLOWER = 38;
            /// <summary>
            /// Brown_mushroom Block
            /// </summary>
            public const byte BROWN_MUSHROOM = 39;
            /// <summary>
            /// Red_mushroom Block
            /// </summary>
            public const byte RED_MUSHROOM = 40;
            /// <summary>
            /// Gold_block Block
            /// </summary>
            public const byte GOLD_BLOCK = 41;
            /// <summary>
            /// Iron_block Block
            /// </summary>
            public const byte IRON_BLOCK = 42;
            /// <summary>
            /// Double_stair Block
            /// </summary>
            public const byte DOUBLE_STAIR = 43;
            /// <summary>
            /// Stair Block
            /// </summary>
            public const byte STAIR = 44;
            /// <summary>
            /// Brick Block
            /// </summary>
            public const byte BRICK = 45;
            /// <summary>
            /// Tnt Block
            /// </summary>
            public const byte TNT = 46;
            /// <summary>
            /// Bookshelf Block
            /// </summary>
            public const byte BOOKSHELF = 47;
            /// <summary>
            /// Mossy_cobblestone Block
            /// </summary>
            public const byte MOSSY_COBBLESTONE = 48;
            /// <summary>
            /// Obsidian Block
            /// </summary>
            public const byte OBSIDIAN = 49;

            /// <summary>
            /// Unknown Block
            /// </summary>
            public const byte UNKNOWN = 255;
        }
    }
}
