using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Utils;

namespace MCForge.API.Events {
    public class RotateEvent : Event<Player, RotateEventArgs> {
    }
    public class RotateEventArgs : EventArgs, ICancelable, IEquatable<RotateEventArgs>, ICloneable {
        public RotateEventArgs(byte rot0, byte rot1) {
            this.Rot0 = rot0;
            this.Rot1 = rot1;
        }
        private bool _canceled = false;
        public bool Canceled {
            get { return _canceled; }
        }

        public void Cancel() {
            _canceled = true;
        }

        public void Allow() {
            _canceled = false;
        }

        public bool Equals(RotateEventArgs other) {
            if (other == null) return false;
            return this.Rot0 == other.Rot0 && this.Rot1 == other.Rot1;
        }

        public object Clone() {
            return new RotateEventArgs(Rot0, Rot1);
        }
        /// <summary>
        /// Horizontal rotation angle
        /// </summary>
        public byte Rot0 {
            get { return _rot[0]; }
            set {
                if (_rot == null) _rot = new byte[2];
                _rot[0] = value;
            }
        }
        /// <summary>
        /// Vertical rotation angle
        /// </summary>
        public byte Rot1 {
            get { return _rot[1]; }
            set {
                if (_rot == null) _rot = new byte[2];
                _rot[1] = value;
            }
        }
        private byte[] _rot;
        /// <summary>
        /// Horizontal and vertical rotation angles
        /// </summary>
        public byte[] Rot {
            get { return _rot; }
            set {
                if (value == null || value.Length != 2) _rot = new byte[2];
                else _rot = value;
            }
        }
    }
}
