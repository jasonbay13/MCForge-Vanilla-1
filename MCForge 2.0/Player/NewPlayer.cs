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



        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public char Color { get; set; }

        private string _displayName = "";
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
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
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
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

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the color of the title.
        /// </summary>
        /// <value>
        /// The color of the title.
        /// </value>
        public char TitleColor { get; set; }

        /// <summary>
        /// Gets or sets the IP.
        /// </summary>
        /// <value>
        /// The IP.
        /// </value>
        public IPAddress IP { get; set; }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        public PlayerGroup Group { get; set; }



        /// <summary>
        /// Gets or sets a value indicating whether this instance is logged in.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is logged in; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoggedIn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is validated.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is validated; otherwise, <c>false</c>.
        /// </value>
        public bool IsValidated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is loading.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is loading; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoading { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is being kicked.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is being kicked; otherwise, <c>false</c>.
        /// </value>
        public bool IsBeingKicked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is admin.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is admin; otherwise, <c>false</c>.
        /// </value>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is hidden; otherwise, <c>false</c>.
        /// </value>
        public bool IsHidden { get; set; }



        /// <summary>
        /// Gets the data base ID.
        /// </summary>
        public long DataBaseID { get; private set; }

        /// <summary>
        /// Gets the first login.
        /// </summary>
        public DateTime FirstLogin { get; private set; }

        /// <summary>
        /// Gets the last login.
        /// </summary>
        public DateTime LastLogin { get; private set; }




        /// <summary>
        /// Gets or sets a value indicating whether static commands are enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if static commands are enabled; otherwise, <c>false</c>.
        /// </value>
        public bool StaticCommandsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the last command used.
        /// </summary>
        /// <value>
        /// The last command used.
        /// </value>
        public ICommand LastCommandUsed { get; set; }



        /// <summary>
        /// Gets or sets the block change history.
        /// </summary>
        /// <value>
        /// The block change history.
        /// </value>
        public BlockChangeHistory BlockChangeHistory { get; set; }

        /// <summary>
        /// Gets the player ID.
        /// </summary>
        public byte PlayerID { get; private set; }



        /// <summary>
        /// Gets or sets the current position.
        /// </summary>
        /// <value>
        /// The current position.
        /// </value>
        public Vector3S CurrentPosition { get; set; }

        /// <summary>
        /// Gets or sets the old position.
        /// </summary>
        /// <value>
        /// The old position.
        /// </value>
        public Vector3S OldPosition { get; set; }

        /// <summary>
        /// Gets or sets the current pitch.
        /// </summary>
        /// <value>
        /// The current pitch.
        /// </value>
        public byte CurrentPitch { get; set; }

        /// <summary>
        /// Gets or sets the current yaw.
        /// </summary>
        /// <value>
        /// The current yaw.
        /// </value>
        public byte CurrentYaw { get; set; }

        /// <summary>
        /// Gets or sets the old pitch.
        /// </summary>
        /// <value>
        /// The old pitch.
        /// </value>
        public byte OldPitch { get; set; }

        /// <summary>
        /// Gets or sets the old yaw.
        /// </summary>
        /// <value>
        /// The old yaw.
        /// </value>
        public byte OldYaw { get; set; }


        /// <summary>
        /// Gets the extra data.
        /// </summary>
        public ExtraData<object, object> ExtraData { get; private set; }

        private readonly Random Random = new Random();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="NewPlayer"/> class.
        /// </summary>
        public NewPlayer() {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewPlayer"/> class.
        /// </summary>
        /// <param name="Client">The client.</param>
        public NewPlayer(TcpClient Client)
            : this() {



        }



    }
}
