using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Bookcase : Block
    {
        public override string Name
        {
            get { return "bookcase"; }
        }
        public override byte VisibleBlock
        {
            get { return 47; }
        }
    }
}
