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
