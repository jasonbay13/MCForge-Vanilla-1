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

namespace MCForge.API
{
	public abstract class Event {

		/// <summary>
		/// The type of event (currently not used, but still important.)
		/// </summary>
		protected EventType _type;
		/// <summary>
		/// The priority of the event (currently ignored)
		/// </summary>
		protected Priority _pri;
		/// <summary>
		/// Used to tell two different events apart.
		/// </summary>
		internal string tag;
		/// <summary>
		/// Data to pass to future commands.
		/// </summary>
		public object datapass { get; set; }

	}

	public enum EventType {
		/// <summary>
		/// Events dealing with players
		/// </summary>
		Player,
		/// <summary>
		/// Events dealing with the server
		/// </summary>
		Server,
		/// <summary>
		/// Events dealing with the world (Block, levels, etc.)
		/// </summary>
		World
	}
}
