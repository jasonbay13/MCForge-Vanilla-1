using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Utils;

namespace MCForge.World.Drawing {
    public interface IBrush {
        IEnumerable<Vector3S> Draw(Vector3S pos, byte block, int size);
    }
}
