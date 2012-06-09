using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Plugin;

namespace Plugins.AntiGriefingPlugin {
    public class PluginAntiGrief : IPlugin {
        #region IPlugin Members

        public string Name {
            get { throw new NotImplementedException(); }
        }

        public string Author {
            get { throw new NotImplementedException(); }
        }

        public int Version {
            get { throw new NotImplementedException(); }
        }

        public string CUD {
            get { throw new NotImplementedException(); }
        }

        public void OnLoad(string[] args) {
            throw new NotImplementedException();
        }

        public void OnUnload() {
            throw new NotImplementedException();
        }

        #endregion
    }
}
