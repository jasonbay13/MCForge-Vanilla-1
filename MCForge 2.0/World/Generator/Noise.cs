/* Based on implementation of 3D Perlin Noise after Ken Perlin's reference implementation by Rene Schulte
 * Original Copyright (c) 2009 Rene Schulte
 * License.txt reproduced below

Microsoft Public License (Ms-PL)
[OSI Approved License]

This license governs use of the accompanying software. If you use the software, you
accept this license. If you do not accept the license, do not use the software.

1. Definitions
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the
same meaning here as under U.S. copyright law.
A "contribution" is the original software, or any additions or changes to the software.
A "contributor" is any person that distributes its contribution under this license.
"Licensed patents" are a contributor's patent claims that read directly on its contribution.

2. Grant of Rights
(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

3. Conditions and Limitations
(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.

 */

using System;

namespace MCForge.World {
    /// <summary>
    /// Implementation of 3D Perlin Noise after Ken Perlin's reference implementation.
    /// </summary>
    public class PerlinNoise {
        #region Fields

        private int[] permutation;
        private int[] p;

        #endregion

        #region Properties

        public float Frequency { get; set; }
        public float Amplitude { get; set; }
        public float Persistence { get; set; }
        public int Octaves { get; set; }

        #endregion

        #region Contructors

        public PerlinNoise() {
            permutation = new int[256];
            p = new int[permutation.Length * 2];
            InitNoiseFunctions();

            // Default values
            Frequency = 0.023f;
            Amplitude = 2.2f;
            Persistence = 0.9f;
            Octaves = 2;
        }

        #endregion

        #region Methods

        public void InitNoiseFunctions() {
            Random rand = new Random();

            // Fill empty
            for (int i = 0; i < permutation.Length; i++) {
                permutation[i] = -1;
            }

            // Generate random numbers
            for (int i = 0; i < permutation.Length; i++) {
                while (true) {
                    int iP = rand.Next() % permutation.Length;
                    if (permutation[iP] == -1) {
                        permutation[iP] = i;
                        break;
                    }
                }
            }

            // Copy
            for (int i = 0; i < permutation.Length; i++) {
                p[permutation.Length + i] = p[i] = permutation[i];
            }
        }

        public float Compute(float x, float y, float z = 0, bool useNewNoise = true) {
            float noise = 0;
            float amp = this.Amplitude;
            float freq = this.Frequency;
            for (float i = 0; i < this.Octaves; i++) {
                noise += useNewNoise ? (Noise(x * freq, y * freq, z * freq) * amp) : (NoiseUtils.InterpolatedNoise(x * freq, y * freq, new Random().Next()) * amp);
                freq *= 2;                                // octave is the float of the previous frequency
                amp *= this.Persistence;
            }

            // Clamp and return the result
            if (noise < 0) {
                return 0;
            }
            else if (noise > 1) {
                return 1;
            }
            return noise;
        }

        private float Noise(float x, float y, float z) {
            // Find unit cube that contains point
            int iX = (int)Math.Floor(x) & 255;
            int iY = (int)Math.Floor(y) & 255;
            int iZ = (int)Math.Floor(z) & 255;

            // Find relative x, y, z of the point in the cube.
            x -= (float)Math.Floor(x);
            y -= (float)Math.Floor(y);
            z -= (float)Math.Floor(z);

            // Compute fade curves for each of x, y, z
            float u = Fade(x);
            float v = Fade(y);
            float w = Fade(z);

            // Hash coordinates of the 8 cube corners
            int A = p[iX] + iY;
            int AA = p[A] + iZ;
            int AB = p[A + 1] + iZ;
            int B = p[iX + 1] + iY;
            int BA = p[B] + iZ;
            int BB = p[B + 1] + iZ;

            // And add blended results from 8 corners of cube.
            return Lerp(w, Lerp(v, Lerp(u, Grad(p[AA], x, y, z),
                               Grad(p[BA], x - 1, y, z)),
                       Lerp(u, Grad(p[AB], x, y - 1, z),
                               Grad(p[BB], x - 1, y - 1, z))),
               Lerp(v, Lerp(u, Grad(p[AA + 1], x, y, z - 1),
                               Grad(p[BA + 1], x - 1, y, z - 1)),
                       Lerp(u, Grad(p[AB + 1], x, y - 1, z - 1),
                               Grad(p[BB + 1], x - 1, y - 1, z - 1))));
        }

        private static float Fade(float t) {
            // Smooth interpolation parameter
            return (t * t * t * (t * (t * 6 - 15) + 10));
        }

        private static float Lerp(float alpha, float a, float b) {
            // Linear interpolation
            return (a + alpha * (b - a));
        }

        private static float Grad(int hashCode, float x, float y, float z) {
            // Convert lower 4 bits of hash code into 12 gradient directions
            int h = hashCode & 15;
            float u = h < 8 ? x : y;
            float v = h < 4 ? y : h == 12 || h == 14 ? x : z;
            return (((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v));
        }

        #endregion
    }

    /// <summary>
    /// 6
    /// </summary>
    public class NoiseUtils {

        public static float SmoothNoise(int x, int y, long seed = 0) {
            float corners = (Noise2D(x - 1, y - 1, seed) +
                             Noise2D(x + 1, y - 1, seed) +
                             Noise2D(x - 1, y + 1, seed) +
                             Noise2D(x + 1, y + 1, seed)) / 16;

            float sides = Noise2D(x - 1, y, seed) +
                          Noise2D(x + 1, y, seed) +
                          Noise2D(x, y - 1, seed) +
                          Noise2D(x, y + 1, seed) / 8;

            float center = Noise2D(x, y, seed) / 4;
            return corners + sides + center;
        }

        public static float Noise2D(int x, int y, long seed = 0) {
            long n = x + y * 57 + seed;
            n = (n << 13) ^ n;
            return (float)(1.0 - ((n * (n * n * 15731 + 789221) + 0x5208dd0d) & int.MaxValue) / 1073741824.0);
        }

        public static float Interpolate(float a, float b, float x) {
            float ft = x * (float)Math.PI;
            float f = (float)(1 - Math.Cos(ft)) / 2;

            return a * (1 - f) + b * f;
        }

        public static float InterpolatedNoise(float x, float y, long seed = 0) {
            int wholePartX = (int)x;
            float fractionPartX = x - wholePartX;

            int wholePartY = (int)y;
            float fractionPartY = y - wholePartY;

            float v1 = SmoothNoise(wholePartX, wholePartY, seed);
            float v2 = SmoothNoise(wholePartX + 1, wholePartY, seed);
            float v3 = SmoothNoise(wholePartX, wholePartY + 1, seed);
            float v4 = SmoothNoise(wholePartX + 1, wholePartY + 1, seed);

            float i1 = Interpolate(v1, v2, fractionPartX);
            float i2 = Interpolate(v3, v4, fractionPartX);

            return Interpolate(i1, i2, fractionPartY);
        }

        public static void Normalize(float[,] map) {
            if (map == null)
                throw new NullReferenceException("Map array is null");
            float min = float.MaxValue,
                  max = float.MinValue;
            unsafe {
                fixed (float* stright = map) {
                    for (int i = 0; i < map.Length; i++) {
                        min = Math.Min(min, stright[i]);
                        max = Math.Max(max, stright[i]);
                    }
                    float diff = max - min;
                    float av = 1;
                    float flip = av / diff;
                    float shift = -min * av / diff;

                    for (int i = 0; i < map.Length; i++) {
                        float amt = (stright[i] * flip) + shift;
                        stright[i] = amt;
                    }
                }
            }
        }


        public static float NegateEdge(float x, float y, float width, float height) {
            float tempx = 0.0f, tempy = 0.0f;
            float temp;
            if (x != 0) { tempx = (x / width) * 0.5f; }
            if (y != 0) { tempy = (y / height) * 0.5f; }
            tempx = Math.Abs(tempx - 0.25f);
            tempy = Math.Abs(tempy - 0.25f);
            if (tempx > tempy) {
                temp = tempx - 0.15f;
            }
            else {
                temp = tempy - 0.15f;
            }
            if (temp > 0.0f) { return temp; }
            return 0.0f;
        }

        public static float Range(float input, float low, float high) {
            if (high <= low) { return low; }
            return low + (input * (high - low));
        }

        public static float lerp(float x, float x1, float x2, float q00, float q01) {
            return ((x2 - x) / (x2 - x1)) * q00 + ((x - x1) / (x2 - x1)) * q01;
        }

        public static float biLerp(float x, float y, float q11, float q12, float q21, float q22, float x1, float x2, float y1, float y2) {
            float r1 = lerp(x, x1, x2, q11, q21);
            float r2 = lerp(x, x1, x2, q12, q22);

            return lerp(y, y1, y2, r1, r2);
        }

        public static float triLerp(float x, float y, float z, float q000, float q001, float q010, float q011, float q100, float q101, float q110, float q111, float x1, float x2, float y1, float y2, float z1, float z2) {
            float x00 = lerp(x, x1, x2, q000, q100);
            float x10 = lerp(x, x1, x2, q010, q110);
            float x01 = lerp(x, x1, x2, q001, q101);
            float x11 = lerp(x, x1, x2, q011, q111);
            float r0 = lerp(y, y1, y2, x00, x01);
            float r1 = lerp(y, y1, y2, x10, x11);

            return lerp(z, z1, z2, r0, r1);
        }

        public static float GetAverage5(float[,] map, float x, float y) {
            if (IsOnBounds(map, x - 1, y - 1))
                return 0f;

            if (IsOnBounds(map, x + 1, y + 1))
                return 0f;

            return
                (map[(int)x, (int)y] +
                 map[(int)x + 1, (int)y] +
                 map[(int)x - 1, (int)y] +
                 map[(int)x, (int)y + 1] +
                 map[(int)x, (int)y - 1]) / 5;
        }
    	
        public static float GetAverage9(float[,] map, float x, float y) {
            if (IsOnBounds(map, x - 1, y - 1))
                return 0f;

            if (IsOnBounds(map, x + 1, y + 1))
                return 0f;

            return (
              map[(int)x, (int)y] +
              map[(int)x + 1, (int)y] +
              map[(int)x - 1, (int)y] +
              map[(int)x, (int)y + 1] +
              map[(int)x, (int)y - 1] +
              map[(int)x + 1, (int)y + 1] +
              map[(int)x - 1, (int)y + 1] +
              map[(int)x + 1, (int)y - 1] +
              map[(int)x - 1, (int)y - 1]) / 9;
        }

        /// <summary>
        /// Determines whether the location [is on bounds] in [the specified big map].
        /// </summary>
        /// <param name="bigMap">The big map.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        ///   <c>true</c> if [is on bounds] [the specified big map]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOnBounds(double[,] bigMap, double x, double y) {
            return x <= 0 ||
                   y <= 0 ||
                   x >= bigMap.GetLength(0) ||
                   y >= bigMap.GetLength(1);
        }
    	
    	/// <summary>
    	/// Is the X and Y in the bounds of the map bigMap
    	/// </summary>
    	/// <param name="bigMap">The map to check</param>
    	/// <param name="x">The X cord.</param>
    	/// <param name="y">The Y cord.</param>
    	/// <returns>If true, then the position is out of bounds of the map. Otherwise it is not</returns>
        public static bool IsOnBounds(float[,] bigMap, float x, float y) {
            return x <= 0 ||
                   y <= 0 ||
                   x >= bigMap.GetLength(0) ||
                   y >= bigMap.GetLength(1);
        }

    }
}
