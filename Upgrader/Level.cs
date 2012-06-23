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
using System.Drawing;
using MCForge.World.Loading_and_Saving;
using System.Threading;
using MCForge.Utils;

namespace MCForge.World {
    /// <summary>
    /// This class is used for loading/saving/handling/manipulation of server levels.
    /// This is a slimmed down version for upgrading
    /// </summary>
    public class Level {

        internal const long MAGIC_NUMBER = 28542713840690029;
        //As a note, the coordinates are right, it is xzy, its based on the users view, not the map itself.
        //WIDTH = X, LENGTH = Z, DEPTH = Y
        //NEST ORDER IS XZY

        /// <summary>
        /// List of levels on the server (loaded)
        /// </summary>
        public static List<Level> Levels { get; set; }

        static Level() {
            Levels = new List<Level>();
        }

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


        public bool BackupLevel {
            get;
            set;
        }


        /// <summary>
        /// Name of the level
        /// </summary>
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
            set { _TotalBlocks = value; }
        }
        /// <summary>
        /// This is the size of the level
        /// </summary>
        public Vector3S Size { get; set; }
        /// <summary>
        /// Levels current Spawn position
        /// </summary>
        public Vector3S SpawnPos { get; set; }
        /// <summary>
        /// Levels current Spawn ROT
        /// </summary>
        public byte[] SpawnRot { get; set; }

        /// <summary>
        /// This holds the map data for the entire map
        /// </summary>
        public byte[] Data;
        
        /// <summary>
        /// This is the current Save Version for this MCForge version
        /// </summary>
        public const byte Version = 1;

        /// <summary>
        /// Empty level with null/default values that need to be assigned after initialized
        /// </summary>
        /// <param name="size">Base size of map (can be changed)</param>
        public Level(Vector3S size) {
            Size = size;
            SpawnPos = size / 2;
            SpawnRot = new byte[] { 128, 128 };
            //data = new byte[Size.x, Size.z, Size.y];
            Data = new byte[TotalBlocks];
            BackupLevel = true;
            //ExtraData = new ExtraData<object, object>();
            //if (ServerSettings.HasKey("PhysicsInterval")) {
            //    physicsSleep = ServerSettings.GetSettingInt("PhysicsInterval");

           // }
            //else physicsSleep = 100;
            physics = new Thread(() => {
                //while (!Server.ShuttingDown) {
                //    Thread.Sleep(physicsSleep);
                //    MCForge.Interfaces.Blocks.Block.DoTick(this);
                //}
            });
            physics.Start();
        }
        int physicsSleep;
        Thread physics;

        /// <summary>
        /// Load a level.
        /// </summary>
        /// <returns>The loaded level</returns>
        //TODO: Load all the types of levels (old mcforge, new mcforge, fcraft, minecpp, etc...)
        public static Level LoadLevel(string levelName) {
            //if (FindLevel(levelName) != null)
            //    return null;
            string name = levelName.Split('\\')[1].Split('.')[0];
            Console.WriteLine("Converting " + name);
            //string Name = "levels\\" + levelName + ".lvl";
            Level finalLevel = new Level(new Vector3S(32, 32, 32));
            finalLevel.Name = levelName;
            try {
                BinaryReader Binary = null;
                try {
                    Binary = new BinaryReader(File.Open(levelName, FileMode.Open));
                }
                catch (Exception e) { 
                	Console.WriteLine(e.ToString());
                	return null;
                }

                using (Binary) {
                    long v = Binary.ReadInt64();
                    if (v != MAGIC_NUMBER) //The magic number
                    {
                        //Binary.Dispose();
                        Binary.Close();
                        return new MCForgeOldMap().Load(name, levelName);
                    }
                    else //Is a new MCForge level!
                    {
                    	return null;
                    }
                }
                Binary.Close();
                //Binary.Dispose();
                //finalLevel.HandleMetaData();
                //Logger.Log("[Level] " + levelName + " was loaded");
                return finalLevel;
            }
            catch (Exception e) { 
                //Logger.Log(e.Message); Logger.Log(e.StackTrace); } 
                return null;
            }
        }

        /// <summary>
        /// Unloads this instance.
        /// </summary>
        public void Unload() {
            SaveToBinary();
            physics.Abort();
            Levels.Remove(this);
        }

        /// <summary>
        /// Saves this world to a given directory
        /// in the MCForge-only binary format.
        /// </summary>
        /// <remarks>The resulting files are not compatible with the official Minecraft software.</remarks>
        public bool SaveToBinary() {
            string Name = "levels\\" + this.Name + ".lvl";
            if (!Directory.Exists("levels")) Directory.CreateDirectory("levels");
            var Binary = new BinaryWriter(File.Open(Name, FileMode.Create));

            try {
                Binary.Write(0x6567726f66636d); //Magic Number to make sure it is a compatible file.
                Binary.Write(Size.x + "@" + Size.y + "@" + Size.z);
                Binary.Write(SpawnPos.x + "!" + SpawnPos.y + "!" + SpawnPos.z); //Unused
                Binary.Write(SpawnRot[0] + "~" + SpawnRot[1]); //Unused
                Binary.Write(0); //No extra data
                //lock (ExtraData) {
                    /*foreach (KeyValuePair<object, object> pair in ExtraData) {
                        if (pair.Key != null && pair.Value != null) {
                            Binary.Write(pair.Key.ToString());
                            if (pair.Value.GetType() == typeof(List<string>)) {
                                List<string> tmp = (List<string>)pair.Value;
                                Binary.Write(MiscUtils.ToString(tmp));
                            }
                            else
                                Binary.Write(pair.Value.ToString());
                        }
                    }*/
                //}
                Binary.Write(_TotalBlocks);
                Binary.Write(Compress(Data).Length);
                Binary.Write(Compress(Data));
                Binary.Write("EOF"); //EOF makes sure the entire file saved.
            }
            finally {
                Binary.Flush();
                Binary.Close();
                //Binary.Dispose();
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
            for (int i = 0; i < Data.Length; ++i) {
                FEBD(i);
            }
        }

        /// <summary>
        /// Determines whether the position is in bounds of the level.
        /// </summary>
        /// <param name="x">The x pos.</param>
        /// <param name="z">The z pos.</param>
        /// <param name="y">The y pos.</param>
        /// <returns>
        ///   <c>true</c> if [is in bounds] [the specified x]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInBounds(int x, int z, int y) {
            return (x > 0 && x < Size.x && y > 0 && y < Size.y && z > 0 && z < Size.z);
        }


        /// <summary>
        /// Determines whether the specified vector is in bounds of the level.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>
        ///   <c>true</c> if [is in bounds] [the specified vector]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInBounds(Vector3S vector) {
            return IsInBounds(vector.x, vector.z, vector.y);
        }

        /// <summary>
        /// Determines whether the specified vector is in bounds of the level.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>
        ///   <c>true</c> if [is in bounds] [the specified vector]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInBounds(Vector3D vector) {
            return IsInBounds((int)vector.x, (int)vector.z, (int)vector.y);
        }
        #region SetBlock And Overloads
        internal void SetBlock(Vector3S pos, byte block) {
            SetBlock(pos.x, pos.z, pos.y, block);
        }
        internal void SetBlock(int x, int z, int y, byte block) {
            SetBlock(PosToInt(x, z, y), block);
        }
        internal void SetBlock(int pos, byte block) {
            if (pos >= 0 && pos < Data.Length)
                Data[pos] = block;
        }
        #endregion
        /// <summary>
        /// Convert a pos (xzy) into a single INT pos
        /// </summary>
        /// <param name="x">X position to convert</param>
        /// <param name="z">Z position to convert</param>
        /// <param name="y">Y position to convert</param>
        /// <returns>an integer representing the given block position in the DATA array above.</returns>
        public int PosToInt(int x, int z, int y) {
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
        public Vector3S IntToPos(int pos) {
            short y = (short)(pos / Size.x / Size.z); pos -= y * Size.x * Size.z;
            short z = (short)(pos / Size.x); pos -= z * Size.x;
            short x = (short)pos;

            return new Vector3S(x, z, y);
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
            /// <summary>
            /// Grass half way up the map, flat
            /// </summary>
            Flat,
            /// <summary>
            /// White walls, with a black floor
            /// </summary>
            Pixel,

            /// <summary>
            /// Inspired from the Nether
            /// </summary>
            Hell
        }

        public enum SaveTypes {
            /// <summary>
            /// New MCForge File Format
            /// </summary>
            MCForge2,

            /// <summary>
            /// Old MCForge Level Format
            /// </summary>
            MCForge,

            /// <summary>
            /// fCraft Level Format
            /// </summary>
            fCraft,
            /// <summary>
            /// Mine c++ Level Format
            /// </summary>
            MineCPP,
            Minecraft
        }

        /// <summary>
        /// Compresses the specified byte array.
        /// </summary>
        /// <param name="input">The byte array.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private byte[] Compress(byte[] input) {
            using (MemoryStream ms = new MemoryStream()) {
                using (GZipStream deflateStream = new GZipStream(ms, CompressionMode.Compress)) {
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
        private static byte[] Decompress(byte[] gzip) {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress)) {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream()) {
                    int count = 0;
                    do {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0) {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        /// <summary>
        /// Adds the level to the level list.
        /// </summary>
        /// <param name="level">Name of the level.</param>
        public static void AddLevel(Level level) {
            if (Levels.Contains(level))
                return;

            Levels.Add(level);
        }

        public override string ToString() {
            return Name;
        }
    }
}
