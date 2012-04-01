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
using System.Threading;
using MCForge;
using System.IO;
namespace CommandDll
{
	public class CmdMeasure : ICommand
	{
        public string Name { get { return "Measure"; } }
		public CommandTypes Type { get { return CommandTypes.information; } }
		public string Author { get { return "Gamemakergm"; } }
		public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
		public void Use(Player p, string[] args)
		{
            CatchPos cpos = new CatchPos();
            if (args.Length == 1)
            {
                cpos.ignore = Blocks.NameToByte(args[0]);
                if (cpos.ignore == (byte)(Blocks.Types.zero))
                {
                    p.SendMessage("Could not find block specified.");
                }
                p.SendMessage("Ignoring " + args[0]);
            }
            else
            cpos.ignore = (byte)(Blocks.Types.zero); //So it doesn't ignore air.
            p.SendMessage("Place two blocks to determine the edges.");
			p.CatchNextBlockchange(new Player.BlockChangeDelegate(CatchBlock), (object)cpos);
		}
		public void CatchBlock(Player p, ushort x, ushort z, ushort y, byte NewType, bool placed, object DataPass)
		{
            CatchPos cpos = (CatchPos)DataPass;
			cpos.FirstBlock = new Point3(x, z, y);
			p.CatchNextBlockchange(new Player.BlockChangeDelegate(CatchBlock2), (object)cpos);
		}
		public void CatchBlock2(Player p, ushort x, ushort z, ushort y, byte NewType, bool placed, object DataPass)
		{
            CatchPos cpos = (CatchPos)DataPass;
            Point3 FirstBlock = cpos.FirstBlock;
            ushort xx, zz, yy;
            int count = 0;
            for (xx = Math.Min((ushort)(FirstBlock.x), x); xx <= Math.Max((ushort)(FirstBlock.x), x); ++xx)
                for (zz = Math.Min((ushort)(FirstBlock.z), z); zz <= Math.Max((ushort)(FirstBlock.z), z); ++zz)
                    for (yy = Math.Min((ushort)(FirstBlock.y), y); yy <= Math.Max((ushort)(FirstBlock.y), y); ++yy)
                    {
                        if (p.level.GetBlock(xx, zz, yy) != cpos.ignore)
                        {
                            count++;
                        }
                    }
            p.SendMessage(count + " blocks are between (" + FirstBlock.x + ", " + FirstBlock.z + ", " + FirstBlock.y + ") and (" + x + ", " + z + ", " + y + ")");
        }
       
        public void Help(Player p)
		{
            p.SendMessage("/measure [ignore] - Measures all the blocks between two points.");
            p.SendMessage("[ignore] - Enter a block to ignore them");
		}

        public struct CatchPos
        {
            public Point3 FirstBlock;
            public byte ignore;
            public int count;
        }

		public void Initialize()
		{
			Command.AddReference(this, new string[2] { "measure", "ms" });
		}
	}
}
