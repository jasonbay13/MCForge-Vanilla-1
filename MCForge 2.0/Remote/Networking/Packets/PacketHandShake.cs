using System;
using MCForge.Remote.Networking;

namespace MCForge.Remote.Packets {
    public class PacketHandShake : Packet {
        public PacketHandShake(IRemote remote) :base(remote) {
        }

        public override PacketID PacketID {
            get { return Remote.PacketID.Handshake; }
        }

        public override void ReadPacket(Networking.PacketData data) {
            throw new NotImplementedException();
        }

        public override PacketData WritePacket() {
            throw new NotImplementedException();
        }
    }
}

