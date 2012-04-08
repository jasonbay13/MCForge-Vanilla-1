using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Plugin;
using MCForge.Entity;
using MCForge.API.PlayerEvent;

namespace Plugins {
    public class PluginChatAdditions : IPlugin{
        public string Name {
            get { return "ChatAdditions"; }
        }

        public string Author {
            get { return "headdetect"; }
        }

        public int Version {
            get { return 1; }
        }

        public string CUD {
            get { return "com.mcforge.chat"; }
        }

        public void OnLoad() {
            OnPlayerChat.Register(OnChat, MCForge.API.Priority.System_Level);
        }

        public void OnUnload() {
            
        }

        void OnChat(OnPlayerChat args) {
            //This will be used for the mess that is in HandleChat in Player.cs
        }
    }
}
