using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Core;

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
        public void DrawOnLevel(Level mLevel, Vector3 startPos, byte blockOverlay = 1) {
            for (int i = 0; i < components.Length; i++) {
                var comp = components[i];
                if (comp.block == 254)
                    continue;
                mLevel.SetBlock(comp.pos + startPos, comp.block == 255 ? blockOverlay : comp.block);
            }
        }

        /// <summary>
        /// A static component for the letter 'A' (capitolized)
        /// </summary>
        public static readonly WorldComponent LetterA = new WorldComponent(
            new BlockComponent(255, new Vector3(1, 0, 0)), new BlockComponent(255, new Vector3(2, 0, 0)),
            new BlockComponent(255, new Vector3(0, 0, 1)), new BlockComponent(255, new Vector3(3, 0, 1)),
            new BlockComponent(255, new Vector3(0, 0, 2)), new BlockComponent(255, new Vector3(1, 0, 2)), new BlockComponent(255, new Vector3(2, 0, 2)), new BlockComponent(255, new Vector3(3, 0, 2)),
            new BlockComponent(255, new Vector3(0, 0, 3)), new BlockComponent(255, new Vector3(3, 0, 3)),
            new BlockComponent(255, new Vector3(0, 0, 4)), new BlockComponent(255, new Vector3(3, 0, 4)), 
            new BlockComponent(255, new Vector3(0, 0, 5)), new BlockComponent(255, new Vector3(3, 0, 5))
       );

    }

    

    internal struct BlockComponent {
        internal byte block = 0;
        internal Vector3 pos = default(Vector3);

        internal BlockComponent(byte block, Vector3 mVec){
            this.block = block;
            this.pos = mVec;
        }
        
    }
}
