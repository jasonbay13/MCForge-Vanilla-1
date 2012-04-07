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
using MCForge.Groups;

namespace MCForge.Core
{
    public class ServerSettings
    {
        public static int port = 25565;
        public static string configPath = "config/";

        public static string[] WelcomeText = new string[1] { "Welcome to my Server." };
        public static int PhysicsTick;
        public static bool VerifyAccounts = false;
        public static bool Public = true;
        public static string salt = "";
        public static string password = "Ta33fa30do";
        public static byte MaxPlayers = 10;
        public static byte version = 7;

        public static string MOTD = "Testing 1.. 2..";
        public static string NAME = "Test Server... +hax";

        public static bool EnableGUI = false;

        /// <summary>
        /// Is message appending enabled?
        /// </summary>
        public static bool Appending = true;
        /// <summary>
        /// Allow people teleport to higher ranks?
        /// </summary>
        public static bool higherranktp = false;

        /// <summary>
        /// The default Permission value for new players
        /// </summary>
        public static byte DefaultRank = (byte)Groups.Permission.Guest;
        /// <summary>
        /// Gets or sets the default group.
        /// </summary>
        /// <value>The default group.</value>
        /// <remarks></remarks>
        public static PlayerGroup DefaultGroup
        {
            get
            {
                foreach (PlayerGroup group in PlayerGroup.groups.ToArray())
                {
                    Server.Log(DefaultRank + "." + group.permission);
                    if (group.permission == DefaultRank)
                        return group;
                }
                return new PlayerGroup((byte)Permission.Guest, "Guest", Colors.white, "guests.txt");
            }
            set
            {
                DefaultRank = value.permission;
            }
        }

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
