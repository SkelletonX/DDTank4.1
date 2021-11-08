// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetBonusAttackBeginMatchEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetBonusAttackBeginMatchEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_value;

    public PetBonusAttackBeginMatchEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetBonusAttackBeginMatchEquip, elementID)
    {
      this.m_count = count;
      this.m_currentId = skillId;
      if (skillId != 134)
      {
        if (skillId != 135)
          return;
        this.m_value = 300;
      }
      else
        this.m_value = 100;
    }

    public override void OnAttached(Living player)
    {
      player.PetEffects.BonusAttack += this.m_value;
    }

    public override void OnRemoved(Living player)
    {
      player.PetEffects.BonusAttack -= this.m_value;
    }

    public override bool Start(Living living)
    {
      PetBonusAttackBeginMatchEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetBonusAttackBeginMatchEquip) as PetBonusAttackBeginMatchEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
