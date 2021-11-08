// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddLuckAllMatchEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddLuckAllMatchEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetAddLuckAllMatchEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetAddLuckAllMatchEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      switch (skillId)
      {
        case 86:
        case 186:
          this.m_value = 500;
          break;
        case 87:
          this.m_value = 800;
          break;
        case 185:
          this.m_value = 300;
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
      if (this.rand.Next(10000) >= this.m_probability || living.PetEffects.CurrentUseSkill != this.m_currentId || living.PetEffects.AddLuckValue >= this.m_value)
        return;
      (living as Player).Lucky += (double) this.m_value;
      living.PetEffects.AddLuckValue += this.m_value;
    }

    public override bool Start(Living living)
    {
      PetAddLuckAllMatchEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddLuckAllMatchEquipEffect) as PetAddLuckAllMatchEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
