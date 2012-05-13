
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Core;

namespace MCForge.World.Generator {

    //TODO: finish documentation

    /// <summary>
    /// 
    /// </summary>
    public sealed class LevelGenerator {
        #region Variables
        private GeneratorArgs GenArgs;
        private Level Level;
        private PerlinNoise NoiseGenerator;
        private float[,] map, overlay, plants;
        private Random random;


        /// <summary>
        /// Event handler for recieving progress updates
        /// </summary>
        public static EventHandler<GenerationEventArgs> OnProgressArgs;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelGenerator"/> class.
        /// </summary>
        /// <param name="level">The level to generate.</param>
        public LevelGenerator(Level level) {

            if (level.Size.x != level.Size.z)
                throw new ArgumentException("The x and z must be the same to use advanced level generation");

            //Create a standard mountain level
            GenArgs = new GeneratorArgs() {
                MaxLevelGenerationHeight = level.Size.y / 2,
                MinLevelGenerationHeight = 0,
                TopLayer = Block.BlockList.GLASS,
                BottomLayer = Block.BlockList.DIRT,
                OverlayLayer = Block.BlockList.WHITE_CLOTH,
                Seed = new Random().Next(),
            };
            Level = level;
            random = new Random((int)GenArgs.Seed);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LevelGenerator"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="args">The Generator arguments.</param>
        public LevelGenerator(Level level, GeneratorArgs args) {

            if (level.Size.x != level.Size.z)
                throw new ArgumentException("The x and z must be the same to use advanced level generation");

            this.Level = level;
            this.GenArgs = args;

            if (args.MaxLevelGenerationHeight == -1)
                args.MaxLevelGenerationHeight = level.Size.y;

            if (args.MinDepth == -1)
                args.MaxLevelGenerationHeight = level.Size.y;

            random = new Random((int)args.Seed);
        }



        #endregion

        #region Generation

        /// <summary>
        /// Generates the map.
        /// </summary>
        public void Generate() {
            if (Level == null)
                throw new NullReferenceException("Level to generate was null");

            if (OnProgressArgs != null)
                OnProgressArgs(this, new GenerationEventArgs("Creating Dimentions...", 10));

            NoiseGenerator = new PerlinNoise() {
                Amplitude = GenArgs.Amplitude,
                Frequency = GenArgs.WaveFrequency,
                Octaves = GenArgs.MoutainOctaves,
                Persistence = GenArgs.Persistence
            };

            NoiseGenerator.InitNoiseFunctions();
            map = new float[Level.Size.x, Level.Size.z];
            overlay = new float[Level.Size.x, Level.Size.z];

            if (GenArgs.PlantMushrooms || GenArgs.PlantFlowers || GenArgs.PlantSaplings)
                plants = new float[Level.Size.x, Level.Size.z];


            if (OnProgressArgs != null)
                OnProgressArgs(this, new GenerationEventArgs("Building...", 20));
            Generate3DTerrain();

            if (OnProgressArgs != null)
                OnProgressArgs(this, new GenerationEventArgs("Raking...", 10));
            ApplyFilter();

            if (OnProgressArgs != null)
                OnProgressArgs(this, new GenerationEventArgs("Creating Moutains...", 20));
            GenerateNoise();


            if (OnProgressArgs != null)
                OnProgressArgs(this, new GenerationEventArgs("Cleaning Moutains..", 10));
            NoiseUtils.Normalize(map);

            if (OnProgressArgs != null)
                OnProgressArgs(this, new GenerationEventArgs("Setting blocks...", 20));
            SetBlocks();

            if (OnProgressArgs != null)
                OnProgressArgs(this, new GenerationEventArgs("Planting..", 20));
            // TODO: SetPlants();

            if (OnProgressArgs != null)
                OnProgressArgs(this, new GenerationEventArgs("Growing...", 20));
            // TODO: GrowPlants();

            if (OnProgressArgs != null)
                OnProgressArgs(this, new GenerationEventArgs("Created map " + Level.Name, 10));
        }
        #endregion

        #region Helper methods

        /// <summary>
        /// Fills the X layer.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <param name="block">The block.</param>
        public void FillX(int y, int z, Block block) {
            for (int x = 0; x < Level.Size.x; x++)
                Level.SetBlock(x, z, y, block);
        }

        /// <summary>
        /// Fills the Y layer.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="z">The z.</param>
        /// <param name="block">The block.</param>
        public void FillY(int x, int z, Block block) {
            for (int y = 0; y < Level.Size.y; y++)
                Level.SetBlock(x, z, y, block);
        }



        /// <summary>
        /// Fills the Z layer.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="block">The block.</param>
        public void FillZ(int x, int y, Block block) {
            for (int z = 0; z < Level.Size.z; z++)
                Level.SetBlock(x, z, y, block);
        }


        /// <summary>
        /// Fills the plane XY.
        /// </summary>
        /// <param name="z">The z.</param>
        /// <param name="block">The block.</param>
        public void FillPlaneXY(int z, Block block) {
            for (int x = 0; x < Level.Size.x; x++)
                for (int y = 0; y < Level.Size.y; y++)
                    Level.SetBlock(x, z, y, block);
        }


        /// <summary>
        /// Fills the plane XZ.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <param name="block">The block.</param>
        public void FillPlaneXZ(int y, Block block) {
            for (int x = 0; x < Level.Size.x; x++)
                for (int z = 0; z < Level.Size.z; z++)
                    Level.SetBlock(x, z, y, block);
        }

        /// <summary>
        /// Fills the plane ZY.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="block">The block.</param>
        public void FillPlaneZY(int x, Block block) {
            for (int z = 0; z < Level.Size.z; z++)
                for (int y = 0; y < Level.Size.y; y++)
                    Level.SetBlock(x, z, y, block);
        }
        /// <summary>
        /// Returns if the specified location is on the border of the map
        /// </summary>
        /// <param name="x">Location of the block on the x axis</param>
        /// <param name="z">Location of the block on the z axis</param>
        /// <param name="y">Location of the block on the y axis</param>
        /// <returns>Returns if the specified location is on the border of the map</returns>
        public bool IsOnEdges(int x, int y, int z) {
            return (x == 0 || x == Level.Size.x - 1 ||
                    z == 0 || z == Level.Size.z - 1 ||
                    y == 0 || y == Level.Size.y);
        }

        /// <summary>
        /// Generates a blocky non smooth version of a terrain. To be smoothed later in other methods
        /// </summary>
        public void GenerateNoise() {
            for (int z = 0; z < Level.Size.z; z++)
                for (int x = 0; x < Level.Size.x; x++)
                    overlay[x, z] = NoiseGenerator.Compute(x, z, x, true);

        }

        /// <summary>
        /// Applies the filter.
        /// </summary>
        public void ApplyFilter() {
            for (int z = 0; z < Level.Size.z; z++)
                for (int x = 0; x < Level.Size.x; x++) {
                    map[x, z] = NoiseUtils.GetAverage9(map, x, z);
                }
        }

        /// <summary>
        /// Sets the blocks.
        /// </summary>
        public void SetBlocks() {
            DateTime curr = DateTime.Now;
            for (int i = 0; i < overlay.Length - 1; i++) {
                int x = i % (Level.Size.x);
                int z = i / (Level.Size.x);
                int y = (int)NoiseUtils.Range(overlay[x, z],
                                             GenArgs.MinLevelGenerationHeight - NoiseUtils.NegateEdge(x, z, Level.Size.x, Level.Size.z),
                                             GenArgs.MaxLevelGenerationHeight - NoiseUtils.NegateEdge(x, z, Level.Size.x, Level.Size.z));

                if (y >= GenArgs.MaxLevelGenerationHeight / 2)
                    Level.SetBlock(x, z, y + 1, GenArgs.OverlayLayer);


                Level.SetBlock(x, z, y, GenArgs.TopLayer);

                for(int toGround = y - 1; toGround >= GenArgs.MinLevelGenerationHeight; toGround--)
                    Level.SetBlock(x, z, toGround, GenArgs.BottomLayer);

            }
            // Logger.Log((curr.Millisecond - DateTime.Now.Millisecond) + " = total time");

        }

        /// <summary>
        /// Transforms the 2D height map into 3D
        /// </summary>
        public void Generate3DTerrain() {

            if (Level.Size.x != Level.Size.z)
                throw new Exception("Width and length must be equal to generate 3D terrain");


            var Ran = new Random();

            int maxHeight = GenArgs.MaxLevelGenerationHeight,
                minHeight = GenArgs.MinLevelGenerationHeight,
                currHeight = minHeight;

            int halfX = Level.Size.x / 2,
               halfY = Level.Size.y / 2,
               halfZ = Level.Size.z / 2,
               count = Level.Size.x + Level.Size.z;

            float displace = .02f,
                  gap = maxHeight;


            for (int i = 0; i < count; i++) {

                float rand = (float)(Ran.NextDouble() * 360),
                      sRand = (float)Math.Sin(rand),
                      cRand = (float)Math.Cos(rand),
                      root = (float)(halfX * halfX + halfZ * halfZ),
                      sk = (float)(Ran.NextDouble() * 2 * root - root);

                for (int z = 0; z < Level.Size.z; z++)
                    for (int x = 0; x < Level.Size.x; x++) {

                        if ((i - halfZ) * cRand + (x - halfX) * sRand + sk > 0)
                            map[x, z] += displace;
                        else
                            map[x, z] += -displace;

                        if (map[x, z] > 1f)
                            map[x, z] = 1f;

                        if (map[x, z] < 0f)
                            map[x, z] = 0f;


                    }
                displace -= .0025f;
                if (displace < minHeight)
                    displace = minHeight;
            }
        }
        #endregion

        #region Extra

        /// <summary>
        /// Generate a tree at any location
        /// </summary>
        /// <param name="type">Type of tree to generate</param>
        /// <param name="pos">Location to generate the tree</param>
        public void GenerateTree(TreeType type, Vector3 pos) {
            switch (type) {
                case TreeType.Big:
                case TreeType.Fat:
                case TreeType.NotchBig:
                case TreeType.NotchSmall:
                case TreeType.Small:
                case TreeType.Thin:
                    break;
            }
        }


        /// <summary>
        /// Sets the position of the spawn point.
        /// </summary>
        public void SetPosition() {
            //TODO: Estimate the best position to place the player
            SetPosition(new Vector3(), new Vector2());
        }


        /// <summary>
        /// Sets the position of the spawn point.
        /// </summary>
        /// <param name="manualPosition">The position.</param>
        /// <param name="angleRot">The angle rotation.</param>
        public void SetPosition(Vector3 manualPosition, Vector2 angleRot) {
            Level.SpawnPos = manualPosition;
            Level.SpawnRot = new byte[2] { (byte)angleRot.x, (byte)angleRot.z };
        }
        #endregion

        #region InnerClasses

        /// <summary>
        /// Generation Event args to track the status of the current generation job
        /// </summary>
        public class GenerationEventArgs : EventArgs {

            /// <summary>
            /// Message with in the event args
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Increment to increase by
            /// </summary>
            public byte Increment { get; set; }

            /// <summary>
            /// Constructor for a generator event argument
            /// </summary>
            /// <param name="message">Message to give</param>
            /// <param name="increment">Value to increase by</param>
            public GenerationEventArgs(string message, byte increment) {
                Message = message;
                Increment = increment;
            }

        }

        /// <summary>
        /// Enum containing the types of trees that can be generated
        /// </summary>
        public enum TreeType {
            /// <summary>
            /// MCForge's version of a big tree
            /// </summary>
            Big,

            /// <summary>
            /// MCForge's version of a small tree
            /// </summary>
            Small,

            /// <summary>
            /// MCForge's verion of a thin tree
            /// </summary>
            Thin,

            /// <summary>
            /// MCForge's version of a fat tree
            /// </summary>
            Fat,

            /// <summary>
            /// Notch's version of a big tree
            /// </summary>
            NotchBig,

            /// <summary>
            /// Notch's version of a small tree
            /// </summary>
            NotchSmall,

        }


        #endregion

    }
}
