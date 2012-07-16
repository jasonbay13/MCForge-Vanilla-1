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
    public class PacketPositionAndOrientation : Packet
    {
        /// <summary>
        /// Gets or sets the Player ID.
        /// </summary>
        /// <value>The ID of the player.</value>
        /// <remarks></remarks>
        public Byte ID { get; set; }

        /// <summary>
        /// Gets or sets the player location.
        /// </summary>
        /// <value>The location of the player.</value>
        /// <remarks></remarks>
        public Vector3S Location { get; set; }

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
        /// Initializes a new instance of the <see cref="PacketPositionAndOrientation"/> class.
        /// </summary>
        public PacketPositionAndOrientation(byte ID, Vector3S Location, byte Yaw, byte Pitch)
            : base(PacketIDs.PosAndRot)
        {
            this.ID = ID;
            this.Location = Location;
            this.Yaw = Yaw;
            this.Pitch = Pitch;
        }

        public override void ReadPacket(byte[] packetData)
        {
            ID = packetData[0];
            Location = new Vector3S()
            {
                x = ReadShort(packetData, 1),
                y = ReadShort(packetData, 3),
                z = ReadShort(packetData, 5)
            };

            Yaw = packetData[7];

            Pitch = packetData[8];
        }

        public override byte[] WritePacket()
        {
            return new byte[] { ID, }
                .Concat(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Location.x))).ToArray()
                .Concat(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Location.y))).ToArray()
                .Concat(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Location.z))).ToArray()
                .Concat(new byte[] { Yaw, Pitch }).ToArray();
        }
    }
}
