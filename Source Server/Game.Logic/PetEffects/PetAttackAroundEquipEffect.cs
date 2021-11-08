// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAttackAroundEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAttackAroundEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetAttackAroundEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetAttackAroundEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      if (skillId != 78)
      {
        if (skillId != 79)
          return;
        this.m_value = 400;
      }
      else
        this.m_value = 200;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerShoot += new PlayerEventHandle(this.player_AfterKilledByLiving);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerShoot -= new PlayerEventHandle(this.player_AfterKilledByLiving);
    }

    private void player_AfterKilledByLiving(Living living)
    {
      if (this.rand.Next(10000) >= this.m_probability)
        return;
      foreach (Living living1 in living.Game.Map.FindAllNearestEnemy(living.X, living.Y, 110.0, living))
      {
        living1.SyncAtTime = true;
        living1.AddBlood(-this.m_value, 1);
        living1.SyncAtTime = false;
        if (living1.Blood <= 0)
          living1.Die();
      }
    }

    public override bool Start(Living living)
    {
      PetAttackAroundEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAttackAroundEquipEffect) as PetAttackAroundEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
