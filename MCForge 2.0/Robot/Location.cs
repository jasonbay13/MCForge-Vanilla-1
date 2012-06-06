/* Copyright 2011 M. Mohsen (Email: m.mohsen.mahmoud@gmail.com)
 * 
 * This file is part of AStar.
 * 
 * AStar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * AStar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * */
using System;

namespace AStar
{
    /// <summary>
    /// This class represents a loc in a 2D plane.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Represents the x position of the loc.
        /// </summary>
        private int x;

        /// <summary>
        /// Represents the y position of the loc.
        /// </summary>
        private int y;

        /// <summary>
        /// Gets or sets the X position of the loc.
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        /// <summary>
        /// Gets or sets the Y position of the loc.
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        /// <summary>
        /// Constructs a <i>Location</i> at X = 0, Y = 0.
        /// </summary>
        public Location()
        {
            x = 0;
            y = 0;
        }

        /// <summary>
        /// Constructs a <i>Location</i> at the specified X and Y values.
        /// </summary>
        /// <param name="x">Initial X position.</param>
        /// <param name="y">Initial Y position.</param>
        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Returns true if the locations are equal, otherwise, returns false.
        /// </summary>
        /// <param name="lhs">The left hand side loc.</param>
        /// <param name="rhs">The right hand side loc.</param>
        /// <returns>Returns true if the locations are equal, otherwise, returns false.</returns>
        public static bool operator ==(Location lhs, Location rhs)
        {
            // If both are null or both are references to the same object, return true.
            if (Object.ReferenceEquals(lhs, rhs))
                return true;

            // If only one is null, not both, return false.
            if ((object)lhs == null || (object)rhs == null)
                return false;

            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        /// <summary>
        /// Returns true if the two locations are not equal, otherwise, returns false.
        /// </summary>
        /// <param name="lhs">The left hand side loc.</param>
        /// <param name="rhs">The right hand side loc.</param>
        /// <returns>Returns true if the two locations are not equal, otherwise, returns false.</returns>
        public static bool operator !=(Location lhs, Location rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// This method overrides the System.Object.Equals(object obj) method. Returns true if the specified
        /// object equals the current location.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>Returns true if the specified object equals the current location.</returns>
        public override bool Equals(object obj)
        {
            // If obj cannot be casted to location.
            Location loc = obj as Location;
            if ((object)loc == null)
                return false;

            return this.x == loc.x && this.y == loc.y;
        }

        /// <summary>
        /// Returns true if the specified location equals the current location.
        /// </summary>
        /// <param name="loc">The location to compare to.</param>
        /// <returns>Returns true if the specified location equals the current location.</returns>
        public bool Equals(Location loc)
        {
            if ((object)loc == null)
                return false;

            return this.x == loc.x && this.y == loc.y;
        }

        /// <summary>
        /// This method overrides the System.Object.GetHashCode() method. Returns the hash code of the loc.
        /// </summary>
        /// <returns>The hash code of the loc.</returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode();
        }

        /// <summary>
        /// This method overrides the System.Object.ToString() method. Returns the <i>string</i> representation of the loc.
        /// </summary>
        /// <returns>The <i>string</i> representation of the loc.</returns>
        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }
    }
}
