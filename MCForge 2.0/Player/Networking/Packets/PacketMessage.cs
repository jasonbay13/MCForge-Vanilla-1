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
    public class PacketMessage : Packet
    {
        /// <summary>
        /// Gets or sets the Player ID.
        /// </summary>
        /// <value>The ID of the player.</value>
        /// <remarks></remarks>
        public Byte ID { get; set; }

        /// <summary>
        /// Gets or sets the Message.
        /// </summary>
        /// <value>The message.</value>
        /// <remarks></remarks>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketMessage"/> class.
        /// </summary>
        public PacketMessage(byte ID, string Message)
            : base(PacketIDs.Message)
        {
            this.ID = ID;
            this.Message = Message;
        }

        public override void ReadPacket(byte[] packetData)
        {
            ID = packetData[0];

            Message = ReadString(packetData, 2, 62); //64 or 62
        }

        public override byte[] WritePacket()
        {
            return new byte[] { ID }.Concat(MakeString(Message)).ToArray();
        }
    }
}
