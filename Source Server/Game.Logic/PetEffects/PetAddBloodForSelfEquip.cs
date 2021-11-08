// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddBloodForSelfEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddBloodForSelfEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetAddBloodForSelfEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetAddBloodForSelfEquip, elementID)
    {
      this.m_count = count;
      if (skillId != 61)
      {
        if (skillId != 62)
          return;
        this.m_value = 800;
      }
      else
        this.m_value = 500;
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
        living.Game.SendPetBuff(living, this.Info, false);
        this.Stop();
      }
      else
      {
        living.SyncAtTime = true;
        living.AddBlood(this.m_value);
        living.SyncAtTime = false;
      }
    }

    public override bool Start(Living living)
    {
      PetAddBloodForSelfEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddBloodForSelfEquip) as PetAddBloodForSelfEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
