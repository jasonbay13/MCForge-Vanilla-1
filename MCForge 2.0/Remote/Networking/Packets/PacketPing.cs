using System;

namespace MCForge.Remote.Packets {
    public class PacketPing : Packet {
        public PacketPing() {
        }

        #region implemented abstract members of ComputerRemote.Packet
        public override PacketID PacketID {
            get {
                throw new NotImplementedException();
            }
        }

        public override int Length {
            get {
                throw new NotImplementedException();
            }
        }

        public override byte[] Data {
            get {
                throw new NotImplementedException();
            }
        }

        public override void ReadPacket(byte[] data) {
            throw new NotImplementedException();
        }

        public override void WritePacket(IRemote c) {
            throw new NotImplementedException();
        }

        #endregion
    }
}

