// Decompiled with JetBrains decompiler
// Type: Game.Logic.AI.ABrain
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.AI
{
  public abstract class ABrain
  {
    protected Living m_body;
    protected BaseGame m_game;

    public virtual void Dispose()
    {
    }

    public virtual void OnAfterTakedBomb()
    {
    }
        public virtual void OnShootedSay()
        {
        }

        public virtual void OnBeforeTakedDamage(
      Living source,
      ref int damageAmount,
      ref int criticalAmount)
    {
    }

    public virtual void OnAfterTakedFrozen()
    {
    }

    public virtual void OnBeginNewTurn()
    {
    }

    public virtual void OnBeginSelfTurn()
    {
    }

    public virtual void OnCreated()
    {
    }

    public virtual void OnDie()
    {
    }

    public virtual void OnDiedSay()
    {
    }

    public virtual void OnKillPlayerSay()
    {
    }

    public virtual void OnShootedSay(int delay)
    {
    }

    public virtual void OnStartAttacking()
    {
    }

    public virtual void OnStopAttacking()
    {
    }

    public virtual void OnDiedEvent()
    {
    }

    public Living Body
    {
      get
      {
        return this.m_body;
      }
      set
      {
        this.m_body = value;
      }
    }

    public BaseGame Game
    {
      get
      {
        return this.m_game;
      }
      set
      {
        this.m_game = value;
      }
    }
  }
}
