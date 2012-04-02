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

namespace MCForge.API.PlayerEvent
{
    public class OnPlayerConnect: Event, Cancelable, PlayerEvent
    {
        protected bool _canceled = false;
        public delegate void OnCall(OnPlayerConnect eventargs);
        protected Player p;

        /// <summary>
        /// Create a new Event to call
        /// </summary>
        /// <param name="p">The player connected to the event</param>
        public OnPlayerConnect(Player p) { this.p = p; }

        internal OnPlayerConnect() { }

        public void Cancel(bool value)
        {
            _canceled = value;
        }

        public Player GetPlayer()
        {
            return p;
        }

        public bool IsCanceled { get { return _canceled; } }

        public override void Call()
        {
            EventHelper.cache.ForEach(e =>
                {
                    if (e.GetType() == GetType())
                        ((OnCall)e.Delegate)(this);
                });
            if (IsCanceled && p.isOnline)
                p.Kick("");
        }
        public static void Register(OnCall method, Priority priority)
        {
            EventHelper temp = new EventHelper(method, priority, new OnPlayerConnect());
            EventHelper.Push(temp);
        }
    }
}
