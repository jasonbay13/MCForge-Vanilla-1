using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Plugin;
using MCForge.API.Events;
using MCForge.Entity;
using MCForge.Utils;

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

        public void OnLoad ( string[] args ) {
            pList = new Dictionary<Player, string>();

            Player.OnAllPlayersCommand.Normal += OnCommand;
        }

        public void SetNick ( Player p, string nick ) {
            pList.Add( p, nick );
            p.OnPlayerChat.Normal += new ChatEvent.EventHandler( OnChat );
        }

        void OnCommand ( Player p, CommandEventArgs e ) {
            if ( e.Command.ToLower() != "nickname" || ( e.Command.ToLower() != "help" && e.Args.Length == 1 && e.Args[ 0 ].ToLower() == "nickname" ) )
                return;

            e.Cancel();

            if ( e.Command.ToLower() == "help" ) {
                Help( p );
                return;
            }

            if ( e.Args.Length < 1 ) {
                if ( pList.ContainsKey( p ) ) {
                    pList.Remove( p );
                    p.OnPlayerChat.Normal -= OnChat;
                    p.SendMessage( "Removed nick name" );
                }
                return;
            }

            SetNick( p, String.Join( " ", e.Args ) );
            p.SendMessage( "Set nick name" );
        }

        void Help ( Player p ) {
            p.SendMessage( "/nickname <name to use> - add a name for yourself" );
            p.SendMessage( "/nicname - remove existing nickname" );
        }
        public void OnUnload ( ) {

        }

        void OnChat ( Player pl, ChatEventArgs args ) {
            if ( pList.ContainsKey( pl ) ) {
                args.Cancel();
                 var mColor = pl.Color ?? pl.Group.Color;
                Player.UniversalChat(mColor + pList[ pl ] + ": &f" + args.Message );
                Player.UniversalChatOps( mColor + pl.Username + ": &f" + args.Message );
            }
            else {
                pl.OnPlayerChat.Normal -= new ChatEvent.EventHandler( OnChat );
            }
        }
    }
}
