// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetActiveDamageEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetActiveDamageEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetActiveDamageEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetActiveDamageEquip, elementID)
    {
      this.m_count = count;
      if (skillId != 42)
      {
        if (skillId != 43)
          return;
        this.m_value = 300;
      }
      else
        this.m_value = 400;
    }

    public override void OnAttached(Living living)
    {
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
      living.BeforeTakeDamage += new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
    }

    public override void OnRemoved(Living living)
    {
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
      living.BeforeTakeDamage -= new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
    }

    private void player_BeforeTakeDamage(
      Living living,
      Living source,
      ref int damageAmount,
      ref int criticalAmount)
    {
      if (living.PetEffects.AddGuardValue >= this.m_value)
        return;
      (living as Player).BaseGuard += (double) this.m_value;
      living.PetEffects.AddGuardValue = this.m_value;
    }

    private void player_BeginFitting(Living living)
    {
      --this.m_count;
      if (this.m_count >= 0)
        return;
      (living as Player).BaseDamage -= (double) living.PetEffects.AddDameValue;
      living.PetEffects.AddDameValue = 0;
      this.Stop();
    }

    public override bool Start(Living living)
    {
      PetActiveDamageEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetActiveDamageEquip) as PetActiveDamageEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
