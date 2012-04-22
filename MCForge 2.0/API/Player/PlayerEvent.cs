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
using System.Collections.Generic;
using MCForge.Core;
using MCForge.Entity;

namespace MCForge.API.PlayerEvent
{
    /// <summary>
    /// An event that is connected to a player
    /// </summary>
	public abstract class PlayerEvent : Event, Cancelable {
    
		protected Player _target;
		/// <summary>
		/// The player that this event applies to.  If null, applies to all players.
		/// </summary>
		public virtual Player Player { get { return _target;} }
		
		private static bool _canceled;
		
		internal PlayerEvent(Player p) { _target = p; }
		
		/// <summary>
		/// Do we want to prevent the default proccessing?
		/// </summary>
		public bool cancel {
			get { return _canceled; }
		}

		/// <summary>
		/// Prevent default processing until event is unregistered. (or Allow() is called)
		/// </summary>
		public void Cancel() {
			_canceled = true;
		}

		/// <summary>
		/// Allow default processing. (until Cancel() is called)
		/// </summary>
		public void Allow() {
			_canceled = false;
		}

		/// <summary>
		/// Unregisters the event from the queue.
		/// </summary>
		public abstract void Unregister();
	}
}
