// Decompiled with JetBrains decompiler
// Type: Game.Logic.MacroDropInfo
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic
{
  public class MacroDropInfo
  {
    public MacroDropInfo(int dropCount, int maxDropCount)
    {
      this.DropCount = dropCount;
      this.MaxDropCount = maxDropCount;
    }

    public int DropCount { get; set; }

    public int MaxDropCount { get; set; }

    public int SelfDropCount { get; set; }
  }
}
