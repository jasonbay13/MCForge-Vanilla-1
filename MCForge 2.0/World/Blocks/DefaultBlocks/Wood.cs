using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Wood : Block
    {
        public override string Name
        {
            get { return "wood"; }
        }
        public override byte VisableBlock
        {
            get { return 5; }
        }
    }
}
