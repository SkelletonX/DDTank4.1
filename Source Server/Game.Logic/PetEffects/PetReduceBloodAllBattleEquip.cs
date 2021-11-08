// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetReduceBloodAllBattleEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetReduceBloodAllBattleEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetReduceBloodAllBattleEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetReduceBloodAllBattleEquip, elementID)
    {
      this.m_count = count;
      if (skillId > 131)
      {
        switch (skillId)
        {
          case 150:
            break;
          case 151:
          case 190:
            this.m_value = 1000;
            return;
          case 189:
            goto label_10;
          default:
            return;
        }
      }
      else
      {
        switch (skillId)
        {
          case 103:
            this.m_value = 200;
            return;
          case 104:
            this.m_value = 300;
            return;
          case 105:
          case 130:
            break;
          case 131:
            goto label_10;
          default:
            return;
        }
      }
      this.m_value = 500;
      return;
label_10:
      this.m_value = 800;
    }

    public override void OnAttached(Living living)
    {
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
    }

    private void player_BeginFitting(Living living)
    {
      --this.m_count;
      if (this.m_count < 0)
      {
        this.Stop();
      }
      else
      {
        living.SyncAtTime = true;
        living.AddBlood(-this.m_value, 1);
        living.SyncAtTime = false;
        if (living.Blood > 0)
          return;
        living.Die();
      }
    }

    public override bool Start(Living living)
    {
      PetReduceBloodAllBattleEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetReduceBloodAllBattleEquip) as PetReduceBloodAllBattleEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
