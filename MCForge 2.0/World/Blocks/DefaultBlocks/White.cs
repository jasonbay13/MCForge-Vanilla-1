using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class White : Block
    {
        public override string Name
        {
            get { return "white"; }
        }
        public override byte VisibleBlock
        {
            get { return 36; }
        }
    }
}
