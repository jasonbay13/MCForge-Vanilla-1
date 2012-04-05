using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MCForge.Core;
using System.Xml;

namespace MCForge.Groups
{
    class PlayerGroupProperties
    {
        public static void Save()
        {
            if (!Directory.Exists(ServerSettings.configPath)) Directory.CreateDirectory(ServerSettings.configPath);
            using (XmlWriter writer = XmlWriter.Create(ServerSettings.configPath + "groups.xml"))
            {
                writer.WriteStartDocument();
                //writer.WriteString("\r\n");
                writer.WriteStartElement("Groups");

                foreach (PlayerGroup group in PlayerGroup.groups.ToArray())
                {
                    writer.WriteString("\r\n\t");
                    writer.WriteStartElement("Group");

                    writer.WriteString("\r\n\t\t");
                    writer.WriteElementString("name", group.name);

                    writer.WriteString("\r\n\t\t");
                    writer.WriteElementString("permission", group.permission.ToString());

                    writer.WriteString("\r\n\t\t");
                    writer.WriteElementString("color", group.colour.ToString().Remove(0, 1));

                    writer.WriteString("\r\n\t");
                    writer.WriteEndElement();
                }

                writer.WriteString("\r\n");
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public void Load()
        {
            using (XmlReader reader = XmlReader.Create(ServerSettings.configPath + "groups.xml"))
            {
                string name; byte permission; string color;
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "name":
                            name = reader.ReadContentAsString();
                            break;
                        case "permission":
                            try
                            {
                                permission = byte.Parse(reader.ReadContentAsString());
                            }
                            catch
                            {
                                Server.Log("[Group Error] Group has a malformed permission, skipping", ConsoleColor.Red, ConsoleColor.White);
                            }
                            break;
                        case "color":
                            color = reader.ReadContentAsString();
                            break;
                    }
                }
            }
        }
    }
}
