// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetReduceBaseGuardEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetReduceBaseGuardEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetReduceBaseGuardEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetReduceBaseGuardEquip, elementID)
    {
      this.m_count = count;
      switch (skillId)
      {
        case 106:
          this.m_value = 70;
          break;
        case 107:
          this.m_value = 120;
          break;
        case 154:
          this.m_value = 100;
          break;
        case 155:
          this.m_value = 200;
          break;
      }
    }

    public override void OnAttached(Living living)
    {
      living.BaseGuard -= (double) this.m_value;
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      living.BaseGuard += (double) this.m_value;
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
      PetReduceBaseGuardEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetReduceBaseGuardEquip) as PetReduceBaseGuardEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
