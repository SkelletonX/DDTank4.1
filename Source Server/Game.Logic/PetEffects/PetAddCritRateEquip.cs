// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddCritRateEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddCritRateEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetAddCritRateEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetAddCritRateEquip, elementID)
    {
      this.m_count = count;
      switch (skillId)
      {
        case 136:
          this.m_value = 30;
          break;
        case 137:
        case 151:
          this.m_value = 50;
          break;
        case 150:
          this.m_value = 20;
          break;
      }
    }

    public override void OnAttached(Living living)
    {
      (living as Player).PetEffects.CritRate += this.m_value;
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      (living as Player).PetEffects.CritRate -= this.m_value;
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
      PetAddCritRateEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddCritRateEquip) as PetAddCritRateEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
