// Decompiled with JetBrains decompiler
// Type: Game.Logic.LoadingFileInfo
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic
{
  public class LoadingFileInfo
  {
    public string ClassName;
    public string Path;
    public int Type;

    public LoadingFileInfo(int type, string path, string className)
    {
      this.Type = type;
      this.Path = path;
      this.ClassName = className;
    }
  }
}
