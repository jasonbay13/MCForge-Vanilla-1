using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Utilities.Settings {
    public abstract class ExtraSettings {
        public abstract string SettingsName { get; }

        public abstract void OnLoad();
        public abstract void Save();
        public abstract void SetDefaults();
    }
}
