using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Blocks {
    class ActiveLava : Block {
        public override byte VisibleBlock {
            get { return 8; }
        }

        public override string Name {
            get { return "Active_Water"; }
        }
    }
}
