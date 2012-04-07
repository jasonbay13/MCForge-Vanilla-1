using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Indigo : Block
    {
        public override string Name
        {
            get { return "indigo"; }
        }
        public override byte VisableBlock
        {
            get { return 30; }
        }
    }
}
