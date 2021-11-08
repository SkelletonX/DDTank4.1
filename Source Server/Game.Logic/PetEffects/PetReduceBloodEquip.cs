// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetReduceBloodEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetReduceBloodEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetReduceBloodEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetReduceBloodEquip, elementID)
    {
      this.m_count = count;
      if (skillId != 189)
      {
        if (skillId != 190)
          return;
        this.m_value = 1000;
      }
      else
        this.m_value = 800;
    }

    public override void OnAttached(Living living)
    {
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
    }

    private void player_BeginFitting(Living living)
    {
      --this.m_count;
      if (this.m_count < 0)
      {
        this.Stop();
      }
      else
      {
        if (living.Game.RoomType != eRoomType.Match && living.Game.RoomType != eRoomType.Freedom)
          return;
        living.SyncAtTime = true;
        living.AddBlood(-this.m_value, 1);
        living.SyncAtTime = false;
        if (living.Blood > 0)
          return;
        living.Die();
      }
    }

    public override bool Start(Living living)
    {
      PetReduceBloodEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetReduceBloodEquip) as PetReduceBloodEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
