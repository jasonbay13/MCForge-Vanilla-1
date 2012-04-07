using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Gray : Block
    {
        public override string Name
        {
            get { return "gray"; }
        }
        public override byte VisableBlock
        {
            get { return 35; }
        }
    }
}
