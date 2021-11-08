// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddBloodAllPlayerAroundEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddBloodAllPlayerAroundEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetAddBloodAllPlayerAroundEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetAddBloodAllPlayerAroundEquip, elementID)
    {
      this.m_count = count;
      switch (skillId)
      {
        case 43:
          this.m_value = 300;
          break;
        case 45:
          this.m_value = 500;
          break;
        case 46:
          this.m_value = 800;
          break;
      }
    }

    public override void OnAttached(Living player)
    {
      player.BeginSelfTurn += new LivingEventHandle(this.player_SelfTurn);
    }

    public override void OnRemoved(Living player)
    {
      player.BeginSelfTurn += new LivingEventHandle(this.player_SelfTurn);
    }

    private void player_SelfTurn(Living living)
    {
      --this.m_count;
      if (this.m_count < 0)
      {
        this.Stop();
      }
      else
      {
        living.SyncAtTime = true;
        living.AddBlood(this.m_value);
        living.SyncAtTime = false;
        foreach (Living living1 in living.Game.Map.FindAllNearestSameTeam(living.X, living.Y, 150.0, living))
        {
          if (living1 != living)
          {
            living1.SyncAtTime = true;
            living1.AddBlood(this.m_value);
            living1.SyncAtTime = false;
          }
        }
      }
    }

    public override bool Start(Living living)
    {
      PetAddBloodAllPlayerAroundEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddBloodAllPlayerAroundEquip) as PetAddBloodAllPlayerAroundEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
