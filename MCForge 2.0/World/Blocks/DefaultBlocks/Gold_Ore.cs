using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Gold_Ore : Block
    {
        public override string Name
        {
            get { return "gold_ore"; }
        }
        public override byte VisableBlock
        {
            get { return 14; }
        }
    }
}
