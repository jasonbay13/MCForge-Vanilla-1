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
using System.IO;
using System.IO.Compression;
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
        public string name { get; set; }

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
        public Dictionary<string, string> ExtraData;

        private Level(Vector3 size) {
            Size = size;
            //data = new byte[Size.x, Size.z, Size.y];
            data = new byte[TotalBlocks];

            ExtraData = new Dictionary<string, string>();
        }

        /// <summary>
        /// Create a level with a specified type and a specified size
        /// </summary>
        /// <param name="size">The size to create the level.</param>
        /// <param name="type">The type of the level you want to create</param>
        /// <returns>returns the level that was created</returns>
        public static Level CreateLevel(Vector3 size, LevelTypes type, String name = "main") {
            Level newlevel = new Level(size);
            newlevel.name = name;
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
        /// Load a level.
        /// </summary>
        /// <returns>The loaded level</returns>
        public static Level LoadLevel(string levelName) {
            string Name = "levels\\" + levelName + ".lvl";
            Level finalLevel = new Level(new Vector3(32, 32, 32));
            finalLevel.name = levelName;
            try
            {
                var Binary = new BinaryReader(File.Open(Name, FileMode.Open));

                using (Binary)
                {
                    long v = Binary.ReadInt64();
                    if (v != 28542713840690029) //The magic number
                    {
                        Server.Log("Not a new MCForge Level! Attemping to load old MCForge level format!", ConsoleColor.Red, ConsoleColor.Black);
                        //TODO Load old MCForge level here
                    }
                    else //Is a new MCForge level!
                    {
                        string s = Binary.ReadString();
                        int x = Convert.ToInt32(s.Split('@')[0]);
                        int y = Convert.ToInt32(s.Split('@')[1]);
                        int z = Convert.ToInt32(s.Split('@')[2]);
                        finalLevel.Size = new Vector3((short)x, (short)z, (short)y);

                        s = Binary.ReadString();
                        x = Convert.ToInt32(s.Split('!')[0]);
                        y = Convert.ToInt32(s.Split('!')[1]);
                        z = Convert.ToInt32(s.Split('!')[2]);
                        finalLevel.SpawnPos = new Vector3((short)x, (short)z, (short)y);

                        s = Binary.ReadString();
                        int heading = Convert.ToInt32(s.Split('~')[0]);
                        int yaw = Convert.ToInt32(s.Split('~')[1]);
                        finalLevel.SpawnRot = new byte[2] { (byte)heading, (byte)yaw };

                        finalLevel.SpawnPos = new Vector3((short)(finalLevel.Size.x / 2), (short)(finalLevel.Size.z / 2), (short)(finalLevel.Size.y));
                        finalLevel.SpawnRot = new byte[2] { 0, 0 };

                        int count = Binary.ReadInt32();

                        for (int i = 0; i < count; i++) //Metadata for blocks
                        {
                            string key = Binary.ReadString();
                            string value = Binary.ReadString();
                            finalLevel.ExtraData[key] = value;
                        }

                        finalLevel._TotalBlocks = Binary.ReadInt32();
                        int ByteLength = Binary.ReadInt32();
                        byte[] b = Decompress(Binary.ReadBytes(ByteLength));
                        finalLevel.data = new byte[finalLevel._TotalBlocks];
                        finalLevel.data = b;
                        try
                        {
                            string EOF = Binary.ReadString();
                            if (EOF != "EOF")
                            {
                                Binary.Dispose();
                                return null;
                            }
                        }
                        catch { Binary.Dispose(); return null; }
                    }
                }
                Binary.Dispose();
                return finalLevel;
            }
            catch (Exception e) { Server.Log(e.Message); Server.Log(e.StackTrace); } return null;
        }


        /// <summary>
        /// Saves this world to a given directory
        /// in the MCForge-only binary format.
        /// </summary>
        /// <param name="TargetDirectory">The target directory.</param>
        /// <param name="FileName">Name of the file.</param>
        /// <remarks>The resulting files are not compatible with the official Minecraft software.</remarks>
        public bool SaveToBinary()
        {
            string Name = "levels\\" + name + ".lvl";
            var Binary = new BinaryWriter(File.Open(Name, FileMode.Create));

            try
            {
                Binary.Write(0x6567726f66636d); //Magic Number to make sure it is a compatible file.
                Binary.Write(Size.x + "@" + Size.y + "@" + Size.z);
                Binary.Write(SpawnPos.x + "!" + SpawnPos.y + "!" + SpawnPos.z); //Unused
                Binary.Write(SpawnRot[0] + "~" + SpawnRot[1]); //Unused
                Binary.Write(ExtraData.Count);
                foreach (var pair in ExtraData)
                {
                    Binary.Write(pair.Key);
                    Binary.Write(pair.Value);
                }
                Binary.Write(_TotalBlocks);
                Binary.Write(Compress(data).Length);
                Binary.Write(Compress(data));
                Binary.Write("EOF"); //EOF makes sure the entire file saved.
            }
            finally
            {
                Binary.Dispose();
            }
            return true;
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

        /// <summary>
        /// Compresses the specified byte array.
        /// </summary>
        /// <param name="input">The byte array.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private byte[] Compress(byte[] input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream deflateStream = new GZipStream(ms, CompressionMode.Compress))
                {
                    deflateStream.Write(input, 0, input.Length);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Decompresses the specified byte array.
        /// </summary>
        /// <param name="gzip">The byte array.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        /// <summary>
        /// Finds the specified level.
        /// </summary>
        /// <param name="LevelName">Name of the level.</param>
        public static Level FindLevel(string LevelName)
        {
            try
            {
                return levels.Find(delegate(Level e)
                {
                    if (e.name == LevelName)
                        return true;
                    else
                        return false;
                });
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Adds the level to the level list.
        /// </summary>
        /// <param name="level">Name of the level.</param>
        public static void AddLevel(Level level)
        {
            levels.Add(level);
        }
    }
}

