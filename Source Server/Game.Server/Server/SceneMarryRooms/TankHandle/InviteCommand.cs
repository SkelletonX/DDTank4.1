// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankHandle.InviteCommand
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;

namespace Game.Server.SceneMarryRooms.TankHandle
{
  [MarryCommandAttbute(4)]
  public class InviteCommand : IMarryCommandHandler
  {
    public bool HandleCommand(
      TankMarryLogicProcessor process,
      GamePlayer player,
      GSPacketIn packet)
    {
      if (player.CurrentMarryRoom != null && player.CurrentMarryRoom.RoomState == eRoomState.FREE && (player.CurrentMarryRoom.Info.GuestInvite || player.PlayerCharacter.ID == player.CurrentMarryRoom.Info.GroomID || player.PlayerCharacter.ID == player.CurrentMarryRoom.Info.BrideID))
      {
        GSPacketIn packet1 = packet.Clone();
        packet1.ClearContext();
        GamePlayer playerById = WorldMgr.GetPlayerById(packet.ReadInt());
        if (playerById != null && playerById.CurrentRoom == null && playerById.CurrentMarryRoom == null)
        {
          packet1.WriteByte((byte) 4);
          packet1.WriteInt(player.PlayerCharacter.ID);
          packet1.WriteString(player.PlayerCharacter.NickName);
          packet1.WriteInt(player.CurrentMarryRoom.Info.ID);
          packet1.WriteString(player.CurrentMarryRoom.Info.Name);
          packet1.WriteString(player.CurrentMarryRoom.Info.Pwd);
          packet1.WriteInt(player.MarryMap);
          playerById.Out.SendTCP(packet1);
          return true;
        }
      }
      return false;
    }
  }
}
