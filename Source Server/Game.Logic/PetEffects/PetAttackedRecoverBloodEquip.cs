// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAttackedRecoverBloodEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAttackedRecoverBloodEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_value;

    public PetAttackedRecoverBloodEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetAttackedRecoverBloodEquip, elementID)
    {
      this.m_count = count;
      this.m_currentId = skillId;
      if (skillId != 110)
      {
        if (skillId != 111)
          return;
        this.m_value = 1000;
      }
      else
        this.m_value = 800;
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
      if (this.rand.Next(10000) >= 3500)
        return;
      living.SyncAtTime = true;
      living.AddBlood(this.m_value);
      living.SyncAtTime = false;
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
      PetAttackedRecoverBloodEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAttackedRecoverBloodEquip) as PetAttackedRecoverBloodEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
