using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Robot;
using MCForge.Core;
using MCForge.Utils;

namespace MCForge.API.Events
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
