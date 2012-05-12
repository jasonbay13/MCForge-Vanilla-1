using MCForge.Core;
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
using MCForge.Interface.Command;
using MCForge.World;

namespace CommandDll
{
    public class CmdGoto : ICommand
    {
        public string Name { get { return "Goto"; } }
        public CommandTypes Type { get { return CommandTypes.mod; } }
        public string Author { get { return "Snowl"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            Level tempLevel = Level.FindLevel(args[0]);
            if (tempLevel != null)
            {
                #region Send and Spawn
                p.GlobalDie();
                p.IsLoading = true;
                p.Level = tempLevel;
                short x = (short)((0.5 + tempLevel.SpawnPos.x) * 32);
                short y = (short)((1 + tempLevel.SpawnPos.y) * 32);
                short z = (short)((0.5 + tempLevel.SpawnPos.z) * 32);
                p.Pos = new Vector3(x, z, y);
                p.Rot = tempLevel.SpawnRot;
                p.oldPos = p.Pos;
                p.oldRot = p.Rot;
                p.SendSpawn(p);
                p.IsLoading = false;
                p.SpawnOtherPlayersForThisPlayer();
                p.SpawnThisPlayerToOtherPlayers();
                p.SpawnBotsForThisPlayer();
                #endregion
                Player.UniversalChat(p.Username + " went to " + args[0] + "!");
            }
            else
            {
                p.SendMessage("This level does not exist!");
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/goto <level> - Goes to <level>.");
            p.SendMessage("Shortcut: /g");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "goto", "g" });
        }
    }
}

