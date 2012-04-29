using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Utils {
    public static class MiscUtils {
        public static object GetIfExist(this Dictionary<object, object> dict, object key) {
            if (dict.ContainsKey(key))
                return dict[key];
            return null;
        }

        public static void CreateIfNotExist(this Dictionary<object, object> dict, object key, object value) {
            if (!dict.ContainsKey(key))
                dict.Add(key, value);
        }
    }
}
