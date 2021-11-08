// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Actions.BombAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using System;

namespace Game.Logic.Phy.Actions
{
  public class BombAction
  {
    public int Param1;
    public int Param2;
    public int Param3;
    public int Param4;
    public float Time;
    public int Type;

    public BombAction(float time, ActionType type, int para1, int para2, int para3, int para4)
    {
      this.Time = time;
      this.Type = (int) type;
      this.Param1 = para1;
      this.Param2 = para2;
      this.Param3 = para3;
      this.Param4 = para4;
    }

    public int TimeInt
    {
      get
      {
        return (int) Math.Round((double) this.Time * 1000.0);
      }
    }
  }
}
