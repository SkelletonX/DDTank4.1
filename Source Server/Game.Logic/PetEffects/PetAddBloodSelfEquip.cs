// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetAddBloodSelfEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetAddBloodSelfEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetAddBloodSelfEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetAddBloodSelfEquip, elementID)
    {
      this.m_count = count;
      if (skillId != 181)
      {
        if (skillId != 182)
          return;
        this.m_value = 1000;
      }
      else
        this.m_value = 800;
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
        if (living.Game.RoomType != eRoomType.Match && living.Game.RoomType != eRoomType.Freedom)
          return;
        living.SyncAtTime = true;
        living.AddBlood(this.m_value);
        living.SyncAtTime = false;
      }
    }

    public override bool Start(Living living)
    {
      PetAddBloodSelfEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetAddBloodSelfEquip) as PetAddBloodSelfEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
