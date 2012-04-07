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
    public class OnPlayerConnect: PlayerEvent
    {
        public delegate void OnCall(OnPlayerConnect eventargs);

        /// <summary>
        /// Create a new Event to call
        /// </summary>
        /// <param name="p">The player connected to the event</param>
        public OnPlayerConnect(Player p) : base(p) { }

        internal OnPlayerConnect() { }

        public override bool IsCancelable
        {
            get { return true; }
        }

        /// <summary>
        /// Call the event
        /// </summary>
        public override void Call()
        {
            Muffins.cache.ForEach(e =>
                {
                    if (e.type.GetType() == GetType())
                    {
                        datapass = e.datapass;
                        ((OnCall)e.Delegate)(this);
                        if (_unregister)
                        {
                            _unregister = false;
                            Muffins.cache.Remove(e);
                        }
                    }
                });
            if (IsCanceled && GetPlayer().isOnline)
                GetPlayer().Kick("");
        }

        /// <summary>
        /// Register this event
        /// </summary>
        /// <param name="method">The method to call when this event gets excuted</param>
        /// <param name="priority">The importance of the call</param>
        public static void Register(OnCall method, Priority priority, object passdata = null)
        {
            Muffins temp = new Muffins(method, priority, new OnPlayerConnect(), passdata, null);
            Muffins.GiveDerpyMuffins(temp);
        }
    }
}
