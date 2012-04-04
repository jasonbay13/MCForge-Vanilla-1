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
using System.Linq;
using System.Text;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using System.IO;

namespace MCForge.Groups
{
    /// <summary>
    /// All the default permission values
    /// </summary>
    /// <remarks></remarks>
    public enum Permission : byte
    {
        Guest = 0,
        Builder = 30,
        AdvBuilder = 50,
        Operator = 80,
        SuperOP = 100,
        Owner = 120
    }
    /// <summary>
    /// The main permission system for MCForge
    /// </summary>
    /// <remarks></remarks>
    public class PlayerGroup
    {
        /// <summary>
        /// A list of all the available groups 
        /// </summary>
        public static List<PlayerGroup> groups = new List<PlayerGroup>();


        /// <summary>
        /// A list of all the players in the group (includes offline players)
        /// </summary>
        public List<string> players = new List<string>();
        /// <summary>
        /// The name of the group
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The permission level of the group.
        /// </summary>
        public byte permission { get; set; }
        /// <summary>
        /// The colour of the group.
        /// </summary>
        public string colour { get; set; }

        string _file;
        /// <summary>
        /// The filename to save the group list into
        /// </summary>
        public string file
        {
            get
            {
                return "ranks/" + _file;
            }
            set
            {
                _file = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerGroup"/> class.
        /// </summary>
        /// <param name="perm">The permission level of the group.</param>
        /// <param name="name">The name of the group.</param>
        /// <param name="colour">The colour of the group.</param>1
        /// <param name="file">The filename to save the group player list in</param>
        /// <remarks></remarks>
        public PlayerGroup(byte perm, string name, string colour, string file)
        {
            foreach (PlayerGroup g in groups.ToArray())
            {
                if (name.ToLower() == g.name.ToLower())
                {
                    throw new ArgumentException("Cannot have 2 groups of the same name");
                }
            }

            string file1 = "ranks/" + file;
            if (!Directory.Exists(Path.GetDirectoryName(file1)))
                Directory.CreateDirectory(Path.GetDirectoryName(file1));
            if (!File.Exists(file1))
            {
                File.Create(file1).Close();
                Server.Log("[Groups] " + file + " was created", ConsoleColor.DarkGreen, ConsoleColor.Black);
            }
            permission = perm;
            this.name = name;
            this.colour = colour;
            this.file = file;

            LoadGroup();
            groups.Add(this);
        }

        /// <summary>
        /// Sends the message to the group.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public void SendMessage(string message)
        {
            foreach (Player p in Server.Players.ToArray())
            {
                if (p.group == this)
                    p.SendMessage(message);
            }
        }

        /// <summary>
        /// Determines whether this instance can execute the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns><c>true</c> if this instance can execute the specified command; otherwise, <c>false</c>.</returns>
        public bool CanExecute(ICommand command)
        {
            if (command.Permission <= permission)
                return true;
            return false;
        }

        /// <summary>
        /// Saves the group to disk.
        /// </summary>
        /// <returns><c>true</c> if no errors saving</returns>
        /// <remarks></remarks>
        public bool SaveGroup()
        {
            //try
            //{
                if (!Directory.Exists(Path.GetDirectoryName(file)))
                    Directory.CreateDirectory(Path.GetDirectoryName(file));

                TextWriter o = new StreamWriter(file);
                foreach (string s in players.ToArray())
                    o.WriteLine(s.ToLower());
                o.Flush();
                o.Close();
                return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        /// <summary>
        /// Loads the group from disk.
        /// </summary>
        /// <returns><c>true</c> if no errors when loading</returns>
        /// <remarks></remarks>
        public bool LoadGroup()
        {
            try
            {
                string line;
                TextReader file = new StreamReader(this.file);

                while ((line = file.ReadLine()) != null)
                {
                    Server.Log(this.file + ":" + line);
                    if (!string.IsNullOrEmpty(line))
                        if (!players.Contains(line.ToLower()))
                            players.Add(line.ToLower());
                }
                file.Close();
                file.Dispose();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void AddPlayer(Player p)
        {
            p.group = this;
            players.Add(p.USERNAME.ToLower());
            SaveGroup();
        }

        /// <summary>
        /// Initializes the default groups.
        /// </summary>
        /// <remarks></remarks>
        public static void InitDefaultGroups()
        {
            new PlayerGroup((byte)Permission.Guest, "Guest", Colors.white, "guests.txt");
            new PlayerGroup((byte)Permission.Builder, "Builder", Colors.green, "builders.txt");
            new PlayerGroup((byte)Permission.AdvBuilder, "AdvBuilder", Colors.lime, "advbuilders.txt");
            new PlayerGroup((byte)Permission.Operator, "Operator", Colors.purple, "ops.txt");
            new PlayerGroup((byte)Permission.SuperOP, "SuperOp", Colors.maroon, "superops.txt");
            new PlayerGroup((byte)Permission.Owner, "Owner", Colors.blue, "owners.txt");
        }

        /// <summary>
        /// Checks if the specified group exists
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool Exists(string name)
        {
            foreach (PlayerGroup g in PlayerGroup.groups)
            {
                if (g.name.ToLower() == name.ToLower())
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Finds the specified group.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        public static PlayerGroup Find(string name)
        {
            if (name == "op" && !Exists(name)) name = "operator";

            foreach (PlayerGroup g in PlayerGroup.groups)
            {
                if (g.name.ToLower() == name.ToLower())
                    return g;
            }

            return null;
        }
    }
}
