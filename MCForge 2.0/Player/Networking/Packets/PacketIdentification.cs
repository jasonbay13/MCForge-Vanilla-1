using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using System.IO;
using MCForge.Utils.Settings;
using MCForge.Core;
using MCForge.Utils;

namespace MCForge.Networking.Packets {

    public class PacketIdentification : Packet {

        /// <summary>
        /// Protocol Version. Value = 7 (0x07)
        /// </summary>
        public const byte PROTOCOL_VERSION = 0x07;

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the verification key.
        /// </summary>
        /// <value>
        /// The verification key.
        /// </value>
        public string VerificationKey { get; set; }

        private byte OpByte;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketIdentification"/> class.
        /// </summary>
        public PacketIdentification()
            : base(PacketIDs.Identification) {
            //Is read packet
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketIdentification"/> class.
        /// </summary>
        /// <param name="isPlayerOp">if set to <c>true</c> [is player op].</param>
        public PacketIdentification(bool isPlayerOp)
            : base(PacketIDs.Identification) {
            OpByte = (byte)(isPlayerOp ? 0x00 : 0x64);
        }


        public override void ReadPacket(byte[] packetData) {
            if(packetData[0] != PROTOCOL_VERSION)
                throw new IOException("Invalid Client");  

            Username = ReadString(packetData, 1);
            VerificationKey = ReadString(packetData, 65);
        }

        public override byte[] WritePacket() {
            byte[] data = new byte[Packet.PacketSizes[(int)PacketID]];

            data[0] = PROTOCOL_VERSION;
            CopyString(data, ServerSettings.GetSetting("ServerName"), 1);
            CopyString(data, ServerSettings.GetSetting("MOTD"), 65);
            data[129] = OpByte;

            return data;
        }
    }
}
