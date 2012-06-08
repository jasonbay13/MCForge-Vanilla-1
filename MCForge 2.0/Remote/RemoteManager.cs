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
using MCForge.API.Events.Remote;

namespace MCForge.Remote {
    public class RemoteManager {

        public static List<IRemote> RemoteList { get; set; }

        private static TcpListener serverSocket;

        public static readonly RemoteConnectEvent OnRemoteConnect = new RemoteConnectEvent();

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public static int Port {
            get { return ServerSettings.GetSettingInt("Remote-Port"); }
            set { ServerSettings.SetSetting("Remote-Port", value.ToString()); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable remote].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable remote]; otherwise, <c>false</c>.
        /// </value>
        public static bool EnableRemote {
            get { return ServerSettings.GetSettingBoolean("Enable-Remote"); }
            set { ServerSettings.SetSetting("Enable-Remote", value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the binding IP.
        /// </summary>
        /// <value>
        /// The binding IP.
        /// </value>
        public static string BindingIP {
            get { return ServerSettings.GetSetting("Remote-IP"); }
            set { ServerSettings.SetSetting("Remote-IP", value); }
        }


        /// <summary>
        /// Initializes the <see cref="RemoteManager"/> class.
        /// </summary>
        static RemoteManager() {
            RemoteList = new List<IRemote>();
        }

        /// <summary>
        /// Starts listening for clients.
        /// </summary>
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

            RemoteConnectEventArgs args = new RemoteConnectEventArgs();
            OnRemoteConnect.Call(type, args);

            if (args.Canceled) {
                client.Close();
                reader.Close();

                if (!Server.ShuttingDown)
                    serverSocket.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);

                return;
            }

            try {
                remote = (IRemote)Activator.CreateInstance(args.Assembly.GetType(type), client);
                ThreadPool.QueueUserWorkItem(new WaitCallback(remote.Run));
            }
            catch (Exception e) {
                Logger.LogError(e);
                if (remote != null)
                    remote.Disconnect("Caused Error");
            }
            if (!Server.ShuttingDown)
                serverSocket.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

        #region Utilities

        /// <summary>
        /// Gets a remote from the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A remote from the login name</returns>
        public static IRemote GetRemoteByLoginName(string name) {
            foreach (var remote in RemoteList)
                if (remote.Username == name)
                    return remote;
            return null;
        }

        #endregion

    }
}
