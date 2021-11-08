// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Object.Ball
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using System.Drawing;

namespace Game.Logic.Phy.Object
{
  public class Ball : PhysicalObj
  {
    private int _liveCount;

    public Ball(int id, string action)
      : base(id, "", "asset.game.six.ball", action, 1, 1, 0)
    {
      this.m_rect = new Rectangle(-15, -15, 30, 30);
    }

    public Ball(int id, string name, string defaultAction, int scale, int rotation)
      : base(id, name, "asset.game.six.ball", defaultAction, scale, rotation, 0)
    {
      this.m_rect = new Rectangle(-30, -30, 60, 60);
    }

    public override void CollidedByObject(Physics phy)
    {
      if (!(phy is SimpleBomb))
        return;
      (phy as SimpleBomb).Owner.PickPhy((PhysicalObj) this);
    }

    public int LiveCount
    {
      get
      {
        return this._liveCount;
      }
      set
      {
        this._liveCount = value;
      }
    }

    public new int Type
    {
      get
      {
        return 2;
      }
    }
  }
}
