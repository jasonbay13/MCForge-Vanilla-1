using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Utils;
using MCForge.Networking;
using System.IO;
using System.Net;

namespace MCForge.Networking.Packets
{
    public class PacketPositionUpdate : Packet
    {
        /// <summary>
        /// Gets or sets the Player ID.
        /// </summary>
        /// <value>The ID of the player.</value>
        /// <remarks></remarks>
        public Byte ID { get; set; }

        /// <summary>
        /// Gets or sets the player change in location.
        /// </summary>
        /// <value>The change in location of the player.</value>
        /// <remarks></remarks>
        public Vector3S Location { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketPositionUpdate"/> class.
        /// </summary>
        public PacketPositionUpdate(byte ID, Vector3S Location)
            : base(PacketIDs.PosUpdate)
        {
            this.ID = ID;
            this.Location = Location;
        }

        public override void ReadPacket(byte[] packetData)
        {
            throw new IOException("Is a write only packet");
        }

        public override byte[] WritePacket()
        {
            return new byte[] { ID, }
                .Concat(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Location.x))).ToArray()
                .Concat(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Location.y))).ToArray()
                .Concat(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Location.z))).ToArray();
        }
    }
}
