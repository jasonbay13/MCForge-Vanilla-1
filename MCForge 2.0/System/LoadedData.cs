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

namespace MCForge.Core
{
	public class PDB : IComparable<PDB>
	{
		public static List<PDB> DB = new List<PDB>();

		string username = null;

		/// <summary>
		/// This allows a PDB to be compared to another PDB by looking at the username, and subsequently, it can then be sorted.
		/// </summary>
		/// <param name="pdb">The comparing PDB</param>
		/// <returns>returns a comparison of the usernames</returns>
		public int CompareTo(PDB pdb)
		{
			return username.CompareTo(pdb.username);
		}
	}

	public class GDB : IComparable<GDB>
	{
		public static List<GDB> DB = new List<GDB>();

		byte id = 0;

		public int CompareTo(GDB gdb)
		{
			return id.CompareTo(gdb.id);
		}
	}
	
}
