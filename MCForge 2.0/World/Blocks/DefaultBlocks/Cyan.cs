using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Cyan : Block
    {
        public override string Name
        {
            get { return "cyan"; }
        }
        public override byte VisibleBlock
        {
            get { return 27; }
        }
    }
}
