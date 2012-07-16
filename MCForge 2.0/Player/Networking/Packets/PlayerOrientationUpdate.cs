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
    public class PacketOrientationUpdate : Packet
    {
        /// <summary>
        /// Gets or sets the Player ID.
        /// </summary>
        /// <value>The ID of the player.</value>
        /// <remarks></remarks>
        public Byte ID { get; set; }

        /// <summary>
        /// Gets or sets the player yaw.
        /// </summary>
        /// <value>The yaw of the player.</value>
        /// <remarks></remarks>
        public Byte Yaw { get; set; }

        /// <summary>
        /// Gets or sets the player pitch.
        /// </summary>
        /// <value>The pitch of the player.</value>
        /// <remarks></remarks>
        public Byte Pitch { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketOrientationUpdate"/> class.
        /// </summary>
        public PacketOrientationUpdate(byte ID, byte Yaw, byte Pitch)
            : base(PacketIDs.PosAndRotUpdate)
        {
            this.ID = ID;
            this.Yaw = Yaw;
            this.Pitch = Pitch;
        }

        public override void ReadPacket(byte[] packetData)
        {
            throw new IOException("Is a write only packet");
        }

        public override byte[] WritePacket()
        {
            return new byte[] { ID, }
                .Concat(new byte[] { Yaw, Pitch }).ToArray();
        }
    }
}
