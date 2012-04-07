using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Coal : Block
    {
        public override string Name
        {
            get { return "coal"; }
        }
        public override byte VisableBlock
        {
            get { return 16; }
        }
    }
}
