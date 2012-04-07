using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class TNT : Block
    {
        public override string Name
        {
            get { return "tnt"; }
        }
        public override byte VisableBlock
        {
            get { return 46; }
        }
    }
}
