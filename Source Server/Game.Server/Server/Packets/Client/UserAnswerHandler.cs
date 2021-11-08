// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserAnswerHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler(15, "New User Answer Question")]
  public class UserAnswerHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      byte num1 = packet.ReadByte();
      int num2 = packet.ReadInt();
      bool flag = false;
      if (num1 == (byte) 1)
        flag = packet.ReadBoolean();
      if (num1 == (byte) 1)
      {
        List<ItemInfo> info = (List<ItemInfo>) null;
        if (DropInventory.AnswerDrop(num2, ref info))
        {
          int gold = 0;
          int money = 0;
          int giftToken = 0;
          int medal = 0;
          int honor = 0;
          int hardCurrency = 0;
          int token = 0;
          int dragonToken = 0;
          int magicStonePoint = 0;
          foreach (ItemInfo itemInfo in info)
          {
            ShopMgr.FindSpecialItemInfo(itemInfo, ref gold, ref money, ref giftToken, ref medal, ref honor, ref hardCurrency, ref token, ref dragonToken, ref magicStonePoint);
            if (itemInfo != null)
              client.Player.AddTemplate(itemInfo, itemInfo.Template.BagType, itemInfo.Count, eGameView.CaddyTypeGet);
            client.Player.AddGold(gold);
            client.Player.AddMoney(money);
            client.Player.AddGiftToken(giftToken);
          }
        }
        if (flag)
          client.Player.PlayerCharacter.openFunction((Step) num2);
      }
      if (num1 == (byte) 2)
        client.Player.PlayerCharacter.openFunction((Step) num2);
      client.Player.UpdateAnswerSite(num2);
      return 1;
    }
  }
}
