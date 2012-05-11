using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MCForge.Core;
using System.Xml;
using MCForge.Utilities.Settings;
using System.Xml.XPath;
using MCForge.Utilities;

namespace MCForge.Groups
{
    class PlayerGroupProperties
    {
        static string PropertiesPath = FileUtils.PropertiesPath + "groups.xml";
        public static void Save()
        {
            if (!Directory.Exists(FileUtils.PropertiesPath)) Directory.CreateDirectory(FileUtils.PropertiesPath);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.CloseOutput = true;
            settings.NewLineOnAttributes = true;

            using (XmlWriter writer = XmlWriter.Create(PropertiesPath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Groups");

                foreach (PlayerGroup group in PlayerGroup.groups.ToArray())
                {
                    writer.WriteStartElement("Group");
                    writer.WriteElementString("name", group.name);
                    writer.WriteElementString("permission", group.permission.ToString());
                    writer.WriteElementString("color", group.colour.ToString().Remove(0, 1));
                    writer.WriteElementString("maxblockchanges", group.maxBlockChange.ToString());
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
			try {
				PlayerGroup group = new PlayerGroup();
				using (XmlReader reader = XmlReader.Create(PropertiesPath))
					while (reader.Read()) {
						if (reader.IsStartElement()) {
							switch (reader.Name.ToLower()) {
								case "name":
									group.name = reader.ReadString();
									break;
								case "permission":
									try { group.permission = byte.Parse(reader.ReadString()); } catch { }
									break;
								case "color":
									group.colour = '&' + reader.ReadString();
									break;
								case "file":
									group.file = reader.ReadString();
									break;
								case "maxblockchanges":
									try { group.maxBlockChange = int.Parse(reader.ReadString()); } catch { }
									break;
							}
						} else {
                            try { group.add(); group = new PlayerGroup(); }
                            catch { Logger.Log("Failed to add a group!", LogType.Error); }
							break;
						}
					}
			} catch {}
            CommandPermissionOverrides.Load();
        } 
    }
}
