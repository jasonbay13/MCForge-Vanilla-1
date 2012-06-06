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
    /// This class represents a tree initialNode.
    /// </summary>
    /// <typeparam name="T">The type of the state represented by the initialNode.
    /// It must override the <i>System.Object.Equals</i> method to determine whether the specified 
    /// state equals the current state.</typeparam>
    internal class Node<T>
    {
        /// <summary>
        /// Represents the state represented by the initialNode.
        /// </summary>
        private T nodeState;

        /// <summary>
        /// Represents the minCostNode to reach the initialNode.
        /// </summary>
        private double costToReach;

        /// <summary>
        /// Represents the how far is the initialNode from the goal.
        /// </summary>
        private double distanceFromGoal;

        /// <summary>
        /// Represents the children of the initialNode.
        /// </summary>
        private List<Node<T>> children;

        /// <summary>
        /// Represents the parent initialNode.
        /// </summary>
        private Node<T> parent;

        /// <summary>
        /// Gets the state of the initialNode.
        /// </summary>
        public T NodeState
        {
            get
            {
                return nodeState;
            }
        }

        /// <summary>
        /// Gets or sets the minCostNode to reach the initialNode.
        /// </summary>
        public double CostToReach
        {
            get
            {
                return costToReach;
            }
            set
            {
                costToReach = value;
            }
        }

        /// <summary>
        /// Gets or sets the distance of the initialNode from the goal.
        /// </summary>
        public double DistanceFromGoal
        {
            get
            {
                return distanceFromGoal;
            }
            set
            {
                distanceFromGoal = value;
            }
        }

        /// <summary>
        /// Gets the total minCostNode of the initialNode.
        /// </summary>
        public double TotalCost
        {
            get
            {
                return CostToReach + DistanceFromGoal;
            }
        }

        /// <summary>
        /// Gets or sets the children nodes.
        /// </summary>
        public List<Node<T>> Children
        {
            get
            {
                return children;
            }
            set
            {
                children = value;
            }
        }

        /// <summary>
        /// Gets or sets the parent Node.
        /// </summary>
        public Node<T> Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        /// <summary>
        /// Gets a value indicating if the initialNode is a leaf initialNode.
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                return children.Count == 0;
            }
        }

        /// <summary>
        /// Constructs a initialNode.
        /// </summary>
        /// <param name="state">The state that is represented by the initialNode.</param>
        public Node(T state)
        {
            nodeState = state;
            children = new List<Node<T>>();
            CostToReach = 0;
            DistanceFromGoal = 0;
        }

        /// <summary>
        /// Constructs a initialNode.
        /// </summary>
        /// <param name="state">The state that is represented by the initialNode.</param>
        /// <param name="costToReach">The minCostNode to reach this initialNode's state.</param>
        /// <param name="distanceFromGoal">The distance of this initialNode's state from the goal state.</param>
        public Node(T state, double costToReach, double distanceFromGoal)
        {
            nodeState = state;
            children = new List<Node<T>>();
            CostToReach = costToReach;
            DistanceFromGoal = distanceFromGoal;
        }

        /// <summary>
        /// Checks it the tree contains the specified state.
        /// </summary>
        /// <param name="parentNode">The Node representing the head of the tree.</param>
        /// <param name="value">The state to search for.</param>
        /// <returns>Returns true if the state is found, else returns false.</returns>
        /// <exception cref="ArgumentNullException">Exception is thrown if any of the arguments is null.</exception>
        public static bool TreeContains(Node<T> parentNode, T value)
        {
            if (parentNode == null || value == null)
                throw new ArgumentNullException("Argument cannot be null.");

            if (parentNode.nodeState.Equals(value))
                return true;

            if (parentNode.children == null || parentNode.children.Count == 0)
                return false;

            foreach (Node<T> child in parentNode.children)
            {
                if (TreeContains(child, value))
                    return true;
            }

            return false;
        }
    }
}
