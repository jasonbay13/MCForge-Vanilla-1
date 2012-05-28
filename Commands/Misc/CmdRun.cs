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
using System.Collections.Generic;
using System.Threading;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;
using MCForge.World.Blocks;
using MCForge.Utils;
using MCForge.World;
namespace CommandDll {
    public class CmdRun : ICommand {
        public string Name { get { return "Run"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "ninedrafted"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission {
            get {
#if DEBUG
                return 0;
#else 
            return 30;
#endif
            }
        }

        public void Use(Player p, string[] args) {
            p.ExtraData.CreateIfNotExist("IsRunning", false);
            p.ExtraData.ChangeOrCreate("RunCounter", 0);
            p.ExtraData["IsRunning"] = !(bool)p.ExtraData["IsRunning"];
            if (!(bool)p.ExtraData["IsRunning"]) {
                p.OnPlayerMove.Normal -= OnPlayerMove_Normal;
                return;

            }
            p.OnPlayerMove.Normal += new MCForge.API.Events.Event<Player, MCForge.API.Events.MoveEventArgs>.EventHandler(OnPlayerMove_Normal);
            p.SendMessage("You are now running. &cMove!");

        }
        static int count = 0;
        void OnPlayerMove_Normal(Player sender, MCForge.API.Events.MoveEventArgs args) {
            int count = (int)sender.ExtraData["RunCounter"];
            count++;
            sender.ExtraData["RunCounter"] = count;
            if (count % 15 != 0) return;
            sender.ExtraData["RunCounter"] = 0;
            Vector3S tmpPos = new Vector3S(args.FromPosition);
            tmpPos.Horizontal = tmpPos.Horizontal.GetMove(320, args.ToPosition.Horizontal);
            packet pa = new packet();
            pa.Add(packet.types.SendTeleport);
            pa.Add((sbyte)-1);
            pa.Add(tmpPos.x);
            pa.Add((short)(tmpPos.y));
            pa.Add(tmpPos.z);
            pa.Add(sender.Rot);
            sender.oldPos = tmpPos;
            sender.Pos = tmpPos;
            sender.oldRot = sender.Rot;
            sender.SendPacket(pa);
            args.Cancel();
            count++;
        }
        public void Help(Player p) {
            p.SendMessage("/run - Allows you to run");
        }

        public void Initialize() {
            Command.AddReference(this, "run");
        }
    }
}
