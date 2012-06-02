using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Utils;

namespace MCForge.World.Drawing {
    public class CubeBrush : IBrush {


        #region IBrush Members

        IEnumerable<Utils.Vector3S> IBrush.Draw(Vector3S pos, byte block, int size) {
            Vector3S mVec = pos - (size  / 2);
            for (short x= mVec.x; x < size; x++)
                for (short y = mVec.y; y < size; y++)
                    for (short z = mVec.z; z < size; z++)
                        yield return new Vector3S(x, z, y);
        }

        #endregion
    }
}
