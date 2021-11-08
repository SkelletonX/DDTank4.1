// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetLuckMakeDamageEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetLuckMakeDamageEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetLuckMakeDamageEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetLuckMakeDamageEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      switch (skillId)
      {
        case 84:
          this.m_value = 300;
          break;
        case 85:
        case 170:
          this.m_value = 500;
          break;
        case 171:
          this.m_value = 800;
          break;
      }
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.AfterKilledByLiving += new KillLivingEventHanlde(this.player_BeforeTakeDamage);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.AfterKilledByLiving -= new KillLivingEventHanlde(this.player_BeforeTakeDamage);
    }

    private void player_BeforeTakeDamage(
      Living living,
      Living source,
      int damageAmount,
      int criticalAmount)
    {
      if (this.rand.Next(10000) >= this.m_probability || source == living)
        return;
      source.SyncAtTime = true;
      source.AddBlood(-this.m_value, 1);
      source.SyncAtTime = false;
      if (source.Blood > 0)
        return;
      source.Die();
    }

    public override bool Start(Living living)
    {
      PetLuckMakeDamageEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetLuckMakeDamageEquipEffect) as PetLuckMakeDamageEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
