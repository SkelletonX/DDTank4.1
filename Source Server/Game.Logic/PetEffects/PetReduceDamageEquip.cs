// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetReduceDamageEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetReduceDamageEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetReduceDamageEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetReduceDamageEquip, elementID)
    {
      this.m_count = count;
      switch (skillId)
      {
        case 50:
        case 53:
          this.m_value = 100;
          break;
        case 51:
        case 54:
        case 163:
          this.m_value = 200;
          break;
        case 52:
        case 55:
          this.m_value = 300;
          break;
        case 164:
          this.m_value = 250;
          break;
        case 165:
          this.m_value = 350;
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
      damageAmount -= this.m_value;
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
      PetReduceDamageEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetReduceDamageEquip) as PetReduceDamageEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
