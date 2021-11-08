// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetBonusAttackBeginMatchEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetBonusAttackBeginMatchEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetBonusAttackBeginMatchEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetBonusAttackBeginMatchEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      if (skillId != 112)
      {
        if (skillId != 113)
          return;
        this.m_value = 100;
      }
      else
        this.m_value = 80;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PetEffects.BonusAttack += this.m_value;
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PetEffects.BonusAttack -= this.m_value;
    }

    public override bool Start(Living living)
    {
      PetBonusAttackBeginMatchEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetBonusAttackBeginMatchEquipEffect) as PetBonusAttackBeginMatchEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
