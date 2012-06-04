using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Remote.Networking;
using System.Net.Sockets;

namespace MCForge.Remote {
    public class RemoteManager {

        public static List<IRemote> RemoteList { get; set; }

        private static TcpListener serverSocket;

        static RemoteManager() {
            RemoteList = new List<IRemote>();
        }

        public void StartListen() {

        }

        public static IRemote GetRemoteByLoginName(string name) {
            foreach (var remote in RemoteList)
                if (remote.Username == name)
                    return remote;
            return null;
        }

    }
}
