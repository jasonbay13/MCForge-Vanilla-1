using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Remote.Networking;

namespace MCForge.Remote {
   public class RemoteManager {

       public static List<IRemote> RemoteList { get; set; }

       static RemoteManager() {
           RemoteList = new List<IRemote>();
       }

       public static IRemote GetRemoteByLoginName(string name) {
           foreach (var remote in RemoteList)
               if (remote.Username == name)
                   return remote;
           return null;
       }
    }
}
