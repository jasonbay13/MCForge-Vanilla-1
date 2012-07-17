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
    public class PacketDisconnectPlayer : Packet
    {
        /// <summary>
        /// Gets or sets the Player ID.
        /// </summary>
        /// <value>The ID of the player.</value>
        /// <remarks></remarks>
        public byte ID { get; set; }

        /// <summary>
        /// Gets or sets the Message.
        /// </summary>
        /// <value>The message.</value>
        /// <remarks></remarks>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketDisconnectPlayer"/> class.
        /// </summary>
        public PacketDisconnectPlayer(byte ID, string Message)
            : base(PacketIDs.KickPlayer)
        {
            this.ID = ID;
            this.Message = Message;
        }

        public override void ReadPacket(byte[] packetData)
        {
            throw new IOException("Is a write only packet");
        }

        public override byte[] WritePacket()
        {
            return new byte[] { ID }.Concat(MakeString(Message)).ToArray();
        }
    }
}
