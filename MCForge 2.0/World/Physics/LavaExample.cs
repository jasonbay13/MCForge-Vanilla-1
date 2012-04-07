using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Physics
{
    public class LavaExample : PhysicsBlock
    {
        public LavaExample(int x, int y, int z, Level l) : base(x, y, z, l) 
        { 
            l.BlockChange((ushort)X, (ushort)z, (ushort)y, 10); 
        }
        public override void Tick()
        {
            /*if (l.GetBlock(X, Z, Y - 1) == 0) You can also do this, this will add a physics block below it and that physics block will add one below it and so on..
                AddBlock(new LavaExample(X, Y - 1, Z, l));*/ 
            if (l.GetBlock(X, Y - 1, Z) == (byte)Blocks.Types.air)
            {
                l.BlockChange((ushort)X, (ushort)(Y - 1), (ushort)Z, (byte)Blocks.Types.active_lava);
                Y -= 1;
            }
            else
                Remove(); //Removes it from the cache
        }
    }
}
