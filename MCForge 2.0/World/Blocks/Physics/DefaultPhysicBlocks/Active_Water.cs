using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Physics
{
    public class Active_Water : PhysicsBlock
    {
        public override string Name
        {
            get { return "active_water"; }
        }
        public override byte VisibleBlock
        {
            get { return 8; }
        }
        public Active_Water(int x, int y, int z, Level l)
            : base(x, y, z, l)
        {

        }
        public override void Tick()
        {
            throw new NotImplementedException();
        }
    }
}
