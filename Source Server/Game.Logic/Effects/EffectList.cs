// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.EffectList
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;
using log4net;
using System;
using System.Collections;
using System.Reflection;

namespace Game.Logic.Effects
{
  public class EffectList
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected volatile sbyte m_changesCount;
    protected ArrayList m_effects;
    protected int m_immunity;
    protected readonly Living m_owner;

    public EffectList(Living owner, int immunity)
    {
      this.m_owner = owner;
      this.m_effects = new ArrayList(3);
      this.m_immunity = immunity;
    }

    public virtual bool Add(AbstractEffect effect)
    {
      if (this.CanAddEffect(effect.TypeValue))
      {
        lock (this.m_effects)
          this.m_effects.Add((object) effect);
        effect.OnAttached(this.m_owner);
        this.OnEffectsChanged(effect);
        return true;
      }
      if (effect.TypeValue == 9 && this.m_owner is SimpleBoss)
        this.m_owner.State = 0;
      return false;
    }

    public void BeginChanges()
    {
      ++this.m_changesCount;
    }

    public bool CanAddEffect(int id)
    {
      if (id <= 40 && id >= 0)
        return (1 << id - 1 & this.m_immunity) == 0;
      return true;
    }

    public virtual void CommitChanges()
    {
      if (--this.m_changesCount < (sbyte) 0)
      {
        if (EffectList.log.IsWarnEnabled)
          EffectList.log.Warn((object) ("changes count is less than zero, forgot BeginChanges()?\n" + Environment.StackTrace));
        this.m_changesCount = (sbyte) 0;
      }
      if ((uint) this.m_changesCount > 0U)
        return;
      this.UpdateChangedEffects();
    }

    public virtual IList GetAllOfType(Type effectType)
    {
      ArrayList arrayList = new ArrayList();
      lock (this.m_effects)
      {
        foreach (AbstractEffect effect in this.m_effects)
        {
          if (effect.GetType().Equals(effectType))
            arrayList.Add((object) effect);
        }
      }
      return (IList) arrayList;
    }

    public virtual AbstractEffect GetOfType(eEffectType effectType)
    {
      lock (this.m_effects)
      {
        foreach (AbstractEffect effect in this.m_effects)
        {
          if (effect.Type == effectType)
            return effect;
        }
      }
      return (AbstractEffect) null;
    }

    public virtual void OnEffectsChanged(AbstractEffect changedEffect)
    {
      if (this.m_changesCount > (sbyte) 0)
        return;
      this.UpdateChangedEffects();
    }

    public virtual bool Remove(AbstractEffect effect)
    {
      int index = -1;
      lock (this.m_effects)
      {
        index = this.m_effects.IndexOf((object) effect);
        if (index < 0)
          return false;
        this.m_effects.RemoveAt(index);
      }
      if (index == -1)
        return false;
      effect.OnRemoved(this.m_owner);
      this.OnEffectsChanged(effect);
      return true;
    }

    public void StopAllEffect()
    {
      if (this.m_effects.Count <= 0)
        return;
      AbstractEffect[] abstractEffectArray = new AbstractEffect[this.m_effects.Count];
      this.m_effects.CopyTo((Array) abstractEffectArray);
      foreach (AbstractEffect abstractEffect in abstractEffectArray)
        abstractEffect.Stop();
      this.m_effects.Clear();
    }

    public void StopEffect(Type effectType)
    {
      IList allOfType = this.GetAllOfType(effectType);
      this.BeginChanges();
      foreach (AbstractEffect abstractEffect in (IEnumerable) allOfType)
        abstractEffect.Stop();
      this.CommitChanges();
    }

    protected virtual void UpdateChangedEffects()
    {
    }

    public ArrayList List
    {
      get
      {
        return this.m_effects;
      }
    }
  }
}
