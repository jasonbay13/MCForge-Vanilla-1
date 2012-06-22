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
using MCForge.Utils;
using MCForge.World;

namespace MCForge.Interfaces.Blocks {
    public interface IBlock {
        //TODO: Add OnPlayerIsInBlock(..) for portals
        /// <summary>
        /// The name of this block. Used to create from commands.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Returns the display block type for a specific location
        /// </summary>
        /// <param name="blockPosition">The block position</param>
        /// <param name="level">The level</param>
        /// <returns>The display block type</returns>
        byte GetDisplayType(Vector3S blockPosition, Level level);
        /// <summary>
        /// Gets called when a player steps on this block.
        /// </summary>
        /// <param name="p">The player steps on this block</param>
        /// <param name="blockPosition">The position of the block</param>
        /// <param name="level">The level</param>
        void OnPlayerStepsOn(Player p, Vector3S blockPosition, Level level);
        /// <summary>
        /// Gets called when a player creates or destroys this block, return true to prevent the block from being replaced by players
        /// </summary>
        /// <param name="p">The player destroying the block</param>
        /// <param name="blockPosition">The position of the block</param>
        /// <param name="holding">The block holding</param>
        /// <param name="level">The level</param>
        /// <returns>True to prevent the block from being replaced by players</returns>
        bool OnAction(Player p, Vector3S blockPosition, byte holding, Level level);
        /// <summary>
        /// Gets called when physics should be updated.
        /// </summary>
        /// <param name="blockPositions">All block positions of this block type</param>
        /// <param name="level">The level</param>
        void PhysicsTick(Vector3S[] blockPositions, Level level);
        /// <summary>
        /// Initializes the block type
        /// </summary>
        void Initialize();
        /// <summary>
        /// Gets called when the block type is unloaded.
        /// </summary>
        void OnUnload();
        /// <summary>
        /// Gets called when a block is removed
        /// </summary>
        /// <param name="blockPosition">The position of the block</param>
        /// <param name="level">the level</param>
        void OnRemove(Vector3S blockPosition, Level level);
    }
}
