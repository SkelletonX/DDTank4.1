// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetClearHighMPEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetClearHighMPEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;

    public PetClearHighMPEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetClearHighMPEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.BeginSelfTurn += new LivingEventHandle(this.player_BeginSelfTurn);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.BeginSelfTurn -= new LivingEventHandle(this.player_BeginSelfTurn);
    }

    private void player_BeginSelfTurn(Living living)
    {
      if (this.rand.Next(10000) >= this.m_probability)
        return;
      PetAddHighMPEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddHighMPEquipEffect) as PetAddHighMPEquipEffect;
      if (ofType == null || living.TurnNum <= 0)
        return;
      ofType.Stop();
    }

    public override bool Start(Living living)
    {
      PetClearHighMPEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetClearHighMPEquipEffect) as PetClearHighMPEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
