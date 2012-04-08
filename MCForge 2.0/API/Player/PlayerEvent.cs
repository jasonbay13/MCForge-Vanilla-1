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
	public abstract class PlayerEvent : Event {
    
		protected Player _target;
		public Player target { get { return _target;} }
		protected string _msg;
		protected Vector3 _oldPos;
		public delegate void OnCall(PlayerEvent e);
		protected OnCall _queue;
		protected static List<PlayerEvent> _eventQueue = new List<PlayerEvent>();
    
	}
}
