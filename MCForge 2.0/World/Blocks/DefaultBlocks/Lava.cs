using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Lava : Block
    {
        public override string Name
        {
            get { return "lava"; }
        }
        public override byte VisibleBlock
        {
            get { return 11; }
        }
    }
}
