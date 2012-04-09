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
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Core;
using MCForge.World.Blocks;

namespace MCForge.World {
    /// <summary>
    /// This class is used for loading/saving/handling/manipulation of server levels.
    /// </summary>
    public class Level {
        //As a note, the coordinates are right, it is xzy, its based on the users view, not the map itself.
        //WIDTH = X, LENGTH = Z, DEPTH = Y
        //NEST ORDER IS XZY
        public static List<Level> levels = new List<Level>();
        public int PhysicsTick = 100;
        /// <summary>
        /// This delegate is used for looping through the blocks in a level in an automated fashion, and each cycle returns the position in xzy format
        /// </summary>
        /// <param name="x">the loops current block location (x)</param>
        /// <param name="z">the loops current block location (z)</param>
        /// <param name="y">the loops current block location (y)</param>
        public delegate void ForEachBlockDelegateXZY(int x, int z, int y);
        /// <summary>
        /// This delegate is used for looping through the blocks in a level in an automated fashion, and each cycle returns the position in POS format
        /// </summary>
        /// <param name="pos">the loops current block position (pos)</param>
        public delegate void ForEachBlockDelegate(int pos);

        string _name = "main";
        public string Name { get; set; }

        int _TotalBlocks;
        /// <summary>
        /// Get the total blocks in the level
        /// </summary>
        public int TotalBlocks {
            get {
                if (_TotalBlocks == 0) _TotalBlocks = Size.x * Size.z * Size.y;
                return _TotalBlocks;
            }
        }
        /// <summary>
        /// This is the size of the level
        /// </summary>
        public Vector3 Size;
        /// <summary>
        /// Levels current Spawn position
        /// </summary>
        public Vector3 SpawnPos;
        /// <summary>
        /// Levels current Spawn ROT
        /// </summary>
        public byte[] SpawnRot;

        /// <summary>
        /// This holds the map data for the entire map
        /// </summary>
        public byte[] data;

        /// <summary>
        /// Data to store with in the level
        /// </summary>
        public Dictionary<object, object> ExtraData;

        private Level(Vector3 size) {
            Size = size;
            //data = new byte[Size.x, Size.z, Size.y];
            data = new byte[TotalBlocks];

            ExtraData = new Dictionary<object, object>();
        }

        /// <summary>
        /// Create a level with a specified type and a specified size
        /// </summary>
        /// <param name="size">The size to create the level.</param>
        /// <param name="type">The type of the level you want to create</param>
        /// <returns>returns the level that was created</returns>
        public static Level CreateLevel(Vector3 size, LevelTypes type, String name = "main") {
            Level newlevel = new Level(size);
            newlevel.Name = name;

            newlevel.Name = name;
            switch (type) {
                case LevelTypes.Flat:
                    newlevel.CreatePixelArtLevel();
                    break;
            }

            return newlevel;
        }

        private void CreateFlatLevel() {
            int middle = Size.y / 2;
            ForEachBlockXZY(delegate(int x, int z, int y) {
                if (y < middle) {
                    SetBlock((ushort)x, (ushort)z, (ushort)y, Block.BlockList.DIRT);
                    return;
                }
                if (y == middle) {
                    SetBlock((ushort)x, (ushort)z, (ushort)y, Block.BlockList.PINK_CLOTH);
                    return;
                }

            });

            SpawnPos = new Vector3((short)(Size.x / 2), (short)(Size.z / 2), (short)(Size.y));
            SpawnRot = new byte[2] { 0, 0 };
        }

        private void CreatePixelArtLevel() {
            ForEachBlockXZY((x, z, y) => {
                if (x == 0 || x == Size.x - 1 || z == 0 || z == Size.z - 1)
                    SetBlock(x, z, y, Block.BlockList.WHITE_CLOTH);

                if (y == 0)
                    SetBlock(x, z, y, Block.BlockList.GRAY_CLOTH);
            });
            SpawnPos = new Vector3((short)(Size.x / 2), (short)(Size.z / 2), (short)(Size.y));
            SpawnRot = new byte[2] { 0, 0 };
        }

        /// <summary>
        /// Load a level (todo)
        /// </summary>
        /// <returns>the loaded level</returns>
        public Level LoadLevel() {
            //TODO
            return null;
        }

        /// <summary>
        /// loop through all the blocks in xzy running a delegated method for each block, the delegated method will be bassed coordinated in xzy format
        /// </summary>
        /// <param name="FEBD">the delegate to call on each cycle</param>
        public void ForEachBlockXZY(ForEachBlockDelegateXZY FEBD) {
            for (int x = 0; x < Size.x; x++) {
                for (int z = 0; z < Size.z; z++) {
                    for (int y = 0; y < Size.y; y++) {
                        FEBD(x, z, y);
                    }
                }
            }
        }
        /// <summary>
        /// loop through all the blocks in xzy running a delegated method for each block, the delegated method will be passed coordinated in int format
        /// </summary>
        /// <param name="FEBD">the delegate to call on each cycle</param>
        public void ForEachBlock(ForEachBlockDelegate FEBD) {
            for (int i = 0; i < data.Length; ++i) {
                FEBD(i);
            }
        }


        /// <summary>
        /// Causes a block change for the level
        /// </summary>
        /// <param name="x">Location of x</param>
        /// <param name="z">Location of z</param>
        /// <param name="y">Location of y</param>
        /// <param name="block">Block to set</param>
        public void BlockChange(ushort x, ushort z, ushort y, byte block) {
            if (y == Size.y) return;
            byte currentType = GetBlock(x, z, y);

            if (block == currentType) return;

            SetBlock(x, z, y, block);
            Player.GlobalBlockchange(this, x, z, y, block);

            //TODO Special stuff for block changing
        }

        #region SetBlock And Overloads
        void SetBlock(Vector3 pos, Block block) {
            block.SetBlock(pos, this);
        }
        void SetBlock(int x, int z, int y, Block block) {
            block.SetBlock(x, y, z, this);
        }
        void SetBlock(ushort x, ushort z, ushort y, Block block) {
            block.SetBlock(x, y, z, this);
        }
        void SetBlock(int pos, Block block) {
            block.SetBlock(pos, this);
        }
        void SetBlock(Vector3 pos, byte block) {
            SetBlock(pos.x, pos.z, pos.y, block);
        }
        internal void SetBlock(int x, int z, int y, byte block) {
            SetBlock((ushort)x, (ushort)z, (ushort)y, block);
        }
        internal void SetBlock(ushort x, ushort z, ushort y, byte block) {
            SetBlock(PosToInt(x, z, y), block);

        }
        internal void SetBlock(int pos, byte block) {
            data[pos] = block;
        }
        #endregion
        #region GetBlock and Overloads
        /// <summary>
        /// get the block (byte) at an xzy pos
        /// </summary>
        /// <param name="pos">the pos to check and return</param>
        /// <returns>a byte that represents the blocktype at the given location</returns>
        public byte GetBlock(Vector3 pos) {
            return GetBlock(pos.x, pos.z, pos.y);
        }
        /// <summary>
        /// get the block at a given xzy pos
        /// </summary>
        /// <param name="x">x pos to get</param>
        /// <param name="z">z pos to get</param>
        /// <param name="y">y pos to get</param>
        /// <returns>a byte that represents the blocktype at the given location</returns>
        public byte GetBlock(int x, int z, int y) {
            return GetBlock(PosToInt((ushort)x, (ushort)z, (ushort)y));
        }
        /// <summary>
        /// get the block at a given xzy position
        /// </summary>
        /// <param name="x">x pos to get</param>
        /// <param name="z">z pos to get</param>
        /// <param name="y">y pos to get</param>
        /// <returns>a byte that represents the blocktype at the given location</returns>
        public byte GetBlock(ushort x, ushort z, ushort y) {
            return GetBlock(PosToInt(x, z, y));
        }
        /// <summary>
        /// Get the block at a given pos in the data array
        /// </summary>
        /// <param name="pos">the pos to get the block from</param>
        /// <returns>a byte that represents the blocktype at the given location</returns>
        public byte GetBlock(int pos) {
            try {
                return data[pos];
            }
            catch (Exception e) {
                Server.Log(e);
                return (Block)50; //Unknown Block
            }
        }
        #endregion

        /// <summary>
        /// Convert a pos (xzy) into a single INT pos
        /// </summary>
        /// <param name="x">X position to convert</param>
        /// <param name="z">Z position to convert</param>
        /// <param name="y">Y position to convert</param>
        /// <returns>an integer representing the given block position in the DATA array above.</returns>
        public int PosToInt(ushort x, ushort z, ushort y) {
            if (x < 0) { return -1; }
            if (x >= Size.x) { return -1; }
            if (y < 0) { return -1; }
            if (y >= Size.y) { return -1; }
            if (z < 0) { return -1; }
            if (z >= Size.z) { return -1; }
            return x + z * Size.x + y * Size.x * Size.z;
        }
        /// <summary>
        /// Convert an int POS to an xzy pos
        /// </summary>
        /// <param name="pos">The int pos to convert</param>
        /// <returns>a 3 dimensional representation of the block position</returns>
        public Vector3 IntToPos(int pos) {
            short y = (short)(pos / Size.x / Size.z); pos -= y * Size.x * Size.z;
            short z = (short)(pos / Size.x); pos -= z * Size.x;
            short x = (short)pos;

            return new Vector3(x, z, y);
        }
        /// <summary>
        /// Return the position (int) relative to a given block position (int) given an offset of xzy
        /// </summary>
        /// <param name="pos">the integral pos to start at</param>
        /// <param name="x">the offset along the x axis</param>
        /// <param name="z">the offset along the z axis</param>
        /// <param name="y">the offset along the y axis</param>
        /// <returns>returns an int representing the offset block location in the data array</returns>
        public int IntOffset(int pos, int x, int z, int y) {
            return pos + x + z * Size.x + y * Size.x * Size.z;
        }

        /// <summary>
        /// An enumeration of all the types of levels
        /// </summary>
        public enum LevelTypes {
            Flat,
        }


        public static object FindLevel(string p) {
            throw new NotImplementedException();
        }
    }
}
