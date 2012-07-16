using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using System.IO;
using MCForge.Utils.Settings;
using MCForge.Core;
using MCForge.Utils;
using System.IO.Compression;
using System.Net;

namespace MCForge.Networking.Packets
{
    public class PacketLevelFinalize : Packet
    {
        /// <summary>
        /// Gets or sets the level size.
        /// </summary>
        /// <value>The size of the level.</value>
        /// <remarks></remarks>
        public Vector3S Size { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketLevelDataChunk"/> class.
        /// </summary>
        public PacketLevelFinalize(Vector3S Size)
            : base(PacketIDs.LevelFinalize)
        {
            this.Size = Size;
        }

        public override void ReadPacket(byte[] packetData)
        {
            throw new IOException("Is a write only packet");
        }

        public override byte[] WritePacket()
        {
            byte[] data = new byte[Packet.PacketSizes[(int)PacketID]];

            CopyShort(data, Size.x, 0);
            CopyShort(data, Size.y, 2);
            CopyShort(data, Size.z, 4);

            return data;
        }
    }
}
