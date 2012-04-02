using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MCForge
{
    class Heartbeat
    {
        /// <summary>
        /// Sends a heartbeat to minecraft.net.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="Public">if set to <c>true</c> [public].</param>
        /// <param name="salt">The salt.</param>
        /// <param name="onlineUsers">The online users.</param>
        /// <param name="maxUsers">The max users.</param>
        static string minecraftHeartbeat(int port, string serverName, bool Public, string salt, int onlineUsers, byte maxUsers, byte verson)
        {
            string URL = "";
            string requestURL = 
                "http://www.minecraft.net/heartbeat.jsp?port=" + port +
                "&max=" + maxUsers + 
                "&name=" + System.Web.HttpUtility.HtmlEncode(serverName) + 
                "&public=" + Public +
                "&version=" + verson + 
                "&salt=" + System.Web.HttpUtility.HtmlEncode(salt) + 
                "&users=" + onlineUsers;

            WebRequest Heartbeat = WebRequest.Create(requestURL);
            Stream responseStream = Heartbeat.GetResponse().GetResponseStream();
            StreamReader responseStreamReader = new StreamReader(responseStream);
            string line = ""; int i = 0;

            while (line != null)
            {
                i++;
                line = responseStreamReader.ReadLine();
                if (line != null)
                    URL = line;
            }
            responseStream.Close(); responseStreamReader.Close();
            return URL;
        }

        //add another function for another heartbeat
        //also add a reference to the function in sendHeartBeat

        /// <summary>
        /// Sends all the heartbeats.
        /// </summary>
        /// <param name="port">The port the server is running on.</param>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="Public">If the server is public or not.</param>
        /// <param name="salt">The server salt salt.</param>
        /// <param name="onlineUsers">The number of online users.</param>
        /// <param name="maxUsers">The maximum amount of users.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string[] sendHeartbeat()
        {
            string[] output = new string[1]; int i = 0;
            output[i] = minecraftHeartbeat(ServerSettings.port, 
                ServerSettings.NAME, 
                ServerSettings.Public,
                ServerSettings.salt,
                Server.Players.Count,
                ServerSettings.MaxPlayers,
                ServerSettings.version);

            if (Server.URL != output[i]) Server.Log("URL Found/Updated: " + output[i], ConsoleColor.Green, ConsoleColor.Black);
            Server.URL = output[i];
            writeURL(output[i], "text/heartbeaturl.txt"); 

            //i++;

            return output;
        }

        static void writeURL(string URL, string file)
        {
            if (!Directory.Exists(Path.GetDirectoryName(file)))
                Directory.CreateDirectory(Path.GetDirectoryName(file));

            TextWriter o = new StreamWriter(file);
            o.WriteLine(URL);
            o.Close();
        }
    }
}
