// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.AuctionDeleteHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(194, "撤消拍卖")]
  public class AuctionDeleteHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int auctionID = packet.ReadInt();
      string translation = LanguageMgr.GetTranslation("AuctionDeleteHandler.Fail");
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        if (playerBussiness.DeleteAuction(auctionID, client.Player.PlayerCharacter.ID, ref translation))
        {
          client.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
          client.Out.SendAuctionRefresh((AuctionInfo) null, auctionID, false, (ItemInfo) null);
        }
        else
        {
          AuctionInfo auctionSingle = playerBussiness.GetAuctionSingle(auctionID);
          client.Out.SendAuctionRefresh(auctionSingle, auctionID, auctionSingle != null, (ItemInfo) null);
        }
        client.Out.SendMessage(eMessageType.GM_NOTICE, translation);
      }
      return 0;
    }
  }
}
