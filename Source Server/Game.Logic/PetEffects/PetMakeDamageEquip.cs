// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetMakeDamageEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetMakeDamageEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetMakeDamageEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetMakeDamageEquip, elementID)
    {
      this.m_count = count;
      switch (skillId)
      {
        case 65:
          this.m_value = 400;
          break;
        case 66:
          this.m_value = 800;
          break;
        case 166:
          this.m_value = 600;
          break;
        case 167:
          this.m_value = 1200;
          break;
      }
    }

    public override void OnAttached(Living player)
    {
      player.AfterKilledByLiving += new KillLivingEventHanlde(this.player_BeforeTakeDamage);
      player.BeginSelfTurn += new LivingEventHandle(this.player_SelfTurn);
    }

    public override void OnRemoved(Living player)
    {
      player.AfterKilledByLiving -= new KillLivingEventHanlde(this.player_BeforeTakeDamage);
      player.BeginSelfTurn += new LivingEventHandle(this.player_SelfTurn);
    }

    private void player_BeforeTakeDamage(
      Living living,
      Living source,
      int damageAmount,
      int criticalAmount)
    {
      if (source == living)
        return;
      source.SyncAtTime = true;
      source.AddBlood(-this.m_value, 1);
      source.SyncAtTime = false;
      if (source.Blood > 0)
        return;
      source.Die();
    }

    private void player_SelfTurn(Living living)
    {
      --this.m_count;
      if (this.m_count >= 0)
        return;
      living.Game.SendPetBuff(living, this.Info, false);
      this.Stop();
    }

    public override bool Start(Living living)
    {
      PetMakeDamageEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetMakeDamageEquip) as PetMakeDamageEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
