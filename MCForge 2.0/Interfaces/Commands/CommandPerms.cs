using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Core;
using MCForge.Utilities;
using MCForge.Utils;

namespace MCForge.Interface.Command
{
    public static class ExtraCommandPerms
    {
        public static List<ExtraCommandPerm> extraperms = new List<ExtraCommandPerm>();

        public static void AddDefaultPerms()
        {
            if (!Add(Command.Find("review"), "reviewnextperm", 80, 1, "The minimal rank that can use review next")) { Logger.Log("Error adding reviewnextperm extra permission!", LogType.Error); }
            if (!Add(Command.Find("review"), "reviewclearperm", 80, 2, "The minimal rank that can clear the review queue")) { Logger.Log("Error adding reviewclearperm extra permission!", LogType.Error); }
        }
        public static ExtraCommandPerm Find(string name)
        {
            List<ExtraCommandPerm> found = new List<ExtraCommandPerm>();
            foreach (ExtraCommandPerm ecp in extraperms) { if (name == ecp.Name) { found.Add(ecp); } }
            if (found.Count == 1) { return found[0]; }
            return null;
        }
        public static ExtraCommandPerm Find(string name)
        {
            List<ExtraCommandPerm> found = new List<ExtraCommandPerm>();
            foreach (ExtraCommandPerm ecp in extraperms) { if (name == ecp.Name) { found.Add(ecp); } }
            if (found.Count == 1) { return found[0]; }
            return null;
        }
        /// <summary>
        /// Add an extra permission for a command
        /// </summary>
        /// <param name="command">The command that the permission will be assigned to</param>
        /// <param name="name">The name of the permission</param>
        /// <param name="permission">Permission you want to be used</param>
        /// <param name="number">The number of the permission for the command</param>
        /// <param name="description">Description of the permission</param>
        public static bool Add(ICommand command, string name, byte permission, int number, string description)
        {
            byte o; int o1;
            if (command == null || String.IsNullOrWhiteSpace(name.Trim()) || !byte.TryParse(permission.ToString(), out o) || !int.TryParse(number.ToString(), out o1) || String.IsNullOrWhiteSpace(description)) { return false; }
            ExtraCommandPerm ecp = new ExtraCommandPerm();
            ecp.Command = command;
            ecp.Name = name;
            ecp.Permission = permission;
            ecp.Number = number;
            ecp.Description = description;
            extraperms.Add(ecp);
            return true;
        }
        
    }
    public class ExtraCommandPerm
    {
        /// <summary>
        /// The command that the permission will be assigned to
        /// </summary>
        public ICommand Command;
        /// <summary>
        /// The name of the command
        /// </summary>
        public string Name;
        /// <summary>
        /// Permission you want to be used
        /// </summary>
        public byte Permission;
        /// <summary>
        /// The number of the permission for the command
        /// </summary>
        public int Number;
        /// <summary>
        /// Description of the permission
        /// </summary>
        public string Description;
    }
}
