using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks
{
    public class Obsidian : Block
    {
        public override string Name
        {
            get { return "obsidian"; }
        }
        public override byte VisableBlock
        {
            get { return 49; }
        }
    }
}
