using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using System.IO;
using MCForge.Utils.Settings;
using MCForge.Core;
using MCForge.Utils;
using System.IO.Compression;
using System.Net;

namespace MCForge.Networking.Packets
{
    public class PacketLevelDataChunk : Packet
    {
        /// <summary>
        /// Gets or sets the length of the chunk.
        /// </summary>
        /// <value>The length of the chunk.</value>
        /// <remarks></remarks>
        public short ChunkLength { get; set; }
        /// <summary>
        /// Gets or sets the chunk data.
        /// </summary>
        /// <value>The chunk data.</value>
        /// <remarks></remarks>
        public byte[] ChunkData { get; set; }
        /// <summary>
        /// Gets or sets the percentage complete.
        /// </summary>
        /// <value>The percentage complete.</value>
        /// <remarks></remarks>
        public byte PercentageComplete { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketLevelDataChunk"/> class.
        /// </summary>
        public PacketLevelDataChunk(short ChunkLength, byte[] ChunkData, byte PercentageComplete) : base(PacketIDs.LevelDataChunk)
        {
            this.ChunkLength = ChunkLength;
            this.ChunkData = ChunkData;
            this.PercentageComplete = PercentageComplete;
        }

        public override void ReadPacket(byte[] packetData)
        {
            throw new IOException("Is a write only packet");
        }

        public override byte[] WritePacket()
        {
            return new byte[] {}
                .Concat(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(ChunkLength))).ToArray()
                .Concat(ChunkData).ToArray()
                .Concat(new byte[] { PercentageComplete }).ToArray();
        }
    }
}
