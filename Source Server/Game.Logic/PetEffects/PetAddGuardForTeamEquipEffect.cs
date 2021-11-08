// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddGuardForTeamEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddGuardForTeamEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;

    public PetAddGuardForTeamEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetAddGuardForTeamEquipEffect, elementID)
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
        allTeamPlayer.AddPetEffect((AbstractPetEffect) new PetAddGuardEquip(this.m_count, this.m_currentId, this.Info.ID.ToString()), 0);
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
      PetAddGuardForTeamEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddGuardForTeamEquipEffect) as PetAddGuardForTeamEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
