using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Utils;

namespace MCForge.World.Blocks {
    public struct BlockChange {

        public Vector3S Position;
        public byte BlockFrom, BlockTo;
        public bool Deleted;
        public DateTime Time;

        public BlockChange(Vector3S pos, byte from, byte to, bool deleted) {
            Position = pos;
            BlockFrom = from;
            BlockTo = to;
            Deleted = deleted;
            Time = DateTime.Now;
        }
    }
}
