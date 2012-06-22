/*
Copyright 2012 MCForge
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
using MCForge.Robot;
using MCForge.Core;
using MCForge.Utils;

namespace MCForge.API.Events.Robot
{
    public class TargetPlayerEvent : Event<Bot, TargetPlayerArgs>
    {
    }
    /// <summary>
    /// PlayerMoveEventArgs
    /// </summary>
    public class TargetPlayerArgs : EventArgs, ICancelable
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="fromPosition">The position where the move started</param>
        public TargetPlayerArgs(Player player)
        {
            this.Player = player;
        }
        /// <summary>
        /// The position where the move started
        /// </summary>
        public Player Player { get; private set; }
        private bool canceled = false;
        /// <summary>
        /// Whether or not the handling should be canceled
        /// </summary>
        public bool Canceled
        {
            get { return canceled; }
        }
        /// <summary>
        /// Cancels the handling
        /// </summary>
        public void Cancel()
        {
            canceled = true;
        }
        /// <summary>
        /// Allows the handling
        /// </summary>
        public void Allow()
        {
            canceled = false;
        }
    }
}
