using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Magenta : Block
    {
        public override string Name
        {
            get { return "magenta"; }
        }
        public override byte VisibleBlock
        {
            get { return 32; }
        }
    }
}
