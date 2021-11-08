// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Object.TurnedLiving
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using System;

namespace Game.Logic.Phy.Object
{
  public class TurnedLiving : Living
  {
    public int DefaultDelay;
    public int m_psychic;
    protected int m_delay;
    private int m_dander;
    private int int_8;
    private int int_9;

    public int Delay
    {
      get
      {
        return this.m_delay;
      }
      set
      {
        this.m_delay = value;
      }
    }

    public int Dander
    {
      get
      {
        return this.m_dander;
      }
      set
      {
        this.m_dander = value;
      }
    }

    public int PetMP
    {
      get
      {
        return this.int_9;
      }
      set
      {
        this.int_9 = value;
      }
    }

    public int PetMaxMP
    {
      get
      {
        return this.int_8;
      }
      set
      {
        this.int_8 = value;
      }
    }

    public void AddPetMP(int value)
    {
      if (value <= 0)
        return;
      if (this.IsLiving && this.PetMP < this.PetMaxMP)
      {
        this.int_9 += value;
        if (this.int_9 <= this.PetMaxMP)
          return;
        this.int_9 = this.PetMaxMP;
      }
      else
        this.int_9 = this.PetMaxMP;
    }

    public void RemovePetMP(int value)
    {
      if (value <= 0 || !this.IsLiving || this.PetMP <= 0)
        return;
      this.int_9 -= value;
      if (this.int_9 >= 0)
        return;
      this.int_9 = 0;
    }

    public TurnedLiving(
      int id,
      BaseGame game,
      int team,
      string name,
      string modelId,
      int maxBlood,
      int immunity,
      int direction)
      : base(id, game, team, name, modelId, maxBlood, immunity, direction)
    {
      this.int_8 = 100;
      this.int_9 = 10;
    }

    public override void Reset()
    {
      base.Reset();
    }

    public void AddDelay(int value)
    {
      this.m_delay += value;
    }

    public override void PrepareSelfTurn()
    {
      base.PrepareSelfTurn();
    }

    public void AddDander(int value)
    {
      if (value <= 0 || !this.IsLiving)
        return;
      this.SetDander(this.m_dander + value);
    }

    public void SetDander(int value)
    {
      this.m_dander = Math.Min(value, 200);
      if (!this.SyncAtTime)
        return;
      this.m_game.SendGameUpdateDander(this);
    }

    public virtual void StartGame()
    {
    }

    public virtual void Skip(int spendTime)
    {
      if (!this.IsAttacking)
        return;
      this.StopAttacking();
      this.m_game.CheckState(0);
    }

    public int psychic
    {
      get
      {
        return this.m_psychic;
      }
      set
      {
        this.m_psychic = value;
      }
    }
  }
}
