using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using MCForge.Networking;
using System.Net;
using MCForge.Interface.Command;
using MCForge.World;
using MCForge.Core;
using MCForge.Utils;
using MCForge.Groups;

namespace MCForge.Entity {
    public class NewPlayer {

        #region Properties

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public TcpClient Client { get; set; }

        /// <summary>
        /// Gets or sets the packet queue.
        /// </summary>
        /// <value>
        /// The packet queue.
        /// </value>
        public PacketQueue PacketQueue { get; set; }



        public string Username { get; private set; }

        public char Color { get; set; }

        private string _displayName = "";
        public string DisplayName {
            get {
                return _displayName;
            }
            set {
                _displayName = value;
                //if (IsLoggedIn) {
                //    this.GlobalDie();
                //     SpawnThisPlayerToOtherPlayers();
                //  }
            }
        }

        private Level _level;
        public Level Level {
            get {
                if ( _level == null )
                    _level = Server.Mainlevel;
                return _level;
            }
            set {
                _level = value;
                //  if (IsLoggedIn)
                //    SendMap();
            }
        }

        public string Title { get; set; }

        public char TitleColor { get; set; }

        public IPAddress IP { get; set; }

        public PlayerGroup Group { get; set; }



        public bool IsLoggedIn { get; set; }

        public bool IsValidated { get; set; }

        public bool IsLoading { get; set; }

        public bool IsBeingKicked { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsHidden { get; set; }



        public long DataBaseID { get; set; }

        public DateTime FirstLogin { get; set; }

        public DateTime LastLogin { get; set; }




        public bool StaticCommandsEnabled { get; set; }

        public ICommand LastCommandUsed { get; set; }



        public BlockChangeHistory BlockChangeHistory { get; set; }

        public byte PlayerID { get; set; }



        public Vector3S CurrentPosition { get; set; }

        public Vector3S OldPosition { get; set; }

        public byte CurrentPitch { get; set; }

        public byte CurrentYaw { get; set; }

        public byte OldPitch { get; set; }

        public byte OldYaw { get; set; }


        public ExtraData<object, object> ExtraData { get; private set; }

        private readonly Random Random = new Random();

        #endregion

        public NewPlayer() {

        }

        public NewPlayer(TcpClient Client)
            : this() {



        }



    }
}
