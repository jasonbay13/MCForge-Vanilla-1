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
