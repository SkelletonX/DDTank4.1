// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetDamageAllEnemyEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;
using System;

namespace Game.Logic.PetEffects
{
  public class PetDamageAllEnemyEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetDamageAllEnemyEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetDamageAllEnemyEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      if (skillId != 114)
      {
        if (skillId != 115)
          return;
        this.m_value = 4000;
      }
      else
        this.m_value = 2000;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerBuffSkillPet += new PlayerEventHandle(this.player_AfterBuffSkill);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerBuffSkillPet -= new PlayerEventHandle(this.player_AfterBuffSkill);
    }

    private void player_AfterBuffSkill(Living living)
    {
      if (this.rand.Next(10000) >= this.m_probability || living.PetEffects.CurrentUseSkill != this.m_currentId)
        return;
      foreach (Player allEnemyPlayer in living.Game.GetAllEnemyPlayers(living))
      {
        allEnemyPlayer.SyncAtTime = true;
        allEnemyPlayer.AddBlood(-this.m_value, 1);
        allEnemyPlayer.SyncAtTime = false;
        if (allEnemyPlayer.Blood <= 0)
        {
          Console.WriteLine("petdamage g die");
          allEnemyPlayer.Die();
        }
      }
    }

    public override bool Start(Living living)
    {
      PetDamageAllEnemyEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetDamageAllEnemyEquipEffect) as PetDamageAllEnemyEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
