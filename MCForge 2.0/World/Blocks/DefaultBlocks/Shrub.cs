using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Shrub : Block
    {
        public override string Name
        {
            get { return "shrub"; }
        }
        public override byte VisibleBlock
        {
            get { return 6; }
        }
    }
}
