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
using System.Collections.Generic;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Groups;
using MCForge.Interface.Command;

namespace MCForge.Commands
{
    public class CmdEconomy : ICommand
    {
        public string Name { get { return "Economy"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "economy", "eco" });
        }
        int max = 16777215;
        public void Use(Player p, string[] args)
        {
            //TODO: Zoning after buying a level
            Economy.Load();
            int ln = args.Length;
            switch (args[0].ToLower())
            {
                #region Setup
                case "setup":
                case "set":
                    if (!(p is ConsolePlayer)) { if (p.Group.Permission < Economy.SetupPermission) { p.SendMessage("You do not have permission to use /economy setup!"); return; } }
                    if (!cargs(p, ln, 2)) return;
                    switch (args[1].ToLower())
                    {
                        case "economy":
                        case "eco":
                            if (!cargs(p, ln, 3)) return;
                            switch (args[2].ToLower())
                            {
                                case "enable":
                                case "on":
                                    if (!Economy.Enabled)
                                    {
                                        Economy.Enabled = true;
                                        p.SendMessage("Economy successfully enabled.");
                                    }
                                    else { p.SendMessage("Economy is already enabled!"); }
                                    break;
                                case "disable":
                                case "off":
                                    if (Economy.Enabled)
                                    {
                                        Economy.Enabled = false;
                                        p.SendMessage("Economy successfully enabled.");
                                    }
                                    else { p.SendMessage("Economy is already enabled!"); }
                                    break;
                                default:
                                    p.SendMessage("Invalid option!");
                                    return;
                            }
                            break;
                        #region Color
                        case "color":
                        case "colors":
                        case "colour":
                        case "colours":
                            if (!cargs(p, ln, 3)) return;
                            switch (args[2].ToLower())
                            {
                                case "enable":
                                case "on":
                                    if (!Economy.ColorsEnabled)
                                    {
                                        Economy.ColorsEnabled = true;
                                        p.SendMessage("Colors successfully enabled.");
                                    }
                                    else { p.SendMessage("Colors are already enabled!"); }
                                    break;
                                case "disable":
                                case "off":
                                    if (Economy.ColorsEnabled)
                                    {
                                        Economy.ColorsEnabled = false;
                                        p.SendMessage("Colors successfully disabled.");
                                    }
                                    else { p.SendMessage("Colors are already disabled!"); }
                                    break;
                                case "price":
                                case "cost":
                                    if (!cargs(p, ln, 4)) return;
                                    if (!CheckPrice(p, args[3])) return;
                                    Economy.ColorPrice = int.Parse(args[3]);
                                    p.SendMessage("Color price successfully set to &3" + args[3] + " " + Server.DefaultColor + Server.Moneys + ".");
                                    break;
                                default:
                                    p.SendMessage("Invalid option!");
                                    return;
                            }
                            break;
                        #endregion
                        #region Title
                        case "title":
                        case "titles":
                            switch (args[2].ToLower())
                            {
                                case "enable":
                                case "on":
                                    if (!Economy.TitlesEnabled)
                                    {
                                        Economy.TitlesEnabled = true;
                                        p.SendMessage("Titles successfully enabled.");
                                    }
                                    else { p.SendMessage("Titles are already enabled!"); }
                                    break;
                                case "disable":
                                case "off":
                                    if (Economy.TitlesEnabled)
                                    {
                                        Economy.TitlesEnabled = false;
                                        p.SendMessage("Titles successfully disabled.");
                                    }
                                    else { p.SendMessage("Titles are already disabled!"); }
                                    break;
                                case "price":
                                case "cost":
                                    if (!CheckPrice(p, args[3])) return;
                                    Economy.TitlePrice = int.Parse(args[3]);
                                    p.SendMessage("Title price successfully set to &3" + args[3] + " " + Server.DefaultColor + Server.Moneys + ".");
                                    break;
                                default:
                                    p.SendMessage("Invalid option!");
                                    return;
                            }
                            break;
                        #endregion
                        #region Level
                        case "level":
                        case "levels":
                        case "world":
                        case "worlds":
                        case "map":
                        case "maps":
                            if (!cargs(p, ln, 3)) return;
                            Economy.Level lvl = new Economy.Level();
                            switch (args[2].ToLower())
                            {
                                case "enable":
                                case "on":
                                    if (!Economy.LevelsEnabled)
                                    {
                                        Economy.LevelsEnabled = true;
                                        p.SendMessage("Levels successfully enabled.");
                                    }
                                    else { p.SendMessage("Levels are already enabled!"); }
                                    break;
                                case "disable":
                                case "off":
                                    if (Economy.LevelsEnabled)
                                    {
                                        Economy.LevelsEnabled = false;
                                        p.SendMessage("Levels successfully disabled.");
                                    }
                                    else { p.SendMessage("Levels are already disabled!"); }
                                    break;
                                case "add":
                                case "create":
                                case "new":
                                    if (!cargs(p, ln, 9)) return;
                                    if (!CheckPrice(p, args[8])) return;
                                    if (!Valid(args[4]) || !Valid(args[5]) || !Valid(args[6])) { p.SendMessage("Invalid level size!"); return; }
                                    if (Economy.LevelExists(args[3])) { p.SendMessage("A level named \"" + args[3] + "\" already exists in the economy level list. To rename or edit said level entry, use &a/economy setup level edit <old name> <new name> <new x> <new y> <new z> <new type> <new price>" + Server.DefaultColor + "."); return; }
                                    lvl.Name = args[3].ToLower();
                                    lvl.X = args[4];
                                    lvl.Y = args[5];
                                    lvl.Z = args[6];
                                    lvl.Type = args[7].ToLower();
                                    lvl.Price = int.Parse(args[8]);
                                    Economy.LevelList.Add(lvl);
                                    p.SendMessage("Successfully added the level preset \"" + args[3] + "\".");
                                    lvl = null;
                                    break;
                                case "delete":
                                case "remove":
                                case "del":
                                    if (!cargs(p, ln, 4)) return;
                                    if (!Economy.LevelExists(args[3])) { p.SendMessage("Level \"" + args[3] + "\" is not in the economy level list!"); return; }
                                    Economy.LevelList.Remove(lvl);
                                    p.SendMessage("Level preset \"" + lvl.Name + "\" successfully deleted");
                                    break;
                                case "edit":
                                case "change":
                                    if (!cargs(p, ln, 10)) return;
                                    if (!CheckPrice(p, args[9])) return;
                                    if (!Valid(args[5]) || !Valid(args[6]) || !Valid(args[7])) { p.SendMessage("Invalid level size!"); return; }
                                    if (!Economy.LevelExists(args[3])) { p.SendMessage("Level \"" + args[3] + "\" is not in the economy level list!"); return; }
                                    Economy.LevelList.Remove(Economy.FindLevel(args[3]));
                                    lvl = new Economy.Level();
                                    lvl.Name = args[4];
                                    lvl.X = args[5];
                                    lvl.Y = args[6];
                                    lvl.Z = args[7];
                                    lvl.Type = args[8];
                                    lvl.Price = int.Parse(args[9]);
                                    Economy.LevelList.Add(lvl);
                                    if (args[3] != args[4])
                                        p.SendMessage("Level \"" + args[3] + "\" successfully edited and/or renamed to \"" + args[4] + "\".");
                                    if (args[3] == args[4])
                                        p.SendMessage("Level \"" + args[3] + "\" successfully edited.");
                                    break;
                                default:
                                    p.SendMessage("Invalid option!");
                                    return;
                            }
                            break;
                        #endregion
                        #region Rank
                        case "rank":
                        case "ranks":
                        case "group":
                        case "groups":
                        case "promotion":
                        case "promotions":
                            if (!cargs(p, ln, 3)) return;
                            PlayerGroup group;
                            Economy.Rank rank;
                            switch (args[2].ToLower())
                            {
                                case "enable":
                                case "on":
                                    if (!Economy.RanksEnabled)
                                    {
                                        Economy.RanksEnabled = true;
                                        p.SendMessage("Ranks successfully enabled.");
                                    }
                                    else { p.SendMessage("Ranks are already enabled!"); }
                                    break;
                                case "disable":
                                case "off":
                                    if (Economy.RanksEnabled)
                                    {
                                        Economy.RanksEnabled = false;
                                        p.SendMessage("Ranks successfully disabled.");
                                    }
                                    else { p.SendMessage("Ranks are already disabled!"); }
                                    break;
                                case "add":
                                case "create":
                                    if (!cargs(p, ln, 5)) return;
                                    if (!CheckPrice(p, args[4])) return;
                                    if (Economy.RankExists(args[3])) { p.SendMessage("The rank \"" + args[3] + "\" is already in the economy system. To edit said rank entry, use &a/economy setup rank edit <rank name> <new price>" + Server.DefaultColor + "."); return; }
                                    group = PlayerGroup.Find(args[3]);
                                    if (group == null) { p.SendMessage("Rank \"" + args[3] + "\" not found!"); return; }
                                    rank = new Economy.Rank();
                                    rank.Group = group;
                                    rank.Price = int.Parse(args[4]);
                                    Economy.RankList.Add(rank);
                                    p.SendMessage("Rank \"" + rank.Group.Name + "\" added to the economy system with the price set to " + rank.Price + ".");
                                    rank = null;
                                    group = null;
                                    break;
                                case "remove":
                                case "delete":
                                case "del":
                                    if (!cargs(p, ln, 4)) return;
                                    if (!Economy.RankExists(args[3])) { p.SendMessage("The rank \"" + args[3] + "\" is not in the economy system."); return; }
                                    Economy.RankList.Remove(Economy.FindRank(args[3]));
                                    p.SendMessage("Rank \"" + args[3] + "\" successfully removed from the economy system.");
                                    break;
                                case "edit":
                                case "change":
                                    if (!cargs(p, ln, 5)) return;
                                    if (!CheckPrice(p, args[4])) return;
                                    if (!Economy.RankExists(args[3])) { p.SendMessage("The rank \"" + args[3] + "\" is not in the economy system."); return; }
                                    group = PlayerGroup.Find(args[3]);
                                    if (group == null) { p.SendMessage("Rank \"" + args[3] + "\" not found!"); return; }
                                    Economy.RankList.Remove(Economy.FindRank(args[3]));
                                    rank = new Economy.Rank();
                                    rank.Group = group;
                                    rank.Price = int.Parse(args[4]);
                                    Economy.RankList.Add(rank);
                                    p.SendMessage("Price for rank \"" + args[3] + "\" set to &3" + args[4] + " " + Server.DefaultColor + Server.Moneys + ".");
                                    rank = null;
                                    group = null;
                                    break;
                                default:
                                    p.SendMessage("Invalid option!");
                                    return;
                            }
                            break;
                        #endregion
                        default:
                            p.SendMessage("Invalid option!");
                            return;
                    }
                    Economy.Save();
                    break;
                #endregion
                #region Info
                case "info":
                case "information":
                    if (!cargs(p, ln, 2)) return;
                    switch (args[1].ToLower())
                    {
                        case "color":
                        case "colors":
                        case "colour":
                        case "colours":
                            p.SendMessage("Colors cost &3" + Economy.ColorPrice + " " + Server.DefaultColor + Server.Moneys + ".");
                            break;
                        case "title":
                        case "titles":
                            p.SendMessage("Titles cost &3" + Economy.ColorPrice + " " + Server.DefaultColor + Server.Moneys + ".");
                            break;
                        case "level":
                        case "levels":
                        case "map":
                        case "maps":
                        case "world":
                        case "worlds":
                            /*foreach (Economy.Level lvl in Economy.LevelList)
                            {
                                p.SendMessage("&a" + lvl.Name + ": &9Size: " + lvl.X + "&2x&9" + lvl.Y + "&2x&9" + lvl.Z + " &6Type: " + lvl.Type + " &3Price: " + lvl.Price);
                            }*/
                            if (!cargs(p, ln, 3)) return;
                            if (string.IsNullOrEmpty(args[2])) args[2] = "1";
                            List<Economy.Level> list = Economy.LevelList;
                            int Page = 0;
                            try { Page = int.Parse(args[2]); }
                            catch { p.SendMessage("Invalid number!"); return; }
                            int Pages = 0;
                            if (list.Count % 10 > 0) Pages = (list.Count / 10) + 1;
                            else Pages = list.Count / 10;
                            if (Page > Pages) { p.SendMessage("You can't view page &3" + Page + Server.DefaultColor + ", there are only &a" + Pages + Server.DefaultColor + " pages!"); return; }
                            p.SendMessage("Page &3" + Page + Server.DefaultColor + " of &a" + Pages);
                            for (int i = 0; i <= list.Count; i++)
                                if (list.Count / i == Page)
                                    p.SendMessage("&a" + list[i].Name + ": &9Size: " + list[i].X + "&2x&9" + list[i].Y + "&2x&9" + list[i].Z + " &6Type: " + list[i].Type + " &3Price: " + list[i].Price);
                            break;
                        case "rank":
                        case "ranks":
                        case "promotion":
                        case "promotions":
                        case "group":
                        case "groups":
                            foreach (Economy.Rank R in Economy.RankList)
                            {
                                p.SendMessage(R.Group.Color + R.Group.Name + Server.DefaultColor + ", Price: &3" + R.Price);
                            }
                            break;
                    }
                    break;
                #endregion
                #region Buy
                case "buy":
                    if (!Economy.Enabled) { p.SendMessage("Economy is not enabled!"); return; }
                    if (!cargs(p, ln, 2)) return;
                    switch (args[1].ToLower())
                    {
                        #region Title
                        case "title":
                            if (!cargs(p, ln, 3)) return;
                            if (!Economy.TitlesEnabled) { p.SendMessage("Titles are not enabled for the economy system!"); return; }
                            if (!CanAfford(p, Economy.TitlePrice)) return;
                            p.ExtraData["Money"] = (int)p.ExtraData["Money"] - Economy.TitlePrice;
                            Command.Find("title").Use(null, new string[] { p.Username, args[2] });
                            break;
                        #endregion
                        #region Color
                        case "color":
                        case "colour":
                            if (!cargs(p, ln, 3)) return;
                            if (!Economy.ColorsEnabled) { p.SendMessage("Titles are not enabled for the economy system!"); return; }
                            if (!CanAfford(p, Economy.ColorPrice)) return;
                            p.ExtraData["Money"] = (int)p.ExtraData["Money"] - Economy.ColorPrice;
                            Command.Find("color").Use(null, new string[] { p.Username, args[2] });
                            break;
                        #endregion
                        #region Level
                        case "level":
                        case "map":
                        case "world":
                            if (!cargs(p, ln, 4)) return;
                            Economy.Level lvl = Economy.FindLevel(args[2]);
                            if (lvl == null) { p.SendMessage("Level \"" + args[2] + "\" is not in the economy levels list!"); return; }
                            if (!CanAfford(p, lvl.Price)) return;
                            p.ExtraData["Money"] = (int)p.ExtraData["Money"] - lvl.Price;
                            Command.Find("newlvl").Use(null, new string[] { args[3], lvl.X, lvl.Z, lvl.Y, lvl.Type });
                            //System.Threading.Thread.Sleep(1000);
                            Command.Find("goto").Use(p, new string[] { args[3] });
                            break;
                        #endregion
                        #region Rank
                        case "rank":
                        case "promotion":
                            if (!cargs(p, ln, 3)) return;
                            Economy.Rank rank = Economy.FindRank(args[2]);
                            if (rank == null) { p.SendMessage("Rank \"" + args[2] + "\" is not in the economy ranks list."); return; }
                            if (!CanAfford(p, rank.Price)) return;
                            if (rank.Group.Permission <= p.Group.Permission) { p.SendMessage("You cannot purchase a rank with a permission lower than your own!"); return; }
                            p.ExtraData["Money"] = (int)p.ExtraData["Money"] - rank.Price;
                            rank.Group.AddPlayer(p);
                            Player.UniversalChat(p.Color + p.Username + Server.DefaultColor + " has purchased the rank " + rank.Group.Color + rank.Group.Name + Server.DefaultColor + " for &3" + rank.Price + " " + Server.DefaultColor + Server.Moneys + "!");
                            break;
                        #endregion
                    }
                    break;
                #endregion
                #region Help
                case "help":
                    if (!cargs(p, ln, 2)) return;
                    string msg = "";
                    foreach (string arg in args)
                        if (arg != args[0])
                            msg += arg + " ";
                    msg = msg.Trim().ToLower();
                    if (msg.StartsWith("info") || msg.StartsWith("information"))
                        p.SendMessage("/eco info <color/title/level/rank> - Gives you information and pricing for the selected option.");
                    switch (msg)
                    {
                        #region Setup
                        case "setup":
                            p.SendMessage("/eco setup economy <enable/disable> - Enable or disable economy.");
                            p.SendMessage("/eco setup color <enable/disable/price> - Enable, disable, or set the price of colors.");
                            p.SendMessage("/eco setup title <enable/disable/price> - Enable, disable, or set the price of titles.");
                            p.SendMessage("/eco setup level <add/delete/edit> - Add, delete, or edit buy-able levels.");
                            p.SendMessage("/eco setup rank <add/delete/edit> - Add, delete, or edit buy-able ranks.");
                            p.SendMessage("&aUse /eco help setup <color/title/level/rank> for more specific help.");
                            break;
                        case "setup economy":
                            p.SendMessage("/eco setup economy <enable/disable> - Enable or disable economy.");
                            break;
                        #region Color
                        case "setup color":
                            p.SendMessage("&3/eco setup color");
                            p.SendMessage("enable - Enables colors for the economy system.");
                            p.SendMessage("disable - Disables colors for the economy system.");
                            p.SendMessage("price <amount> - Sets the price of a color to <amount>.");
                            break;
                        case "setup color enable":
                            p.SendMessage("/eco setup color enable - Enables colors for the economy system.");
                            break;
                        case "setup color disable":
                            p.SendMessage("/eco setup color disable - Disables colors for the economy system.");
                            break;
                        case "setup color price":
                        case "setup color cost":
                            p.SendMessage("/eco setup color price <amount> - Sets the price of a color to <amount>.");
                            break;
                        #endregion
                        #region Title
                        case "setup title":
                            p.SendMessage("&3/eco setup title");
                            p.SendMessage("enable - Enables titles for the economy system.");
                            p.SendMessage("disable - Disables titles for the economy system.");
                            p.SendMessage("price <amount> - Sets the price of a title to <amount>.");
                            break;
                        case "setup title enable":
                            p.SendMessage("/eco setup title enable - Enables titles for the economy system.");
                            break;
                        case "setup title disable":
                            p.SendMessage("/eco setup title disable - Disables titles for the economy system.");
                            break;
                        case "setup title price":
                        case "setup title cost":
                            p.SendMessage("/eco setup title price <amount> - Sets the price of a title to <amount>.");
                            break;
                        #endregion
                        #region Level
                        case "setup level":
                            p.SendMessage("&3/eco setup level");
                            p.SendMessage("add <name> <X> <Y> <Z> <type> <price> - Adds a level preset named <name> with size values <X>, <Y>, <Z>, and type <type> to the economy system at a price of <price>.");
                            p.SendMessage("delete <name> - Deletes a level preset named <name>.");
                            p.SendMessage("edit <old name> <new name> <X> <Y> <Z> <type> <price> - Edits the level preset named <old name> to have the size values <X>, <Y>, <Z>, and type <type> at price <price>. If <new name> is different than <old name>, the preset will be renamed.");
                            break;
                        case "setup level add":
                        case "setup level create":
                        case "setup level new":
                            p.SendMessage("/eco setup level add <name> <X> <Y> <Z> <type> <price> - Adds a level preset named <name> with size values <X>, <Y>, <Z>, and type <type> to the economy system at a price of <price>.");
                            break;
                        case "setup level delete":
                        case "setup level remove":
                        case "setup level del":
                            p.SendMessage("/eco setup level delete <name> - Deletes a level preset named <name>.");
                            break;
                        case "setup level edit":
                        case "setup level change":
                            p.SendMessage("/eco setup level edit <old name> <new name> <X> <Y> <Z> <type> <price> - Edits the level preset named <old name> to have the size values <X>, <Y>, <Z>, and type <type> at price <price>. If <new name> is different than <old name>, the preset will be renamed.");
                            break;
                        #endregion
                        #region Rank
                        case "setup rank":
                            p.SendMessage("&3/eco setup rank");
                            p.SendMessage("add <rank> <price> - Allows <rank> to be purchased for <price> " + Server.Moneys + ".");
                            p.SendMessage("delete <rank> - Removes <rank> from the economy ranks list.");
                            p.SendMessage("edit <rank> <new price> - Changes the price of <rank> to <new price>.");
                            break;
                        case "setup rank add":
                        case "setup rank new":
                        case "setup rank create":
                            p.SendMessage("/eco setup rank add <rank> <price> - Allows <rank> to be purchased for <price> " + Server.Moneys + ".");
                            break;
                        case "setup rank delete":
                        case "setup rank remove":
                        case "setup rank del":
                            p.SendMessage("/eco delete <rank> - Removes <rank> from the economy ranks list.");
                            break;
                        case "setup rank edit":
                        case "setup rank change":
                            p.SendMessage("/eco edit <rank> <new price> - Changes the price of <rank> to <new price>.");
                            break;
                        #endregion
                        #endregion
                        #region Buy
                        case "buy":
                            p.SendMessage("/eco buy <color/title/level/rank> - Buy a color, title, level, or rank.");
                            p.SendMessage("&aUse /eco help buy <color/title/level/rank> for more specific help.");
                            break;
                        case "buy color":
                            p.SendMessage("/eco buy color <color> - Buy and apply the selected color.");
                            break;
                        case "buy title":
                            p.SendMessage("/eco buy title <title> - Buy and apply the selected title.");
                            break;
                        case "buy level":
                            p.SendMessage("/eco buy level <level> - Buy <level>. (The level will be zoned for you automatically.)");
                            break;
                        case "buy rank":
                            p.SendMessage("/eco buy rank <rank> - Buy <rank>.");
                            break;
                        #endregion
                        default:
                            p.SendMessage("Invalid option!");
                            return;
                    }
                    break;
                default:
                    p.SendMessage("Invalid option!");
                    return;
                #endregion
            }
        }
        public void Help(Player p)
        {
            p.SendMessage("Use /economy help <setup/buy/info> for more information.");
            p.SendMessage("Shortcut: /eco");
        }
        public bool CanAfford(Player p, int price)
        {
            if (price > (int)p.ExtraData["Money"]) { p.SendMessage("You cannot afford that! You need &3" + price + " " + Server.DefaultColor + Server.Moneys + ", but you only have &c" + p.ExtraData["Money"] + "!"); return false; }
            else
                return true;
        }
        public bool CheckPrice(Player p, string s)
        {
            int setprice = 0;
            try { setprice = int.Parse(s); }
            catch { p.SendMessage("Prices must be numbers!"); return false; }
            if (setprice > max) { p.SendMessage("The price cannot be over &3" + max + " " + Server.DefaultColor + Server.Moneys + "!"); return false; }
            return true;
        }
        public bool cargs(Player p, int args_length, int needed) // For the curious code-reader, it's called cargs because it stands for check args. C:
        {
            if (args_length < needed) { p.SendMessage("You did not enter enough arguments!"); return false; }
            return true;
        }
        public bool Valid(string value)
        {
            switch (ushort.Parse(value))
            {
                case 2:
                case 4:
                case 8:
                case 16:
                case 32:
                case 64:
                case 128:
                case 256:
                case 512:
                case 1024:
                case 2048:
                case 4096:
                case 8192:
                    return true;
            }
            return false;
        }
    }
}