using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Networking.Packets {
    public class PacketPing : Packet {

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketPing"/> class.
        /// </summary>
        public PacketPing() : base(PacketIDs.Ping) {

        }

        public override void ReadPacket(byte[] packetData) {
            throw new System.IO.IOException("Is Write-only packet");
        }

        public override byte[] WritePacket() {
            return new byte[0];
        }
    }
}
