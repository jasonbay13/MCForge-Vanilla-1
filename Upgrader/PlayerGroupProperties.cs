using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using MCForge.Utils;

namespace MCForge.Groups
{
	/// <summary>
	/// MCForge 6 Player Group Properties saver
	/// </summary>
    class PlayerGroupProperties
    {
        static string PropertiesPath = FileUtils.PropertiesPath + "groups.xml";
        public static void Save()
        {
            FileUtils.CreateDirIfNotExist(FileUtils.PropertiesPath);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.CloseOutput = true;
            settings.NewLineOnAttributes = true;

            using (XmlWriter writer = XmlWriter.Create(PropertiesPath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Groups");

                foreach (PlayerGroup group in PlayerGroup.Groups.ToArray())
                {
                    writer.WriteStartElement("Group");
                    writer.WriteElementString("name", group.Name);
                    writer.WriteElementString("permission", group.Permission.ToString());
                    writer.WriteElementString("color", group.Colour.ToString().Remove(0, 1));
                    writer.WriteElementString("maxblockchanges", group.MaxBlockChange.ToString());
                    writer.WriteElementString("file", group.File.Replace("ranks/", ""));
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
            try
            {
                PlayerGroup group = new PlayerGroup();
                using (XmlReader reader = XmlReader.Create(PropertiesPath))
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToLower())
                            {
                                case "name":
                                    group.Name = reader.ReadString();
                                    break;
                                case "permission":
                                    try { group.Permission = byte.Parse(reader.ReadString()); }
                                    catch { }
                                    break;
                                case "color":
                                    group.Colour = '&' + reader.ReadString();
                                    break;
                                case "file":
                                    group.File = reader.ReadString();
                                    break;
                                case "maxblockchanges":
                                    try { group.MaxBlockChange = int.Parse(reader.ReadString()); }
                                    catch { }
                                    break;
                            }
                        }
                        else if (group.Name != null)
                        {
                            try { 
                                group.add(); group = new PlayerGroup(); }
                            catch { //Logger.Log("Failed to add a group!", LogType.Error); }
                        	}
                            //break;
                        }
                    }
            }
            catch { }
            //CommandPermissionOverrides.Load();
        } 
    }
}