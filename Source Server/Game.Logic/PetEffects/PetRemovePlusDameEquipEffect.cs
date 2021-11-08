// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetRemovePlusDameEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetRemovePlusDameEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;

    public PetRemovePlusDameEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetRemovePlusDameEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.AfterKilledByLiving += new KillLivingEventHanlde(this.player_AfterKilledByLiving);
      player.BeforeTakeDamage += new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.AfterKilledByLiving -= new KillLivingEventHanlde(this.player_AfterKilledByLiving);
      player.BeforeTakeDamage -= new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
    }

    private void player_AfterKilledByLiving(
      Living living,
      Living target,
      int damageAmount,
      int criticalAmount)
    {
      this.ReduceDame(living);
    }

    private void player_BeforeTakeDamage(
      Living living,
      Living source,
      ref int damageAmount,
      ref int criticalAmount)
    {
      this.ReduceDame(living);
    }

    private void ReduceDame(Living living)
    {
      if (living.PetEffects.AddDameValue <= 0)
        return;
      (living as Player).BaseDamage -= (double) living.PetEffects.AddDameValue;
      living.PetEffects.AddDameValue = 0;
    }

    public override bool Start(Living living)
    {
      PetRemovePlusDameEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetRemovePlusDameEquipEffect) as PetRemovePlusDameEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
