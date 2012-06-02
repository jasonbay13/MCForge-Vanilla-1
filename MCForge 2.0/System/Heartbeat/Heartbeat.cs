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
using System.Threading;
using MCForge.Utils.Settings;
using MCForge.Utils;
using System.Drawing;

namespace MCForge.Core.HeartService
{
	class Heartbeat
	{
		public static void PrepareHearts()
		{
			Heart.hearts.Add(new MBeat());
			Heart.hearts.Add(new WBeat());
		}
		/// <summary>
		/// Sends all the heartbeats.
		/// </summary>
		public static string[] sendHeartbeat()
		{
			if (Heart.hearts.Count == 0) PrepareHearts();
			string[] output = new string[Heart.hearts.Count];
			for (int i = 0; i < output.Length; i++)
			{
				try
				{
                    string url = Heart.hearts[i].URL + "?" + Heart.hearts[i].Prepare();
                    WebRequest Heartbeat = WebRequest.Create(url);
                    Stream responseStream = Heartbeat.GetResponse().GetResponseStream();
                    StreamReader responseStreamReader = new StreamReader(responseStream);
                    output[i] = Heart.hearts[i].OnPump(responseStreamReader);
                    responseStream.Close();
				}
				catch (Exception e)
				{
					output[i] = "Error when sending heartbeat to " + Heart.hearts[i].URL;
					Logger.LogError(e);
				}
			}
			//Minecraft URL stuff...
			if (output[0] == "bad heartbeat! (salt is too long)")
			{
				//saltlength is not limited by salt.Length
				//an approxitmately maximum is UrlEncode(salt).Length==60 (sometimes 66 is accepted and next time 62 is too long)
				ServerSettings.GenerateSalt();
				output = sendHeartbeat(); //loops till output[i] claims not about salt is too long anymore
			}
			else
			{
				if (Server.URL != output[0]) Logger.Log("URL Found/Updated: " + output[0], Color.Green, Color.Black);
				Server.URL = output[0];
				writeURL(output[0], "text/heartbeaturl.txt");
			}
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
