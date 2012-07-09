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
using MCForge.Utils;
using MCForge.World;
using MCForge.Utils.Settings;
namespace MCForge.Commands {
    public class CmdFly : ICommand {
        public string Name { get { return "Fly"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Gamemakergm"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Use(Player p, string[] args) {
            p.ExtraData.CreateIfNotExist("IsFlying", false);
            p.ExtraData["IsFlying"] = !(bool)p.ExtraData["IsFlying"];
            if (!(bool)p.ExtraData["IsFlying"]) {
                p.OnPlayerBigMove.Normal -= OnPlayerBigMove_Normal;
                p.ResendBlockChange(glasses, (Vector3S)p.ExtraData["FlyLastPos"]);
                return;
            }
            p.SendBlockChangeWhereAir(glasses, p.belowBlock, 20);
            p.ExtraData["FlyLastPos"] = p.belowBlock;
            p.OnPlayerBigMove.Normal += OnPlayerBigMove_Normal;
            p.SendMessage("You are now flying. &cJump!");
        }

        void OnPlayerBigMove_Normal(Player sender, API.Events.MoveEventArgs args) {
            sender.ResendBlockChange(glasses, (Vector3S)sender.ExtraData["FlyLastPos"]);
            sender.ExtraData["FlyLastPos"] = sender.belowBlock;
            sender.SendBlockChangeWhereAir(glasses, sender.belowBlock, 20);
        }
        public void Help(Player p) {
            p.SendMessage("/fly - Allows you to fly");
        }
        Vector3S[] glasses;
        public void Initialize() {
            string setting = ServerSettings.GetSetting("FlySize");
            string[] split = setting.Split(' ', ',', ';', ':');
            int xz = 5;
            int y = 2;
            bool midblock = ServerSettings.GetSettingBoolean("FlyMidBlock");
            if (split.Length == 1) {
                try { y = int.Parse(split[0]); }
                catch { }
            }
            else if (split.Length > 2) {
                try { xz = int.Parse(split[0]); }
                catch { }
                try { y = int.Parse(split[1]); }
                catch { }
            }
            List<Vector3S> blocks = new List<Vector3S>();
            for (int a = -xz / 2; a < xz / 2 + ((xz % 2 != 0) ? 1 : 0); a++) {
                for (int b = -xz / 2; b < xz / 2 + ((xz % 2 != 0) ? 1 : 0); b++) {
                    for (int c = 0; c > -y; c--) {
                        blocks.Add(new Vector3S((short)a, (short)b, (short)c));
                    }
                }
            }
            if (midblock) blocks.Add(new Vector3S(0, 0, 1));
            glasses = blocks.ToArray();
            Command.AddReference(this, "fly");
        }
    }
}
