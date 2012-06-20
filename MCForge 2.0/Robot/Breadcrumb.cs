using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Robot
{
    /// <summary>
    /// Author: Roy Triesscheijn (http://www.royalexander.wordpress.com)
    /// Class defining BreadCrumbs used in path finding to mark our routes
    /// </summary>
    public class BreadCrumb : IComparable<BreadCrumb>
    {
        public Point3D position;
        public BreadCrumb next;
        public int cost = Int32.MaxValue;
        public bool onClosedList = false;
        public bool onOpenList = false;

        public BreadCrumb(Point3D position)
        {
            this.position = position;
        }

        public BreadCrumb(Point3D position, BreadCrumb parent)
        {
            this.position = position;
            this.next = parent;
        }

        //Overrides default Equals so we check on position instead of object memory location
        public override bool Equals(object obj)
        {
            return (obj is BreadCrumb) && ((BreadCrumb)obj).position == this.position;
        }

        //Faster Equals for if we know something is a BreadCrumb
        public bool Equals(BreadCrumb breadcrumb)
        {
            return breadcrumb.position == this.position;
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }

        #region IComparable<> interface
        public int CompareTo(BreadCrumb other)
        {
            return cost.CompareTo(other.cost);
        }
        #endregion
    }
}
