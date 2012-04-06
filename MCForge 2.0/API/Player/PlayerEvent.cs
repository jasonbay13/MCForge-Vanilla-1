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
    public abstract class PlayerEvent : Event, Cancelable //We can assume we can cancel most player events..because we can
    {
        internal Player p;
        internal object datapass;
        bool _canceled;
        internal bool _unregister;
        /// <summary>
        /// Get the player connected to the event
        /// </summary>
        /// <returns>The player</returns>
        public virtual Player GetPlayer()
        {
            return p;
        }
        public virtual void Unregister(bool value)
        {
            _unregister = value;
        }
        public abstract bool IsCancelable { get; }
        public virtual bool IsCanceled { get { return _canceled; } }
        public virtual void Cancel(bool value)
        {
            if (IsCancelable)
                _canceled = value;
        }
        public virtual object GetData()
        {
            return datapass;
        }
        public PlayerEvent(Player p)
        {
            this.p = p;
        }
        internal PlayerEvent() { }
        public abstract void Call();
    }
}
