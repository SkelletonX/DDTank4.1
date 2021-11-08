// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetStopMovingEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetStopMovingEquip : AbstractPetEffect
  {
    private int m_count;

    public PetStopMovingEquip(int count, string elementID)
      : base(ePetEffectType.PetStopMovingEquip, elementID)
    {
      this.m_count = count;
    }

    public override void OnAttached(Living living)
    {
      living.SpeedMultX(0);
      living.PetEffects.StopMoving = true;
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      living.SpeedMultX(3);
      living.PetEffects.StopMoving = false;
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
      PetStopMovingEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetStopMovingEquip) as PetStopMovingEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
