// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AbstractEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Phy.Object;
using System;

namespace Game.Logic.Effects
{
  public abstract class AbstractEffect
  {
    protected static ThreadSafeRandom random = new ThreadSafeRandom();
    protected Random rand = new Random();
    public bool IsTrigger;
    protected Living m_living;
    private eEffectType m_type;

    public AbstractEffect(eEffectType type)
    {
      this.m_type = type;
    }

    public virtual void OnAttached(Living living)
    {
    }

    public virtual void OnRemoved(Living living)
    {
    }

    public virtual bool Start(Living living)
    {
      this.m_living = living;
      return this.m_living.EffectList.Add(this);
    }

    public virtual bool Stop()
    {
      if (this.m_living != null)
        return this.m_living.EffectList.Remove(this);
      return false;
    }

    public eEffectType Type
    {
      get
      {
        return this.m_type;
      }
    }

    public int TypeValue
    {
      get
      {
        return (int) this.m_type;
      }
    }
  }
}
