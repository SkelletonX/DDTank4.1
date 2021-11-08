// Decompiled with JetBrains decompiler
// Type: Game.Base.Events.ScriptEvent
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

namespace Game.Base.Events
{
  public class ScriptEvent : RoadEvent
  {
    public static readonly ScriptEvent Loaded = new ScriptEvent("Script.Loaded");
    public static readonly ScriptEvent Unloaded = new ScriptEvent("Script.Unloaded");

    protected ScriptEvent(string name)
      : base(name)
    {
    }
  }
}
