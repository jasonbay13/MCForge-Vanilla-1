using System;

namespace MCForge.Remote.Packets {
    public class PacketInvalid : Packet {
        public PacketInvalid(IRemote remote) :base(remote) {
        }

        public override PacketID PacketID {
            get { return Remote.PacketID.Invalid; }
        }

        public override void ReadPacket(Networking.PacketData data) {
            throw new NotImplementedException();
        }

        public override Networking.PacketData WritePacket() {
            return null;
        }
    }
}

