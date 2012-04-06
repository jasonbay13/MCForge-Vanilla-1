using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Network.Packets {
    class PacketPing : Packet {
        public override PacketID PacketID {
            get { throw new NotImplementedException(); }
        }

        public override int Length {
            get { throw new NotImplementedException(); }
        }

        public override byte[] Data {
            get { throw new NotImplementedException(); }
        }

        public override void ReadPacket(Entity.Player p) {
            throw new NotImplementedException();
        }

        public override void WritePacket(Entity.Player p) {
            throw new NotImplementedException();
        }

        public override void HandlePacket( Entity.Player Player) {
            throw new NotImplementedException();
        }
    }
}
