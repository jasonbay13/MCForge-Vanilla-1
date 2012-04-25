using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Plugin;
using MCForge.API.PlayerEvent;
using MCForge.Entity;

namespace Plugins {
    public class PluginNickName : IPlugin {
        private Dictionary<Player, string> pList;

        public string Name {
            get {
                return "NickName";
            }
        }

        public string Author {
            get {
                return "headdetect";
            }
        }

        public int Version {
            get {
                return 1;
            }
        }

        public string CUD {
            get {
                return "com.mcforge.nickname";
            }
        }

        public void OnLoad(string[] args) {
            pList = new Dictionary<Player, string>();



        }

        public void OnUnload() {
            
        }

        void OnChat(OnPlayerChat args) {
            if (pList.ContainsKey(args.Player)) {
                var p = Player.Find(pList[args.Player]);
                //Player.UniversalChat(p.voicestring + p.color + p.prefix + p.Username + ": &f" + args.message);
            }
        }
    }
}
