using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Yellow : Block
    {
        public override string Name
        {
            get { return "yellow"; }
        }
        public override byte VisibleBlock
        {
            get { return 23; }
        }
    }
}
