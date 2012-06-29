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
using System.IO;
using System.IO.Compression;
using MCForge.Utils;

namespace MCForge.World.Loading_and_Saving {
    public class MCForgeOldMap : IMap {

        #region IMap Members

        public Level.SaveTypes Type {
            get { return Level.SaveTypes.MCForge; }
        }

        public Level Load(string levelName, string path) {
            Level finalLevel = new Level(new Vector3S(32, 32, 32));
            finalLevel.Name = levelName;
            using (FileStream fs = File.OpenRead(path)) {
                using (GZipStream gs = new GZipStream(fs, CompressionMode.Decompress)) {
                    byte[] ver = new byte[2];
                    gs.Read(ver, 0, ver.Length);
                    ushort version = BitConverter.ToUInt16(ver, 0);

                    //if (version != 1874) //Is a old MCForge level!
                    //    throw new Exception(path + " is not a valid MCForge Level");
                    // Older levels WILL STILL WORK WITH THIS so you don't need this check. .dat files won't load though.

                    ushort[] vars = new ushort[6];
                    byte[] rot = new byte[2];
                    byte[] header = new byte[16]; 
                    
                    gs.Read(header, 0, header.Length);

                    vars[0] = BitConverter.ToUInt16(header, 0); //X
                    vars[1] = BitConverter.ToUInt16(header, 2); //Z
                    vars[2] = BitConverter.ToUInt16(header, 4); //Y
                    vars[3] = BitConverter.ToUInt16(header, 6); //SpawnX
                    vars[4] = BitConverter.ToUInt16(header, 8); //SpawnZ
                    vars[5] = BitConverter.ToUInt16(header, 10); //SpawnY

                    rot[0] = header[12]; //SpawnHeading
                    rot[1] = header[13]; //SpawnYaw

                    finalLevel.Size = new Vector3S((short)vars[0], (short)vars[1], (short)vars[2]);
                    finalLevel.SpawnPos = new Vector3S((short)vars[3], (short)vars[4], (short)vars[5]);
                    finalLevel.SpawnRot = new byte[2] { rot[0], rot[1] };
                    finalLevel.TotalBlocks = finalLevel.Size.x * finalLevel.Size.z * finalLevel.Size.y;

                    byte[] blocks = new byte[finalLevel.Size.x * finalLevel.Size.z * finalLevel.Size.y];
                    gs.Read(blocks, 0, blocks.Length);
                    finalLevel.Data = new byte[finalLevel.TotalBlocks];
                    for (int x = 0; x < finalLevel.Size.x; x++)
                        for (int y = 0; y < finalLevel.Size.y; y++)
                            for (int z = 0; z < finalLevel.Size.z; z++)
                                finalLevel.SetBlock(x, z, y, (byte)OldMCForgeToNewMCForge.Convert(blocks[finalLevel.PosToInt((ushort)x, (ushort)z, (ushort)y)]));  //Converts all custom blocks to normal blocks.

                }
            }
            finalLevel.HandleMetaData();
            Logger.Log("[Level] " + levelName + " was loaded");
            return finalLevel;
        }


        #endregion
    }
}
