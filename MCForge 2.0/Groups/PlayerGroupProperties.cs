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

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.CloseOutput = true;
            settings.NewLineOnAttributes = true;

            using (XmlWriter writer = XmlWriter.Create(ServerSettings.configPath + "groups.xml", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Groups");

                foreach (PlayerGroup group in PlayerGroup.groups.ToArray())
                {
                    writer.WriteStartElement("Group");
                    writer.WriteElementString("name", group.name);
                    writer.WriteElementString("permission", group.permission.ToString());
                    writer.WriteElementString("color", group.colour.ToString().Remove(0, 1));

                    writer.WriteElementString("file", group.file);
                    //{
                    //    writer.WriteStartElement("players");
                    //    foreach (string s in group.players)
                    //    {
                    //        writer.WriteElementString("player", s);
                    //    }
                    //    writer.WriteEndElement();
                    //}
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public static void Load()
        {
            using (XmlReader reader = XmlReader.Create(ServerSettings.configPath + "groups.xml"))
            {
                PlayerGroup group = new PlayerGroup();

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name.ToLower())
                        {                         
                            case "name":
                                group.name = reader.ReadString();
                                Server.Log("[Group] Name: " + group.name);
                                break;
                            case "permission":
                                try { group.permission = byte.Parse(reader.ReadString()); }
                                catch { }
                                Server.Log("[Group] Permission: " + group.permission);
                                break;
                            case "color":
                                group.colour = '&' + reader.ReadString();
                                Server.Log("[Group] Color: " + group.colour);
                                break;
                            case "file":
                                group.file = reader.ReadString();
                                Server.Log("[Group] File: " + group.file);
                                break;
                            case "maxblockchanges":
                                try { group.maxBlockChange = int.Parse(reader.ReadString()); }
                                catch { }
                                Server.Log("[Group] Max Block Changes: " + group.maxBlockChange);
                                break;
                        }
                    }
                    else
                    {
                        try { group.add(); group = new PlayerGroup(); }
                        catch { }
                        break;
                    }
                }
            }
        }
    }
}
