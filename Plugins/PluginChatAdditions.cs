using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Plugin;
using MCForge.Entity;
using MCForge.API.PlayerEvent;
using MCForge.API.System;

namespace Plugins {
    public class PluginChatAdditions : IPlugin {
        public string Name {
            get { return "ChatAdditions"; }
        }

        public string Author {
            get { return "headdetect"; }
        }

        public Version Version { 
            get { 
                return new Version(1, 0); 
            } 
        }

        public string CUD {
            get { return "com.mcforge.chat"; }
        }

        public void OnLoad() {
            OnPlayerChatRaw.Register(OnChat);
        }

        public void OnUnload() {
            OnPlayerChatRaw.Unregister(OnChat);
        }

        void OnChat(OnPlayerChatRaw args) {
            //This will be used for the mess that is in HandleChat in Player.cs
            if (args.Message.StartsWith("/womid ")) {
                args.Player.ExtraData.Add("UsingWom", true);
                args.IsCanceled = true;
            }

        }
    }
}
