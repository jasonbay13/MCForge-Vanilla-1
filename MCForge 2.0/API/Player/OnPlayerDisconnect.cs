/*
Copyright 2011 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using MCForge.Entity;

namespace MCForge.API.PlayerEvent {
    /// <summary>
    /// The OnPlayerConnect event is executed everytime a player disconnects from the server
    /// This event can be canceled (but the player will still disconnect)
    /// </summary>
    public sealed class OnPlayerDisconnect : PlayerEvent {

        private Player uSoStoopid;

        public override Player Player {
            get {
                return uSoStoopid;
            }
        }

        /// <summary>
        /// Creates a new event.  This is NOT meant to be used by user-code, only internally by events.
        /// </summary>
        /// <param name="playerConnected">Player that disconnected</param>
        internal OnPlayerDisconnect(Player playerDisconnected) {
            uSoStoopid = playerDisconnected;
        }

        /// <summary>
        /// The delegate used for callbacks.  The caller will have this method run when the event fires.
        /// </summary>
        /// <param name="args">The Event that fired</param>
        public delegate void OnCall(OnPlayerDisconnect args);


        private static List<OnCall> _eventQueue = new List<OnCall>();

        /// <summary>
        /// This is meant to be called from the code where you mean for the event to happen.
        /// 
        /// In this case, it is called from the command processing code.
        /// </summary>
        public void Call() {
            _eventQueue.ForEach(method => {
                method(this);
            });
        }

        /// <summary>
        /// Used to register a method to be executed when the event is fired.
        /// </summary>
        /// <param name="callback">The method to register</param>
        public static void Register(OnCall callback) {
            _eventQueue.Add(callback);
        }


        /// <summary>
        /// Used to unregister a method that was previously registered.
        /// </summary>
        /// <param name="callback">The method to unregister</param>
        public static void Unregister(OnCall callback) {
            if (_eventQueue.Contains(callback))
                _eventQueue.RemoveAll(cur => cur == callback);
        }

        [Obsolete("Please use Unregister(OnCall)", true)]
        public override void Unregister() {
            throw new NotImplementedException();
        }
    }
}
