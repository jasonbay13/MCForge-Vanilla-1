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
using System.Collections.Generic;

namespace AStar
{
    /// <summary>
    /// This class uses the A* algorithm to find the route to move 
    /// from an initial location to one of many possible goal locations in a 2-D plane.
    /// </summary>
    public class RouteFinder
    {
        /// <summary>
        /// Represents the A* algorithm used to calculate the route.
        /// </summary>
        private AStar<Location> aStar;

        /// <summary>
        /// Represents the initial location to start moving from.
        /// </summary>
        private Location initialLocation;

        /// <summary>
        /// Represents the list of goal locations.
        /// </summary>
        private List<Location> goalLocations;

        /// <summary>
        /// Represents the list of obstacles.
        /// </summary>
        private List<Location> obstacles;

        /// <summary>
        /// Represents the minimum X locaion allowed on the map.
        /// </summary>
        private readonly int minX;

        /// <summary>
        /// Represents the minimum Y locaion allowed on the map.
        /// </summary>
        private readonly int minY;

        /// <summary>
        /// Represents the maximum X locaion allowed on the map.
        /// </summary>
        private readonly int maxX;

        /// <summary>
        /// Represents the maximum Y locaion allowed on the map.
        /// </summary>
        private readonly int maxY;

        /// <summary>
        /// Specifies whether the route can have diagonal moves.
        /// </summary>
        private readonly bool canMoveDiagonally;

        /// <summary>
        /// Represents the difference in locations for the diagonal cells. It contains two columns and four rows.
        /// The first column represents the difference in X position and the second represents the difference in Y position.
        /// Each row represents an adjacent cell.
        /// </summary>
        private readonly int[,] diagonalAdjacentDeltas;

        /// <summary>
        /// Represents the difference in locations for the non-diagonal cells. It contains two columns and four rows.
        /// The first column represents the difference in X position and the second represents the difference in Y position.
        /// Each row represents an adjacent cell.
        /// </summary>
        private readonly int[,] nonDiagonalAdjacentDeltas;

        /// <summary>
        /// Gets or sets the initial location to start moving from.
        /// </summary>
        /// <exception cref="ArgumentNullException">Exception is thrown if the initial location is set to null.</exception>
        public Location InitialLocation
        {
            get
            {
                return initialLocation;
            }
            set
            {
                if (value == null)
                    throw new InvalidOperationException("InitialLocation cannot be set to null.");

                initialLocation = value;
            }
        }

        /// <summary>
        /// Constructs a new <i>RouteFinder</i>.
        /// </summary>
        /// <param name="minX">Specifies the minimum X locaion allowed on the map</param>
        /// <param name="minY">Specifies the minimum Y locaion allowed on the map</param>
        /// <param name="maxX">Specifies the maximum X locaion allowed on the map</param>
        /// <param name="maxY">Specifies the maximum Y locaion allowed on the map</param>
        /// <param name="canMoveDiagonally">Indicates whether the route can include diagonal moves.</param>
        public RouteFinder(int minX, int minY, int maxX, int maxY, bool canMoveDiagonally)
        {
            if (maxX < minX || maxY < minY)
                throw new ArgumentException("The values of maxX and maxY cannot be less than minX and minY respectively.");

            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
            this.canMoveDiagonally = canMoveDiagonally;
            goalLocations = new List<Location>();
            obstacles = new List<Location>();
            aStar = new AStar<Location>(GetAdjacentLocations, CalculateCostToMove, CalculateDistanceToGoal);
            diagonalAdjacentDeltas = new int[,] { { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };
            nonDiagonalAdjacentDeltas = new int[,] { { -1, 0 }, { 0, -1 }, { 1, 0 }, { 0, 1 } };
        }

        /// <summary>
        /// Adds a new goal location.
        /// </summary>
        /// <param name="goal">The location of the goal to be added.</param>
        /// <exception cref="ArgumentNullException">Exception thrwon if argument is <i>null</i>.</exception>
        /// <exception cref="InvalidOperationException">If the location of the goal is not valid.</exception>
        public void AddGoal(Location goal)
        {
            if (goal == null)
                throw new ArgumentNullException("goal", "Argument cannot be null.");

            if (goal == InitialLocation)
                return;

            foreach (Location obstacleLoc in obstacles)
            {
                if (goal == obstacleLoc)
                    return;
            }

            goalLocations.Add(goal);
        }

        /// <summary>
        /// Clears any previously added goals.
        /// </summary>
        public void ClearGoals()
        {
            goalLocations.Clear();
        }

        /// <summary>
        /// Adds a new obstacle.
        /// </summary>
        /// <param name="obstacle">The location of the obstacle to be added.</param>
        /// <exception cref="ArgumentNullException">Exception thrown if the argument is <i>null</i>.</exception>
        /// <exception cref="InvalidOperationException">If the location of the obstacle is not valid.</exception>
        public void AddObstacle(Location obstacle)
        {
            if (obstacle == null)
                throw new ArgumentNullException("obstacle", "Argument cannot be null.");

            if (obstacle == InitialLocation)
                return;

            foreach (Location goalLoc in goalLocations)
            {
                if (obstacle == goalLoc)
                    return;
            }

            obstacles.Add(obstacle);
        }

        /// <summary>
        /// Clears any previously added obstacle.
        /// </summary>
        public void ClearObstacles()
        {
            obstacles.Clear();
        }

        /// <summary>
        /// Calculates the adjacent locations to a specified location.
        /// </summary>
        /// <param name="location">The location to find the adjacents of.</param>
        /// <returns>A list of the locations adjacent to the specified location. 
        /// If all the adjacent locations are invalid (obstacles or out of bounds) it returns an empty list.</returns>
        private List<Location> GetAdjacentLocations(Location location)
        {
            if (location == null)
                throw new ArgumentNullException("location", "Argument cannot be null.");

            int[,] nonDiagonalAdjacentDeltas = new int[,] { { -1, 0 }, { 0, -1 }, { 1, 0 }, { 0, 1 } };

            int[,] diagonalAdjacentDeltas = new int[,] { { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };

            List<Location> adjacent = new List<Location>();
            GetAdjacentLocations(location, adjacent, nonDiagonalAdjacentDeltas);

            if (canMoveDiagonally)
                GetAdjacentLocations(location, adjacent, diagonalAdjacentDeltas);

            return adjacent;
        }

        /// <summary>
        /// For the specified location, it adds the valid adjacent cells to the specified list of adjacent locations.
        /// </summary>
        /// <param name="location">The location to get the adjacents of.</param>
        /// <param name="adjacent">The list of adjacent cells.</param>
        /// <param name="deltas">An array that represents the differences between the specified location and its adjacents.
        /// It should consist of two columns. The first column represents the difference in X position and the second column 
        /// represents teh difference in Y position. Each row should represent an adjacent cell.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="location"/> of the <paramref name="deltas"/> argument is null.</exception>
        private void GetAdjacentLocations(Location location, List<Location> adjacent, int[,] deltas)
        {
            if (location == null)
                throw new ArgumentNullException("location", "The location cannot be null.");

            if (deltas == null)
                throw new ArgumentNullException("deltas", "The deltas argument cannot be null.");

            if (adjacent == null)
                adjacent = new List<Location>();

            for (int i = 0; i < 4; i++)
            {
                Location adj = new Location(location.X + deltas[i, 0], location.Y + deltas[i, 1]);

                // Skip the location if it is invalid.
                if (!IsWithinBounds(location) || IsObstacle(location))
                    continue;

                adjacent.Add(adj);
            }
        }

        /// <summary>
        /// Calculates a value indicating the cost to move from 
        /// location A to location B, given that, B is adjacent to A.
        /// </summary>
        /// <param name="locA">Location A.</param>
        /// <param name="locB">Location B.</param>
        /// <returns>The cost to move from location A to location B.</returns>
        /// <remarks>This function always returns 1.</remarks>
        /// <exception cref="ArgumentNullException">Exception thrown if any of the arguments is <i>null</i>.</exception>
        private double CalculateCostToMove(Location locA, Location locB)
        {
            if (locA == null || locB == null)
                throw new ArgumentNullException("Argument cannot be null.");

            return (locA.X != locB.X && locA.Y != locB.Y) ? 2 : 1;
        }

        /// <summary>
        /// Calculates how far is a specified location from a specified goal location.
        /// </summary>
        /// <param name="loc">The location to calculate from.</param>
        /// <param name="goal">The goal location.</param>
        /// <returns>A value indicating how far the location is from the goal.</returns>
        /// <exception cref="ArgumentNullException">Exception thrown if any of the arguments is <i>null</i>.</exception>
        private double CalculateDistanceToGoal(Location loc, Location goal)
        {
            if(loc == null || goal == null)
                throw new ArgumentNullException("Argument cannot be null.");

            return Math.Abs(loc.X - goal.X) + Math.Abs(loc.Y - goal.Y);
        }

        /// <summary>
        /// Checks if the specified location is the location of an obstacle.
        /// </summary>
        /// <param name="location">The location to look for.</param>
        /// <returns>Returns <i>true</i> if the location is the location of an obstacle, otherwise, returns <i>false</i>.</returns>
        /// <exception cref="ArgumentNullException">Exception thrown if the argument is null.</exception>
        private bool IsObstacle(Location location)
        {
            if (location == null)
                throw new ArgumentNullException("location", "Argument cannot be null.");

            foreach (Location loc in obstacles)
            {
                if (loc == location)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if a specified location is within the minimum and maximum bounds of the map.
        /// </summary>
        /// <param name="location">The location to check.</param>
        /// <returns>Returns <i>true</i> if the location is within bounds.</returns>
        private bool IsWithinBounds(Location location)
        {
            return location.X >= minX && location.X <= maxX && location.Y >= minY && location.Y <= maxY;
        }

        /// <summary>
        /// Calculates the route from the initial loc to the nearest goal.
        /// </summary>
        /// <returns>A list of the locations to go to in order to reach the goal. If there is no route, it returns <i>null</i>.</returns>
        /// <exception cref="InvalidOperationException">If the operation is not valid.</exception>
        public List<Location> CalculateRoute()
        {
            aStar.InitialState = initialLocation;
            aStar.GoalStates = goalLocations;
            return aStar.CalculateSolution();
        }
    }
}