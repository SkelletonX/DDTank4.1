// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddDamageEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddDamageEquip : AbstractPetEffect
  {
    private int m_count;

    public PetAddDamageEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetAddDamageEquip, elementID)
    {
      this.m_count = count;
    }

    public override void OnAttached(Living player)
    {
      player.BeginSelfTurn += new LivingEventHandle(this.player_SelfTurn);
    }

    public override void OnRemoved(Living player)
    {
      player.BeginSelfTurn -= new LivingEventHandle(this.player_SelfTurn);
    }

    private void player_SelfTurn(Living living)
    {
      --this.m_count;
      if (this.m_count >= 0)
        return;
      (living as Player).BaseDamage -= (double) living.PetEffects.AddDameValue;
      living.PetEffects.AddDameValue = 0;
      this.Stop();
    }

    public override bool Start(Living living)
    {
      PetAddDamageEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddDamageEquip) as PetAddDamageEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
