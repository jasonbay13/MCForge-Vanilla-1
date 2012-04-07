using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Water : Block
    {
        public override string Name
        {
            get { return "water"; }
        }
        public override byte VisableBlock
        {
            get { return 9; }
        }
    }
}
