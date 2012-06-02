using System;

namespace MCForge.Remote.Packets {
    public class PacketLogin : Packet {
        public PacketLogin(IRemote remote)
            : base(remote) {
        }
        public override PacketID PacketID {
            get { return Remote.PacketID.Login; }
        }

        public override void ReadPacket(Networking.PacketData data) {
            throw new NotImplementedException();
        }

        public override Networking.PacketData WritePacket() {
            throw new NotImplementedException();
        }
    }
}

