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
using MCForge.Utils;

namespace MCForge.Entity
{
	/// <summary>
	/// The Sender can be anything from the player to the console
	/// </summary>
	public abstract class Sender
	{
		/// <summary>
		/// Send this sender a message
		/// </summary>
		/// <param name="message">The message to send</param>
		public virtual void SendMessage(string message) 
		{
			Logger.Log(message);
		}
	}
}
