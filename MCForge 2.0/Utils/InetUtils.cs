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
            IPAddress[] AddressList = Dns.GetHostAddresses(DnsCheck);
            bool DNSFine = false;
            foreach (IPAddress IPA in AddressList)
            {
                if (IPA.ToString() == "131.107.255.255")
                    DNSFine = true;
            }
            if (DNSFine == false)
                return false;

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
