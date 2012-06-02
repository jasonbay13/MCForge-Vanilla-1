using System;

namespace MCForge.Remote.Packets {
    public class PacketDisconnect : Packet {
        public PacketDisconnect(IRemote remote) :base(remote) {
        }


        public override PacketID PacketID {
            get { return Remote.PacketID.Disconnect; }
        }

        public override void ReadPacket(Networking.PacketData data) {
            throw new NotImplementedException();
        }

        public override Networking.PacketData WritePacket() {
            throw new NotImplementedException();
        }
    }
}

