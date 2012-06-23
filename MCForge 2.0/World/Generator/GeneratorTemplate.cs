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
