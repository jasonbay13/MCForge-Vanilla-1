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
        /// <param name="ticks">The elapsed milliseconds since last tick.</param>
        void PhysicsTick(int millisecodns);
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
