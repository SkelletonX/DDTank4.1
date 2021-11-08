// Decompiled with JetBrains decompiler
// Type: Game.Base.ResourceUtil
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using System.IO;
using System.Reflection;

namespace Game.Base
{
  public class ResourceUtil
  {
    public static void ExtractResource(string fileName, Assembly assembly)
    {
      ResourceUtil.ExtractResource(fileName, fileName, assembly);
    }

    public static void ExtractResource(string resourceName, string fileName, Assembly assembly)
    {
      FileInfo fileInfo = new FileInfo(fileName);
      if (!fileInfo.Directory.Exists)
        fileInfo.Directory.Create();
      using (StreamReader streamReader = new StreamReader(ResourceUtil.GetResourceStream(resourceName, assembly)))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) File.Create(fileName)))
          streamWriter.Write(streamReader.ReadToEnd());
      }
    }

    public static Stream GetResourceStream(string fileName, Assembly assem)
    {
      fileName = fileName.ToLower();
      foreach (string manifestResourceName in assem.GetManifestResourceNames())
      {
        if (manifestResourceName.ToLower().EndsWith(fileName))
          return assem.GetManifestResourceStream(manifestResourceName);
      }
      return (Stream) null;
    }
  }
}
