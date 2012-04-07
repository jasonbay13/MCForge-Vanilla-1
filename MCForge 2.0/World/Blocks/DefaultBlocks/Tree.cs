using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Tree : Block
    {
        public override string Name
        {
            get { return "tree"; }
        }
        public override byte VisableBlock
        {
            get { return 17; }
        }
    }
}
