/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
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
using System.IO;
using System.Collections.Generic;


namespace MCForge.Commands
{
    public class CmdLevels : Command
    {
        public override string name { get { return "levels"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdLevels() { }

        public override void Use(Player p, string message)
        { // TODO
            try
            {
                if (message != "") { Help(p); return; }
                foreach(Level l in Server.levels)
                {
                    Player.SendMessage(p, l.name + " - " + l.physics);
                }
                Player.SendMessage(p, "Use &4/unloaded for unloaded levels.");
            }
            catch (Exception e)
            {
                Server.ErrorLog(e);
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/levels - Lists all loaded levels and their physics levels.");
        }
    }
}