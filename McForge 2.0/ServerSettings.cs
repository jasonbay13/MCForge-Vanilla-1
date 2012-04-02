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

namespace MCForge
{
	class ServerSettings
    {
        public static int port = 25565;
		public static string configPath = "config/";

		public static string[] WelcomeText = new string[1] { "Welcome to my Server." };

		public static bool VerifyAccounts = false;
		public static string salt = "";
		public static string password = "Ta33fa30do";
		public static byte MaxPlayers = 10;
		public static byte version = 7;

		public static string MOTD = "Testing 1.. 2..";
		public static string NAME = "Test Server... +hax";

        public static bool EnableGUI = false;

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
