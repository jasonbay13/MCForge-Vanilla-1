using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;

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
    public class Group
    {
        public static List<Group> groups = new List<Group>();

        /// <summary>
        /// The name of the group
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The permission level of the group.
        /// </summary>
        public byte permission { get; set; }

		/// <summary>
		/// The color of the group.
		/// </summary>
		public string color { get; set; }
		/// <summary>
		/// The colour of the group.
		/// </summary>
		public string colour { get { return color; } set { this.color = value;} }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="perm">The permission level of the group.</param>
        /// <param name="name">The name of the group.</param>
        /// <param name="colour">The colour of the group.</param>
        /// <remarks></remarks>
        public Group(byte perm, string name, string colour)
        {
            foreach (Group g in groups.ToArray())
            {
                if (name.ToLower() == g.name.ToLower())
                {
                    throw new ArgumentException("Cannot have 2 groups of the same name");
                }
            }
            permission = perm;
            this.name = name;
            this.colour = colour;

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

        public static void InitDefaultGroups()
        {
            new Group((byte)Permission.Guest, "Guest", Colors.white);
            new Group((byte)Permission.Builder, "Builder", Colors.green);
            new Group((byte)Permission.AdvBuilder, "AdvBuilder", Colors.lime);
            new Group((byte)Permission.Operator, "Operator", Colors.purple);
            new Group((byte)Permission.SuperOP, "SuperOp", Colors.maroon);
            new Group((byte)Permission.Owner, "Owner", Colors.blue);
        }

	}
}
