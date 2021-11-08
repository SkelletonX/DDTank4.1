// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankHandle.GunsaluteCommand
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets;

namespace Game.Server.SceneMarryRooms.TankHandle
{
  [MarryCommandAttbute(11)]
  public class GunsaluteCommand : IMarryCommandHandler
  {
    public bool HandleCommand(
      TankMarryLogicProcessor process,
      GamePlayer player,
      GSPacketIn packet)
    {
      if (player.CurrentMarryRoom != null)
      {
        packet.ReadInt();
        if (ItemMgr.FindItemTemplate(packet.ReadInt()) != null && !player.CurrentMarryRoom.Info.IsGunsaluteUsed && (player.CurrentMarryRoom.Info.GroomID == player.PlayerCharacter.ID || player.CurrentMarryRoom.Info.BrideID == player.PlayerCharacter.ID))
        {
          player.CurrentMarryRoom.ReturnPacketForScene(player, packet);
          player.CurrentMarryRoom.Info.IsGunsaluteUsed = true;
          GSPacketIn packet1 = player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("GunsaluteCommand.Successed1", (object) player.PlayerCharacter.NickName));
          player.CurrentMarryRoom.SendToPlayerExceptSelfForScene(packet1, player);
          GameServer.Instance.LoginServer.SendMarryRoomInfoToPlayer(player.CurrentMarryRoom.Info.GroomID, true, player.CurrentMarryRoom.Info);
          GameServer.Instance.LoginServer.SendMarryRoomInfoToPlayer(player.CurrentMarryRoom.Info.BrideID, true, player.CurrentMarryRoom.Info);
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
            playerBussiness.UpdateMarryRoomInfo(player.CurrentMarryRoom.Info);
          return true;
        }
      }
      return false;
    }
  }
}
