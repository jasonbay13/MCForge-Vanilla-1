using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Cobblestone : Block
    {
        public override string Name
        {
            get { return "cobblestone"; }
        }
        public override byte VisibleBlock
        {
            get { return 4; }
        }
    }
}
