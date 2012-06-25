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
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Utils;
using MCForge.World;

namespace Plugins.WoMPlugin
{
    public class CmdCompass : ICommand
    {
        public string Name { get { return "WoMCompass"; } }
        public CommandTypes Type { get { return CommandTypes.Custom; } }
        public string Author { get { return "Gamemakergm"; } }
        public int Version { get { return 1; ; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            p.ExtraData.CreateIfNotExist<object, object>("WoMCompass", false);
            switch (args.Length)
            {
                case 0:
                    p.ExtraData["WoMCompass"] = !(bool)(p.ExtraData.GetIfExist<object, object>("WoMCompass") ?? false);
                    if ((bool)p.ExtraData["WoMCompass"] == true)
                    {
                        p.SendMessage("Compass activated!");
                        WOM.SendDetail(p, "Look around to activate the Compass!");
                    }
                    else
                    {
                        p.SendMessage("Compass deactivated!");
                        DeactiveMessage(p);
                    }
                    break;
                case 1:
                    switch (args[0])
                    {
                        case "on":
                            p.ExtraData["WoMCompass"] = true;
                            p.SendMessage("Compass activated!");
                            WOM.SendDetail(p, "Look around to activate the Compass!");
                            break;
                        case "off":
                            p.ExtraData["WoMCompass"] = false;
                            p.SendMessage("Compass deactivated!");
                            DeactiveMessage(p);
                            break;
                        default:
                            p.SendMessage("Invalid argument!");
                            break;
                    }
                    break;
                default:
                    p.SendMessage("Invalid arguments!");
                    break;
            }
        }
        void DeactiveMessage(Player p)
        {
            CFGSettings a = (CFGSettings)PluginWoMTextures.CFGDict.GetIfExist<Level, CFGSettings>(p.Level);
            WOM.SendDetail(p, a.GetSetting("detail.user"));
        }
        public void Help(Player p)
        {
            p.SendMessage("/compass [on/off] - Toggles the Compass.");
        }
        public void Initialize()
        {
            Command.AddReference(this, "compass");
        }
    }
}
