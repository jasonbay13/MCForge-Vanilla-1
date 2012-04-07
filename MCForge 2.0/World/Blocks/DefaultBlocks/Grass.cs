using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Grass : Block
    {
        public override string Name
        {
            get { return "grass"; }
        }
        public override byte VisableBlock
        {
            get { return 2; }
        }
    }
}
