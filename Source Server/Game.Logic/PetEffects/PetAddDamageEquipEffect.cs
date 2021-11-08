// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddDamageEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddDamageEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetAddDamageEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetAddDamageEquipEffect, elementID)
    {
      this.m_count = count - 1;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      switch (skillId)
      {
        case 106:
          this.m_value = 50;
          break;
        case 107:
        case 136:
          this.m_value = 100;
          break;
        case 137:
          this.m_value = 150;
          break;
        case 160:
          this.m_value = 45;
          break;
        case 161:
          this.m_value = 75;
          break;
        case 162:
          this.m_value = 115;
          break;
      }
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
      if (this.rand.Next(10000) >= this.m_probability || living.PetEffects.CurrentUseSkill != this.m_currentId || living.PetEffects.AddDameValue >= this.m_value)
        return;
      living.PetEffectTrigger = true;
      (living as Player).BaseDamage += (double) this.m_value;
      living.PetEffects.AddDameValue = this.m_value;
      new PetAddDamageEquip(this.m_count, this.m_currentId, this.Info.ID.ToString()).Start(living);
    }

    public override bool Start(Living living)
    {
      PetAddDamageEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddDamageEquipEffect) as PetAddDamageEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
