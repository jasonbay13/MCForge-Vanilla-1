using System;

namespace MCForge.Remote.Packets {
    public class PacketPing : Packet {
        public PacketPing(IRemote remote) :base(remote) {

        }
        public override PacketID PacketID {
            get { return PacketID.Ping; }
        }

        public override void ReadPacket(Networking.PacketData data) {
            throw new NotImplementedException();
        }

        public override Networking.PacketData WritePacket() {
            throw new NotImplementedException();
        }
    }
}

