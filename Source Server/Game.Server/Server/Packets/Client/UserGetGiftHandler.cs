// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserGetGiftHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(218, "领取奖品")]
  public class UserGetGiftHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      UserGiftInfo[] allGifts = (UserGiftInfo[]) null;
      PlayerInfo player = client.Player.PlayerCharacter;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        allGifts = playerBussiness.GetAllUserReceivedGifts(num);
        if (player.ID != num)
        {
          GamePlayer playerById = WorldMgr.GetPlayerById(num);
          player = playerById == null ? playerBussiness.GetUserSingleByUserID(num) : playerById.PlayerCharacter;
        }
      }
      if (allGifts != null && player != null)
        client.Out.SendGetUserGift(player, allGifts);
      return 0;
    }
  }
}
