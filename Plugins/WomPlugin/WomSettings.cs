/*
Copyright 2011 MCForge
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
*/﻿
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

namespace Plugins.WomPlugin
{
    class WomSettings : ExtraSettings
    {
        private readonly List<SettingNode> _cfgvalues = new List<SettingNode>() {
            //new SettingNode("server.name", ServerSettings.GetSetting("ServerName"), null),
            //new SettingNode("server.detail", ServerSettings.GetSetting("motd"), null),
            new SettingNode("server.name", "Rawr!", null),
            new SettingNode("server.detail", "Okay", null),
            new SettingNode("detail.user", "Har Har", null),
            new SettingNode("environment.fog", "1377559", null),
            new SettingNode("environment.sky", "1377559", null),
            new SettingNode("environment.cloud", "1377559", null),
            new SettingNode("environment.level", "0", null),
            new SettingNode("environment.edge", "685cfceb13ccee86d3b93f163f4ac6e4a28347bf", null),
            new SettingNode("environment.terrain", "f3dac271d7bce9954baad46e183a6a910a30d13b", null),
            new SettingNode("environment.side", "7c0fdebeb6637929b9b3170680fa7a79b656c3f7", null),
            new SettingNode("server.sendwomid","true", null),
        };
        //Concept of the level name is the cfg name.
        /*private readonly List<SettingNode> _values = new List<SettingNode>() {
            //new SettingNode("LevelName", "main", null),
            //new SettingNode("ConfigPath", ServerSettings.GetSetting("configpath") + "main.cfg", null),
        };*/
        public override string SettingsName { get { return "WomSettings"; } }
        public List<Level> LevelsWithTextures { get; private set; }
        public string path = ServerSettings.GetSetting("configpath") + "wom/";


        public override void OnLoad()
        {
            Logger.Log("[WoM] WomSettings loaded!");
            LevelsWithTextures = new List<Level>();
            if (!Directory.Exists(path))
            {
                Logger.Log("[WoM] Config folder not found. Creating");
                Directory.CreateDirectory(path);
            }
            foreach (Level l in Level.Levels)
            {
                if (!Directory.Exists(path + l.Name + ".cfg"))
                {
                    Logger.Log("[WoM] Cfg for " + l.Name + " not found. Creating");
                    CreateConfigFile(l);
                }
            }

            foreach (var s in Directory.GetFiles(path, "*.cfg"))
            {
                Logger.Log("[WoM] Config found. Loading: " + s);
                LoadFile(s);
            }
        }

        public void LoadFile(string levelPath)
        {
            if (!levelPath.EndsWith(".cfg"))
                return;
            var levelName = Path.GetFileNameWithoutExtension(levelPath);
            Level level = Level.FindLevel(levelName);
            if (level == null)
            {
                Logger.Log(String.Format("{0} was formatted incorrectly (level not found), this file will not be loaded", levelPath), Color.Red, Color.Black);
            }
            string[] lines = File.ReadAllLines(levelPath);
            if (lines.Count() != 11)
            {
                Logger.Log(String.Format("{0} was formatted incorrectly, this file will not be loaded", levelPath), Color.Red, Color.Black);
                return;
            }
            LevelsWithTextures.Add(level);
            //Check for valid numbers here
            level.ExtraData.CreateIfNotExist<object, object>("WomConfig", lines);
            level.ExtraData["WomConfig"] = lines;
            Logger.Log("[WoM] Succesfully loaded!");
        }

        public void CreateConfigFile(Level l)
        {
            using (var writer = File.CreateText(path + l.Name + ".cfg"))
            {
                foreach (var node in _cfgvalues)
                {
                    writer.WriteLine(node.Key + " = " + node.Value);
                }
                writer.Close();
            }
        }

        public override void Save()
        {

        }
        public override List<SettingNode> Values
        {
            get { return _cfgvalues; }
        }

        public override string PropertiesPath
        {
            get { return ""; }
        }
    }
}
