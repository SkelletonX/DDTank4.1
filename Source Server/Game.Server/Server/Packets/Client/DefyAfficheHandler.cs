// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.DefyAfficheHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(123, "场景用户离开")]
  public class DefyAfficheHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      string str = packet.ReadString();
      int num = 500;
      if (client.Player.PlayerCharacter.Money + client.Player.PlayerCharacter.MoneyLock >= num)
      {
        client.Player.RemoveMoney(num);
        GSPacketIn packet1 = new GSPacketIn((short) 123);
        packet1.WriteString(str);
        GameServer.Instance.LoginServer.SendPacket(packet1);
        client.Player.LastChatTime = DateTime.Now;
        foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
        {
          packet1.ClientID = allPlayer.PlayerCharacter.ID;
          allPlayer.Out.SendTCP(packet1);
        }
        client.Player.OnPlayerDispatches();
      }
      else
        client.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("UserBuyItemHandler.Money"));
      return 0;
    }
  }
}
