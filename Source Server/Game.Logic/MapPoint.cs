// Decompiled with JetBrains decompiler
// Type: Game.Logic.MapPoint
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic
{
  public class MapPoint
  {
    private List<Point> posX = new List<Point>();
    private List<Point> posX1 = new List<Point>();

    public List<Point> PosX
    {
      get
      {
        return this.posX;
      }
      set
      {
        this.posX = value;
      }
    }

    public List<Point> PosX1
    {
      get
      {
        return this.posX1;
      }
      set
      {
        this.posX1 = value;
      }
    }
  }
}
