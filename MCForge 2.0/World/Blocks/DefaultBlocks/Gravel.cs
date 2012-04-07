using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Gravel : Block
    {
        public override string Name
        {
            get { return "gravel"; }
        }
        public override byte VisableBlock
        {
            get { return 13; }
        }
    }
}
