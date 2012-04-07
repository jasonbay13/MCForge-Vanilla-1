using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class YellowFlower : Block
    {
        public override string Name
        {
            get { return "yellow_flower"; }
        }
        public override byte VisibleBlock
        {
            get { return 37; }
        }
    }
}
