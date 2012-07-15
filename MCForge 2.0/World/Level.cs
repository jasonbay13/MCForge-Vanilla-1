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
using MCForge.Robot;
using MCForge.World.Blocks;
using MCForge.Utils;
using System.Drawing;
using MCForge.World.Loading_and_Saving;
using MCForge.Groups;
using MCForge.Interfaces.Blocks;
using System.Threading;
using MCForge.Utils.Settings;
using MCForge.World.Physics;
using MCForge.SQL;
using MCForge.API.Events;

namespace MCForge.World {
    /// <summary>
    /// This class is used for loading/saving/handling/manipulation of server levels.
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

        /// <summary>
        /// Gets the unloaded levels.
        /// </summary>
        public static List<string> UnloadedLevels {
            get {
                string[] temp = Directory.GetFiles(FileUtils.LevelsPath, "*lvl");

                for (int i = 0; i < temp.Length; i++) {
                    temp[i] = temp[i].Replace(".lvl", "");
                    temp[i] = temp[i].Replace(FileUtils.LevelsPath, "");
                }

                List<string> args = temp.ToList<string>();

                foreach (var lvl in Levels)
                    if (args.Contains(lvl.Name))
                        args.Remove(lvl.Name);
                return args;
            }
        }

        static Level() {
            Levels = new List<Level>();
        }
        
        public List<PhysicsBlock> pblocks = new List<PhysicsBlock>();
        /// <summary>
        /// Doing thread doing the ticks for all physics blocks in this levels.
        /// </summary>
        public Thread PhysicsThread;
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
        
        public PlayerGroup visit = PlayerGroup.Default;

        /// <summary>
        /// Data to store with in the level
        /// </summary>
        public ExtraData<object, object> ExtraData  { get; private set; }

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
            ExtraData = new ExtraData<object, object>();
            if (ServerSettings.HasKey("PhysicsInterval")) {
                physicsSleep = ServerSettings.GetSettingInt("PhysicsInterval");

            }
            else physicsSleep = 100;
            physics = new Thread(() => {
                while (!Server.ShuttingDown) {
                    Thread.Sleep(physicsSleep);
                    MCForge.Interfaces.Blocks.Block.DoTick(this);
                }
            });
            physics.Start();
        }
        int physicsSleep;
        Thread physics;

        /// <summary>
        /// Create a level with a specified type and a specified size. <remarks>Calls OnLevelsLoad event.</remarks>
        /// </summary>
        /// <param name="size">The size to create the level.</param>
        /// <param name="type">The type of the level you want to create</param>
        /// <param name="name">Name of the level to create</param>
        /// <returns>
        /// returns the level that was created
        /// </returns>
        public static Level CreateLevel(Vector3S size, LevelTypes type, string name = "main") {
            Level newlevel = new Level(size) {
                Name = name
            };
            switch (type) {
                case LevelTypes.Flat:
                    var gen = new Generator.LevelGenerator(newlevel);
                    for (int i = newlevel.Size.y / 2; i >= 0; i--)
                        gen.FillPlaneXZ(i, Block.BlockList.DIRT);
                    gen.FillPlaneXZ(newlevel.Size.y / 2, Block.BlockList.GRASS);
                    gen.SetPosition(new Vector3S((short)(newlevel.Size.x / 2 + 1), (short)(newlevel.Size.z / 2 + 1), (short)(newlevel.Size.y / 2 + 1)), new Vector2S());
                    break;
                case LevelTypes.Pixel:
                    newlevel.CreatePixelArtLevel();
                    break;
                case LevelTypes.Hell:
                    Generator.LevelGenerator mGen = new Generator.LevelGenerator(newlevel, Generator.GeneratorTemplate.Hell(newlevel));
                    mGen.Generate();
                    mGen.SetPosition();
                    break;
            }
            OnAllLevelsLoad.Call(newlevel, new LevelLoadEventArgs(true));
            return newlevel;
        }

        private void CreateFlatLevel() {
            int middle = Size.y / 2;
            for (int x = 0; x < Size.x; ++x)
                for (int z = 0; z < Size.z; ++z)
                    for (int y = 0; y <= middle; ++y)
                        Data[x + z * Size.x + y * Size.x * Size.z] = y < middle ? (byte)3 : (byte)2;

            SpawnPos = new Vector3S((short)(Size.x / 2), (short)(Size.z / 2), (short)(Size.y));
            SpawnRot = new byte[2] { 0, 0 };
        }

        private void CreatePixelArtLevel() {
            ForEachBlockXZY((x, z, y) => {
                if (x == 0 || x == Size.x - 1 || z == 0 || z == Size.z - 1)
                    SetBlock(x, z, y, Block.BlockList.WHITE_CLOTH);

                if (y == 0)
                    SetBlock(x, z, y, Block.BlockList.BLACK_CLOTH);
            });

            //TODO: ^ make faster
            SpawnPos = new Vector3S((short)(Size.x / 2), (short)(Size.z / 2), (short)(Size.y));
            SpawnRot = new byte[2] { 0, 0 };
        }

        /// <summary>
        /// Load a level. <remarks>Calls OnLevelLoadEvent.</remarks>
        /// </summary>
        /// <returns>The loaded level</returns>
        //TODO: Load all the types of levels (old mcforge, new mcforge, fcraft, minecpp, etc...)
        public static Level LoadLevel(string levelName) {
            if (FindLevel(levelName) != null)
                return null;

            string Name = "levels/" + levelName + ".lvl";
            Level finalLevel = new Level(new Vector3S(32, 32, 32));
            finalLevel.Name = levelName;
            try {
                BinaryReader Binary = null;
                try {
                    Binary = new BinaryReader(File.Open(Name, FileMode.Open));
                }
                catch { return null; }

                using (Binary) {
                    long v = Binary.ReadInt64();
                    if (v != MAGIC_NUMBER) //The magic number
                    {
                        Binary.Dispose();
                        return new MCForgeOldMap().Load(levelName, Name);
                    }
                    else //Is a new MCForge level!
                    {
                        #region New MCForge Level
                        byte version = Binary.ReadByte();
                        if (version == 1)
                        {
                        	string s = Binary.ReadString();
                        	int x = Convert.ToInt32(s.Split('@')[0]);
                        	int y = Convert.ToInt32(s.Split('@')[1]);
                        	int z = Convert.ToInt32(s.Split('@')[2]);
                        	finalLevel.Size = new Vector3S((short)x, (short)z, (short)y);

                        	s = Binary.ReadString();
                        	x = Convert.ToInt32(s.Split('!')[0]);
                        	y = Convert.ToInt32(s.Split('!')[1]);
                        	z = Convert.ToInt32(s.Split('!')[2]);
                        	finalLevel.SpawnPos = new Vector3S((short)x, (short)z, (short)y);

                        	s = Binary.ReadString();
                        	int heading = Convert.ToInt32(s.Split('~')[0]);
                        	int yaw = Convert.ToInt32(s.Split('~')[1]);
                        	finalLevel.SpawnRot = new byte[2] { (byte)heading, (byte)yaw };

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
                        	finalLevel.Data = new byte[finalLevel._TotalBlocks];
                        	finalLevel.Data = b;
                        	try {
                        		string EOF = Binary.ReadString();
                        		if (EOF != "EOF") {
                        			Binary.Dispose();
                        			return null;
                        		}
                        	}
                        	catch { Binary.Dispose(); return null; }
                        }
                        //TODO: Move to HandleMetaData
                        foreach (string name in MCForge.Interfaces.Blocks.Block.Blocks.Keys) {
                            if (finalLevel.ExtraData["IBlocks" + name] != null && finalLevel.ExtraData["IBlocks" + name].GetType() == typeof(string)) {
                                string hexs = (string)finalLevel.ExtraData["IBlocks" + name];
                                finalLevel.ExtraData["IBlocks" + name] = new List<string>();
                                ((List<string>)finalLevel.ExtraData["IBlocks" + name]).AddHexstrings(hexs);
                            }
                            else if (finalLevel.ExtraData["IBlocks" + name] == null) {
                                finalLevel.ExtraData["IBlocks" + name] = new List<string>();
                            }
                        }
                        #endregion
                    }
                }
                Binary.Dispose();
                finalLevel.HandleMetaData();
                Level.OnAllLevelsLoad.Call(finalLevel, new LevelLoadEventArgs(true));
                Logger.Log("[Level] " + levelName + " was loaded");
                return finalLevel;
            }
            catch (Exception e) { 
                Logger.Log(e.Message); Logger.Log(e.StackTrace); } return null;
        }

        /// <summary>
        /// Unloads this instance.
        /// </summary>
        /// <param name="save">Whether the level will be saved or not.</param>
        public void Unload(bool save = false) {
            if (save)
                SaveToBinary();
            OnLevelUnload.Call(this, new LevelLoadEventArgs(false), OnAllLevelsUnload);
            physics.Abort();
            Levels.Remove(this);
        }

        /// <summary>
        /// Saves this world to a given directory
        /// in the MCForge-only binary format.
        /// </summary>
        /// <remarks>The resulting files are not compatible with the official Minecraft software.</remarks>
        public bool SaveToBinary() {
            string Name = "levels/" + this.Name + ".lvl";
            if (!Directory.Exists("levels")) Directory.CreateDirectory("levels");
            var Binary = new BinaryWriter(File.Open(Name, FileMode.Create));

            try {
                Binary.Write(0x6567726f66636d); //Magic Number to make sure it is a compatible file.
                Binary.Write(Version); //The level saving version
                Binary.Write(Size.x + "@" + Size.y + "@" + Size.z);
                Binary.Write(SpawnPos.x + "!" + SpawnPos.y + "!" + SpawnPos.z); //Unused
                Binary.Write(SpawnRot[0] + "~" + SpawnRot[1]); //Unused
                Binary.Write(ExtraData.Count);
                lock (ExtraData) {
                    foreach (KeyValuePair<object, object> pair in ExtraData) {
                        if (pair.Key != null && pair.Value != null) {
                            Binary.Write(pair.Key.ToString());
                            if (pair.Value.GetType() == typeof(List<string>)) {
                                List<string> tmp = (List<string>)pair.Value;
                                Binary.Write(StringUtils.ToHexString(tmp));
                            }
                            else
                                Binary.Write(pair.Value.ToString());
                        }
                    }
                }
                Binary.Write(_TotalBlocks);
                Binary.Write(Compress(Data).Length);
                Binary.Write(Compress(Data));
                Binary.Write("EOF"); //EOF makes sure the entire file saved.
            }
            finally {
                Binary.Flush();
                Binary.Close();
                Binary.Dispose();
            }
            return true;
        }

        /// <summary>
        /// Loads all levels.
        /// 
        /// </summary>
        public static void LoadAllLevels() {
            FileUtils.CreateDirIfNotExist("levels");
            string[] files = Directory.GetFiles("levels/", "*.lvl");
            foreach (string file in files) {
                Level lvl = LoadLevel(file.Substring(7, file.Length - 11));

                if (lvl == null || Levels.Contains(lvl))
                    continue;
                //Don't judge >.>
                Levels.Add(lvl);
            }

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
        /// Causes a block change for the level
        /// </summary>
        /// <param name="x">Location of x</param>
        /// <param name="z">Location of z</param>
        /// <param name="y">Location of y</param>
        /// <param name="block">Block to set</param>
        /// <param name="p">A player who doesn't need the update.</param>
        public void BlockChange(ushort x, ushort z, ushort y, byte block, Player p = null) {
            if (!IsInBounds(x, z, y) || y == Size.y) {
                Logger.Log("Blockchange((ushort) " + x + ", (ushort)" + z + ", (ushort) " + y + ", (byte) " + block + ", (Player) " + p + ") is outside of level");
                return;
            }
            byte currentType = GetBlock(x, z, y);
            if (block == 0) {
                pblocks.ForEach(pb =>
                                {
                                    if (pb.X == x && pb.Y == y && pb.Z == z)
                                        pblocks.Remove(pb);
                                });
                if (GetBlock(x, z, y - 1) == Block.BlockList.DIRT)
                    BlockChange(x, z, (ushort)(y - 1), Block.BlockList.GRASS, p);
            }
            else
            {
                if (!Block.CanPassLight(block) && GetBlock(x, z, y - 1) == Block.BlockList.GRASS)
                    BlockChange(x, z, (ushort)(y - 1), Block.BlockList.DIRT, p);
                else if (Block.CanPassLight(block) && GetBlock(x, z, y - 1) == Block.BlockList.DIRT)
                    BlockChange(x, z, (ushort)(y - 1), Block.BlockList.GRASS, p);
            }
            if (currentType == 255) {
                    if (MCForge.Interfaces.Blocks.Block.DoAction(p, x, z, y, block, this)) {
                        //may the action caused the block to change, sending to all
                        block = MCForge.Interfaces.Blocks.Block.GetVisibleType(x, z, y, this);
                        Player.GlobalBlockchange(this, x, z, y, block);
                        return;
                    }
                    MCForge.Interfaces.Blocks.Block.RemoveBlock(new Vector3S(x, z, y), this);
            }
            if (block == currentType) return;
            if (p != null) {
                byte blockFrom = GetBlock(x, z, y);
                p.history.Add(new Tuple<short, short, short>((short)x, (short)z, (short)y), blockFrom, block);
                //Database.QueueCommand("INSERT INTO Blocks (UID, X, Y, Z, Level, Deleted, Block, Date, Was) VALUES (" + p.UID + ", " + x + ", " + y + ", " + z + ", '" + Name.MySqlEscape() + "', '" + (block == 0 ? "true" : "false") + "', '" + block.ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + blockFrom.ToString() + "')");
            }

            SetBlock(x, z, y, block);
            if (p == null)
                Player.GlobalBlockchange(this, x, z, y, block);
            else {
                p.SendBlockchangeToOthers(this, x, z, y, block);
                p.SendBlockChange(x, z, y, block);
            }

            if (((Block)block).GetType().BaseType == typeof(PhysicsBlock))
            {
                PhysicsBlock pb = (PhysicsBlock)((PhysicsBlock)block).Clone();
                pb.X = x;
                pb.Y = y;
                pb.Z = z;
                pblocks.Add(pb);
            }

            //TODO Special stuff for block changing
        }

        /// <summary>
        /// Causes a block change for the level
        /// </summary>
        /// <param name="vector">Position of the block</param>
        /// <param name="block">Block to set</param>
        /// <param name="p">A player who doesn't need the update.</param>
        public void BlockChange(Vector3D vector, byte block, Player p = null) {
            BlockChange((ushort)vector.x, (ushort)vector.z, (ushort)vector.y, block, p);
        }

        /// <summary>
        /// Causes a block change for the level
        /// </summary>
        /// <param name="vector">Position of the block</param>
        /// <param name="block">Block to set</param>
        /// <param name="p">A player who doesn't need the update.</param>
        public void BlockChange(Vector3S vector, byte block, Player p = null) {
            BlockChange((ushort)vector.x, (ushort)vector.z, (ushort)vector.y, block, p);
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
            return (x >= 0 && x < Size.x && y >= 0 && y < Size.y && z >= 0 && z < Size.z);
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
        public bool IsInBounds(uint x, uint z, uint y) {
            return (x < Size.x && y < Size.y && z < Size.z);
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
            if (!IsInBounds(x,z,y)) {
                Logger.Log("SetBlock(" + x + ", " + z + ", " + y + ") is outside of the level", LogType.Debug);
                return;
            }
            SetBlock(PosToInt(x, z, y), block);
        }
        internal void SetBlock(int pos, byte block) {
            if (pos >= 0 && pos < Data.Length)
                Data[pos] = block;
        }
        #endregion
        #region GetBlock and Overloads
        /// <summary>
        /// get the block (byte) at an xzy pos
        /// </summary>
        /// <param name="pos">the pos to check and return</param>
        /// <returns>a byte that represents the blocktype at the given location</returns>
        public byte GetBlock(Vector3S pos) {
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
            if (!IsInBounds(x,z,y)) {
                Logger.Log("GetBlock((int)" + x + ", (int)" + z + ", (int)" + y + ") is outside of the level", LogType.Debug);
            }
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
            if (!IsInBounds(x,z,y)) {
                Logger.Log("GetBlock((ushort)" + x + ", (ushort)" + z + ", (ushort)" + y + ") is outside of the level", LogType.Debug);
            }
            return GetBlock(PosToInt(x, z, y));
        }
        /// <summary>
        /// Get the block at a given pos in the data array
        /// </summary>
        /// <param name="pos">the pos to get the block from</param>
        /// <returns>a byte that represents the blocktype at the given location</returns>
        public byte GetBlock(int pos) {
            if (pos >= 0 && pos < Data.Length)
                return Data[pos];
            else {
                //Logger.Log("Out of bounds in Level.GetBlock(int pos)", LogType.Error);
                //Logger.Log("Tried to get block at " + pos + " pos!", LogType.Error);
                //^ Gets annoying with physics
                return Block.BlockList.UNKNOWN; //Unknown Block
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
        /// Finds the specified level.
        /// </summary>
        /// <param name="LevelName">Name of the level.</param>
        public static Level FindLevel(string LevelName) {
            try {
                return Levels.Find(e => {
                    return e.Name.ToLower() == LevelName.ToLower();
                });
            }
            catch (Exception e) { Logger.LogError(e); }
            return null;
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

        /// <summary>
        /// Handles the extra data for the level
        /// </summary>
        public void HandleMetaData() {
            if (ExtraData.Count > 0) {
                try {
                    foreach (var pair in ExtraData) {
                        if (pair.Key.ToString().StartsWith("Bot")) //Load bots
                        {
                            string[] StringSplit = pair.Value.ToString().Split(' ');
                            if (StringSplit.Length == 9) {
                                ushort x = Convert.ToUInt16(StringSplit[4]);
                                ushort y = Convert.ToUInt16(StringSplit[5]);
                                ushort z = Convert.ToUInt16(StringSplit[6]);
                                Bot TemporaryBot = new Bot(StringSplit[0],
                                    new Vector3S(x, z, y),
                                    new byte[] { Convert.ToByte(StringSplit[7]), Convert.ToByte(StringSplit[8]) }, this,
                                    Convert.ToBoolean(StringSplit[1]), Convert.ToBoolean(StringSplit[2]), Convert.ToBoolean(StringSplit[3]));
                            }
                        }
                    }
                }
                catch (Exception e) { Logger.LogError(e); }
            }
        }

        private List<Player> _playerList;

        /// <summary>
        /// Gets or sets the players in this level.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
        public List<Player> Players {
            get {

                if (_playerList == null) {
                    _playerList = new List<Player>();
                    foreach (var p in Server.Players)
                        if (p.Level == this)
                            _playerList.Add(p);
                }
                return _playerList;
            }
            set {
                _playerList = value;
            }
        }

        public override string ToString() {
            return Name;
        }

        #region Events
        /// <summary>
        /// Gets called when a level gets loaded.
        /// </summary>
        public static LevelLoadEvent OnAllLevelsLoad = new LevelLoadEvent();
        /// <summary>
        /// Gets called when this level gets unloaded.
        /// </summary>
        public LevelLoadEvent OnLevelUnload = new LevelLoadEvent();
        /// <summary>
        /// Gets called when a level gets unloaded.
        /// </summary>
        public static LevelLoadEvent OnAllLevelsUnload = new LevelLoadEvent();
        #endregion

    }
}
