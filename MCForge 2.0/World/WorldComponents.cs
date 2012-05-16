using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Core;
using MCForge.Utils;

namespace MCForge.World {
    public class WorldComponent {

        private BlockComponent[] components;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldComponent"/> class.
        /// </summary>
        /// <param name="xyz">The pieces of the component.</param>
        public WorldComponent(params BlockComponent[] components) {
            this.components = components;
        }

        /// <summary>
        /// Draws the on level.
        /// </summary>
        /// <param name="mLevel">The level to draw on.</param>
        /// <param name="startPos">The start pos.</param>
        /// <param name="blockOverlay">A block to set the component as. If it is not already set</param>
        /// <remarks>if the block in the component is set to 255 a block overlay will be used automaticly, no block change will occur if the block is at 254</remarks>
        public void DrawOnLevel(Level mLevel, Vector3S startPos, byte blockOverlay = 1) {
            for (int i = 0; i < components.Length; i++) {
                var comp = components[i];
                if (comp.Block == 254)
                    continue;
                mLevel.SetBlock(comp.Pos + startPos, comp.Block == 255 ? blockOverlay : comp.Block);
            }
        }

        /// <summary>
        /// A static component for the letter 'A' (capitolized)
        /// </summary>
        public static readonly WorldComponent LetterA = new WorldComponent(
            new BlockComponent(255, new Vector3S(1, 0, 0)), new BlockComponent(255, new Vector3S(2, 0, 0)),
            new BlockComponent(255, new Vector3S(0, 0, 1)), new BlockComponent(255, new Vector3S(3, 0, 1)),
            new BlockComponent(255, new Vector3S(0, 0, 2)), new BlockComponent(255, new Vector3S(1, 0, 2)), new BlockComponent(255, new Vector3S(2, 0, 2)), new BlockComponent(255, new Vector3S(3, 0, 2)),
            new BlockComponent(255, new Vector3S(0, 0, 3)), new BlockComponent(255, new Vector3S(3, 0, 3)),
            new BlockComponent(255, new Vector3S(0, 0, 4)), new BlockComponent(255, new Vector3S(3, 0, 4)), 
            new BlockComponent(255, new Vector3S(0, 0, 5)), new BlockComponent(255, new Vector3S(3, 0, 5))
       );

    }



    /// <summary>
    /// A struct containting a Block and position
    /// </summary>
    public struct BlockComponent {
        /// <summary>
        /// The type of block
        /// </summary>
        public byte Block;
        /// <summary>
        /// The position
        /// </summary>
        public Vector3S Pos;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockComponent"/> struct.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="mVec">The position.</param>
       public BlockComponent(byte block, Vector3S mVec){
            this.Block = block;
            this.Pos = mVec;
        }
        
    }
}
