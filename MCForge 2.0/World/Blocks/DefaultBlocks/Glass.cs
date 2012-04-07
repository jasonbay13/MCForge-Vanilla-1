using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Glass : Block
    {
        public override string Name
        {
            get { return "glass"; }
        }
        public override byte VisibleBlock
        {
            get { return 20; }
        }
    }
}
