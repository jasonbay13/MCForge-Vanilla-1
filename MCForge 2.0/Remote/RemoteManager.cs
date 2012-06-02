using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Remote.Networking;

namespace MCForge.Remote {
   public class RemoteManager {

       public static PacketOptions PacketOptions { get; set; }

       public static List<IRemote> RemoteList { get; set; }

       static RemoteManager() {
           PacketOptions = new PacketOptions();
           RemoteList = new List<IRemote>();
       }
    }
}
