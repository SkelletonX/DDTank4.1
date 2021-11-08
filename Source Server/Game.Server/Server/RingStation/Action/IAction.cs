// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.Action.IAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

namespace Game.Server.RingStation.Action
{
  public interface IAction
  {
    void Execute(RingStationGamePlayer player, long tick);

    bool IsFinished(long tick);
  }
}
