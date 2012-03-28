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

namespace McForge
{
	public class Level
	{
		//As a note, the coordinates are right, it is xzy, its based on the users view, not the map itself.
		//WIDTH = X, LENGTH = Z, DEPTH = Y
		//NEST ORDER IS XZY

		public delegate void ForEachBlockDelegateXYZ(int x, int z, int y);
		public delegate void ForEachBlockDelegate(int pos);

		int _TotalBlocks;
		public int TotalBlocks
		{
			get
			{
				if (_TotalBlocks == 0) _TotalBlocks = Size.x * Size.z * Size.y;
				return _TotalBlocks;
			}
		}
		public Point3 Size;
		public Point3 SpawnPos;
		public byte[] SpawnRot;

		//public byte[,,] data; //Three dimensional array :D
		public byte[] data;

		private Level(Point3 size)
		{
			Size = size;
			//data = new byte[Size.x, Size.z, Size.y];
			data = new byte[TotalBlocks];
		}

		public static Level CreateLevel(Point3 size, LevelTypes type)
		{
			Level newlevel = new Level(size);

			switch(type)
			{
				case LevelTypes.Flat:
					newlevel.CreateFlatLevel();
					break;
			}

			return newlevel;
		}

		public void CreateFlatLevel()
		{
			int middle = Size.y / 2;
			ForEachBlockXYZ(delegate(int x, int z, int y)
			{
				if (y < middle)
				{
					SetBlock((ushort)x, (ushort)z, (ushort)y, Blocks.Types.dirt);
					return;
				}
				if(y==middle)
				{
					SetBlock((ushort)x, (ushort)z, (ushort)y, Blocks.Types.grass);
					return;
				}

			});

			SpawnPos = new Point3((short)(Size.x / 2), (short)(Size.z / 2), (short)(Size.y));
			SpawnRot = new byte[2]{0, 0};
		}

		public Level LoadLevel()
		{
			//TODO
			return null;
		}

		public void ForEachBlockXYZ(ForEachBlockDelegateXYZ FEBD)
		{
			for (int x = 0; x < Size.x; x++)
			{
				for (int z = 0; z < Size.z; z++)
				{
					for (int y = 0; y < Size.y; y++)
					{
						FEBD(x, z, y);
					}
				}
			}
		}
		public void ForEachBlock(ForEachBlockDelegate FEBD)
		{
			for (int i = 0; i < data.Length; ++i)
			{
				FEBD(i);
			}
		}

		public void BlockChange(ushort x, ushort z, ushort y, byte block)
		{
			if (y == Size.y) return;
			byte currentType = GetBlock(x, z, y);

			if (block == currentType) return;

			SetBlock(x, z, y, block);

			if (currentType >= 50)
			{
				if (Blocks.CustomBlocks[currentType].VisibleType != block)
					Player.GlobalBlockchange(this, x, z, y, block);
			}
			else
			{
				Player.GlobalBlockchange(this, x, z, y, block);
			}

			//TODO Special stuff for block changing
		}

		#region SetBlock And Overloads
		void SetBlock(Point3 pos, Blocks.Types block)
		{
			SetBlock(pos.x, pos.z, pos.y, (byte)block);
		}
		void SetBlock(int x, int z, int y, Blocks.Types block)
		{
			SetBlock((ushort)x, (ushort)z, (ushort)y, (byte)block);
		}
		void SetBlock(ushort x, ushort z, ushort y, Blocks.Types block)
		{
			SetBlock(x, z, y, (byte)block);
		}
		void SetBlock(int pos, Blocks.Types block)
		{
			SetBlock(pos, (byte)block);
		}
		void SetBlock(Point3 pos, byte block)
		{
			SetBlock(pos.x, pos.z, pos.y, block);
		}
		void SetBlock(int x, int z, int y, byte block)
		{
			SetBlock((ushort)x, (ushort)z, (ushort)y, block);
		}
		void SetBlock(ushort x, ushort z, ushort y, byte block)
		{
			SetBlock(PosToInt(x, z, y), block);
			
		}
		void SetBlock(int pos, byte block)
		{
			data[pos] = block;
		}
		#endregion
		#region GetBlock and Overloads
		public byte GetBlock(Point3 pos)
		{
			return GetBlock(pos.x, pos.z, pos.y);
		}
		public byte GetBlock(int x, int z, int y)
		{
			return GetBlock(PosToInt((ushort)x, (ushort)z, (ushort)y));
		}
		public byte GetBlock(ushort x, ushort z, ushort y)
		{
			return GetBlock(PosToInt(x, z, y));
		}
		public byte GetBlock(int pos)
		{
			return data[pos];
		}
		#endregion

		public int PosToInt(ushort x, ushort z, ushort y)
		{
			if (x < 0) { return -1; }
			if (x >= Size.x) { return -1; }
			if (y < 0) { return -1; }
			if (y >= Size.y) { return -1; }
			if (z < 0) { return -1; }
			if (z >= Size.z) { return -1; }
			return x + z * Size.x + y * Size.x * Size.z;
		}
		public Point3 IntToPos(int pos)
		{
			short y = (short)(pos / Size.x / Size.z); pos -= y * Size.x * Size.z;
			short z = (short)(pos / Size.x); pos -= z * Size.x;
			short x = (short)pos;

			return new Point3(x, z, y);
		}
		public int IntOffset(int pos, int x, int z, int y)
		{
			return pos + x + z * Size.x + y * Size.x * Size.z;
		}

		public enum LevelTypes
		{
			Flat,
		}
	}
}
