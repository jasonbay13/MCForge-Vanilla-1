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
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.World;
using MCForge.Utils;

namespace CommandDll
{
    public class CmdPlace : ICommand
    {
        public string Name { get { return "Place"; } }
        public CommandTypes Type { get { return CommandTypes.Building; } }
        public string Author { get { return "Gamemakergm"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 30; } }


        public void Use(Player p, string[] args)
        {
            byte b = Block.BlockList.UNKNOWN;
            ushort x, z, y;
            Vector3S pos = p.Pos;
            try
            {
                switch (args.Length)
                {
                    case 0:
                        x = (ushort)(pos.x / 32);
                        z = (ushort)(pos.z / 32);
                        y = (ushort)(pos.y / 32 - 2);
                        b = Block.BlockList.STONE;
                        break;
                    case 1:
                        x = (ushort)(pos.x / 32);
                        z = (ushort)(pos.z / 32);
                        y = (ushort)(pos.y / 32 - 1);
                        b = Block.NameToBlock(args[0]);
                        break;
                    case 3:
                        x = Convert.ToUInt16(args[0]);
                        z = Convert.ToUInt16(args[1]);
                        y = Convert.ToUInt16(args[2]);
                        break;
                    case 4:
                        b = Block.NameToBlock(args[0]);
                        x = Convert.ToUInt16(args[1]);
                        z = Convert.ToUInt16(args[2]);
                        y = Convert.ToUInt16(args[3]);
                        break;
                    default:
                        p.SendMessage("Invalid parameters.");
                        return;
                }
            }
            catch
            {
                p.SendMessage("Invalid parameters.");
                return;
            }
            //Need to wait for permissions for cannot place that block type.
            if (y >= p.Level.Size.y)
            {
                y = (ushort)(p.Level.Size.y - 1);
            }
            p.Level.BlockChange(x, z, y, b);
            p.SendMessage("An " + ((Block)b).Name + " block was placed at (" + x + ", " + z + ", " + y + ").");
        }
        public void Help(Player p)
        {
            p.SendMessage("/place [block] <x z y> - Places block at your feet or <x z y>");
            p.SendMessage("Shortcut: /pl");
        }

        public void Initialize()
        {
            string[] CommandStrings = new string[2] { "place", "pl" };
            Command.AddReference(this, CommandStrings);
        }
    }
}
