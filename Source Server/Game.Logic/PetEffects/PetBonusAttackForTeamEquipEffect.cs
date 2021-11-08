// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetBonusAttackForTeamEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetBonusAttackForTeamEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;

    public PetBonusAttackForTeamEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetBonusAttackForTeamEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
    }

    private void ChangeProperty(Player player)
    {
      if (this.rand.Next(100) >= this.m_probability || player.PetEffects.CurrentUseSkill != this.m_currentId)
        return;
      foreach (Living allTeamPlayer in player.Game.GetAllTeamPlayers((Living) player))
        allTeamPlayer.AddPetEffect((AbstractPetEffect) new PetAddAttackEquip(this.m_count, this.m_currentId, this.Info.ID.ToString()), 0);
      player.Game.SendPetBuff((Living) player, this.Info, true);
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerBuffSkillPet += new PlayerEventHandle(this.ChangeProperty);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerBuffSkillPet -= new PlayerEventHandle(this.ChangeProperty);
    }

    public override bool Start(Living living)
    {
      PetBonusAttackForTeamEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetBonusAttackForTeamEquipEffect) as PetBonusAttackForTeamEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
