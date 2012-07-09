using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Physics {
    class Door : PhysicsBlock {
        public Door(byte open, byte closed) {
            this.open = open;
            this.closed = closed;
        }
        public override void Tick(Level l) {
            if (level == null) level = l;
        }
        private byte open = 25;
        private byte closed = 21;
        bool state = true;
        Level level;
        public override byte VisibleBlock {
            get {

                if (state) {
                    if (level.ExtraData["DoorOpen" + X + "," + Z + "," + Y] != null) {
                        if (level.ExtraData["DoorOpen" + X + "," + Z + "," + Y].GetType() == typeof(string)) {
                            try {
                                level.ExtraData["DoorOpen" + X + "," + Z + "," + Y] = byte.Parse((string)level.ExtraData["DoorOpen" + X + "," + Z + "," + Y]);
                            }
                            catch { }
                        }
                        try { return (byte)level.ExtraData["DoorOpen" + X + "," + Z + "," + Y]; }
                        catch { }
                    }
                    return open;
                }
                else {
                    if (level.ExtraData["DoorClose" + X + "," + Z + "," + Y] != null) {
                        if (level.ExtraData["DoorClose" + X + "," + Z + "," + Y].GetType() == typeof(string)) {
                            try {
                                level.ExtraData["DoorClose" + X + "," + Z + "," + Y] = byte.Parse((string)level.ExtraData["DoorOpen" + X + "," + Z + "," + Y]);
                            }
                            catch { }
                        }
                        try { return (byte)level.ExtraData["DoorClose" + X + "," + Z + "," + Y]; }
                        catch { }
                    }
                    return closed;
                }
            }
        }

        public override string Name {
            get { return "Door_$block_$block"; }
        }

        public override byte Permission {
            get { return 0; }
        }
        public override object Clone()
        {
            Door d = new Door(open, closed);
            d.X = X;
            d.Y = Y;
            d.Z = Z;
            return d;
        }
    }
}
