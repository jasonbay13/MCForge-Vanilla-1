using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Physics
{
    public class LavaExample : PhysicsBlock
    {
        int x;
        int y;
        int z;
        public override int X
        {
            get { return x; }
        }
        public override int Y
        {
            get { return y; }
        }
        public override int Z
        {
            get { return z; }
        }
        public override void Tick(Level l)
        {
            if (l.GetBlock(x, y - 1, z) == (byte)Blocks.Types.air)
            {
                l.BlockChange((ushort)x, (ushort)(y - 1), (ushort)z, (byte)Blocks.Types.active_lava);
                y -= 1;
            }
            else
                Remove(); //Removes it from the cache
        }
    }
}
