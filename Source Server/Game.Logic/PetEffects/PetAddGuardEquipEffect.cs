// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddGuardEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddGuardEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetAddGuardEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetPlusGuardEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      switch (skillId)
      {
        case 76:
          this.m_value = 70;
          break;
        case 77:
          this.m_value = 100;
          break;
        case 168:
          this.m_value = 90;
          break;
        case 169:
          this.m_value = 130;
          break;
      }
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
      if (this.rand.Next(10000) >= this.m_probability || living.PetEffects.AddGuardValue >= this.m_value)
        return;
      (living as Player).BaseGuard += (double) this.m_value;
      living.PetEffects.AddGuardValue += this.m_value;
    }

    public override bool Start(Living living)
    {
      PetAddGuardEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetPlusGuardEquipEffect) as PetAddGuardEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
