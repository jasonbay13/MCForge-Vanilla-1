using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using MCForge.Utils;

namespace MCForge.Networking {

    /// <summary>
    /// Structure for sending data along a stream
    /// </summary>
    public abstract class Packet {

        #region Static Methods and Vars

        private static Dictionary<PacketIDs, Type> PacketMap;

        static Packet() {

            PacketMap = new Dictionary<PacketIDs, Type>();

            RegisterPacket(PacketIDs.Identification, typeof(Packets.PacketIdentification));
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

            if ( !PacketMap.ContainsKey(packetId) )
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
        public abstract void ReadPacket(byte[] data);

        /// <summary>
        /// Writes the packet.
        /// </summary>
        /// <returns>A packet ready to be written to the stream</returns>
        public abstract byte[] WritePacket();


        /// <summary>
        /// Array of packet sizes in order. Not incuding PacketID
        /// </summary>
        public static readonly int[] PacketSizes = {
            130, 
            0, 
            0,
            1027,  
            6,
            8, 
            7, 
            73, 
            9,
            6, 
            4, 
            3, 
            1, 
            65,   
            64,   
            1  
        };

        #region Utils

        /// <summary>
        /// Copies the string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="s">The s.</param>
        /// <param name="index">The index.</param>
        public static void CopyString(byte[] bytes, string s, int index) {
            Array.Copy(bytes, index, Encoding.ASCII.GetBytes(StringUtils.Truncate(s, 64, string.Empty).PadRight(64)), 0, 64);
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static void CopyInt(byte[] bytes, int integer, int index) {
            Array.Copy(bytes, index, BitConverter.GetBytes(IPAddress.HostToNetworkOrder(integer)), 0, sizeof(int));
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static void CopyShort( byte[] bytes, short integer, int index) {
            Array.Copy(bytes, index, BitConverter.GetBytes(IPAddress.HostToNetworkOrder(integer)), 0, sizeof(short));
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static void CopyLong(byte[] bytes, long integer, int index) {
            Array.Copy(bytes, index, BitConverter.GetBytes(IPAddress.HostToNetworkOrder(integer)), 0, sizeof(long));
        }


        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="start">The start.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string ReadString(byte[] bytes, int start = 0, int count = 64) {
            return Encoding.ASCII.GetString(bytes, start, count);
        }

        /// <summary>
        /// Reads the int.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static int ReadInt(byte[] bytes, int start = 0) {
            return IPAddress.HostToNetworkOrder(BitConverter.ToInt32(bytes, start));
        }

        /// <summary>
        /// Reads the double.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static double ReadDouble(byte[] bytes, int start = 0) {
            return BitConverter.Int64BitsToDouble(ReadLong(bytes, start));
        }

        /// <summary>
        /// Reads the long.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static long ReadLong(byte[] bytes, int start = 0) {
            return IPAddress.HostToNetworkOrder(BitConverter.ToInt64(bytes, start));
        }


        /// <summary>
        /// Reads the short.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static short ReadShort(byte[] bytes, int start = 0) {
            return IPAddress.HostToNetworkOrder(BitConverter.ToInt16(bytes, start));
        }

        #endregion

    }
}
