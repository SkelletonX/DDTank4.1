// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddGodDamageEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddGodDamageEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_probability;
    private int m_value;

    public PetAddGodDamageEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetAddGodDamageEquip, elementID)
    {
      this.m_count = count;
      this.m_currentId = skillId;
      if (skillId != 187)
      {
        if (skillId != 188)
          return;
        this.m_value = 300;
      }
      else
        this.m_value = 100;
    }

    public override void OnAttached(Living living)
    {
      living.BaseDamage += (double) this.m_value;
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      living.BaseDamage -= (double) this.m_value;
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
    }

    private void player_BeginFitting(Living living)
    {
    }

    public override bool Start(Living living)
    {
      PetAddGodDamageEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddGodDamageEquip) as PetAddGodDamageEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
