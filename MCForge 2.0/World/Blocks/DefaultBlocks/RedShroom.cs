using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class RedShroom : Block
    {
        public override string Name
        {
            get { return "red_shroom"; }
        }
        public override byte VisibleBlock
        {
            get { return 40; }
        }
    }
}
