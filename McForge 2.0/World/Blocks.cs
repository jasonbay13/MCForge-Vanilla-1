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
using MCForge.Core;

namespace MCForge.World
{
    /// <summary>
    /// A class that contains blocks / custom blocks and methods to work with them.
    /// </summary>
    public class Blocks
    {
        /// <summary>
        /// An enumeration of all the default Minecraft blocks
        /// </summary>
        public enum Types : byte
        {
            air = 0,
            stone = 1,
            grass = 2,
            dirt = 3,
            cobblestone = 4,
            wood = 5,
            shrub = 6,
            adminium = 7,
            active_water = 8,
            water = 9,
            active_lava = 10,
            lava = 11,
            sand = 12,
            gravel = 13,
            gold_ore = 14,
            iron_ore = 15,
            coal = 16,
            tree = 17,
            leaves = 18,
            sponge = 19,
            glass = 20,
            red = 21,
            orange = 22,
            yellow = 23,
            greenyellow = 24,
            green = 25,
            springgreen = 26,
            cyan = 27,
            blue = 28,
            blueviolet = 29,
            indigo = 30,
            purple = 31,
            magenta = 32,
            pink = 33,
            black = 34,
            gray = 35,
            white = 36,
            yellow_flower = 37,
            red_flower = 38,
            brown_shroom = 39,
            red_shroom = 40,
            gold = 41,
            iron = 42,
            double_stair = 43,
            stair = 44,
            brick = 45,
            tnt = 46,
            bookcase = 47,
            mossy_cobblestone = 48,
            obsidian = 49,

            custom = 254,
            unknown = 255,
            zero = 0xff,
        }

        public static string GetName(Point3 pos) { return GetName(pos.x, pos.z, pos.y); }
        public static string GetName(short x, short z, short y)
        {
            return "";
        }
        /// <summary>
        /// This function takes in a block name and gives out its byte.
        /// </summary>
        /// <param name="name"></param> Name of the block.
        /// <returns></returns>
        public static byte NameToByte(string name)
        {
            return (byte)((Blocks.Types)Enum.Parse(typeof(Blocks.Types), name));
        }
        /// <summary>
        /// This functions takes in a block byte and returns its name as a string.
        /// </summary>
        /// <param name="type"></param>Byte of the block
        /// <returns></returns>
        public static string ByteToName(byte type)
        {
            return Enum.Parse(typeof(Blocks.Types), type.ToString()).ToString();
        }
        /// <summary>
        /// A DYNAMIC dictionary of all the CustomBlocks in the server.
        /// </summary>
        public static Dictionary<byte, CustomBlock> CustomBlocks = new Dictionary<byte, CustomBlock>();

        public struct CustomBlock
        {
            public byte VisibleType;
            public string Name;

            public CustomBlock(byte ThisBlocksVisibleType, string ThisBlocksName)
            {
                VisibleType = ThisBlocksVisibleType;
                Name = ThisBlocksName;
            }

        }
    }
}
