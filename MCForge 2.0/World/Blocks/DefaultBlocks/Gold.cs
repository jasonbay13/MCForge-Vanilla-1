using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Gold : Block
    {
        public override string Name
        {
            get { return "gold"; }
        }
        public override byte VisibleBlock
        {
            get { return 41; }
        }
    }
}
