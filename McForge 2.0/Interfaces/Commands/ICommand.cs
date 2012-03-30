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
	public interface ICommand
	{
		/// <summary>
		/// The name of the command
		/// </summary>
		string Name { get; }
		/// <summary>
		/// The type of command this is
		/// </summary>
		CommandTypes Type { get; }
		/// <summary>
		/// The author of the command (to add multiple authors just make the string like "Merlin33069, someone else"
		/// </summary>
		string Author { get; }
		/// <summary>
		/// The command version
		/// </summary>
		int Version { get; }
		/// <summary>
		/// Unique identifier for this plugin, will be used later to link to McForge databases
		/// </summary>
		string CUD { get; }

		/// <summary>
		/// The method that will be called when a player uses this command
		/// </summary>
		/// <param name="p">a Player class</param>
		/// <param name="args">the args of the command the player sent</param>
		void Use(Player p, string[] args);
		/// <summary>
		/// The method to run when a player uses the /help command
		/// </summary>
		/// <param name="p">a Player instance</param>
		void Help(Player p);

		/// <summary>
		/// The initialization of the command, you need to add command referances here.
		/// </summary>
		void Initialize();
	}
}
