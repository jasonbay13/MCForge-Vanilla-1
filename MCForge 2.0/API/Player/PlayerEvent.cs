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
using System.Text;
using MCForge;
using MCForge.Entity;

namespace MCForge.API.PlayerEvent
{
    /// <summary>
    /// An event that is connected to a player
    /// </summary>
    public abstract class PlayerEvent : Event, Cancelable //We can assume we can cancel most player events..because we can
    {
        /// <summary>
        /// Get the player connected to the event
        /// </summary>
        /// <returns>The player</returns>
        public Player Player { get; private set; }
        internal object datapass;
        bool _canceled;
        internal bool _unregister;

       
        /// <summary>
        /// Unregister the event
        /// </summary>
        /// <param name="value">True will unregister the event, false wont</param>
        public virtual void Unregister(bool value)
        {
            _unregister = value;
        }

        /// <summary>
        /// Check to see if you can cancel the event
        /// </summary>
        public abstract bool IsCancelable { get; }

        /// <summary>
        /// Check to see if the event is canceled
        /// </summary>
        public bool IsCanceled {
            get {
                return _canceled;
            }
            set {
                if (IsCancelable) return;
                else _canceled = value;
            }
        }
        /// <summary>
        /// Get the data you passed when registering the event
        /// </summary>
        /// <returns>The object in the form of a object type</returns>
        public virtual object GetData()
        {
            return datapass;
        }
        /// <summary>
        /// Create a new instance of the event
        /// </summary>
        /// <param name="p">The player connected to the event</param>
        public PlayerEvent(Player p)
        {
            this.Player = p;
        }
        internal PlayerEvent() { }

        /// <summary>
        /// Call the event
        /// </summary>
        public abstract void Call();
    }
}
