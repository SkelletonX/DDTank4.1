// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddBloodEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddBloodEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetAddBloodEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetAddBloodEquip, elementID)
    {
      this.m_count = count;
      switch (skillId)
      {
        case 93:
          this.m_value = 500;
          break;
        case 94:
          this.m_value = 1000;
          break;
        case 172:
          this.m_value = 1500;
          break;
        case 173:
          this.m_value = 2500;
          break;
      }
    }

    public override void OnAttached(Living player)
    {
      player.BeforeTakeDamage += new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.BeginSelfTurn += new LivingEventHandle(this.player_SelfTurn);
    }

    public override void OnRemoved(Living player)
    {
      player.BeforeTakeDamage -= new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.BeginSelfTurn += new LivingEventHandle(this.player_SelfTurn);
    }

    private void player_BeforeTakeDamage(
      Living living,
      Living source,
      ref int damageAmount,
      ref int criticalAmount)
    {
      if (living.Game.RoomType != eRoomType.Match && living.Game.RoomType != eRoomType.Freedom)
        return;
      living.SyncAtTime = true;
      living.AddBlood(this.m_value);
      living.SyncAtTime = false;
    }

    private void player_SelfTurn(Living living)
    {
      --this.m_count;
      if (this.m_count >= 0)
        return;
      this.Stop();
    }

    public override bool Start(Living living)
    {
      PetAddBloodEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddBloodEquip) as PetAddBloodEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
