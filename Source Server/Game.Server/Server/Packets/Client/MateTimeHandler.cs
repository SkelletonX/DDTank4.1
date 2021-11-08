// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MateTimeHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(85, "场景用户离开")]
  public class MateTimeHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      GamePlayer playerById = WorldMgr.GetPlayerById(num);
      PlayerInfo playerInfo;
      if (playerById != null)
      {
        playerInfo = playerById.PlayerCharacter;
      }
      else
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
          playerInfo = playerBussiness.GetUserSingleByUserID(num);
      }
      GSPacketIn pkg = new GSPacketIn((short) 85, client.Player.PlayerCharacter.ID);
      if (playerInfo == null)
        pkg.WriteDateTime(DateTime.Now);
      else
        pkg.WriteDateTime(playerInfo.LastDate);
      client.SendTCP(pkg);
      return 0;
    }
  }
}
