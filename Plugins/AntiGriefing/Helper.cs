/*
Copyright 2012 MCForge
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
using MCForge.Utils;
using MCForge.SQL;

namespace Plugins.AntiGriefing {
    public class Helper {

        public static string GetOwner(Vector3S e, string level) {
            using (var data = Database.fillData("SELECT * FROM Blocks WHERE X = '" + e.x + "' AND Y = '" + e.y + "' AND Z = '" + e.z + "' AND Level = '" + level.MySqlEscape() + "';")) {

                if (data.Rows.Count == 0) {
                    return null;
                }

                using (var playerData = Database.fillData("SELECT * FROM _players WHERE UID = " + data.Rows[0]["UID"].ToString()))
                    return playerData.Rows[0]["Name"].ToString();
            }
        }

    }
}
