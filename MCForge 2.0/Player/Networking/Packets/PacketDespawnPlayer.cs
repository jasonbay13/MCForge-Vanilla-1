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
    public class PacketDespawnPlayer : Packet
    {
        /// <summary>
        /// Gets or sets the Player ID.
        /// </summary>
        /// <value>The ID of the player.</value>
        /// <remarks></remarks>
        public byte ID { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketDespawnPlayer"/> class.
        /// </summary>
        public PacketDespawnPlayer(byte ID)
            : base(PacketIDs.DespawnPlayer)
        {
            this.ID = ID;
        }

        public override void ReadPacket(byte[] packetData)
        {
            throw new IOException("Is a write only packet");
        }

        public override byte[] WritePacket()
        {
            return new byte[] { ID };
        }
    }
}
