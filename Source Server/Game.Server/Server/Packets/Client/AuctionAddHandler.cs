// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.AuctionAddHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(192, "添加拍卖")]
  public class AuctionAddHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      eBageType eBageType = (eBageType) packet.ReadByte();
      int place = packet.ReadInt();
      int num1 = (int) packet.ReadByte();
      int num2 = packet.ReadInt();
      int num3 = packet.ReadInt();
      int num4 = packet.ReadInt();
      int count = packet.ReadInt();
      string translateId = "AuctionAddHandler.Fail";
      int num5 = 1;
      if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
        return 0;
      }
      if (num2 < 0 || num3 != 0 && num3 < num2)
        return 0;
      int num6 = 1;
      if ((uint) num5 > 0U)
      {
        num6 = 1;
        num5 = 1;
      }
      double num7 = (double) (num6 * num2) * 0.03;
      int num8;
      switch (num4)
      {
        case 0:
          num8 = 1;
          break;
        case 1:
          num8 = 3;
          break;
        default:
          num8 = 6;
          break;
      }
      double num9 = (double) num8;
      int num10 = (int) (num7 * num9);
      int num11 = num10 < 1 ? 1 : num10;
      SqlDataProvider.Data.ItemInfo itemAt = client.Player.GetItemAt(eBageType, place);
      if (num2 < 0)
        translateId = "AuctionAddHandler.Msg1";
      else if (num3 != 0 && num3 < num2)
        translateId = "AuctionAddHandler.Msg2";
      else if (num11 > client.Player.PlayerCharacter.Gold)
        translateId = "AuctionAddHandler.Msg3";
      else if (itemAt == null)
        translateId = "AuctionAddHandler.Msg4";
      else if (itemAt.IsBinds)
        translateId = "AuctionAddHandler.Msg5";
      else if (itemAt.Count >= count && count > 0)
      {
        SqlDataProvider.Data.ItemInfo.CloneFromTemplate(itemAt.Template, itemAt);
        SqlDataProvider.Data.ItemInfo itemInfo = SqlDataProvider.Data.ItemInfo.CloneFromTemplate(itemAt.Template, itemAt);
        itemInfo.Count = count;
        if (itemInfo.ItemID == 0)
        {
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
            playerBussiness.AddGoods(itemInfo);
        }
        AuctionInfo info = new AuctionInfo()
        {
          AuctioneerID = client.Player.PlayerCharacter.ID,
          AuctioneerName = client.Player.PlayerCharacter.NickName,
          BeginDate = DateTime.Now,
          BuyerID = 0,
          BuyerName = "",
          IsExist = true,
          ItemID = itemInfo.ItemID,
          Mouthful = num3,
          PayType = num5,
          Price = num2,
          Rise = num2 / 10
        };
        info.Rise = info.Rise < 1 ? 1 : info.Rise;
        info.Name = itemInfo.Template.Name;
        info.Category = itemInfo.Template.CategoryID;
        AuctionInfo auctionInfo = info;
        int num12;
        switch (num4)
        {
          case 0:
            num12 = 8;
            break;
          case 1:
            num12 = 24;
            break;
          default:
            num12 = 48;
            break;
        }
        int num13 = num12;
        auctionInfo.ValidDate = num13;
        info.TemplateID = itemInfo.TemplateID;
        info.Random = ThreadSafeRandom.NextStatic(GameProperties.BeginAuction, GameProperties.EndAuction);
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          if (playerBussiness.AddAuction(info))
          {
            client.Player.GetInventory(eBageType)?.RemoveCountFromStack(itemAt, count);
            client.Player.SaveIntoDatabase();
            client.Player.RemoveGold(num11);
            translateId = "AuctionAddHandler.Msg6";
            if(itemAt.Template.Quality >= 5) { 
            GameServer.Instance.LoginServer.SendPacket(WorldMgr.SendSysNotice(eMessageType.ChatNormal, LanguageMgr.GetTranslation("GetAuctionHandlerSystem.Msg", client.Player.PlayerCharacter.NickName, info.TemplateID,  info.Name), info.ItemID, info.TemplateID, (string) null)); //GetAuctionHandlerSystem.Msg
                        client.Out.SendAuctionRefresh(info, info.AuctionID, true, itemAt);
                        }
                    }
        }
      }
      else
        translateId = "AuctionAddHandler.Msg13";
      client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId));
      return 0;
    }
  }
}
