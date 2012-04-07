using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Physics
{
    public class Active_Lava : PhysicsBlock
    {
        public override string Name
        {
            get { return "active_lava"; }
        }
        public override byte VisableBlock
        {
            get { return 10; }
        }
        public Active_Lava(int x, int y, int z, Level l)
            : base(x, y, z, l)
        {

        }
        public override void Tick()
        {
            throw new NotImplementedException();
        }
    }
}
