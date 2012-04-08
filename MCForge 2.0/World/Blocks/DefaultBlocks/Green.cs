using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Green : Block
    {
        public override string Name
        {
            get { return "green"; }
        }
        public override byte VisibleBlock
        {
            get { return 25; }
        }
    }
}
