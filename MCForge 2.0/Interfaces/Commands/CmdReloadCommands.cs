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

using MCForge.Entity;

namespace MCForge.Interface.Command
{
	public class CmdReloadCmds : ICommand
	{
		public string Name { get { return "Reload Commands"; } }
		public CommandTypes Type { get { return CommandTypes.Misc; } }
		public string Author { get { return "Merlin33069"; } }
		public int Version { get { return 1; } }
		public string CUD { get { return ""; } }
        public byte Permission { get { return 120; } }

		public void Use(Player p, string[] args)
		{
			Player.UniversalChat("Reloading the Command system, please wait.");
			Command.Commands.Clear();
			LoadAllDlls.InitCommandsAndPlugins();
            Initialize();
		}

		public void Help(Player p)
        {
            p.SendMessage("/reloadcommands - Reloads the command system");
            p.SendMessage("Shortcuts: /reloadcmds, /rc");
		}

		public void Initialize()
		{
            Command.AddReference(this, new string[3] { "reloadcmds", "reloadcommands", "rc" });
		}
	}
}
