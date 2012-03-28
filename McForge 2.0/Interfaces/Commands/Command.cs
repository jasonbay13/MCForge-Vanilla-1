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
	public class Command
	{
		internal static Dictionary<string, ICommand> Commands = new Dictionary<string, ICommand>();

		public static void AddReference(ICommand command, string[] reference)
		{
			foreach (string s in reference)
			{
				AddReference(command, s.ToLower());
			}
		}
		public static void AddReference(ICommand command, string reference)
		{
			if (Commands.ContainsKey(reference))
			{
				Server.Log("[ERROR]: Command " + command.Name + " tried to add a referance that already existed! (" + reference + ")", ConsoleColor.White, ConsoleColor.Red);
				return;
			}
			Commands.Add(reference.ToLower(), command);
		}
	}
}
