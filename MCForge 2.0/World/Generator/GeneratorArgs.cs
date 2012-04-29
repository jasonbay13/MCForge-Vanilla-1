using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Generator {
    /// <summary>
    /// 
    /// </summary>
    public sealed class GeneratorArgs {

        public int MinLevelGenerationHeight { get; set; }
        public int MaxLevelGenerationHeight { get; set; }

        public long Seed { get; set;}

        public int MaxDepth { get; set; }
        public int MinDepth { get; set; }

        public byte OverlayLayer { get; set; }
        public byte TopLayer { get; set; }
        public byte BottomLayer { get; set; }

        public byte LiquidBlock { get; set; }
        public int LiquidLine { get; set; }

        public bool UseNewNoise { get; set; }

        public bool PlantTrees { get; set; }
        public bool PlantSaplings { get; set; }
        public bool PlantFlowers { get; set; }
        public bool PlantMushrooms { get; set; }
    }
}
