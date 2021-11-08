// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetRevertBloodAllPlayerAroundEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetRevertBloodAllPlayerAroundEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetRevertBloodAllPlayerAroundEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetRevertBloodAllPlayerAroundEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      if (skillId != 61)
      {
        if (skillId != 62)
          return;
        this.m_value = 2000;
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
      if (this.rand.Next(10000) >= this.m_probability || living.PetEffects.CurrentUseSkill != this.m_currentId)
        return;
      living.PetEffectTrigger = true;
      living.SyncAtTime = true;
      living.AddBlood(this.m_value);
      living.SyncAtTime = false;
      foreach (Living living1 in living.Game.Map.FindAllNearestSameTeam(living.X, living.Y, 150.0, living))
      {
        if (living1 != living)
        {
          living1.SyncAtTime = true;
          living1.AddBlood(this.m_value);
          living1.SyncAtTime = false;
        }
      }
    }

    public override bool Start(Living living)
    {
      PetRevertBloodAllPlayerAroundEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetRevertBloodAllPlayerAroundEquipEffect) as PetRevertBloodAllPlayerAroundEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
