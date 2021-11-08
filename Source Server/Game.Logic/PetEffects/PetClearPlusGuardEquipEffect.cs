// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetClearPlusGuardEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetClearPlusGuardEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;

    public PetClearPlusGuardEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetClearPlusGuardEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerBeginMoving += new PlayerEventHandle(this.player_WhenMoving);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerBeginMoving -= new PlayerEventHandle(this.player_WhenMoving);
    }

    private void player_WhenMoving(Player player)
    {
      if (player.PetEffects.AddGuardValue <= 0)
        return;
      player.BaseGuard -= (double) player.PetEffects.AddGuardValue;
      player.PetEffects.AddGuardValue = 0;
    }

    public override bool Start(Living living)
    {
      PetClearPlusGuardEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetClearPlusGuardEquipEffect) as PetClearPlusGuardEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
