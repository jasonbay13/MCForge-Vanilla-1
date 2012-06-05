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
    /// This class represents the AStar algorithm.
    /// </summary>
    /// <typeparam name="T">The type of the objects to perform the algorithm on.</typeparam>
    public class AStar<T>
    {
        /// <summary>
        /// Represents a delegate to the function that calculates the successor states. It should take an
        /// object of type <i><typeparamref name="T"/></i> and return an object of type <i>List&lt;<typeparamref name="T"/>&gt;.</i>
        /// </summary>
        /// <param name="state">The state to calculate its successors.</param>
        /// <returns>The successor states.</returns>
        public delegate List<T> SuccessorFunctionDelegate(T state);

        /// <summary>
        /// Represents a delegate to the function that calculates the distance between two states. It should take 
        /// two parameters of type <i><typeparamref name="T"/></i> and return a <i>double</i> indicating the distance between them.
        /// </summary>
        /// <param name="sourceState">The source state to start from.</param>
        /// <param name="successorState">The successor state.</param>
        /// <returns>The distance of the source from the goal. 
        /// It should return 0 <b>only</b> if the <i>sourceState</i> equals the <i>successorState</i>.</returns>
        public delegate double CalculateDistanceDelegate(T sourceState, T successorState);

        /// <summary>
        /// Represents a delegate to the function that calculates the distance between two states given that 
        /// the second one is a goal state. It should take two parameters of type <i><typeparamref name="T"/></i>
        /// and return a <i>double</i> indicating the distance between them.
        /// </summary>
        /// <param name="sourceState">The source state to start from.</param>
        /// <param name="goalState">The goal state.</param>
        /// <returns>The distance of the source from the goal. 
        /// It should return 0 <b>only</b> if the <i><paramref name="sourceState"/></i> equals 
        /// the <i><paramref name="goalState"/></i>.</returns>
        public delegate double CalculateDistanceFromGoalDelegate(T sourceState, T goalState);

        /// <summary>
        /// The delegate to the successor function.
        /// </summary>
        private SuccessorFunctionDelegate successorFunction = null;

        /// <summary>
        /// The delegate to the distance function.
        /// </summary>
        private CalculateDistanceDelegate distanceFunction = null;

        /// <summary>
        /// The delegate to the distance from goal function.
        /// </summary>
        private CalculateDistanceFromGoalDelegate distanceFromGoalFunction = null;

        /// <summary>
        /// Represents the initial state to start the algorithm from.
        /// </summary>
        private T initialState;

        /// <summary>
        /// A list of the goal states (targets to be reached by the algorithm).
        /// </summary>
        private List<T> goalStates;

        /// <summary>
        /// Gets or sets the initial state to start the algorithm from. 
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown on the attempt to set to <i>null</i>.</exception>
        public T InitialState
        {
            get
            {
                return initialState;
            }
            set
            {
                if (value == null)
                    throw new InvalidOperationException("InitialState cannot be set to null.");
                initialState = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of goal states (targets to be reached by the algorithm).
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown on the attempt to set to <i>null</i>.</exception>
        public List<T> GoalStates
        {
            get
            {
                return goalStates;
            }
            set
            {
                if (value == null)
                    throw new InvalidOperationException("GoalStates cannot be set to null.");
                goalStates = value;
            }
        }

        /// <summary>
        /// Constructs an object of <i>AStar</i> given its successorFunction and distanceFunction.
        /// </summary>
        /// <param name="successorFunction">The delegate to the successor function.</param>
        /// <param name="distanceFunction">The delegate to the distance function.</param>
        /// <param name="distanceFromGoalFunction">The delegate to the function that calculates the distance from the nearest goal state.</param>
        /// <param name="initialState">Represents the initial state to start the algorithm from.</param>
        public AStar(SuccessorFunctionDelegate successorFunction, CalculateDistanceDelegate distanceFunction, CalculateDistanceFromGoalDelegate distanceFromGoalFunction, T initialState = default(T))
        {
            this.successorFunction = successorFunction;
            this.distanceFunction = distanceFunction;
            this.distanceFromGoalFunction = distanceFromGoalFunction;
            this.initialState = initialState;
            goalStates = new List<T>();
        }

        /// <summary>
        /// Adds a goal state.
        /// </summary>
        /// <param name="state">The goal state to add.</param>
        public void AddGoalState(T state)
        {
            goalStates.Add(state);
        }

        /// <summary>
        /// Computes the distance of a state from the nearest goal state.
        /// </summary>
        /// <param name="state">The state to start calculating from.</param>
        /// <returns>The distance to the nearest goal state.</returns>
        private double DistanceFromGoal(T state)
        {
            double minDistance = Double.MaxValue;
            double distance = 0;

            foreach (T goal in goalStates)
            {
                distance = distanceFromGoalFunction(state, goal);
                if (distance < minDistance)
                    minDistance = distance;
            }

            return minDistance;
        }

        /// <summary>
        /// Calculates the sequence of states to go from the initial state to the nearest goal state.
        /// </summary>
        /// <returns>A sequence of states as the solution or <i>null</i> if there is no solution.</returns>
        /// <exception cref="InvalidOperationException">If the operation is not valid.</exception>
        public List<T> CalculateSolution()
        {
            // Throw an exception if there are no goal states.
            if (goalStates.Count == 0)
                return null;

            // Initialize the head of the tree.
            Node<T> initialNode = new Node<T>(initialState, 0, DistanceFromGoal(initialState));

            // Throw an exception if the initial state is a goal state.
            if (initialNode.DistanceFromGoal == 0)
                throw new InvalidOperationException("Initial state is a goal state.");

            // Return null if the initial node has no successor states (no solution).
            if (!ConstructChildrenNodes(initialNode, initialNode))
                return null;

            initialNode.Parent = null;

            // Represents all the leaf nodes of the tree.
            List<Node<T>> leafNodes = new List<Node<T>>();
            leafNodes.AddRange(initialNode.Children);

            Node<T> reachedGoal = null;
            do
            {
                reachedGoal = FindGoal(leafNodes);
                // Call ExpandNodeWithMinCost only if the goal is not reached
            } while (reachedGoal == null && ExpandNodeWithMinCost(leafNodes, initialNode));

            // Return null if the goal is not yet reached i.e. the above loop exited because there are no nodes to expand.
            if (reachedGoal == null)
                return null;

            return ConstructSolution(reachedGoal);
        }

        /// <summary>
        /// This method expands the node with minimum cost and removes it from the <i>leafNodes</i> list.
        /// </summary>
        /// <param name="leafNodes">List of the leaf nodes.</param>
        /// <param name="initialNode">The head node of the tree.</param>
        /// <returns>This method returns <i>false</i> if the number of leafNodes is 0, or if all the nodes have no successor nodes.</returns>
        private bool ExpandNodeWithMinCost(List<Node<T>> leafNodes, Node<T> initialNode)
        {
            // Return false if there are no leafNodes initially.
            if (leafNodes.Count == 0)
                return false;

            Node<T> minCostNode = null;
            do
            {
                List<Node<T>>.Enumerator enumerator = leafNodes.GetEnumerator();

                // Must be called before using it because the enumerator is initially positioned before the start of the list.
                enumerator.MoveNext();

                minCostNode = enumerator.Current;
                int minIndex = 0;

                // Start the loop from 1 because the minCostNode initially points to the node with index 0.
                for (int i = 1; enumerator.MoveNext(); i++)
                {
                    if (enumerator.Current.TotalCost < minCostNode.TotalCost)
                    {
                        minIndex = i;
                        minCostNode = enumerator.Current;
                    }
                }

                leafNodes.RemoveAt(minIndex);

                // Exit the loop if the count of leaf nodes is 0 to prevent infinit loops.
            } while (!ConstructChildrenNodes(minCostNode, initialNode) && leafNodes.Count != 0);

            // If the list is empty and there are no children for the minCostNode return false.
            if (leafNodes.Count == 0 && minCostNode.IsLeaf)
                return false;

            leafNodes.AddRange(minCostNode.Children);

            return true;
        }

        /// <summary>
        /// Construct the children nodes for a parent node.
        /// </summary>
        /// <param name="parentNode">The parent node to construct the children of.</param>
        /// <param name="initialNode">The head node of the tree.</param>
        /// <returns>This method returns <i>false</i> if there are no successor states to the parent node; 
        /// otherwise, it returns <i>true</i>.</returns>
        private bool ConstructChildrenNodes(Node<T> parentNode, Node<T> initialNode)
        {
            List<T> successorStates = successorFunction(parentNode.NodeState);

            // Return false if there are no successor states.
            if (successorStates.Count == 0)
                return false;

            List<Node<T>> children = new List<Node<T>>();

            foreach (T state in successorStates)
            {
                // If the state has already been traversed, skip it.
                if (Node<T>.TreeContains(initialNode, state))
                    continue;

                Node<T> child = new Node<T>(state,
                    parentNode.CostToReach + distanceFunction(parentNode.NodeState, state),
                    DistanceFromGoal(state));

                child.Parent = parentNode;
                children.Add(child);
            }

            // Return false if all the successor states have be previously traversed.
            if (children.Count == 0)
                return false;

            parentNode.Children = children;

            return true;
        }

        /// <summary>
        /// This function finds the goal state in the list of leaf states.
        /// </summary>
        /// <param name="leafNodes">The list of nodes to find the goal within.</param>
        /// <returns>This method returns goal state found or <i>null</i> if a goal state is not found.</returns>
        private Node<T> FindGoal(List<Node<T>> leafNodes)
        {
            foreach (Node<T> node in leafNodes)
            {
                if (node.DistanceFromGoal == 0)
                    return node;
            }

            return null;
        }

        /// <summary>
        /// This method constructs the list of states for the solution given the goalNodeReached.
        /// </summary>
        /// <param name="goalNodeReached">The goal node reached.</param>
        /// <returns>A list of states represnting the solution of the algorithm.</returns>
        private List<T> ConstructSolution(Node<T> goalNodeReached)
        {
            List<T> solution = new List<T>();

            while (goalNodeReached.Parent != null)
            {
                solution.Add(goalNodeReached.NodeState);
                goalNodeReached = goalNodeReached.Parent;
            }

            solution.Reverse();
            return solution;
        }
    }
}
