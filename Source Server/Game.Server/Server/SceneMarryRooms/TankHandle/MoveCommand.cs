// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankHandle.MoveCommand
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.SceneMarryRooms.TankHandle
{
  [MarryCommandAttbute(1)]
  public class MoveCommand : IMarryCommandHandler
  {
    public bool HandleCommand(
      TankMarryLogicProcessor process,
      GamePlayer player,
      GSPacketIn packet)
    {
      if (player.CurrentMarryRoom == null || player.CurrentMarryRoom.RoomState != eRoomState.FREE)
        return false;
      player.X = packet.ReadInt();
      player.Y = packet.ReadInt();
      player.CurrentMarryRoom.ReturnPacketForScene(player, packet);
      return true;
    }
  }
}
