// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryStatusHandler
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
  [PacketHandler(246, "请求结婚状态")]
  internal class MarryStatusHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      GamePlayer playerById = WorldMgr.GetPlayerById(num);
      if (playerById != null)
      {
        client.Player.Out.SendPlayerMarryStatus(client.Player, playerById.PlayerCharacter.ID, playerById.PlayerCharacter.IsMarried);
      }
      else
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          PlayerInfo userSingleByUserId = playerBussiness.GetUserSingleByUserID(num);
          client.Player.Out.SendPlayerMarryStatus(client.Player, userSingleByUserId.ID, userSingleByUserId.IsMarried);
        }
      }
      return 0;
    }
  }
}
