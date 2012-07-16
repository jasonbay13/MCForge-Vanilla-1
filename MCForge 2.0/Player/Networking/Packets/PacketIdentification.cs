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


        public override void ReadPacket(PacketData packetData) {
            if(packetData.ReadByte() != PROTOCOL_VERSION)
                throw new IOException("Invalid Client");  

            Username = packetData.ReadString();
            VerificationKey = packetData.ReadString();

            packetData.ReadByte(); //not used
        }

        public override PacketData WritePacket() {
            var packetData = new PacketData();

            packetData.WriteByte(PROTOCOL_VERSION);
            packetData.WriteString(StringUtils.Truncate(ServerSettings.GetSetting("ServerName"), 64, string.Empty));
            packetData.WriteString(StringUtils.Truncate(ServerSettings.GetSetting("MOTD"), 64, string.Empty));
            packetData.WriteByte(OpByte); 

            return packetData;
        }
    }
}
