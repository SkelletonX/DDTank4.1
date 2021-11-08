using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace Game.Base.Config
{
	public class XMLConfigFile : ConfigElement
	{
		public XMLConfigFile()
			: base(null)
		{
		}

		protected XMLConfigFile(ConfigElement parent)
			: base(parent)
		{
		}

		protected bool IsBadXMLElementName(string name)
		{
			if (name == null)
			{
				return false;
			}
			if (name.IndexOf("\\") != -1)
			{
				return true;
			}
			if (name.IndexOf("/") != -1)
			{
				return true;
			}
			if (name.IndexOf("<") != -1)
			{
				return true;
			}
			if (name.IndexOf(">") != -1)
			{
				return true;
			}
			return false;
		}

		protected void SaveElement(XmlTextWriter writer, string name, ConfigElement element)
		{
			bool flag = IsBadXMLElementName(name);
			if (element.HasChildren)
			{
				if (name == null)
				{
					name = "root";
				}
				if (flag)
				{
					writer.WriteStartElement("param");
					writer.WriteAttributeString("name", name);
				}
				else
				{
					writer.WriteStartElement(name);
				}
				foreach (DictionaryEntry child in element.Children)
				{
					SaveElement(writer, (string)child.Key, (ConfigElement)child.Value);
				}
				writer.WriteEndElement();
			}
			else if (name != null)
			{
				if (flag)
				{
					writer.WriteStartElement("param");
					writer.WriteAttributeString("name", name);
					writer.WriteString(element.GetString());
					writer.WriteEndElement();
				}
				else
				{
					writer.WriteElementString(name, element.GetString());
				}
			}
		}

		public void Save(FileInfo configFile)
		{
			if (configFile.Exists)
			{
				configFile.Delete();
			}
			XmlTextWriter xmlTextWriter = new XmlTextWriter(configFile.FullName, Encoding.UTF8);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument();
			SaveElement(xmlTextWriter, null, this);
			xmlTextWriter.WriteEndDocument();
			xmlTextWriter.Close();
		}

		public static XMLConfigFile ParseXMLFile(FileInfo configFile)
		{
			XMLConfigFile xMLConfigFile = new XMLConfigFile(null);
			if (!configFile.Exists)
			{
				return xMLConfigFile;
			}
			ConfigElement configElement = xMLConfigFile;
			XmlTextReader xmlTextReader = new XmlTextReader(configFile.OpenRead());
			while (xmlTextReader.Read())
			{
				if (xmlTextReader.NodeType == XmlNodeType.Element)
				{
					if (xmlTextReader.Name == "root")
					{
						continue;
					}
					if (xmlTextReader.Name == "param")
					{
						string attribute = xmlTextReader.GetAttribute("name");
						if (attribute != null && attribute != "root")
						{
							ConfigElement configElement3 = configElement[attribute] = new ConfigElement(configElement);
							configElement = configElement3;
						}
					}
					else
					{
						ConfigElement configElement4 = new ConfigElement(configElement);
						configElement[xmlTextReader.Name] = configElement4;
						configElement = configElement4;
					}
				}
				else if (xmlTextReader.NodeType == XmlNodeType.Text)
				{
					configElement.Set(xmlTextReader.Value);
				}
				else if (xmlTextReader.NodeType == XmlNodeType.EndElement && xmlTextReader.Name != "root")
				{
					configElement = configElement.Parent;
				}
			}
			xmlTextReader.Close();
			return xMLConfigFile;
		}
	}
}
