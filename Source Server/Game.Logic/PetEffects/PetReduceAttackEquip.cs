// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetReduceAttackEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetReduceAttackEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetReduceAttackEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetReduceAttackEquip, elementID)
    {
      this.m_count = count;
      switch (skillId)
      {
        case 37:
        case 38:
        case 39:
        case 178:
          this.m_value = 100;
          break;
        case 146:
        case 180:
          this.m_value = 300;
          break;
        case 147:
          this.m_value = 500;
          break;
        case 179:
          this.m_value = 200;
          break;
      }
    }

    public override void OnAttached(Living living)
    {
      living.Attack -= (double) this.m_value;
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      living.Attack += (double) this.m_value;
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
      PetReduceAttackEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetReduceAttackEquip) as PetReduceAttackEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
