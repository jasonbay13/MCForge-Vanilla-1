using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MCForge.Core;
using System.Xml;
using MCForge.Utils.Settings;
using System.Xml.XPath;
using MCForge.Utils;

namespace MCForge.Groups
{
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
    }
}