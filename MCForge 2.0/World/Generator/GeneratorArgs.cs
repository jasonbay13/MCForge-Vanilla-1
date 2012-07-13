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
        public double MinLevelGenerationHeight { get; set; }

        /// <summary>
        /// Gets or sets the max height of the level generation.
        /// </summary>
        /// <value>
        /// The height of the max level generation.
        /// </value>
        public double MaxLevelGenerationHeight { get; set; }

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
        public double LiquidLine { get; set; }

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


        public byte FlowingLiquid { get; set; }
    }
}
