using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Remote.Networking;
using System.Net.Sockets;
using MCForge.Utils.Settings;
using System.Net;
using System.IO;
using System.Threading;
using MCForge.Utils;
using MCForge.Core;

namespace MCForge.Remote {
    public class RemoteManager {

        public static List<IRemote> RemoteList { get; set; }

        private static TcpListener serverSocket;

        public static int Port {
            get { return ServerSettings.GetSettingInt("Remote-Port"); }
            set { ServerSettings.SetSetting("Remote-Port", value.ToString()); }
        }

        public static bool EnableRemote {
            get { return ServerSettings.GetSettingBoolean("Enable-Remote"); }
            set { ServerSettings.SetSetting("Enable-Remote", value.ToString()); }
        }

        public static string BindingIP {
            get { return ServerSettings.GetSetting("Remote-IP"); }
            set { ServerSettings.SetSetting("Remote-IP", value); }
        }


        static RemoteManager() {
            RemoteList = new List<IRemote>();
        }

        public void StartListen() {
            if (!EnableRemote)
                return;

            serverSocket = new TcpListener(IPAddress.Parse(BindingIP), Port);
            serverSocket.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

        private void OnConnect(IAsyncResult result) {
            TcpClient client = serverSocket.EndAcceptTcpClient(result);
            StreamReader reader = new StreamReader(client.GetStream());
            string type = reader.ReadLine();
            IRemote remote = null;
            try {
                remote = (IRemote)Activator.CreateInstance(Type.GetType(type), client);
                ThreadPool.QueueUserWorkItem(new WaitCallback(remote.Run));
            }
            catch (Exception e) {
                Logger.LogError(e);
                if (remote != null)
                    remote.Disconnect("Caused Error");
            }
            if (!Server.shuttingDown)
                serverSocket.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

        #region Utilities

        public static IRemote GetRemoteByLoginName(string name) {
            foreach (var remote in RemoteList)
                if (remote.Username == name)
                    return remote;
            return null;
        }

        #endregion

    }
}
