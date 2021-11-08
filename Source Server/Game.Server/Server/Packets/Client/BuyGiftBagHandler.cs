// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.BuyGiftBagHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler((int)ePackageType.BUY_GIFTBAG, "Pacote de Oferta")]
  public class BuyGiftBagHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      /*
      packet.ReadInt();
      int num = 4500;
      packet.ReadBoolean();
      if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
        return 1;
      }
      if (client.Player.MoneyDirect(num, false))
      {
        ItemInfo fromTemplate1 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11023), 1, 104);
        ItemInfo fromTemplate2 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11023), 1, 104);
        ItemInfo fromTemplate3 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11023), 1, 104);
        ItemInfo fromTemplate4 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11020), 1, 104);
        ItemInfo fromTemplate5 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11018), 1, 104);
        if (!client.Player.StoreBag.AddItemTo(fromTemplate1, 0))
          client.Player.AddTemplate(fromTemplate1);
        if (!client.Player.StoreBag.AddItemTo(fromTemplate2, 1))
          client.Player.AddTemplate(fromTemplate2);
        if (!client.Player.StoreBag.AddItemTo(fromTemplate3, 2))
          client.Player.AddTemplate(fromTemplate3);
        if (!client.Player.StoreBag.AddItemTo(fromTemplate4, 3))
          client.Player.AddTemplate(fromTemplate4);
        if (!client.Player.StoreBag.AddItemTo(fromTemplate5, 4))
          client.Player.AddTemplate(fromTemplate5);
        client.Player.RemoveMoney(num);
        client.Out.SendMessage(eMessageType.GM_NOTICE, "A compra foi bem sucedida.!");
      }*/
      return 0;
    }
  }
}
