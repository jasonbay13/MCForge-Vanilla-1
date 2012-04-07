using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class RedFlower : Block
    {
        public override string Name
        {
            get { return "red_flower"; }
        }
        public override byte VisableBlock
        {
            get { return 38; }
        }
    }
}
