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
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;

namespace CommandDll
{
    public class CmdSpawn : ICommand
    {
        public string Name { get { return "Spawn"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
        public string Author { get { return "Gamemakergm"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }

        public void Use(Player p, string[] args)
        {
            if (args.Count() != 0)
            {
                Help(p);
                return;
            }
            Point3 meep = new Point3((short)(p.level.SpawnPos.x * 32), (short)(p.level.SpawnPos.z * 32), (short)(p.level.SpawnPos.y * 32));
            p.SendToPos(meep, p.level.SpawnRot);
        }

        public void Help(Player p)
        {
            p.SendMessage("/spawn - Teleports yourself to the spawn location.");
        }

        public void Initialize()
        {
            Command.AddReference(this, "spawn");
        }
    }
}
