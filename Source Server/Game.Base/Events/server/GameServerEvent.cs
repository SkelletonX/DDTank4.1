// Decompiled with JetBrains decompiler
// Type: Game.Base.Events.GameServerEvent
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

namespace Game.Base.Events
{
  public class GameServerEvent : RoadEvent
  {
    public static readonly GameServerEvent Started = new GameServerEvent("Server.Started");
    public static readonly GameServerEvent Stopped = new GameServerEvent("Server.Stopped");
    public static readonly GameServerEvent WorldSave = new GameServerEvent("Server.WorldSave");

    protected GameServerEvent(string name)
      : base(name)
    {
    }
  }
}
