using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Utils;
using System.IO;

namespace MCForge.Networking.Packets {
    public class PacketPlayerSetBlock : Packet {

        /// <summary>
        /// Gets the position of the block change.
        /// </summary>
        public Vector3S Position {get; private set;}

        /// <summary>
        /// Gets a value indicating whether the block was deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if block deleted; otherwise, <c>false</c>.
        /// </value>
        public bool BlockDeleted {get; private set;}

        /// <summary>
        /// Gets the block.
        /// </summary>
        public byte Block {get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketPlayerSetBlock"/> class.
        /// </summary>
        public PacketPlayerSetBlock()
            : base(PacketIDs.PlayerSetBlock) {
            //Readonly
        }

        public override void ReadPacket(byte[] data) {
            Position = new Vector3S(){
                x = ReadShort(data, 0),
                y = ReadShort(data, 2),
                z = ReadShort(data, 4)
            };

            BlockDeleted = data[5] == 0x00;

            Block = data[6];

        }

        public override byte[] WritePacket() {
            throw new IOException("Is a readonly packet");
        }
    }
}
