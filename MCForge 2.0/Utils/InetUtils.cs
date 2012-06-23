/*
Copyright 2012 MCForge
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
using System.Net.NetworkInformation;
using Timer=System.Threading.Timer;
using System.Net;

namespace MCForge.Utils
{
    public class InetUtils
    {
        public static bool InternetAvailable = false;

        public InetUtils()
        {
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            InetUtils.InternetAvailable = CanConnectToInternet();
        }

        public static bool IsNetworkAvailable(long minimumSpeed) //10000000 filters modems, serial etc
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return false;

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Discard because of standard reasons
                if ((ni.OperationalStatus != OperationalStatus.Up) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
                    continue;

                if (ni.Speed < minimumSpeed)
                    continue;

                // Discard virtual cards (virtual box, virtual pc, etc.)
                if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
                    continue;

                return true;
            }
            return false;
        }

        public static bool CanConnectToInternet() //Uses NSCI (http://technet.microsoft.com/en-us/library/cc766017.aspx)
        {
            string DnsCheck = "dns.msftncsi.com";
            try
            {
                IPAddress[] AddressList = Dns.GetHostAddresses(DnsCheck); //DIS THING
                bool DNSFine = false;
                foreach (IPAddress IPA in AddressList)
                {
                    if (IPA.ToString() == "131.107.255.255")
                        DNSFine = true;
                }
                if (DNSFine == false)
                    return false;
                }
            catch { return false; }

            string ConnectionCheck = "http://www.msftncsi.com/ncsi.txt";
            string Result = GrabWebpage(ConnectionCheck);
            if (Result != "Microsoft NCSI")
                return false;

            return true;
        }

        public static string GrabWebpage(string location)
        {
            try
            {
                using (var client = new WebClient())
                {
                    return client.DownloadString(location);
                }
            }
            catch { return ""; }
        }
    }
}
