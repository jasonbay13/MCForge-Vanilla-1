using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Generator {
    /// <summary>
    /// Templates for <see cref="GeneratorArgs"/>
    /// </summary>
    public static class GeneratorTemplate {

        /// <summary>
        /// Hell-ish type level
        /// </summary>
        public static GeneratorArgs Hell(Level level) {
            return new GeneratorArgs() {
                MaxLevelGenerationHeight = level.Size.y,
                MinLevelGenerationHeight = 0,
                MaxDepth = 2,
                MinDepth = 7,
                TopLayer = Block.BlockList.STONE,
                BottomLayer = Block.BlockList.COBBLESTONE,
                OverlayLayer = Block.BlockList.OBSIDIAN,
                FlowingLiquid = Block.BlockList.LAVA,
                LiquidBlock = Block.BlockList.LAVA,
                LiquidLine = 5,
                Seed = new Random().Next(),
                Amplitude = 1,
                MoutainOctaves = 1,
                Persistence = 2.1f,
                WaveFrequency = .03f,
                UseNewNoise = true
            };
        }

    }
}
