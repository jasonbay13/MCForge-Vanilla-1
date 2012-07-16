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
    public class PacketUpdateType : Packet
    {
        /// <summary>
        /// Gets or sets the Player ID.
        /// </summary>
        /// <value>The ID of the player.</value>
        /// <remarks></remarks>
        public Byte ID { get; set; }

        /// <summary>
        /// Gets or sets the user type.
        /// </summary>
        /// <value>The user type.</value>
        /// <remarks></remarks>
        public byte UserType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketUpdateType"/> class.
        /// </summary>
        public PacketUpdateType(byte ID, byte UserType)
            : base(PacketIDs.UpdateUserType)
        {
            this.ID = ID;
            this.UserType = UserType;
        }

        public override void ReadPacket(byte[] packetData)
        {
            throw new IOException("Is a write only packet");
        }

        public override byte[] WritePacket()
        {
            return new byte[] { ID, UserType };
        }
    }
}
