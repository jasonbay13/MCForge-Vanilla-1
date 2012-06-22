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

namespace MCForge.World.Drawing {
    public class RandomBrush : IBrush {
        #region IBrush Members
        private static Random rnd;
        static RandomBrush() {
            rnd = new Random();
        }

        public IEnumerable<Utils.Vector3S> Draw(Utils.Vector3S pos, byte block, int size) {
            Vector3S mVec = pos - (size / 2);
            for (ushort x = 0; x < size; x++)
                for (ushort y = 0; y < size; y++)
                    for (short z = 0; z < size; z++)
                        if (rnd.Next() % 2 == 0)
                            yield return new Vector3S((ushort)(x + mVec.x), (ushort)(z + mVec.z), (ushort)(y + mVec.y));
        }

        #endregion
    }
}
