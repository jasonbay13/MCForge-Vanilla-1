using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Utils;
using MCForge.Networking;
using System.IO;

namespace MCForge.Networking.Packets
{
    public class PacketServerSetBlock : Packet
    {
        /// <summary>
        /// Gets or sets the block location.
        /// </summary>
        /// <value>The location of the block.</value>
        /// <remarks></remarks>
        public Vector3S Location { get; set; }

        /// <summary>
        /// Gets or sets the block type.
        /// </summary>
        /// <value>The type of the block.</value>
        /// <remarks></remarks>
        public Byte Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketServerSetBlock"/> class.
        /// </summary>
        public PacketServerSetBlock (Vector3S Location, byte Type)
            : base(PacketIDs.ServerSetBlock)
        {
            this.Location = Location;
            this.Type = Type;
        }

        public override void ReadPacket(byte[] packetData)
        {
            throw new IOException("Is a write only packet");
        }

        public override byte[] WritePacket()
        {
            byte[] data = new byte[Packet.PacketSizes[(int)PacketID]];

            CopyShort(data, Location.x, 0);
            CopyShort(data, Location.y, 2);
            CopyShort(data, Location.z, 4);
            CopyByte(data, Type, 6);

            return data;
        }
    }
}
