// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddAttackEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddAttackEquip : AbstractPetEffect
  {
    private int m_added;
    private int m_count;

    public PetAddAttackEquip(int count, int skilId, string elementID)
      : base(ePetEffectType.PetAddAttackEquip, elementID)
    {
      this.m_count = count;
      switch (skilId)
      {
        case 89:
        case 124:
          this.m_added = 100;
          break;
        case 90:
        case 125:
        case 137:
          this.m_added = 300;
          break;
        case 126:
          this.m_added = 500;
          break;
        case 136:
          this.m_added = 200;
          break;
      }
    }

    public override void OnAttached(Living living)
    {
      living.Attack += (double) this.m_added;
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      living.Attack -= (double) this.m_added;
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
    }

    private void player_BeginFitting(Living living)
    {
      --this.m_count;
      if (this.m_count >= 0)
        return;
      living.Game.SendPetBuff(living, this.Info, false);
      this.Stop();
    }

    public override bool Start(Living living)
    {
      PetAddAttackEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddAttackEquip) as PetAddAttackEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
