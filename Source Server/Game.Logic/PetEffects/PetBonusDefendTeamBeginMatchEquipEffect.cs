﻿// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetBonusDefendTeamBeginMatchEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetBonusDefendTeamBeginMatchEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;

    public PetBonusDefendTeamBeginMatchEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetBonusDefendTeamBeginMatchEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.BeginSelfTurn += new LivingEventHandle(this.player_AfterKilledByLiving);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.BeginSelfTurn -= new LivingEventHandle(this.player_AfterKilledByLiving);
    }

    private void player_AfterKilledByLiving(Living living)
    {
      foreach (Living allTeamPlayer in living.Game.GetAllTeamPlayers(living))
        allTeamPlayer.AddPetEffect((AbstractPetEffect) new PetBonusAttackBeginMatchEquip(this.m_count, this.m_currentId, this.Info.ID.ToString()), 0);
    }

    public override bool Start(Living living)
    {
      PetBonusDefendTeamBeginMatchEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetBonusDefendTeamBeginMatchEquipEffect) as PetBonusDefendTeamBeginMatchEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
