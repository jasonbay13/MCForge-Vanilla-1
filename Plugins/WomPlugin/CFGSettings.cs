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
using System.IO;
using System.Linq;
using MCForge.World;
using MCForge.Core;
using MCForge.Utils;
using MCForge.Utils.Settings;

namespace Plugins.WoMPlugin
{
    public class CFGSettings : ExtraSettings
    {
        #region Variables
        private Level l;
        private List<SettingNode> nodes = new List<SettingNode>() {
            new SettingNode("server.name", ServerSettings.GetSetting("ServerName"), "The name of your server. (Top right line)"),
            new SettingNode("server.detail", ServerSettings.GetSetting("MOTD"), "The MOTD of the server. (Second line)"),
            new SettingNode("detail.user", "Welcome to my server! $name", "The User Detail Line text. (Third line)"),
            new SettingNode("server.sendwomid","true", "Causes the client to send the server a /womid (VERSION) message upon load."),
            new SettingNode("environment.fog", "1377559", "The RGB value (Decimal, not Hex) of the colour to use for the fog."),
            new SettingNode("environment.sky", "1377559", "The RGB value (Decimal, not Hex) of the colour to use for the sky."),
            new SettingNode("environment.cloud", "1377559", "The RGB value (Decimal, not Hex) of the colour to use for the clouds."),
            new SettingNode("environment.level", "0", "The elevation of \"ground level\" to use for this map. (Affects where the \"sea\" is on the map."),
            new SettingNode("environment.edge", "685cfceb13ccee86d3b93f163f4ac6e4a28347bf", null),
            new SettingNode("environment.terrain", "f3dac271d7bce9954baad46e183a6a910a30d13b", null),
            new SettingNode("environment.side", "7c0fdebeb6637929b9b3170680fa7a79b656c3f7", null),
            };
        #endregion
        #region ExtraSettings Members
        public override string SettingsName { get { return "CFGSettings"; } }

        public CFGSettings(Level l)
        {
            this.l = l;
        }

        public override void OnLoad()
        {
            Logger.Log("CFGSettings loaded for " + l);

            if (!FileUtils.FileExists(PropertiesPath))
            {
                if (Server.DebugMode)
                {
                    Logger.Log("[WoMTextures] Cfg file for " + l.Name + " not found. Creating.");
                }
                CreateDefaultCFG();
            }
            LoadFile(); //Loads ExtraData
            nodes = LoadSettings();
        }

        public override void Save()
        {
            using (var writer = File.CreateText(PropertiesPath))
            {
                foreach (var node in Values)
                {
                    writer.WriteLine(node.Key + " = " + node.Value);
                }
            }
        }
        public override List<SettingNode> Values
        {
            get { return nodes; }
        }

        public override string PropertiesPath
        {
            get { return ServerSettings.GetSetting("configpath") + "WoMTexturing/" + l + ".cfg"; }
        }
        #endregion
        #region WoMSettings File Stuff
        private void CreateDefaultCFG()
        {
            using (var writer = File.CreateText(PropertiesPath))
            {
                foreach (var node in nodes)
                {
                    writer.WriteLine(node.Key + " = " + node.Value);
                }
            }
        }
        private void LoadFile()
        {
            if (l == null)
            {
                Logger.Log(String.Format("{0} was formatted incorrectly (level not found), this file will not be loaded.", PropertiesPath), LogType.Error);
                return;
            }
            string[] lines = File.ReadAllLines(PropertiesPath);
            if (lines.Length != 11)
            {
                Logger.Log(String.Format("{0} was formatted incorrectly, this file will not be loaded.", PropertiesPath), LogType.Error);
                return;
            }
            l.ExtraData.ChangeOrCreate<object, object>("WoMConfig", lines);
            if (Server.DebugMode)
            {
                Logger.Log("[WoMTextures] Loaded " + PropertiesPath + " succesfully!");
            }
        }
        #endregion
    }
}
