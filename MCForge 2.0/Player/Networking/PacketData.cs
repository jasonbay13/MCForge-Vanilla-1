using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace MCForge.Networking {

    /// <summary>
    /// Byte data inside the packet
    /// </summary>
    public class PacketData : IDisposable {

        private BinaryReader Reader;
        private BinaryWriter Writer;
        private MemoryStream Stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketData"/> class.
        /// </summary>
        /// <remarks>Becomes an OUTPUT only packet data</remarks>
        public PacketData() {
            Stream = new MemoryStream();
            Writer = new BinaryWriter(Stream);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="PacketData"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <remarks>Becomes an INPUT only packet data</remarks>
        public PacketData(byte[] data) {
            Stream = new MemoryStream(data);
            Reader = new BinaryReader(Stream);
        }

        #region Utils

        /// <summary>
        /// Reads the int.
        /// </summary>
        /// <returns></returns>
        public int ReadInt() {
            CheckInput();
            return IPAddress.HostToNetworkOrder(Reader.ReadInt32());
        }

        /// <summary>
        /// Reads the short.
        /// </summary>
        /// <returns></returns>
        public short ReadShort() {
            CheckInput();
            return IPAddress.HostToNetworkOrder(Reader.ReadInt16());
        }

        /// <summary>
        /// Reads the long.
        /// </summary>
        /// <returns></returns>
        public long ReadLong() {
            CheckInput();
            return IPAddress.HostToNetworkOrder(Reader.ReadInt64());
        }

        /// <summary>
        /// Reads the byte.
        /// </summary>
        /// <returns></returns>
        public byte ReadByte() {
            CheckInput();
            return Reader.ReadByte();
        }

        /// <summary>
        /// Reads the Signed byte.
        /// </summary>
        /// <returns></returns>
        public sbyte ReadSByte() {
            CheckInput();
            return Reader.ReadSByte();
        }

        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <returns></returns>
        public string ReadString() {
            CheckInput();
            return Encoding.ASCII.GetString(Reader.ReadBytes(64), 0, 64);
        }


        /// <summary>
        /// Writes the int.
        /// </summary>
        /// <param name="i">The i.</param>
        public void WriteInt(int i) {
            CheckOutput();
            Writer.Write(IPAddress.HostToNetworkOrder(i));
        }

        /// <summary>
        /// Writes the short.
        /// </summary>
        /// <param name="i">The i.</param>
        public void WriteShort(short i) {
            CheckOutput();
            Writer.Write(IPAddress.HostToNetworkOrder(i));
        }

        /// <summary>
        /// Writes the long.
        /// </summary>
        /// <param name="i">The i.</param>
        public void WriteLong(long i) {
            CheckOutput();
            Writer.Write(IPAddress.HostToNetworkOrder(i));
        }

        /// <summary>
        /// Writes the byte.
        /// </summary>
        /// <param name="i">The i.</param>
        public void WriteByte(byte i) {
            CheckOutput();
            Writer.Write(i);
        }

        /// <summary>
        /// Writes the Signed byte.
        /// </summary>
        /// <param name="i">The i.</param>
        public void WriteSByte(sbyte i) {
            CheckOutput();
            Writer.Write(i);
        }

        /// <summary>
        /// Writes the string.
        /// </summary>
        /// <param name="i">The i.</param>
        public void WriteString(string i) {
            CheckOutput();

            if ( i.Length > 64 ) {
                int len = i.Length;

                while ( len > 0 ) {
                    Writer.Write(Encoding.ASCII.GetBytes(i.ToArray(), i.Length - len, 64));
                    len -=64;
                }

                return;
            }

            Writer.Write(Encoding.ASCII.GetBytes(i));
        }


        #endregion

        void CheckInput() {
            if ( Reader == null )
                throw new IOException("This is an output only packet data");
        }

        void CheckOutput() {

            if ( Writer == null )
                throw new IOException("This is an input only packet data");

        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            if ( Reader != null ) {
                Reader.Dispose();
                Reader = null;
            }

            if ( Writer != null ) {
                Writer.Dispose();
                Writer = null;
            }
        }

        #endregion
    }
}
