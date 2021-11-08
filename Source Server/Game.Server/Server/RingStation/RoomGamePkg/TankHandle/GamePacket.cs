// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.RoomGamePkg.TankHandle.GamePacket
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.RingStation.RoomGamePkg.TankHandle
{
  [GameCommandAttbute(91)]
  public class GamePacket : IGameCommandHandler
  {
    public bool HandleCommand(
      TankGameLogicProcessor process,
      RingStationGamePlayer player,
      GSPacketIn packet)
    {
      switch (packet.ReadByte())
      {
        case 6:
          player.NextTurn(packet);
          break;
        case 46:
          player.AddTurn(packet);
          break;
        case 100:
          player.CurRoom.RemovePlayer(player);
          break;
        case 103:
          player.SendLoadingComplete(100);
          break;
      }
      return true;
    }
  }
}
