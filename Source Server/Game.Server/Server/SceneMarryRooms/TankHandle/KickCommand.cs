// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankHandle.KickCommand
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.SceneMarryRooms.TankHandle
{
  [MarryCommandAttbute(7)]
  public class KickCommand : IMarryCommandHandler
  {
    public bool HandleCommand(
      TankMarryLogicProcessor process,
      GamePlayer player,
      GSPacketIn packet)
    {
      if (player.CurrentMarryRoom == null || player.CurrentMarryRoom.RoomState != eRoomState.FREE || player.PlayerCharacter.ID != player.CurrentMarryRoom.Info.GroomID && player.PlayerCharacter.ID != player.CurrentMarryRoom.Info.BrideID)
        return false;
      int userID = packet.ReadInt();
      player.CurrentMarryRoom.KickPlayerByUserID(player, userID);
      return true;
    }
  }
}
