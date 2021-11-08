// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Actions.PetAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using System;

namespace Game.Logic.Phy.Actions
{
  public class PetAction
  {
    public int blood;
    public int damage;
    public int dander;
    public int id;
    public float Time;
    public int Type;

    public PetAction(
      float time,
      PetActionType type,
      int _id,
      int _damage,
      int _dander,
      int _blood)
    {
      this.Time = time;
      this.Type = (int) type;
      this.id = _id;
      this.damage = _damage;
      this.blood = _blood;
      this.dander = _dander;
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
