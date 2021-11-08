// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserChangeItemColorHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler(182, "改变物品颜色")]
  public class UserChangeItemColorHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      eMessageType type = eMessageType.GM_NOTICE;
      string translateId = "UserChangeItemColorHandler.Success";
      packet.ReadInt();
      int slot1 = packet.ReadInt();
      packet.ReadInt();
      int slot2 = packet.ReadInt();
      string str1 = packet.ReadString();
      string str2 = packet.ReadString();
      int num1 = packet.ReadInt();
      ItemInfo itemAt1 = client.Player.EquipBag.GetItemAt(slot2);
      ItemInfo itemAt2 = client.Player.PropBag.GetItemAt(slot1);
      if (itemAt1 != null)
      {
        client.Player.BeginChanges();
        try
        {
          bool flag = false;
          if (itemAt2 != null && itemAt2.IsValidItem())
          {
            client.Player.PropBag.RemoveCountFromStack(itemAt2, 1);
            flag = true;
          }
          else
          {
            ItemMgr.FindItemTemplate(num1);
            List<ShopItemInfo> shopbyTemplatId = ShopMgr.FindShopbyTemplatID(num1);
            int num2 = 0;
            for (int index = 0; index < shopbyTemplatId.Count; ++index)
            {
              if (shopbyTemplatId[index].APrice1 == -1 && (uint) shopbyTemplatId[index].AValue1 > 0U)
                num2 = shopbyTemplatId[index].AValue1;
            }
            if (num2 <= client.Player.PlayerCharacter.Money + client.Player.PlayerCharacter.MoneyLock)
            {
              client.Player.RemoveMoney(num2);
              flag = true;
            }
          }
          if (flag)
          {
            itemAt1.Color = str1 == null ? "" : str1;
            itemAt1.Skin = str2 == null ? "" : str2;
            client.Player.EquipBag.UpdateItem(itemAt1);
          }
        }
        finally
        {
          client.Player.CommitChanges();
        }
      }
      client.Out.SendMessage(type, LanguageMgr.GetTranslation(translateId));
      return 0;
    }
  }
}
