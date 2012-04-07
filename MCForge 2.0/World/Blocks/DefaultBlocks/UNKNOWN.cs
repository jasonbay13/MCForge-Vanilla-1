using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class UNKNOWN : Block
    {
        public override string Name
        {
            get { return "unknown"; }
        }
        public override byte VisibleBlock
        {
            get { return 255; }
        }
    }
}
