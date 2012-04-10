using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MCForge.Core;
using System.Xml;
using MCForge.Utilities.Settings;
using System.Xml.XPath;

namespace MCForge.Groups
{
    class PlayerGroupProperties
    {
        public static void Save()
        {
            if (!Directory.Exists(ServerSettings.GetSetting("configpath"))) Directory.CreateDirectory(ServerSettings.GetSetting("configpath"));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.CloseOutput = true;
            settings.NewLineOnAttributes = true;

            using (XmlWriter writer = XmlWriter.Create(ServerSettings.GetSetting("configpath") + "groups.xml", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Groups");

                foreach (PlayerGroup group in PlayerGroup.groups.ToArray())
                {
                    writer.WriteStartElement("Group");
                    writer.WriteElementString("name", group.name);
                    writer.WriteElementString("permission", group.permission.ToString());
                    writer.WriteElementString("color", group.colour.ToString().Remove(0, 1));

                    writer.WriteElementString("file", group.file.Replace("ranks/", ""));
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
            string file = ServerSettings.GetSetting("configpath") + "groups.xml";
            if (!File.Exists(file)) Save();

            XPathDocument document = new XPathDocument(file);
            XPathNavigator nav = document.CreateNavigator();

            //expressions
            XPathExpression rankName, rankPerm, rankColor, rankFile;
            rankName = nav.Compile("/Groups/Group/name");       //name node
            rankPerm = nav.Compile("/Groups/Group/permission"); //permission node
            rankColor = nav.Compile("/Groups/Group/color");     //color node
            rankFile = nav.Compile("/Groups/Group/file");       //file node
            XPathNodeIterator rankNameIterator = nav.Select(rankName);
            XPathNodeIterator rankPermIterator = nav.Select(rankPerm);
            XPathNodeIterator rankColorIterator = nav.Select(rankColor);
            XPathNodeIterator rankFileIterator = nav.Select(rankFile);

            try
            {
                while (rankNameIterator.MoveNext())
                {
                    if (rankPermIterator.MoveNext() && rankColorIterator.MoveNext() && rankFileIterator.MoveNext())
                    {
                        XPathNavigator rank = rankNameIterator.Current.Clone();
                        XPathNavigator perm = rankPermIterator.Current.Clone();
                        XPathNavigator color = rankColorIterator.Current.Clone();
                        XPathNavigator save = rankFileIterator.Current.Clone();

                        PlayerGroup group = new PlayerGroup(perm.ValueAsInt, rank.Value, color.Value, save.Value);
                    }
                }
            }
            catch
            {
            }

        }
    }
}
