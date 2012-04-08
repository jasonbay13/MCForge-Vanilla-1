using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Blue : Block
    {
        public override string Name
        {
            get { return "blue"; }
        }
        public override byte VisibleBlock
        {
            get { return 28; }
        }
    }
}
