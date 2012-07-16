using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MCForge.Networking {
    public class PacketWriter : BinaryWriter {

        public PacketWriter(Stream Stream) :
            base(Stream) {

        }

        public void WritePacket(Packet packet) {
            WritePacket(packet.WritePacket());
        }

        public void WritePacket(byte[] bytes) {
            Write(bytes); //lol?
        }

    }
}
