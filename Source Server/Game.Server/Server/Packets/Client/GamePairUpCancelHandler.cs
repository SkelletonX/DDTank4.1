// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.GamePairUpCancelHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
  [PacketHandler(210, "撮合取消")]
  public class GamePairUpCancelHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GamePlayer player = client.Player;
      if (player.CurrentRoom != null && player.CurrentRoom.BattleServer != null)
      {
        player.CurrentRoom.BattleServer.RemoveRoom(player.CurrentRoom);
        if (player != player.CurrentRoom.Host)
        {
          player.CurrentRoom.Host.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("Game.Server.SceneGames.PairUp.Failed"));
          RoomMgr.UpdatePlayerState(player, (byte) 0);
        }
        else
          RoomMgr.UpdatePlayerState(player, (byte) 2);
      }
      return 0;
    }
  }
}
