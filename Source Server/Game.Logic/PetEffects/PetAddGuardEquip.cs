// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddGuardEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddGuardEquip : AbstractPetEffect
  {
    private int m_added;
    private int m_count;

    public PetAddGuardEquip(int count, int skilId, string elementID)
      : base(ePetEffectType.PetAddGuardEquip, elementID)
    {
      this.m_count = count;
      if (skilId != 181)
      {
        if (skilId != 182)
          return;
        this.m_added = 200;
      }
      else
        this.m_added = 100;
    }

    public override void OnAttached(Living living)
    {
      if (living.Game.RoomType == eRoomType.Match || living.Game.RoomType == eRoomType.Freedom)
        living.BaseGuard += (double) this.m_added;
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      if (living.Game.RoomType == eRoomType.Match || living.Game.RoomType == eRoomType.Freedom)
        living.BaseGuard -= (double) this.m_added;
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
    }

    private void player_BeginFitting(Living living)
    {
      --this.m_count;
      if (this.m_count >= 0)
        return;
      this.Stop();
    }

    public override bool Start(Living living)
    {
      PetAddGuardEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddGuardEquip) as PetAddGuardEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
