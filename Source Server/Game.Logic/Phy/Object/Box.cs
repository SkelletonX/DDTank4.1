// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Object.Box
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using SqlDataProvider.Data;
using System.Drawing;

namespace Game.Logic.Phy.Object
{
  public class Box : PhysicalObj
  {
    private int _userID;
    private int _liveCount;
    public int m_type;
    private ItemInfo m_item;

    public int UserID
    {
      get
      {
        return this._userID;
      }
      set
      {
        this._userID = value;
      }
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

    public ItemInfo Item
    {
      get
      {
        return this.m_item;
      }
    }

    public override int Type
    {
      get
      {
        return this.m_type;
      }
    }

    public bool IsGhost
    {
      get
      {
        return this.m_type > 1;
      }
    }

    public Box(int id, string model, ItemInfo item, int type)
      : base(id, "", model, "", 1, 1, 0)
    {
      this._userID = 0;
      this.m_rect = new Rectangle(-15, -15, 30, 30);
      this.m_item = item;
      this.m_type = type;
    }

    public override void CollidedByObject(Physics phy)
    {
      if (!(phy is SimpleBomb))
        return;
      SimpleBomb simpleBomb = phy as SimpleBomb;
      if (!(simpleBomb.Owner is Player))
        return;
      simpleBomb.Owner.PickBox(this);
    }
  }
}
