// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.GetPlayerCardHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler(18, "场景用户离开")]
  public class GetPlayerCardHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      GamePlayer playerById = WorldMgr.GetPlayerById(num);
      PlayerInfo player;
      List<UsersCardInfo> userCard;
      if (playerById != null)
      {
        player = playerById.PlayerCharacter;
        userCard = playerById.CardBag.GetCards(0, 4);
      }
      else
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          player = playerBussiness.GetUserSingleByUserID(num);
          userCard = playerBussiness.GetUserCardEuqip(num);
        }
      }
      if (userCard != null && player != null)
        client.Player.Out.SendUpdateCardData(player, userCard);
      return 0;
    }
  }
}
