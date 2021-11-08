// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetBurningBloodShootingEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;
using System;

namespace Game.Logic.PetEffects
{
  public class PetBurningBloodShootingEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_value;

    public PetBurningBloodShootingEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetBurningBloodShootingEquip, elementID)
    {
      this.m_count = count;
      this.m_currentId = skillId;
      if (skillId != 110)
      {
        if (skillId != 111)
          return;
        this.m_value = 1000;
      }
      else
        this.m_value = 800;
    }

    private void ChangeProperty(Player living)
    {
      if (living.ShootCount != 1)
        return;
      living.SyncAtTime = true;
      living.AddBlood(-this.m_value, 1);
      living.SyncAtTime = false;
      if (living.Blood > 0)
        return;
      Console.WriteLine("petburnin g die");
      living.Die();
    }

    public override void OnAttached(Living living)
    {
      (living as Player).PlayerShoot += new PlayerEventHandle(this.ChangeProperty);
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      (living as Player).PlayerShoot -= new PlayerEventHandle(this.ChangeProperty);
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
    }

    private void player_BeginFitting(Living living)
    {
      --this.m_count;
      if (this.m_count >= 0)
        return;
      this.Stop();
    }

    public override bool Start(Living living)
    {
      PetBurningBloodShootingEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetBurningBloodShootingEquip) as PetBurningBloodShootingEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
