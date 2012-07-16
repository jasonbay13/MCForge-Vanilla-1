using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Utils;
using System.IO;

namespace MCForge.Networking.Packets
{
    public class PacketLevelInitialize : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PacketLevelInitialize"/> class.
        /// </summary>
        public PacketLevelInitialize() : base(PacketIDs.LevelInitialize) { }

        public override void ReadPacket(byte[] data)
        {
            throw new IOException("Is a write only packet");
        }

        public override byte[] WritePacket()
        {
            return new byte[0] {};
        }
    }
}
