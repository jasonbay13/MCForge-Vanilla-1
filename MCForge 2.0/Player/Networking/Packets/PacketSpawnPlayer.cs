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
    public class PacketSpawnPlayer : Packet
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
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name of the entity.</value>
        /// <remarks></remarks>
        public string Name { get; set; }

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
        /// Initializes a new instance of the <see cref="PacketSpawnPlayer"/> class.
        /// </summary>
        public PacketSpawnPlayer(byte ID, string Name, Vector3S Location, byte Yaw, byte Pitch)
            : base(PacketIDs.SpawnPlayer)
        {
            this.ID = ID;
            this.Location = Location;
            this.Yaw = Yaw;
            this.Pitch = Pitch;
            this.Name = Name;
        }

        public override void ReadPacket(byte[] packetData)
        {
            throw new IOException("Is a write only packet");
        }

        public override byte[] WritePacket()
        {
            return new byte[] { ID, }
                .Concat(MakeString(Name)).ToArray()
                .Concat(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Location.x))).ToArray()
                .Concat(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Location.y))).ToArray()
                .Concat(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Location.z))).ToArray()
                .Concat(new byte[] { Yaw, Pitch }).ToArray();
        }
    }
}
