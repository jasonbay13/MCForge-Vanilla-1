using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MCForge.Remote
{
    public class Packet
    {
        public List<byte> totalData;
        Encoding encoding;
        bool LittleEndian;

        public Packet(Encoding e, bool LittleEndian)
        {
            totalData = new List<byte>();
            encoding = e;
            this.LittleEndian = LittleEndian;
        }

        public void addData(byte b)
        {
            totalData.Add(b);
        }
        public void addData(string b, bool useShort = true)
        {
            b = b.PadLeft(b.Length);
            byte[] array = encoding.GetBytes(b);
            addData(!useShort ? (short)b.Length : b.Length);
            for (int i = 0; i < array.Count(); i++)
                totalData.Add(array[i]);
        }
        public void addData(long b)
        {
            if (!LittleEndian)
                b = IPAddress.HostToNetworkOrder(b);
            byte[] array = BitConverter.GetBytes(b);
            for (int i = 0; i < array.Count(); i++)
                totalData.Add(array[i]);
        }
        public void addData(int b)
        {
            if (!LittleEndian)
                b = IPAddress.HostToNetworkOrder(b);
            byte[] array = BitConverter.GetBytes(b);
            for (int i = 0; i < array.Count(); i++)
                totalData.Add(array[i]);
        }
        public void addData(short b)
        {
            if (!LittleEndian)
                b = IPAddress.HostToNetworkOrder(b);
            byte[] array = BitConverter.GetBytes(b);
            for (int i = 0; i < array.Count(); i++)
                totalData.Add(array[i]);
        }
        public void addData(bool b)
        {
            totalData.Add((byte)(b ? 0x1 : 0x0));
        }
        public byte[] getData()
        {
            return totalData.ToArray();
        }

    }
}
