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

namespace MCForge.Utils {
    public class ExtraData<T1, T2> : Dictionary<T1, T2> {
        private readonly object locker=new object();
        public new T2 this[T1 key] {
            get {
                if (null == key) return default(T2);
                lock (locker) {
                    if (!base.ContainsKey(key)) return default(T2);
                    else return base[key];
                }
            }
            set {
                if (key == null) return;
                lock (locker) {
                    if (value == null) {
                        if (base.ContainsKey(key))
                            base.Remove(key);
                        return;
                    }
                    if (!base.ContainsKey(key)) base.Add(key, value);
                    else base[key] = value;
                }
            }
        }
        public override string ToString() {
            lock (locker) {
                for (int i = 0; i < base.Keys.Count; i++) {
                }
            }
            return "";
        }
    }
}
