using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Drawing {
    public class SphereBrush : IBrush {
        #region IBrush Members

        public IEnumerable<Utils.Vector3S> Draw(Utils.Vector3S pos, byte block, int size) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
