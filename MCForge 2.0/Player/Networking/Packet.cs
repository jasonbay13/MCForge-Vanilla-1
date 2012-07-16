using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Networking {

    /// <summary>
    /// Structure for sending data along a stream
    /// </summary>
    public abstract class Packet {

        #region Static Methods and Vars

        private static Dictionary<PacketIDs, Type> PacketMap;

        static Packet() {

            PacketMap = new Dictionary<PacketIDs, Type>();

            RegisterPacket((PacketIDs)0x00, typeof(Packets.PacketIdentification));
            //TODO: Register packets 

        }


        /// <summary>
        /// Registers the packet.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="type">The type.</param>
        public static void RegisterPacket(PacketIDs id, Type type) {
            if ( type == null )
                throw new ArgumentNullException("type", "type is null");

            if ( type != typeof(Packet) && !type.IsAbstract )
                throw new ArgumentException("Type must be a packet");

            if ( PacketMap.ContainsKey(id) )
                throw new ArgumentException("Packet " + id + " is already registered");

            PacketMap.Add(id, type);
        }

        /// <summary>
        /// Gets the packet from the packet id.
        /// </summary>
        /// <param name="packetId">The packet id.</param>
        /// <returns>A packet if the packetid is registered. If anything goes wrong nothing will be returned.</returns> (badpokerface)
        public static Packet GetPacket(PacketIDs packetId) {

            if(!PacketMap.ContainsKey(packetId))
                throw new ArgumentException("Packet id is not registered");

            Type type = PacketMap[packetId];

            try {
                return Activator.CreateInstance(type) as Packet;
            }
            catch {
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Gets the packet ID.
        /// </summary>
        public PacketIDs PacketID { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Packet"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public Packet(PacketIDs id) {
            PacketID = id;
        }

        /// <summary>
        /// Reads the packet.
        /// </summary>
        /// <param name="packetData">The packet data.</param>
        public abstract void ReadPacket(PacketData packetData);

        /// <summary>
        /// Writes the packet.
        /// </summary>
        /// <returns>A packet ready to be written to the stream</returns>
        public abstract PacketData WritePacket();


    }
}
