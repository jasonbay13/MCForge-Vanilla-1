using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Utils {
    public class ExtraData<T1, T2> : Dictionary<T1, T2> {
        public new T2 this[T1 key] {
            get {
                if (null == key) return default(T2);
                if (!base.ContainsKey(key)) return default(T2);
                else return base[key];
            }
            set {
                if (key == null) return;
                if (!base.ContainsKey(key)) base.Add(key, value);
                else base[key] = value;
            }
        }
    }
}
