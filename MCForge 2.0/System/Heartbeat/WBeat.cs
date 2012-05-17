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
using System.Net;
using System.IO;
using MCForge.Utils.Settings;
using MCForge.Utils;


namespace MCForge.Core.HeartService
{
	/// <summary>
	/// The WoM heartbeat
	/// </summary>
	public class WBeat : Heart
	{
		public override string URL {
			get {
				return "http://direct.worldofminecraft.com/hb.php";
			}
		}
		public override string Prepare()
		{
			return "salt=" + System.Web.HttpUtility.UrlEncode(ServerSettings.Salt) +
				"&users=" + Server.PlayerCount +
				"&alt=" + ServerSettings.GetSetting("Wom-Alternate_Name") + 
				"&desc=" + ServerSettings.GetSetting("Wom-Server_Description") +
				"&flags=" + ServerSettings.GetSetting("Wom-Server_Flags");
		}
		public override string OnPump(StreamReader responseStreamReader)
		{
			return base.OnPump(responseStreamReader);
		}
	}
}
