// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankHandle.ForbidCommand
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.SceneMarryRooms.TankHandle
{
  [MarryCommandAttbute(8)]
  public class ForbidCommand : IMarryCommandHandler
  {
    public bool HandleCommand(
      TankMarryLogicProcessor process,
      GamePlayer player,
      GSPacketIn packet)
    {
      if (player.CurrentMarryRoom == null || player.PlayerCharacter.ID != player.CurrentMarryRoom.Info.GroomID && player.PlayerCharacter.ID != player.CurrentMarryRoom.Info.BrideID)
        return false;
      int userID = packet.ReadInt();
      if (userID != player.CurrentMarryRoom.Info.BrideID && userID != player.CurrentMarryRoom.Info.GroomID)
      {
        player.CurrentMarryRoom.KickPlayerByUserID(player, userID);
        player.CurrentMarryRoom.SetUserForbid(userID);
      }
      return true;
    }
  }
}
