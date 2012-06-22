using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Utils
{
    public struct tribool : IEquatable<tribool>
    {
        public static tribool True { get { return new tribool(true); } }
        public static tribool False { get { return new tribool(false); } }
        public static tribool Unknown { get { return new tribool(); } }
        enum triboolState { Unknown = 0, True = 1, False = 2 }

        private readonly triboolState state;
        public tribool(bool state)
        {
            this.state = state ? triboolState.True : triboolState.False;
        }
        public static bool operator true(tribool value)
        {
            return value.state == triboolState.True;
        }
        public static bool operator false(tribool value)
        {
            return value.state == triboolState.False;
        }
        public static bool operator ==(tribool x, tribool y)
        {
            return x.state == y.state;
        }
        public static bool operator !=(tribool x, tribool y)
        {
            return x.state != y.state;
        }
        public override string ToString()
        {
            return state.ToString();
        }
        public override bool Equals(object obj)
        {
            return (obj != null && obj is tribool) ? Equals((tribool)obj) : false;
        }
        public bool Equals(tribool value)
        {
            return value == this;
        }
        public override int GetHashCode()
        {
            return state.GetHashCode();
        }
        public static implicit operator tribool(bool value)
        {
            return new tribool(value);
        }
        public static explicit operator bool(tribool value)
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
