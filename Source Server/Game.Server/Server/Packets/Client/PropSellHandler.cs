// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.PropSellHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(55, "出售道具")]
  public class PropSellHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int gold = 0;
      int money = 0;
      int offer = 0;
      int gifttoken = 0;
      int num = 0;
      int damageScore = 0;
      int petScore = 0;
      int iTemplateID = 0;
      int iCount = 0;
      int hardCurrency = 0;
      int LeagueMoney = 0;
      int type = 1;
      int slot = packet.ReadInt();
      int ID = packet.ReadInt();
      ItemInfo itemAt = client.Player.FightBag.GetItemAt(slot);
      if (itemAt != null)
      {
        client.Player.FightBag.RemoveItem(itemAt);
        ShopMgr.SetItemType(ShopMgr.GetShopItemInfoById(ID), type, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref num, ref hardCurrency, ref LeagueMoney, ref num);
        client.Player.AddGold(gold);
      }
      return 0;
    }
  }
}
