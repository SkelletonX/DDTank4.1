// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddDefendEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddDefendEquip : AbstractPetEffect
  {
    private int m_added;
    private int m_count;

    public PetAddDefendEquip(int count, int skilId, string elementID)
      : base(ePetEffectType.AddDefenceEquip, elementID)
    {
      this.m_count = count;
      switch (skilId)
      {
        case 34:
        case 89:
          this.m_added = 100;
          break;
        case 35:
        case 37:
        case 74:
        case 90:
          this.m_added = 300;
          break;
        case 36:
        case 39:
        case 75:
          this.m_added = 500;
          break;
        case 38:
          this.m_added = 400;
          break;
      }
    }

    public override void OnAttached(Living living)
    {
      living.Defence += (double) this.m_added;
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      living.Defence -= (double) this.m_added;
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
      PetAddDefendEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.AddDefenceEquip) as PetAddDefendEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
