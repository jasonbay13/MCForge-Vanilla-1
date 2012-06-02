using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace MCForge.Remote.Networking {
    public class PacketData {
        private MemoryStream _mem;
        private BinaryReader _read;
        private BinaryWriter _write;
        private PacketOptions _options;
        private byte[] _data;
        public PacketData(byte[] data, PacketOptions options) {
            _data = data;
            _mem = new MemoryStream(data);
            _read = new BinaryReader(_mem);
            _options = options;
        }

        public PacketData(PacketOptions options) {
            _mem = new MemoryStream();
            _write = new BinaryWriter(_mem);
            _options = options;
        }

        public void CheckRead() {
            if (_read == null) throw new IOException("This is a output only PacketData");
        }

        public void CheckWrite() {
            if (_write == null) throw new IOException("This is an input only PacketData");
        }

        #region Read Methods

        public byte ReadByte() {
            CheckRead();
            return _read.ReadByte();
        }

        public byte[] ReadAll() {
            CheckRead();
            return _data;
        }

        public byte[] ReadBytes(int start, int length) {
            CheckRead();
            var temp = new byte[length];
            _data.CopyTo(temp, start);
            return temp;
        }

        public byte[] ReadBytes(int len) {
            CheckRead();
            return _read.ReadBytes(len);
        }

        public int ReadInt() {
            CheckRead();
            return _options.UseBigEndian ? IPAddress.HostToNetworkOrder(_read.ReadInt32()) : _read.ReadInt32();
        }

        public short ReadShort() {
            CheckRead();
            return _options.UseBigEndian ? IPAddress.HostToNetworkOrder(_read.ReadInt16()) : _read.ReadInt16();
        }

        public double ReadDouble() {
            CheckRead();
            var doub = _read.ReadDouble();
            if (_options.UseBigEndian)
                doub = BitConverter.Int64BitsToDouble(IPAddress.HostToNetworkOrder(BitConverter.DoubleToInt64Bits(doub)));
            return doub;
        }

        public string ReadString(Encoding StringEncoding) {
            CheckRead();
            var len = !_options.UseShortAsHeaderSize ? _read.ReadInt32() : _read.ReadInt16();
            return StringEncoding.GetString(ReadBytes(len));
        }

        public string ReadString() {
            return ReadString(Encoding.UTF8);
        }

        #endregion

        #region Write Methods


        public void WriteInt(int i) {
            CheckWrite();
            _write.Write(_options.UseBigEndian ? IPAddress.HostToNetworkOrder(i) : i);
        }

        public void WriteShort(short s) {
            CheckWrite();
            _write.Write(_options.UseBigEndian ? IPAddress.HostToNetworkOrder(s) : s);
        }

        public void WriteDouble(double s) {
            CheckWrite();
            if (_options.UseBigEndian)
                s = BitConverter.Int64BitsToDouble(IPAddress.HostToNetworkOrder(BitConverter.DoubleToInt64Bits(s)));
            _write.Write(s);
        }

        public void WriteLong(long s) {
            CheckWrite();
            _write.Write(_options.UseBigEndian ? IPAddress.HostToNetworkOrder(s) : s);
        }

        public void WriteString(string s) {
            WriteString(s, Encoding.UTF8);
        }

        public void WriteString(string s, Encoding enco) {
            CheckWrite();
            if (_options.UseShortAsHeaderSize)
                WriteInt(_options.UseBigEndian ? s.Length * 2 : s.Length);
            else
                WriteShort((short)(_options.UseBigEndian ? s.Length * 2 : s.Length));
            WriteBytes(enco.GetBytes(s));
        }

        public void WriteByte(byte s) {
            CheckWrite();
            _write.Write(s);
        }

        public void WriteBytes(byte[] s) {
            CheckWrite();
            _write.Write(s);
        }

        #endregion


        public static byte[] GetLength(int p, PacketOptions packetOptions) {
            if (packetOptions.UseShortAsHeaderSize) {
                return BitConverter.GetBytes(packetOptions.UseBigEndian ? IPAddress.HostToNetworkOrder(p) : p);
            }
            else {
                return BitConverter.GetBytes(packetOptions.UseBigEndian ? IPAddress.HostToNetworkOrder((short)p) : (short)p);
            }
        }

        internal static int GetLength(System.Net.Sockets.NetworkStream networkStream, PacketOptions packetOptions) {
            byte[] data = new byte[packetOptions.UseShortAsHeaderSize ? 2 : 4];
            networkStream.Read(data, 0, data.Length);
            var toInt = packetOptions.UseShortAsHeaderSize ? (short)BitConverter.ToInt16(data, 0) : BitConverter.ToInt32(data, 0);
            if (packetOptions.UseBigEndian)
                toInt = packetOptions.UseBigEndian ? IPAddress.HostToNetworkOrder((short)toInt) : IPAddress.HostToNetworkOrder(toInt);
            return toInt;
        }
    }
}
