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
using System.Collections.Generic;
using System.IO;
using MCForge.Utils.Settings;
using MCForge.Utils;

namespace MCForge.Core.HeartService
{
	/// <summary>
	/// An abstract class for custom heartbeats
	/// </summary>
	public abstract class Heart
	{
		public static List<Heart> hearts = new List<Heart>();
		public abstract string URL { get; }
		public abstract string Prepare();
		public virtual string OnPump(StreamReader responseStreamReader)
		{
			string line = ""; int i = 0;
			string URL = "";

            while (line != null)
            {
                i++;
                line = responseStreamReader.ReadLine();
                if (line != null)
                    URL = line;
            }
            responseStreamReader.Close();
            return URL;
		}
	}
}
