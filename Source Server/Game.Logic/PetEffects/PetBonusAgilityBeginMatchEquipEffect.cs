// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetBonusAgilityBeginMatchEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetBonusAgilityBeginMatchEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetBonusAgilityBeginMatchEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetBonusAgilityBeginMatchEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      if (skillId != 134)
      {
        if (skillId != 135)
          return;
        this.m_value = 500;
      }
      else
        this.m_value = 300;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PetEffects.BonusAgility += this.m_value;
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PetEffects.BonusAgility -= this.m_value;
    }

    public override bool Start(Living living)
    {
      PetBonusAgilityBeginMatchEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetBonusAgilityBeginMatchEquipEffect) as PetBonusAgilityBeginMatchEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
