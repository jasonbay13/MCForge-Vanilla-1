using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Red : Block
    {
        public override string Name
        {
            get { return "red"; }
        }
        public override byte VisibleBlock
        {
            get { return 21; }
        }
    }
}
