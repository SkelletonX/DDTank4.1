// Decompiled with JetBrains decompiler
// Type: Bussiness.XmlExtends
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Bussiness
{
  public static class XmlExtends
  {
    public static string ToString(this XElement node, bool check)
    {
      StringBuilder output = new StringBuilder();
      XmlWriterSettings settings = new XmlWriterSettings()
      {
        CheckCharacters = check,
        OmitXmlDeclaration = true,
        Indent = true
      };
      using (XmlWriter writer = XmlWriter.Create(output, settings))
        node.WriteTo(writer);
      return output.ToString();
    }
  }
}
