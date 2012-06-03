using System;
using System.Net.NetworkInformation;
using Timer=System.Threading.Timer;

namespace MCForge.Utils
{
    public class InetUntils
    {
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

                Logger.Log("yes");

                return true;
            }
            return false;
        }

        public static bool CanConnectToInternet() //Check if is returning right page (e.g router might display cannot connect page)
        {
            try {
                System.Net.IPHostEntry Temp = System.Net.Dns.GetHostEntry("www.mcforge.net");
            }
            catch { return false; }
            try {
                System.Net.Sockets.TcpClient clnt = new System.Net.Sockets.TcpClient("www.mcforge.com", 80);
                clnt.Close();
            }
            catch { return false; }
            return true;
        }
    }
}
