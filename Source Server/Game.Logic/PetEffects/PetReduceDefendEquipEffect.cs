// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetReduceDefendEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetReduceDefendEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetReduceDefendEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetReduceDefendEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      if (skillId != 86)
      {
        if (skillId != 87)
          return;
        this.m_value = 800;
      }
      else
        this.m_value = 1000;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerBuffSkillPet += new PlayerEventHandle(this.player_AfterKilledByLiving);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerBuffSkillPet -= new PlayerEventHandle(this.player_AfterKilledByLiving);
    }

    private void player_AfterKilledByLiving(Living living)
    {
      if (this.rand.Next(10000) >= this.m_probability || living.PetEffects.CurrentUseSkill != this.m_currentId || living.PetEffects.ReduceDefendValue >= this.m_value)
        return;
      double defence = (living as Player).Defence;
      if ((living as Player).Defence < (double) this.m_value)
        this.m_value -= this.m_value - (int) defence;
      (living as Player).Defence -= (double) this.m_value;
      living.PetEffects.ReduceDefendValue += this.m_value;
    }

    public override bool Start(Living living)
    {
      PetReduceDefendEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetReduceDefendEquipEffect) as PetReduceDefendEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
