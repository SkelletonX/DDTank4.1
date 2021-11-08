// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Object.SimpleWingBoss
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using SqlDataProvider.Data;
using System.Drawing;

namespace Game.Logic.Phy.Object
{
  public class SimpleWingBoss : SimpleBoss
  {
    public SimpleWingBoss(int id, BaseGame game, NpcInfo npcInfo, int direction, int type)
      : base(id, game, npcInfo, direction, type, "")
    {
    }

    public virtual Point StartFalling(bool direct, int delay, int speed)
    {
      return new Point() { X = this.X, Y = this.Y };
    }
  }
}
