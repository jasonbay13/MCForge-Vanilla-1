using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Stair : Block
    {
        public override string Name
        {
            get { return "stair"; }
        }
        public override byte VisibleBlock
        {
            get { return 44; }
        }
    }
}
