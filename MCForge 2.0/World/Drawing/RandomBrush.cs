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
