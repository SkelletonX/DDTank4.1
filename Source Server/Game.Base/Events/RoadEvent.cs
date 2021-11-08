// Decompiled with JetBrains decompiler
// Type: Game.Base.Events.RoadEvent
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

namespace Game.Base.Events
{
  public abstract class RoadEvent
  {
    protected string m_EventName;

    public RoadEvent(string name)
    {
      this.m_EventName = name;
    }

    public virtual bool IsValidFor(object o)
    {
      return true;
    }

    public override string ToString()
    {
      return "DOLEvent(" + this.m_EventName + ")";
    }

    public string Name
    {
      get
      {
        return this.m_EventName;
      }
    }
  }
}
