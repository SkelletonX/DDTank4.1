// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.AuctionUpdateHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(193, "更新拍卖")]
  public class AuctionUpdateHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num1 = packet.ReadInt();
      int num2 = packet.ReadInt();
      bool val = false;
      int num3 = GameProperties.LimitLevel(0);
      if (client.Player.PlayerCharacter.Grade < num3)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, string.Format("Você necessita ser nível {1} para continuar.", (object) num3));
        return 0;
      }
      GSPacketIn packet1 = new GSPacketIn((short) 193, client.Player.PlayerCharacter.ID);
      string translateId = "AuctionUpdateHandler.Fail";
      if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
        return 0;
      }
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        AuctionInfo auctionSingle = playerBussiness.GetAuctionSingle(num1);
        if (auctionSingle == null)
          translateId = "AuctionUpdateHandler.Msg1";
        else if (auctionSingle.PayType == 0 && num2 > client.Player.PlayerCharacter.Gold)
          translateId = "AuctionUpdateHandler.Msg2";
        else if (auctionSingle.PayType == 1 && !client.Player.MoneyDirect(num2, true))
          translateId = "";
        else if (auctionSingle.BuyerID == 0 && auctionSingle.Price > num2)
          translateId = "AuctionUpdateHandler.Msg4";
        else if (auctionSingle.BuyerID != 0 && auctionSingle.Price + auctionSingle.Rise > num2 && (auctionSingle.Mouthful == 0 || auctionSingle.Mouthful > num2))
        {
          translateId = "AuctionUpdateHandler.Msg5";
        }
        else
        {
          int buyerId = auctionSingle.BuyerID;
          auctionSingle.BuyerID = client.Player.PlayerCharacter.ID;
          auctionSingle.BuyerName = client.Player.PlayerCharacter.NickName;
          auctionSingle.Price = num2;
          if (auctionSingle.Mouthful != 0 && num2 >= auctionSingle.Mouthful)
          {
            auctionSingle.Price = auctionSingle.Mouthful;
            auctionSingle.IsExist = false;
          }
          if (playerBussiness.UpdateAuction(auctionSingle, GameProperties.Cess))
          {
            if (auctionSingle.PayType == 0)
              client.Player.RemoveGold(auctionSingle.Price);
            if (auctionSingle.IsExist)
            {
              translateId = "AuctionUpdateHandler.Msg6";
            }
            else
            {
              translateId = "AuctionUpdateHandler.Msg7";
              client.Out.SendMailResponse(auctionSingle.AuctioneerID, eMailRespose.Receiver);
              client.Out.SendMailResponse(auctionSingle.BuyerID, eMailRespose.Receiver);
            }
            if ((uint) buyerId > 0U)
              client.Out.SendMailResponse(buyerId, eMailRespose.Receiver);
            val = true;
          }
        }
        client.Out.SendAuctionRefresh(auctionSingle, num1, auctionSingle != null && auctionSingle.IsExist, (ItemInfo) null);
        if (translateId != "")
          client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId));
      }
      packet1.WriteBoolean(val);
      packet1.WriteInt(num1);
      client.Out.SendTCP(packet1);
      return 0;
    }
  }
}
