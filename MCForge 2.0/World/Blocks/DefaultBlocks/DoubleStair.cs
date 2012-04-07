using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class DoubleStair : Block
    {
        public override string Name
        {
            get { return "double_stair"; }
        }
        public override byte VisableBlock
        {
            get { return 43; }
        }
    }
}
