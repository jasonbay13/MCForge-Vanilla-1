using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Utilities.Settings;
using System.IO;
using MCForge.World;
using MCForge.Core;

namespace Plugins.WomPlugin {
    class WomSettings : ExtraSettings {
        private readonly List<MCForge.Utilities.Settings.SettingNode> _cgfvalues = new List<MCForge.Utilities.Settings.SettingNode>() {
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
        public static List<Level> LevelsWithTextures { get; private set; }

        public override string SettingsName {
            get { return "WomSettings"; }
        }

        public override void OnLoad() {
            LevelsWithTextures = new List<Level>();
            if (!Directory.Exists(ServerSettings.GetSetting("configpath") + "wom/")) {
                Directory.CreateDirectory(ServerSettings.GetSetting("configpath") + "wom/");
                File.WriteAllText(ServerSettings.GetSetting("configpath") + "wom/main.cgf", Config, Encoding.UTF8);
                LoadFile(ServerSettings.GetSetting("configpath") + "wom/main.cgf");
            }
            else {
                foreach (var s in Directory.GetFiles(ServerSettings.GetSetting("configpath") + "wom/", "*.cgf"))
                    LoadFile(s);
            }

        }

        public void LoadFile(string path) {
            if (!path.EndsWith(".cgf"))
                return;
            var lines = File.ReadAllLines(path);
            int start = path.LastIndexOf('/');
            int end =  path.LastIndexOf('.') - start -1;
            var level = Server.FindLevel(path.Substring( start , end));
            if (level == null) {
                Server.Log(String.Format("{0} was formatted incorrectly, this file will not be loaded", path), ConsoleColor.Red, ConsoleColor.Black);
                return;
            }
            LevelsWithTextures.Add(level);
            level.ExtraData.Add("WomConfig", Config);

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

        public override void SetDefaults() {

        }

        public override List<MCForge.Utilities.Settings.SettingNode> Values {
            get { return null; }
        }

        public string Config {
            get {
                var s = new StringBuilder();
                foreach (var node in _cgfvalues)
                    s.Append(node.Key + "=" + node.Value).Append('\n');
                return s.ToString();
            }
        }
    }
}
