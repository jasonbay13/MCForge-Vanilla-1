using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Plugin;
using MCForge.API.Events;
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

        public void SetNick(Player p, string nick) {
            pList.Add(p, nick);
            p.OnPlayerChat.Normal += new ChatEvent.EventHandler(OnChat);
        }

        public void OnUnload() {

        }

        void OnChat(Player pl, ChatEventArgs args) {
            if (pList.ContainsKey(pl)) {
                var p = Player.Find(pList[pl]);
                args.Username = pList[p];
                //Player.UniversalChat(p.voicestring + p.color + p.prefix + p.Username + ": &f" + args.message);
            }
            else {
                pl.OnPlayerChat.Normal -= new ChatEvent.EventHandler(OnChat);
            }
        }
    }
}
