using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Black : Block
    {
        public override string Name
        {
            get { return "black"; }
        }
        public override byte VisableBlock
        {
            get { return 34; }
        }
    }
}
