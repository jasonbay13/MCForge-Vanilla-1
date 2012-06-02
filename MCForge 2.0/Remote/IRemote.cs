using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace MCForge.Remote {
    public interface IRemote {

        NetworkStream NetworkStream { get; set; }

        PacketReader PacketReader { get; set; }

        PacketWriter PacketWriter { get; set; }

        bool CanProcessPackets { get; set; }

        void Disconnect(string message);

    }
}
