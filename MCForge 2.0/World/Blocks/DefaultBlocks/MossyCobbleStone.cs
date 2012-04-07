using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class MossyCobbleStone : Block
    {
        public override string Name
        {
            get { return "mossy_cobblestone"; }
        }
        public override byte VisableBlock
        {
            get { return 48; }
        }
    }
}
