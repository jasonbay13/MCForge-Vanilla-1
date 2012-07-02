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

namespace MCForge.API.Events {
    /// <summary>
    /// PlayerBlockChange event class
    /// </summary>
    public class BlockChangeEvent : Event<Player, BlockChangeEventArgs> {
    }
    /// <summary>
    /// PlayerBlockChangeEventArgs
    /// </summary>
    public class BlockChangeEventArgs : EventArgs, ICancelable, ICloneable, IEquatable<BlockChangeEventArgs> {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="x">The positions X coordinate</param>
        /// <param name="y">The positions Y coordinate</param>
        /// <param name="z">The positions Z coordinate</param>
        /// <param name="action">The ActionType action</param>
        /// <param name="holding">The type of the block</param>
        public BlockChangeEventArgs(ushort x, ushort z, ushort y, ActionType action, byte holding, byte current) : this(action, holding, current, x, z, y) { }
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="x">The positions X coordinate</param>
        /// <param name="y">The positions Y coordinate</param>
        /// <param name="z">The positions Z coordinate</param>
        /// <param name="action">The ActionType action</param>
        /// <param name="holding">The type of the block</param>
        public BlockChangeEventArgs(ActionType action, byte holding, byte current, ushort x, ushort z, ushort y) {
            this.Action = action;
            this.Holding = holding;
            this.Current = current;
            this.X = x;
            this.Z = z;
            this.Y = y;
        }
        /// <summary>
        /// What we are doing with this block.
        /// </summary>
        public ActionType Action { get; set; }
        /// <summary>
        /// The block hold during action.
        /// </summary>
        public byte Holding { get; set; }
        /// <summary>
        /// The current block at the location.
        /// </summary>
        public byte Current { get; set; }
        /// <summary>
        /// The x coordinate of the block changed.
        /// </summary>
        public ushort X { get; set; }
        /// <summary>
        /// The y coordinate of the block changed.
        /// </summary>
        public ushort Y { get; set; }
        /// <summary>
        /// The z coordinate of the block changed.
        /// </summary>
        public ushort Z { get; set; }
        private bool canceled = false;
        /// <summary>
        /// Whether or not the handling should be canceled
        /// </summary>
        public bool Canceled {
            get { return canceled; }
        }
        /// <summary>
        /// Cancels the handling
        /// </summary>
        public void Cancel() {
            canceled = true;
        }
        /// <summary>
        /// Allows the handling
        /// </summary>
        public void Allow() {
            canceled = false;
        }
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <returns>A new instance</returns>
        public object Clone() {
            return new BlockChangeEventArgs(X, Z, Y, Action, Holding, Current);
        }
        /// <summary>
        /// Compares equality (ICancelable and IStoppable are not part of the comparison)
        /// </summary>
        /// <param name="other">The value to be compared to.</param>
        /// <returns>Whether they are equal or not.</returns>
        public bool Equals(BlockChangeEventArgs other) {
            return X == other.X && Z == other.Z && Y == other.Y && Action == other.Action && Holding == other.Holding && Current == other.Current;
        }
    }
    public enum ActionType : byte {
        Delete,
        Place
    }

}