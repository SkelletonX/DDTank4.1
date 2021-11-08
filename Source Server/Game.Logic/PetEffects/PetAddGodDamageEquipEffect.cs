// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddGodDamageEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddGodDamageEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;

    public PetAddGodDamageEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetAddGodDamageEquipEffect, elementID)
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
      if (this.rand.Next(10000) >= this.m_probability || 187 != this.m_currentId && 188 != this.m_currentId)
        return;
      foreach (Player allTeamPlayer in living.Game.GetAllTeamPlayers(living))
      {
        if (allTeamPlayer != living)
          allTeamPlayer.AddPetEffect((AbstractPetEffect) new PetAddGodDamageEquip(this.m_count, this.m_currentId, this.Info.ID.ToString()), 0);
      }
    }

    public override bool Start(Living living)
    {
      PetAddGodDamageEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddGodDamageEquipEffect) as PetAddGodDamageEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
