using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Iron_Ore : Block
    {
        public override string Name
        {
            get { return "iron_ore"; }
        }
        public override byte VisibleBlock
        {
            get { return 15; }
        }
    }
}
