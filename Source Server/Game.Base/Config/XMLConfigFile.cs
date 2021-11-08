// Decompiled with JetBrains decompiler
// Type: Game.Base.Config.XMLConfigFile
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace Game.Base.Config
{
  public class XMLConfigFile : ConfigElement
  {
    public XMLConfigFile()
      : base((ConfigElement) null)
    {
    }

    protected XMLConfigFile(ConfigElement parent)
      : base(parent)
    {
    }

    protected bool IsBadXMLElementName(string name)
    {
      if (name == null)
        return false;
      if (name.IndexOf("\\") == -1 && name.IndexOf("/") == -1 && name.IndexOf("<") == -1)
        return name.IndexOf(">") != -1;
      return true;
    }

    public static XMLConfigFile ParseXMLFile(FileInfo configFile)
    {
      XMLConfigFile xmlConfigFile = new XMLConfigFile((ConfigElement) null);
      if (configFile.Exists)
      {
        ConfigElement parent = (ConfigElement) xmlConfigFile;
        XmlTextReader xmlTextReader = new XmlTextReader((Stream) configFile.OpenRead());
        while (xmlTextReader.Read())
        {
          if (xmlTextReader.NodeType == XmlNodeType.Element)
          {
            if (xmlTextReader.Name != "root")
            {
              if (xmlTextReader.Name == "param")
              {
                string attribute = xmlTextReader.GetAttribute("name");
                if (attribute != null && attribute != "root")
                {
                  ConfigElement configElement = new ConfigElement(parent);
                  parent[attribute] = configElement;
                  parent = configElement;
                }
              }
              else
              {
                ConfigElement configElement = new ConfigElement(parent);
                parent[xmlTextReader.Name] = configElement;
                parent = configElement;
              }
            }
          }
          else if (xmlTextReader.NodeType == XmlNodeType.Text)
            parent.Set((object) xmlTextReader.Value);
          else if (xmlTextReader.NodeType == XmlNodeType.EndElement && xmlTextReader.Name != "root")
            parent = parent.Parent;
        }
        xmlTextReader.Close();
      }
      return xmlConfigFile;
    }

    public void Save(FileInfo configFile)
    {
      if (configFile.Exists)
        configFile.Delete();
      XmlTextWriter writer = new XmlTextWriter(configFile.FullName, Encoding.UTF8)
      {
        Formatting = Formatting.Indented
      };
      writer.WriteStartDocument();
      this.SaveElement(writer, (string) null, (ConfigElement) this);
      writer.WriteEndDocument();
      writer.Close();
    }

    protected void SaveElement(XmlTextWriter writer, string name, ConfigElement element)
    {
      bool flag = this.IsBadXMLElementName(name);
      if (element.HasChildren)
      {
        if (name == null)
          name = "root";
        if (flag)
        {
          writer.WriteStartElement("param");
          writer.WriteAttributeString(nameof (name), name);
        }
        else
          writer.WriteStartElement(name);
        foreach (DictionaryEntry child in element.Children)
          this.SaveElement(writer, (string) child.Key, (ConfigElement) child.Value);
        writer.WriteEndElement();
      }
      else
      {
        if (name == null)
          return;
        if (flag)
        {
          writer.WriteStartElement("param");
          writer.WriteAttributeString(nameof (name), name);
          writer.WriteString(element.GetString());
          writer.WriteEndElement();
        }
        else
          writer.WriteElementString(name, element.GetString());
      }
    }
  }
}
