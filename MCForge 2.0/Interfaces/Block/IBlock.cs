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
        /// Gets called when a player steps in this block.
        /// </summary>
        /// <param name="p">The player steps in this block</param>
        /// <param name="blockPosition">The position of the block</param>
        /// <param name="level">The level</param>
        void OnPlayerStepsIn(Player p, Vector3S blockPosition, Level level);

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
        /// Gets called when a block is removed. (not necessary th  rough players action)
        /// </summary>
        /// <param name="blockPosition">The position of the block</param>
        /// <param name="level">The level</param>
        void OnRemove(Vector3S blockPosition, Level level);

        /// <summary>
        /// Gets called when a block of this type gets placed somewhere,
        /// </summary>
        /// <param name="blockPosition">The position of the block.</param>
        /// <param name="level">The level</param>
        /// <param name="type">The block types overgiven as $block parameters in block name.</param>
        void OnCreate(Vector3S blockPosition, Level level, params byte[] type);

        /* Introduction
         *
         * The blocks are stored inside the levels extradata, so everthing can be saved as part 
         * of the level. To get even the state stored inside the level file, we need to add them 
         * to the levels extradata as well. Make sure you use a type that os convertible to and 
         * from a string.
         * 
         * The OnCreate methods byte[] type parameter are parsed out of the block name the user 
         * passes to a command. For readability the underline sign ('_') is ingored if it is in
         * front of a dollar sign ('$'). The text after the dollar sign is parsed up to the next 
         * dollar sign or the end of the string and is only allowed to contain names of normal
         * block types others will be ignored.
         */
    }
}
