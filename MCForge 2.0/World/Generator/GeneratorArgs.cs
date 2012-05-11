
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Generator {
    /// <summary>
    /// Level Generation Arguments
    /// </summary>
    public sealed class GeneratorArgs {

        /// <summary>
        /// Gets or sets the min height of the level generation.
        /// </summary>
        /// <value>
        /// The height of the min level generation.
        /// </value>
        public int MinLevelGenerationHeight { get; set; }

        /// <summary>
        /// Gets or sets the max height of the level generation.
        /// </summary>
        /// <value>
        /// The height of the max level generation.
        /// </value>
        public int MaxLevelGenerationHeight { get; set; }

        /// <summary>
        /// Gets or sets the seed.
        /// </summary>
        /// <value>
        /// The seed.
        /// </value>
        public long Seed { get; set;}

        /// <summary>
        /// Gets or sets the max depth.
        /// </summary>
        /// <value>
        /// The max depth.
        /// </value>
        public int MaxDepth { get; set; }

        /// <summary>
        /// Gets or sets the min depth.
        /// </summary>
        /// <value>
        /// The min depth.
        /// </value>
        public int MinDepth { get; set; }


        /// <summary>
        /// Gets or sets the block overlay layer.
        /// </summary>
        /// <value>
        /// The overlay layer.
        /// </value>
        public byte OverlayLayer { get; set; }

        /// <summary>
        /// Gets or sets the top layer of block.
        /// </summary>
        /// <value>
        /// The top layer.
        /// </value>
        public byte TopLayer { get; set; }


        /// <summary>
        /// Gets or sets the bottom layer.
        /// </summary>
        /// <value>
        /// The bottom layer.
        /// </value>
        public byte BottomLayer { get; set; }

        /// <summary>
        /// Gets or sets the liquid block.
        /// </summary>
        /// <value>
        /// The liquid block.
        /// </value>
        public byte LiquidBlock { get; set; }

        /// <summary>
        /// Gets or sets the liquid line.
        /// </summary>
        /// <value>
        /// The liquid line.
        /// </value>
        public int LiquidLine { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use new noise].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use new noise]; otherwise, <c>false</c>.
        /// </value>
        public bool UseNewNoise { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to [plant trees].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [plant trees]; otherwise, <c>false</c>.
        /// </value>
        public bool PlantTrees { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [plant saplings].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [plant saplings]; otherwise, <c>false</c>.
        /// </value>
        public bool PlantSaplings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [plant flowers].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [plant flowers]; otherwise, <c>false</c>.
        /// </value>
        public bool PlantFlowers { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [plant mushrooms].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [plant mushrooms]; otherwise, <c>false</c>.
        /// </value>
        public bool PlantMushrooms { get; set; }

        /// <summary>
        /// Gets or sets the amplitude.
        /// </summary>
        /// <value>
        /// The amplitude.
        /// </value>
        public float Amplitude { get; set; }

        /// <summary>
        /// Gets or sets the wave frequency.
        /// </summary>
        /// <value>
        /// The wave frequency.
        /// </value>
        public float WaveFrequency { get; set; }

        /// <summary>
        /// Gets or sets the moutain octaves.
        /// </summary>
        /// <value>
        /// The moutain octaves.
        /// </value>
        public int MoutainOctaves { get; set; }

        /// <summary>
        /// Gets or sets the persistence.
        /// </summary>
        /// <value>
        /// The persistence.
        /// </value>
        public float Persistence { get; set; }

        /// <summary>
        /// A constant for the max size of a map (to be used when the size is unknown)
        /// </summary>
        public const int MAX_SIZE = -1;
    }
}
