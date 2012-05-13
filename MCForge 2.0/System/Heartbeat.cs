/*
Copyright 2011 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using MCForge.Utilities.Settings;
using MCForge.Utilities;
using System.Drawing;

namespace MCForge.Core
{
    class Heartbeat
    {
        /// <summary>
        /// Sends a heartbeat to minecraft.net.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="Public">If set to <c>true</c> [public].</param>
        /// <param name="salt">The salt.</param>
        /// <param name="onlineUsers">The online users.</param>
        /// <param name="maxUsers">The max users.</param>
        static string minecraftHeartbeat(int port, string serverName, bool Public, string salt, int onlineUsers, byte maxUsers, byte version)
        {
            string URL = "";
            string requestURL =
                "http://www.minecraft.net/heartbeat.jsp?port=" + port +
                "&max=" + maxUsers +
                "&name=" + System.Web.HttpUtility.UrlEncode(serverName) +
                "&public=" + Public +
                "&version=" + version +
                "&salt=" + System.Web.HttpUtility.UrlEncode(salt) +
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
        public static string[] sendHeartbeat()
        {
            string[] output = new string[1]; int i = 0;
            try
            {
                output[i] = minecraftHeartbeat(ServerSettings.GetSettingInt("port"),
                    ServerSettings.GetSetting("servername"),
                    ServerSettings.GetSettingBoolean("public"),
                    ServerSettings.Salt,
                    Server.PlayerCount,
                    (byte)ServerSettings.GetSettingInt("maxplayers"),
                    ServerSettings.Version);
            }
            catch (Exception e)
            {
                output[i] = "Error when sending heartbeat";
                Logger.LogError(e);
            }
            if (output[i] == "bad heartbeat! (salt is too long)")
            {
                //saltlength is not limited by salt.Length
                //an approxitmately maximum is UrlEncode(salt).Length==60 (sometimes 66 is accepted and next time 62 is too long)
                ServerSettings.GenerateSalt();
                output = sendHeartbeat(); //loops till output[i] claims not about salt is too long anymore
            }
            else
            {
                if (Server.URL != output[i]) Logger.Log("URL Found/Updated: " + output[i], Color.Green, Color.Black);
                Server.URL = output[i];
                writeURL(output[i], "text/heartbeaturl.txt");
            }

            //i++;

            return output;
        }

        static void writeURL(string URL, string file)
        {
            if (!Directory.Exists(Path.GetDirectoryName(file)))
                Directory.CreateDirectory(Path.GetDirectoryName(file));
			TextWriter o = null;
			try {
				o = new StreamWriter(file);
				o.WriteLine(URL);
				o.Flush();
			} catch (Exception) { // we DON'T CARE if it isn't updated every single time.
			} finally {
				if (o != null)
					o.Close();
			}
        }
    }
}
