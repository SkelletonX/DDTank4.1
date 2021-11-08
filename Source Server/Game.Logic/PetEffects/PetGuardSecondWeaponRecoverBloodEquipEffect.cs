// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetGuardSecondWeaponRecoverBloodEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetGuardSecondWeaponRecoverBloodEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetGuardSecondWeaponRecoverBloodEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetGuardSecondWeaponRecoverBloodEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      if (skillId != 80)
      {
        if (skillId != 81)
          return;
        this.m_value = 1000;
      }
      else
        this.m_value = 500;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerGuard += new PlayerEventHandle(this.player_AfterKilledByLiving);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerGuard -= new PlayerEventHandle(this.player_AfterKilledByLiving);
    }

    private void player_AfterKilledByLiving(Living living)
    {
      if (this.rand.Next(10000) >= this.m_probability)
        return;
      living.SyncAtTime = true;
      living.AddBlood(this.m_value);
      living.SyncAtTime = false;
    }

    public override bool Start(Living living)
    {
      PetGuardSecondWeaponRecoverBloodEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetGuardSecondWeaponRecoverBloodEquipEffect) as PetGuardSecondWeaponRecoverBloodEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
