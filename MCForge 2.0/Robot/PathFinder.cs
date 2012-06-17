using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.World;
using MCForge.Utils;

namespace MCForge.Robot
{
    /// <summary>
    /// Author: Roy Triesscheijn (http://www.royalexander.wordpress.com)
    /// Class providing 3D pathfinding capabilities using A*.
    /// Heaviliy optimized for speed therefore uses slightly more memory
    /// On rare cases finds the 'almost optimal' path instead of the perfect path
    /// this is because we immediately return when we find the exit instead of finishing
    /// 'neighbour' loop.
    /// </summary>
    public static class PathFinder
    {
        /// <summary>
        /// Method that switfly finds the best path from start to end.
        /// </summary>
        /// <returns>The starting breadcrumb traversable via .next to the end or null if there is no path</returns>        
        public static BreadCrumb FindPath(Level world, Point3D start, Point3D end)
        {
            //note we just flip start and end here so you don't have to.            
            return FindPathReversed(world, end, start);
        }

        /// <summary>
        /// Method that switfly finds the best path from start to end. Doesn't reverse outcome
        /// </summary>
        /// <returns>The end breadcrump where each next is a step back)</returns>
        private static BreadCrumb FindPathReversed(Level world, Point3D start, Point3D end)
        {
            MinHeap<BreadCrumb> openList = new MinHeap<BreadCrumb>(256);
            BreadCrumb[, ,] brWorld = new BreadCrumb[world.Size.x, world.Size.y, world.Size.z];
            BreadCrumb node;
            Point3D tmp;
            int cost;
            int diff;

            BreadCrumb current = new BreadCrumb(start);
            current.cost = 0;

            BreadCrumb finish = new BreadCrumb(end);
            brWorld[current.position.X, current.position.Y, current.position.Z] = current;
            openList.Add(current);

            while (openList.Count > 0)
            {
                //Find best item and switch it to the 'closedList'
                current = openList.ExtractFirst();
                current.onClosedList = true;

                //Find neighbours
                for (int i = 0; i < surrounding.Length; i++)
                {
                    tmp = current.position + surrounding[i];
                    if (world.GetBlock(tmp.X, tmp.Z, tmp.Y) == Block.BlockList.AIR)
                    {
                        //Check if we've already examined a neighbour, if not create a new node for it.
                        if (brWorld[tmp.X, tmp.Y, tmp.Z] == null)
                        {
                            node = new BreadCrumb(tmp);
                            brWorld[tmp.X, tmp.Y, tmp.Z] = node;
                        }
                        else
                        {
                            node = brWorld[tmp.X, tmp.Y, tmp.Z];
                        }

                        //If the node is not on the 'closedList' check it's new score, keep the best
                        if (!node.onClosedList)
                        {
                            diff = 0;
                            if (current.position.X != node.position.X)
                            {
                                diff += 1;
                            }
                            if (current.position.Y != node.position.Y)
                            {
                                diff += 1;
                            }
                            if (current.position.Z != node.position.Z)
                            {
                                diff += 1;
                            }
                            cost = current.cost + diff + node.position.GetDistanceSquared(end);

                            if (cost < node.cost)
                            {
                                node.cost = cost;
                                node.next = current;
                            }

                            //If the node wasn't on the openList yet, add it 
                            if (!node.onOpenList)
                            {
                                //Check to see if we're done
                                if (node.Equals(finish))
                                {
                                    node.next = current;
                                    return node;
                                }
                                node.onOpenList = true;
                                openList.Add(node);
                            }
                        }
                    }
                }
            }
            return null; //no path found
        }

        //Neighbour options
        private static Point3D[] surrounding = new Point3D[]{                        
            //Top slice (Y=1)
            new Point3D(-1,1,1), new Point3D(0,1,1), new Point3D(1,1,1),
            new Point3D(-1,1,0), new Point3D(0,1,0), new Point3D(1,1,0),
            new Point3D(-1,1,-1), new Point3D(0,1,-1), new Point3D(1,1,-1),
            //Middle slice (Y=0)
            new Point3D(-1,0,1), new Point3D(0,0,1), new Point3D(1,0,1),
            new Point3D(-1,0,0), new Point3D(1,0,0), //(0,0,0) is self
            new Point3D(-1,0,-1), new Point3D(0,0,-1), new Point3D(1,0,-1),
            //Bottom slice (Y=-1)
            new Point3D(-1,-1,1), new Point3D(0,-1,1), new Point3D(1,-1,1),
            new Point3D(-1,-1,0), new Point3D(0,-1,0), new Point3D(1,-1,0),
            new Point3D(-1,-1,-1), new Point3D(0,-1,-1), new Point3D(1,-1,-1)            
        };
    }
}
