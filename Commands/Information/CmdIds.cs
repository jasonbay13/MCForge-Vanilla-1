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
using MCForge;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;

namespace CommandDll
{
	public class CmdIds : ICommand
	{
        public string Name { get { return "PlayerIds"; } }
		public CommandTypes Type { get { return CommandTypes.Information; } }
        public string Author { get { return "Merlin33069"; } }
		public Version Version { get { return new Version(1,0); } }
		public string CUD { get { return ""; } }
        public byte Permission { get { return 120; } }
		public void Use(Player p, string[] args)
		{
			Server.ForeachPlayer(delegate(Player pl)
			{
				p.SendMessage(pl.Username + " " + pl.id);
			});
		}
		public void Help(Player p)
		{

		}
		public void Initialize()
		{
            Command.AddReference(this, new string[1] { "ids" });
		}
	}
}
