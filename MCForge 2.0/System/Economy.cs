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
*/
using System;
using System.Collections.Generic;
using System.IO;
using MCForge.Groups;

namespace MCForge.Core
{
    public class Economy
    {
        public static byte SetupPermission = 80;

        public static bool Enabled = false;

        public static bool TitlesEnabled = false;
        public static int TitlePrice = 100;

        public static bool ColorsEnabled = false;
        public static int ColorPrice = 100;

        public static bool LevelsEnabled = false;
        public static List<Economy.Level> LevelList = new List<Economy.Level>();
        public class Level
        {
            public string Name;
            public string X;
            public string Y;
            public string Z;
            public string Type;
            public int Price;
        }

        public static bool RanksEnabled = false;
        public static List<Rank> RankList = new List<Rank>();
        public class Rank
        {
            public PlayerGroup Group;
            public int Price;
        }

        public static void Load()
        {
            RankList.Clear();
            LevelList.Clear();
            foreach (string line in File.ReadAllLines("properties/economy.properties"))
            {
                string[] s = line.Split(':');
                if (s[0] == "SetupPermission")
                    SetupPermission = byte.Parse(s[1]);
                if (s[0] == "EconomyEnabled")
                    Enabled = bool.Parse(s[1]);
                if (s[0] == "TitlesEnabled")
                    TitlesEnabled = bool.Parse(s[1]);
                if (s[0] == "TitlePrice")
                    TitlePrice = int.Parse(s[1]);
                if (s[0] == "ColorsEnabled")
                    ColorsEnabled = bool.Parse(s[1]);
                if (s[0] == "ColorPrice")
                    ColorPrice = int.Parse(s[1]);
                if (s[0] == "RanksEnabled")
                    RanksEnabled = bool.Parse(s[1]);
                if (s[0] == "Rank")
                {
                    Rank rank = new Rank();
                    rank.Group = PlayerGroup.Find(s[1]);
                    rank.Price = int.Parse(s[2]);
                    RankList.Add(rank);
                }
                if (s[0] == "LevelsEnabled")
                    LevelsEnabled = bool.Parse(s[1]);
                if (s[0] == "Level")
                {
                    Economy.Level lvl = new Economy.Level();
                    lvl.Name = s[1];
                    lvl.X = s[2];
                    lvl.Y = s[3];
                    lvl.Z = s[4];
                    lvl.Type = s[5];
                    lvl.Price = int.Parse(s[6]);
                    LevelList.Add(lvl);
                }
            }
        }

        public static void CreateFile()
        {
            using (StreamWriter SW = File.CreateText("properties/economy.properties"))
            {
                SW.WriteLine("# All prices must be 16777215 or less");
                SW.WriteLine();
                SW.WriteLine("EconomyEnabled:False");
                SW.WriteLine();
                SW.WriteLine("# Permission number | Permission of rank allowed to use /eco setup");
                SW.WriteLine("SetupPermission:80");
                SW.WriteLine();
                SW.WriteLine("TitlesEnabled:False");
                SW.WriteLine("TitlePrice:100");
                SW.WriteLine();
                SW.WriteLine("ColorsEnabled:False");
                SW.WriteLine("ColorPrice:100");
                SW.WriteLine();
                SW.WriteLine("RanksEnabled:False");
                SW.WriteLine("# Rank Format:");
                SW.WriteLine("# Rank:<RankName>:<Price>");
                SW.WriteLine("Rank:AdvBuilder:1000");
                SW.WriteLine();
                SW.WriteLine("LevelsEnabled:" + LevelsEnabled);
                SW.WriteLine("# Level Format:");
                SW.WriteLine("# Level:<LevelName>:<X>:<Y>:<Z>:<Type>:<Price>");
                SW.WriteLine("Level:smallflat:64:64:64:flat:1000");
                SW.Close();
                SW.Dispose();
            }
        }

        public static void Save()
        {
            string n = Environment.NewLine;
            List<string> l = new List<string>();
            l.Add("# All prices must be 16777215 or less");
            l.Add(n);
            l.Add("EconomyEnabled:" + Enabled);
            l.Add(n);
            l.Add("# All prices must be 16777215 or less");
            l.Add(n);
            l.Add("EconomyEnabled:" + Enabled);
            l.Add(n);
            l.Add("# Permission number | Permission of rank allowed to use /eco setup");
            l.Add("SetupPermission:" + SetupPermission);
            l.Add(n);
            l.Add("TitlesEnabled:" + TitlesEnabled);
            l.Add("TitlePrice:" + TitlePrice);
            l.Add(n);
            l.Add("ColorsEnabled:" + ColorsEnabled);
            l.Add("ColorPrice:" + ColorPrice);
            l.Add(n);
            l.Add("RanksEnabled:" + RanksEnabled);
            l.Add("# Rank Format:");
            l.Add("# Rank:<RankName>:<Price>");
            foreach (Rank rank in RankList)
            {
                l.Add("Rank:" + rank.Group.Name + ":" + rank.Price);
            }
            l.Add(n);
            l.Add("LevelsEnabled:" + LevelsEnabled);
            l.Add("# Level Format:");
            l.Add("# Level:<LevelName>:<X>:<Y>:<Z>:<Type>:<Price>");
            foreach (Economy.Level lvl in LevelList)
            {
                l.Add("Level:" + lvl.Name + ":" + lvl.X + ":" + lvl.Y + ":" + lvl.Z + ":" + lvl.Type + ":" + lvl.Price);
            }
            File.WriteAllLines("properties/economy.properties", l.ToArray());
            l = null;
            /*File.Delete("properties/economy.properties");
            using (StreamWriter SW = File.CreateText("properties/economy.properties"))
            {
                SW.WriteLine("# All prices must be 16777215 or less");
                SW.WriteLine();
                SW.WriteLine("EconomyEnabled:" + Enabled);
                SW.WriteLine();
                SW.WriteLine("# Permission number | Permission of rank allowed to use /eco setup");
                SW.WriteLine("SetupPermission:" + SetupPermission);
                SW.WriteLine();
                SW.WriteLine("TitlesEnabled:" + TitlesEnabled);
                SW.WriteLine("TitlePrice:" + TitlePrice);
                SW.WriteLine();
                SW.WriteLine("ColorsEnabled:" + ColorsEnabled);
                SW.WriteLine("ColorPrice:" + ColorPrice);
                SW.WriteLine();
                SW.WriteLine("RanksEnabled:" + RanksEnabled);
                SW.WriteLine("# Rank Format:");
                SW.WriteLine("# Rank:<RankName>:<Price>");
                foreach (Rank rank in RankList)
                {
                    SW.WriteLine("Rank:" + rank.Group.Name + ":" + rank.Price);
                }
                SW.WriteLine();
                SW.WriteLine("LevelsEnabled:" + LevelsEnabled);
                SW.WriteLine("# Level Format:");
                SW.WriteLine("# Level:<LevelName>:<X>:<Y>:<Z>:<Type>:<Price>");
                foreach (Economy.Level lvl in LevelList)
                {
                    SW.WriteLine("Level:" + lvl.Name + ":" + lvl.X + ":" + lvl.Y + ":" + lvl.Z + ":" + lvl.Type + ":" + lvl.Price);
                }
                SW.Close();
            }*/
        }

        public static Economy.Level FindLevel(string name)
        {
            Economy.Level found = null;
            foreach (Economy.Level lvl in LevelList)
                if (lvl.Name.ToLower() == name.ToLower())
                    found = lvl;
            return found;
        }

        public static Rank FindRank(string name)
        {
            Rank found = null;
            foreach (Rank rank in RankList)
                if (rank.Group.Name.ToLower() == name.ToLower())
                    found = rank;
            return found;
        }

        public static bool LevelExists(string name)
        {
            foreach (Economy.Level lvl in LevelList)
                if (lvl.Name.ToLower() == name.ToLower())
                    return true;
            return false;
        }

        public static bool RankExists(string name)
        {
            foreach (Rank rank in RankList)
                if (rank.Group.Name.ToLower() == name.ToLower())
                    return true;
            return false;
        }
    }
}