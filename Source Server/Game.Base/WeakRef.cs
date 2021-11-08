// Decompiled with JetBrains decompiler
// Type: Game.Base.WeakRef
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using System;

namespace Game.Base
{
  public class WeakRef : WeakReference
  {
    private static readonly WeakRef.NullValue NULL = new WeakRef.NullValue();

    public WeakRef(object target)
      : base(target == null ? (object) WeakRef.NULL : target)
    {
    }

    public WeakRef(object target, bool trackResurrection)
      : base(target == null ? (object) WeakRef.NULL : target, trackResurrection)
    {
    }

    public override object Target
    {
      get
      {
        object target = base.Target;
        if (target != WeakRef.NULL)
          return target;
        return (object) null;
      }
      set
      {
        base.Target = value == null ? (object) WeakRef.NULL : value;
      }
    }

    private class NullValue
    {
    }
  }
}
