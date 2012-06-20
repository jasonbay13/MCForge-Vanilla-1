using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MCForge.World;
using MCForge.Core;
using MCForge.Utils;
using System.Drawing;
using MCForge.Utils.Settings;

namespace Plugins.WomPlugin {
    class WomSettings : ExtraSettings {
        private readonly List<SettingNode> _cgfvalues = new List<MCForge.Utils.Settings.SettingNode>() {
            new SettingNode("server.name", ServerSettings.GetSetting("ServerName"), null),
            new SettingNode("server.detail", ServerSettings.GetSetting("motd"), null),
            new SettingNode("detail.user", "User line", null),
            new SettingNode("environment.fog", "1377559", null),
            new SettingNode("environment.sky", "1377559", null),
            new SettingNode("environment.cloud", "1377559", null),
            new SettingNode("environment.level", "0", null),
            new SettingNode("environment.edge", "685cfceb13ccee86d3b93f163f4ac6e4a28347bf", null),
            new SettingNode("environment.terrain", "f3dac271d7bce9954baad46e183a6a910a30d13b", null),
            new SettingNode("environment.side", "7c0fdebeb6637929b9b3170680fa7a79b656c3f7", null),
            new SettingNode("server.sendwomid","true", null),
        };
        private readonly List<SettingNode> _values = new List<SettingNode>() {
            new SettingNode("LevelName", "main", null),
            new SettingNode("ConfigPath", ServerSettings.GetSetting("configpath") + "main.cfg", null),
        };

        public List<Level> LevelsWithTextures { get; private set; }

        public override string SettingsName {
            get { return "WomSettings"; }
        }

        public override void OnLoad() {
            Logger.Log("[WoM] WomSettings loaded!");
            LevelsWithTextures = new List<Level>();
            string path = (ServerSettings.GetSetting("configpath") + "wom/");
            if (!Directory.Exists(path)) {
                Logger.Log("[WoM] Config folder not found. Creating");
                Directory.CreateDirectory(ServerSettings.GetSetting("configpath") + "wom/");
            }
           
                foreach (Level l in Level.Levels)
                {
                    if (!Directory.Exists(path + l + ".wom.property"))
                    {
                        Logger.Log("[WoM] Cfg for " + l + " not found. Creating");
                        CreateFile(l, path);
                    }
                }

                foreach (var s in Directory.GetFiles(ServerSettings.GetSetting("configpath") + "wom/", "*.wom.property"))
                {
                    LoadFile(s);
                    Logger.Log("[WoM] Config found. Loading: " + s);
                }
            }

        public void LoadFile(string path) {
            if (!path.EndsWith(".wom.property"))
                return;
            var lines = File.ReadAllLines(path);
            if (lines.Count() != 2) {
                Logger.Log(String.Format("{0} was formatted incorrectly, this file will not be loaded", path), Color.Red, Color.Black);
                return;
            }
            var level = Level.FindLevel(lines[0].Split('=')[1].Trim());
            if(level == null){
                Logger.Log(String.Format("{0} was formatted incorrectly (level not found), this file will not be loaded", path), Color.Red, Color.Black);
                return;
            }
            LevelsWithTextures.Add(level);
            var cfg = lines[1].Split('=')[1].Trim();
            if (cfg == null)
            {
                return;
            }
            else
            {
                level.ExtraData["WomConfig"] = cfg;
            }
        }
        public void CreateFile(Level l, string path)
        {
            if (Directory.Exists(path + ".wom.property"))
            {
                return;
            }
            else
            {
                using (var writer = File.CreateText(path + l + ".wom.property"))
                    foreach (var node in _values)
                        writer.WriteLine(node.Key + " = " + node.Value);
            }
        }

        public void CreateConfigFile(string path) {
            using (var writer = File.CreateText(path)) {
                foreach (var node in _cgfvalues) {
                    writer.WriteLine(node.Key + " = " + node.Value);
                }
            }
        }
        
        public override void Save() {

        }
        public override List<MCForge.Utils.Settings.SettingNode> Values {
            get { return _values; }
        }

        public override string PropertiesPath {
            get { return ""; }
        }
    }
}
