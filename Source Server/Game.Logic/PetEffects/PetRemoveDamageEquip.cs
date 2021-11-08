// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetRemoveDamageEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetRemoveDamageEquip : AbstractPetEffect
  {
    private int m_count;

    public PetRemoveDamageEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetRemoveDamageEquip, elementID)
    {
      this.m_count = count;
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
      damageAmount = 0;
      criticalAmount = 0;
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
      PetRemoveDamageEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetRemoveDamageEquip) as PetRemoveDamageEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
