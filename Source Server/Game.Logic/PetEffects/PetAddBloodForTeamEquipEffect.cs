// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddBloodForTeamEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddBloodForTeamEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetAddBloodForTeamEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetAddBloodForTeamEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      if (skillId != 82)
      {
        if (skillId != 83)
          return;
        this.m_value = 600;
      }
      else
        this.m_value = 300;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerShoot += new PlayerEventHandle(this.player_AfterKilledByLiving);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerShoot -= new PlayerEventHandle(this.player_AfterKilledByLiving);
    }

    private void player_AfterKilledByLiving(Player living)
    {
      if (this.rand.Next(10000) >= this.m_probability || !living.IsCure())
        return;
      living.PetEffectTrigger = true;
      foreach (Player allTeamPlayer in living.Game.GetAllTeamPlayers((Living) living))
      {
        if (allTeamPlayer != living)
        {
          allTeamPlayer.SyncAtTime = true;
          allTeamPlayer.AddBlood(this.m_value);
          allTeamPlayer.SyncAtTime = false;
        }
      }
    }

    public override bool Start(Living living)
    {
      PetAddBloodForTeamEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddBloodForTeamEquipEffect) as PetAddBloodForTeamEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
