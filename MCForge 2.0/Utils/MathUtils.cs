using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Core;

namespace MCForge.Utils {

    /// <summary>
    /// A class full of static methods to help assist common 3D and 2D math algorithms
    /// </summary>
    public class MathUtils {

        private static Random _random;
        public static Random Random {
            get {
                if (_random == null)
                    _random = new Random(DateTime.Now.Millisecond);
                return _random;
            }
        }

        #region Abs Vector

        /// <summary>
        /// Gets a vector where every indices has been Math.Absoluted.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A absoluted Vector</returns>
        public static Vector3S AbsVector(Vector3S Vector) {
            return new Vector3S(Math.Abs(Vector.x),
                                Math.Abs(Vector.z),
                                Math.Abs(Vector.y));
        }

        /// <summary>
        /// Gets a vector where every indices has been Math.Absoluted.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A absoluted Vector</returns>
        public static Vector3D AbsVector(Vector3D Vector) {
            return new Vector3D(Math.Abs(Vector.x),
                                Math.Abs(Vector.z),
                                Math.Abs(Vector.y));
        }

        /// <summary>
        /// Gets a vector where every indices has been Math.Absoluted.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A absoluted Vector</returns>
        public static Vector2S AbsVector(Vector2S Vector) {
            return new Vector2S(Math.Abs(Vector.x),
                                Math.Abs(Vector.z));
        }

        /// <summary>
        /// Gets a vector where every indices has been Math.Absoluted.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A absoluted Vector</returns>
        public static Vector2D AbsVector(Vector2D Vector) {
            return new Vector2D(Math.Abs(Vector.x),
                                Math.Abs(Vector.z));
        }

        #endregion

        #region Sin Vector

        /// <summary>
        /// Gets a vector where every indices has been Math.Sined.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A sined Vector</returns>
        public static Vector2S SinVector(Vector2S Vector) {
            return new Vector2S((short)Math.Sin(Vector.x),
                                (short)Math.Sin(Vector.z));
        }

        /// <summary>
        /// Gets a vector where every indices has been Math.Sined.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A sined Vector</returns>
        public static Vector2D SinVector(Vector2D Vector) {
            return new Vector2D(Math.Sin(Vector.x),
                                Math.Sin(Vector.z));
        }

        /// <summary>
        /// Gets a vector where every indices has been Math.Sined.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A sined Vector</returns>
        public static Vector3D SinVector(Vector3D Vector) {
            return new Vector3D(Math.Sin(Vector.x),
                                Math.Sin(Vector.z),
                                Math.Sin(Vector.y));
        }

        /// <summary>
        /// Gets a vector where every indices has been Math.Sined.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A sined Vector</returns>
        public static Vector3S SinVector(Vector3S Vector) {
            return new Vector3S((short)Math.Sin(Vector.x),
                                (short)Math.Sin(Vector.z),
                                (short)Math.Sin(Vector.y));
        }

        #endregion

        #region Cos Vector

        /// <summary>
        /// Gets a vector where every indices has been Math.Cosined.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A cosined Vector</returns>
        public static Vector2S CosVector(Vector2S Vector) {
            return new Vector2S((short)Math.Cos(Vector.x),
                                (short)Math.Cos(Vector.z));
        }

        /// <summary>
        /// Gets a vector where every indices has been Math.Cosined.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A cosined Vector</returns>
        public static Vector2D CosVector(Vector2D Vector) {
            return new Vector2D(Math.Cos(Vector.x),
                                Math.Cos(Vector.z));
        }

        /// <summary>
        /// Gets a vector where every indices has been Math.Cosined.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A cosined Vector</returns>
        public static Vector3D CosVector(Vector3D Vector) {
            return new Vector3D(Math.Cos(Vector.x),
                                Math.Cos(Vector.z),
                                Math.Cos(Vector.y));
        }

        /// <summary>
        /// Gets a vector where every indices has been Math.Cosined.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A cosined Vector</returns>
        public static Vector3S CosVector(Vector3S Vector) {
            return new Vector3S((short)Math.Cos(Vector.x),
                                (short)Math.Cos(Vector.z),
                                (short)Math.Cos(Vector.y));
        }

        #endregion

        #region Tan Vector

        /// <summary>
        /// Gets a vector where every indices has been Math.Tangented.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A tangified Vector</returns>
        public static Vector2S TanVector(Vector2S Vector) {
            return new Vector2S((short)Math.Tan(Vector.x),
                                (short)Math.Tan(Vector.z));
        }

        /// <summary>
        /// Gets a vector where every indices has been Math.Tangented.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A tangified Vector</returns>
        public static Vector2D TanVector(Vector2D Vector) {
            return new Vector2D(Math.Tan(Vector.x),
                                Math.Tan(Vector.z));
        }

        /// <summary>
        /// Gets a vector where every indices has been Math.Tangented.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A tangified Vector</returns>
        public static Vector3D TanVector(Vector3D Vector) {
            return new Vector3D(Math.Tan(Vector.x),
                                Math.Tan(Vector.z),
                                Math.Tan(Vector.y));
        }

        /// <summary>
        /// Gets a vector where every indices has been Math.Tangented.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A tangified Vector</returns>
        public static Vector3S TanVector(Vector3S Vector) {
            return new Vector3S((short)Math.Tan(Vector.x),
                                (short)Math.Tan(Vector.z),
                                (short)Math.Tan(Vector.y));
        }

        #endregion

        #region Sign Vectors


        /// <summary>
        /// Gets a vector where every indices has been signed
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A signed Vector</returns>
        public static Vector2S SignVector(Vector2S Vector) {
            return new Vector2S((short)Math.Sign(Vector.x),
                                (short)Math.Sign(Vector.z));
        }
        /// <summary>
        /// Gets a vector where every indices has been signed
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A signed Vector</returns>
        public static Vector2D SignVector(Vector2D Vector) {
            return new Vector2D(Math.Sign(Vector.x),
                                Math.Sign(Vector.z));
        }

        /// <summary>
        /// Gets a vector where every indices has been signed
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A signed Vector</returns>
        public static Vector3D SignVector(Vector3D Vector) {
            return new Vector3D(Math.Sign(Vector.x),
                                Math.Sign(Vector.z),
                                Math.Sign(Vector.y));
        }

        /// <summary>
        /// Gets a vector where every indices has been signed
        /// </summary>
        /// <param name="Vector">The vector.</param>
        /// <returns>A signed Vector</returns>
        public static Vector3S SignVector(Vector3S Vector) {
            return new Vector3S((short)Math.Sign(Vector.x),
                                (short)Math.Sign(Vector.z),
                                (short)Math.Sign(Vector.y));
        }

        #endregion
    }
}
