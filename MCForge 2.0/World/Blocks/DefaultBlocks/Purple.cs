using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Purple : Block
    {
        public override string Name
        {
            get { return "purple"; }
        }
        public override byte VisibleBlock
        {
            get { return 31; }
        }
    }
}
