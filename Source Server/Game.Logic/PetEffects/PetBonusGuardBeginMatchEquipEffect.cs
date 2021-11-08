// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetBonusGuardBeginMatchEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetBonusGuardBeginMatchEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetBonusGuardBeginMatchEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetBonusGuardBeginMatchEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      if (skillId != 152)
      {
        if (skillId != 153)
          return;
        this.m_value = 200;
      }
      else
        this.m_value = 100;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PetEffects.BonusGuard += this.m_value;
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PetEffects.BonusGuard -= this.m_value;
    }

    public override bool Start(Living living)
    {
      PetBonusGuardBeginMatchEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetBonusGuardBeginMatchEquipEffect) as PetBonusGuardBeginMatchEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
