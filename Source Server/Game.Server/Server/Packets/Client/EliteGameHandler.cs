// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.EliteGameHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Logic;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.Packets.Client
{
  [PacketHandler(162, "添加拍卖")]
  public class EliteGameHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      switch (packet.ReadByte())
      {
        case 1:
          GSPacketIn packet1 = new GSPacketIn((short) 162);
          packet1.WriteByte((byte) 1);
          packet1.WriteInt(ExerciseMgr.EliteStatus);
          client.Out.SendTCP(packet1);
          break;
        case 2:
          if (ExerciseMgr.EliteStatus == 5 && client.Player.PlayerCharacter.Grade >= 30)
          {
            client.Out.SendEliteGameStartRoom();
            break;
          }
          break;
        case 3:
          GSPacketIn packet2 = new GSPacketIn((short) 162);
          packet2.WriteByte((byte) 3);
          packet2.WriteInt(client.Player.PlayerCharacter.EliteRank);
          packet2.WriteInt(client.Player.PlayerCharacter.EliteScore);
          client.Out.SendTCP(packet2);
          break;
        case 4:
          int param = packet.ReadInt();
          List<PlayerEliteGameInfo> list = ExerciseMgr.EliteGameChampionPlayersList.Where<KeyValuePair<int, PlayerEliteGameInfo>>((Func<KeyValuePair<int, PlayerEliteGameInfo>, bool>) (a => a.Value.GameType == param)).Select<KeyValuePair<int, PlayerEliteGameInfo>, PlayerEliteGameInfo>((Func<KeyValuePair<int, PlayerEliteGameInfo>, PlayerEliteGameInfo>) (a => a.Value)).ToList<PlayerEliteGameInfo>();
          GSPacketIn packet3 = new GSPacketIn((short) 162);
          packet3.WriteByte((byte) 4);
          packet3.WriteInt(param);
          packet3.WriteInt(list.Count);
          foreach (PlayerEliteGameInfo playerEliteGameInfo in list)
          {
            packet3.WriteInt(playerEliteGameInfo.UserID);
            packet3.WriteString(playerEliteGameInfo.NickName);
            packet3.WriteInt(playerEliteGameInfo.Rank);
            packet3.WriteInt(playerEliteGameInfo.Status);
            packet3.WriteInt(playerEliteGameInfo.Winer);
          }
          client.Out.SendTCP(packet3);
          break;
      }
      return 0;
    }
  }
}
