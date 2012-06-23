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

namespace MCForge.Utils
{
    public struct TriBool : IEquatable<TriBool>
    {
        public static TriBool True { get { return new TriBool(true); } }
        public static TriBool False { get { return new TriBool(false); } }
        public static TriBool Unknown { get { return new TriBool(); } }
        enum triboolState { Unknown = 0, True = 1, False = 2 }

        private readonly triboolState state;
        public TriBool(bool state)
        {
            this.state = state ? triboolState.True : triboolState.False;
        }
        public static bool operator true(TriBool value)
        {
            return value.state == triboolState.True;
        }
        public static bool operator false(TriBool value)
        {
            return value.state == triboolState.False;
        }
        public static bool operator ==(TriBool x, TriBool y)
        {
            return x.state == y.state;
        }
        public static bool operator !=(TriBool x, TriBool y)
        {
            return x.state != y.state;
        }
        public override string ToString()
        {
            return state.ToString();
        }
        public override bool Equals(object obj)
        {
            return (obj != null && obj is TriBool) ? Equals((TriBool)obj) : false;
        }
        public bool Equals(TriBool value)
        {
            return value == this;
        }
        public override int GetHashCode()
        {
            return state.GetHashCode();
        }
        public static implicit operator TriBool(bool value)
        {
            return new TriBool(value);
        }
        public static explicit operator bool(TriBool value)
        {
            switch (value.state)
            {
                case triboolState.True: return true;
                case triboolState.False: return false;
                default: throw new InvalidCastException();
            }
        }
    }
}
