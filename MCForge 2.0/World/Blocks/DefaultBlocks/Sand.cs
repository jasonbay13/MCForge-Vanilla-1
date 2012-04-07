using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Sand : Block
    {
        public override string Name
        {
            get { return "sand"; }
        }
        public override byte VisableBlock
        {
            get { return 12; }
        }
    }
}
