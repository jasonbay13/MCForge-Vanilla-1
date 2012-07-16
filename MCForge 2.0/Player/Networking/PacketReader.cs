using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace MCForge.Networking {
    public class PacketReader : BinaryReader {

        public PacketReader(Stream Stream)
            : base(Stream) {

        }

        public Packet ReadPacket() {

            Packet packet = null;
            int id = ReadByte();

            if ( id > Packet.PacketSizes.Length ) {
                throw new IOException("Recieved ID that was out of bounds");
            }

            int len = Packet.PacketSizes[id];
            byte[] data = new byte[len];
            Read(data, 0, len);

            packet = Packet.GetPacket((PacketIDs)id);

            if(packet == null)
                throw new IOException("Recieved malformed packet");

            packet.ReadPacket(data);

            return packet;
        }
    }
}
