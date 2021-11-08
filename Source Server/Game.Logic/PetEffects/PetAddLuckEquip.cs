// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddLuckEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddLuckEquip : AbstractPetEffect
  {
    private int m_added;
    private int m_count;

    public PetAddLuckEquip(int count, int skilId, string elementID)
      : base(ePetEffectType.PetAddLuckEquip, elementID)
    {
      this.m_count = count;
      switch (skilId)
      {
        case 28:
        case 89:
          this.m_added = 100;
          break;
        case 29:
        case 90:
          this.m_added = 300;
          break;
        case 30:
          this.m_added = 500;
          break;
      }
    }

    public override void OnAttached(Living living)
    {
      living.Lucky += (double) this.m_added;
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      living.Lucky -= (double) this.m_added;
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
      PetAddLuckEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddLuckEquip) as PetAddLuckEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
