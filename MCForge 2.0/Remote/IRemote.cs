using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using MCForge.Remote.Networking;

namespace MCForge.Remote {
    public interface IRemote {

        string Username { get; set; }

        int RemoteID { get; set; }

        PacketOptions PacketOptions { get; set; }

        NetworkStream NetworkStream { get; set; }

        PacketReader PacketReader { get; set; }

        PacketWriter PacketWriter { get; set; }

        bool CanProcessPackets { get; set; }

        void Disconnect(string message);

    }
}
