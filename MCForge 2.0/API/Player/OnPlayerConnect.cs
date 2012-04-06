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
using MCForge.Entity;

namespace MCForge.API.PlayerEvent
{
    /// <summary>
    /// The OnPlayerConnect event is executed everytime a player connects to the server
    /// This event can be canceled
    /// </summary>
    public class OnPlayerConnect: Event, Cancelable, PlayerEvent
    {
        bool _canceled = false;
        object datapass;
        public delegate void OnCall(OnPlayerConnect eventargs);
        
        Player p;

        /// <summary>
        /// Create a new Event to call
        /// </summary>
        /// <param name="p">The player connected to the event</param>
        public OnPlayerConnect(Player p) { this.p = p; }

        internal OnPlayerConnect() { }
        public object GetData()
        {
            return datapass;
        }
        /// <summary>
        /// Cancel the event
        /// </summary>
        /// <param name="value">True will cancel the event, false will un-cancel the event</param>
        public void Cancel(bool value)
        {
            _canceled = value;
        }

        /// <summary>
        /// Get the player connected to the event
        /// </summary>
        /// <returns>The player</returns>
        public Player GetPlayer()
        {
            return p;
        }

        /// <summary>
        /// Is the event canceled
        /// </summary>
        public bool IsCanceled { get { return _canceled; } }

        /// <summary>
        /// Call the event
        /// </summary>
        public override void Call()
        {
            Muffins.cache.ForEach(e =>
                {
                    if (e.type.GetType() == GetType() && ((Player)(e.target) == p || e.target == null))
                    {
                        datapass = e.datapass;
                        ((OnCall)e.Delegate)(this);
                    }
                });
            if (IsCanceled && p.isOnline)
                p.Kick("");
        }

        /// <summary>
        /// Register this event
        /// </summary>
        /// <param name="method">The method to call when this event gets excuted</param>
        /// <param name="priority">The importance of the call</param>
        public static void Register(OnCall method, Priority priority, object passdata = null, Player target = null)
        {
            Muffins temp = new Muffins(method, priority, new OnPlayerConnect(), passdata, target);
            Muffins.GiveDerpyMuffins(temp);
        }
    }
}
