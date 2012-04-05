/*
Copyright 2011 MCForge
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

namespace MCForge.API
{
    public class Muffins
    {
        internal static List<Muffins> cache = new List<Muffins>();
        public object Delegate;
        public Priority priority;
        public Event type;
        public Muffins(object Delegate, Priority pri, Event type) { this.Delegate = Delegate; this.priority = pri; this.type = type; }
        public static void Organize()
        {
            //TODO
            //Organize them, Low being called first and System_Level being called last
        }
        public static void GiveDerpyMuffins(Muffins c)
        {
            cache.Add(c);
            Organize();
        }
    }
}
