// Decompiled with JetBrains decompiler
// Type: Game.Base.WeakMulticastDelegate
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using log4net;
using System;
using System.Reflection;
using System.Text;

namespace Game.Base
{
  public class WeakMulticastDelegate
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private MethodInfo method;
    private WeakMulticastDelegate prev;
    private WeakReference weakRef;

    public WeakMulticastDelegate(Delegate realDelegate)
    {
      if (realDelegate.Target != null)
        this.weakRef = (WeakReference) new WeakRef(realDelegate.Target);
      this.method = realDelegate.Method;
    }

    private WeakMulticastDelegate Combine(Delegate realDelegate)
    {
      this.prev = new WeakMulticastDelegate(realDelegate)
      {
        prev = this.prev
      };
      return this;
    }

    public static WeakMulticastDelegate Combine(
      WeakMulticastDelegate weakDelegate,
      Delegate realDelegate)
    {
      if ((object) realDelegate == null)
        return (WeakMulticastDelegate) null;
      if (weakDelegate != null)
        return weakDelegate.Combine(realDelegate);
      return new WeakMulticastDelegate(realDelegate);
    }

    private WeakMulticastDelegate CombineUnique(Delegate realDelegate)
    {
      bool flag = this.Equals(realDelegate);
      if (!flag && this.prev != null)
      {
        for (WeakMulticastDelegate prev = this.prev; !flag && prev != null; prev = prev.prev)
        {
          if (prev.Equals(realDelegate))
            flag = true;
        }
      }
      if (!flag)
        return this.Combine(realDelegate);
      return this;
    }

    public static WeakMulticastDelegate CombineUnique(
      WeakMulticastDelegate weakDelegate,
      Delegate realDelegate)
    {
      if ((object) realDelegate == null)
        return (WeakMulticastDelegate) null;
      if (weakDelegate != null)
        return weakDelegate.CombineUnique(realDelegate);
      return new WeakMulticastDelegate(realDelegate);
    }

    public string Dump()
    {
      StringBuilder stringBuilder = new StringBuilder();
      WeakMulticastDelegate multicastDelegate = this;
      int num = 0;
      for (; multicastDelegate != null; multicastDelegate = multicastDelegate.prev)
      {
        ++num;
        if (multicastDelegate.weakRef == null)
        {
          stringBuilder.Append("\t");
          stringBuilder.Append(num);
          stringBuilder.Append(") ");
          stringBuilder.Append(multicastDelegate.method.Name);
          stringBuilder.Append(Environment.NewLine);
        }
        else if (multicastDelegate.weakRef.IsAlive)
        {
          stringBuilder.Append("\t");
          stringBuilder.Append(num);
          stringBuilder.Append(") ");
          stringBuilder.Append(multicastDelegate.weakRef.Target);
          stringBuilder.Append(".");
          stringBuilder.Append(multicastDelegate.method.Name);
          stringBuilder.Append(Environment.NewLine);
        }
        else
        {
          stringBuilder.Append("\t");
          stringBuilder.Append(num);
          stringBuilder.Append(") INVALID.");
          stringBuilder.Append(multicastDelegate.method.Name);
          stringBuilder.Append(Environment.NewLine);
        }
      }
      return stringBuilder.ToString();
    }

    protected bool Equals(Delegate realDelegate)
    {
      if (this.weakRef == null)
      {
        if (realDelegate.Target == null)
          return this.method == realDelegate.Method;
        return false;
      }
      if (this.weakRef.Target == realDelegate.Target)
        return this.method == realDelegate.Method;
      return false;
    }

    public void Invoke(object[] args)
    {
      for (WeakMulticastDelegate multicastDelegate = this; multicastDelegate != null; multicastDelegate = multicastDelegate.prev)
      {
        int tickCount = Environment.TickCount;
        if (multicastDelegate.weakRef == null)
          multicastDelegate.method.Invoke((object) null, args);
        else if (multicastDelegate.weakRef.IsAlive)
          multicastDelegate.method.Invoke(multicastDelegate.weakRef.Target, args);
        if (Environment.TickCount - tickCount > 500 && WeakMulticastDelegate.log.IsWarnEnabled)
          WeakMulticastDelegate.log.Warn((object) ("Invoke took " + (object) (Environment.TickCount - tickCount) + "ms! " + multicastDelegate.ToString()));
      }
    }

    public void InvokeSafe(object[] args)
    {
      for (WeakMulticastDelegate multicastDelegate = this; multicastDelegate != null; multicastDelegate = multicastDelegate.prev)
      {
        int tickCount = Environment.TickCount;
        try
        {
          if (multicastDelegate.weakRef == null)
            multicastDelegate.method.Invoke((object) null, args);
          else if (multicastDelegate.weakRef.IsAlive)
            multicastDelegate.method.Invoke(multicastDelegate.weakRef.Target, args);
        }
        catch (Exception ex)
        {
          if (WeakMulticastDelegate.log.IsErrorEnabled)
            WeakMulticastDelegate.log.Error((object) nameof (InvokeSafe), ex);
        }
        if (Environment.TickCount - tickCount > 500 && WeakMulticastDelegate.log.IsWarnEnabled)
          WeakMulticastDelegate.log.Warn((object) ("InvokeSafe took " + (object) (Environment.TickCount - tickCount) + "ms! " + multicastDelegate.ToString()));
      }
    }

    public static WeakMulticastDelegate operator +(
      WeakMulticastDelegate d,
      Delegate realD)
    {
      return WeakMulticastDelegate.Combine(d, realD);
    }

    public static WeakMulticastDelegate operator -(
      WeakMulticastDelegate d,
      Delegate realD)
    {
      return WeakMulticastDelegate.Remove(d, realD);
    }

    private WeakMulticastDelegate Remove(Delegate realDelegate)
    {
      if (this.Equals(realDelegate))
        return this.prev;
      WeakMulticastDelegate prev = this.prev;
      WeakMulticastDelegate multicastDelegate = this;
      for (; prev != null; prev = prev.prev)
      {
        if (prev.Equals(realDelegate))
        {
          multicastDelegate.prev = prev.prev;
          prev.prev = (WeakMulticastDelegate) null;
          break;
        }
        multicastDelegate = prev;
      }
      return this;
    }

    public static WeakMulticastDelegate Remove(
      WeakMulticastDelegate weakDelegate,
      Delegate realDelegate)
    {
      if ((object) realDelegate == null || weakDelegate == null)
        return (WeakMulticastDelegate) null;
      return weakDelegate.Remove(realDelegate);
    }

    public override string ToString()
    {
      Type type = (Type) null;
      if (this.method != (MethodInfo) null)
        type = this.method.DeclaringType;
      object obj = (object) null;
      if (this.weakRef != null && this.weakRef.IsAlive)
        obj = this.weakRef.Target;
      return new StringBuilder(64).Append("method: ").Append(type == (Type) null ? "(null)" : type.FullName).Append('.').Append(this.method == (MethodInfo) null ? "(null)" : this.method.Name).Append(" target: ").Append(obj == null ? "null" : obj.ToString()).ToString();
    }
  }
}
