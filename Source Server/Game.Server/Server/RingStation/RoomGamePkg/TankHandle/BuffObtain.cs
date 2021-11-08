// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.RoomGamePkg.TankHandle.BuffObtain
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.RingStation.RoomGamePkg.TankHandle
{
  [GameCommandAttbute(186)]
  public class BuffObtain : IGameCommandHandler
  {
    public bool HandleCommand(
      TankGameLogicProcessor process,
      RingStationGamePlayer player,
      GSPacketIn packet)
    {
      return true;
    }
  }
}
