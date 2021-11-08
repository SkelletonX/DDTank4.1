// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ItemInlayHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler(121, "物品镶嵌")]
  public class ItemInlayHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GSPacketIn pkg = packet.Clone();
      pkg.ClearContext();
      int num1 = packet.ReadInt();
      int place1 = packet.ReadInt();
      int num2 = packet.ReadInt();
      int num3 = packet.ReadInt();
      int place2 = packet.ReadInt();
      SqlDataProvider.Data.ItemInfo itemAt1 = client.Player.GetItemAt((eBageType) num1, place1);
      SqlDataProvider.Data.ItemInfo itemAt2 = client.Player.GetItemAt((eBageType) num3, place2);
      string str1 = "";
      int inlayGoldPrice = GameProperties.InlayGoldPrice;
      if (itemAt1 == null || itemAt2 == null || itemAt2.Template.Property1 != 31)
        return 0;
      if (client.Player.PlayerCharacter.Gold >= inlayGoldPrice)
      {
        string[] strArray = itemAt1.Template.Hole.Split('|');
        if (num2 > 0 && num2 < 7)
        {
          client.Player.RemoveGold(inlayGoldPrice);
          bool flag = false;
          string str2;
          switch (num2)
          {
            case 1:
              if (Convert.ToInt32(strArray[0].Split(',')[1]) == itemAt2.Template.Property2)
              {
                itemAt1.Hole1 = itemAt2.TemplateID;
                str2 = str1 + "," + (object) itemAt2.ItemID + itemAt2.Template.Name;
                flag = true;
                break;
              }
              break;
            case 2:
              if (Convert.ToInt32(strArray[1].Split(',')[1]) == itemAt2.Template.Property2)
              {
                itemAt1.Hole2 = itemAt2.TemplateID;
                str2 = str1 + "," + (object) itemAt2.ItemID + itemAt2.Template.Name;
                flag = true;
                break;
              }
              break;
            case 3:
              if (Convert.ToInt32(strArray[2].Split(',')[1]) == itemAt2.Template.Property2)
              {
                itemAt1.Hole3 = itemAt2.TemplateID;
                str2 = str1 + "," + (object) itemAt2.ItemID + itemAt2.Template.Name;
                flag = true;
                break;
              }
              break;
            case 4:
              if (Convert.ToInt32(strArray[3].Split(',')[1]) == itemAt2.Template.Property2)
              {
                itemAt1.Hole4 = itemAt2.TemplateID;
                str2 = str1 + "," + (object) itemAt2.ItemID + itemAt2.Template.Name;
                flag = true;
                break;
              }
              break;
            case 5:
              if (Convert.ToInt32(strArray[4].Split(',')[1]) == itemAt2.Template.Property2)
              {
                if ((uint) itemAt1.Hole5 > 0U)
                {
                  SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemAt1.Hole5), 1, 102);
                  fromTemplate.IsBinds = true;
                  fromTemplate.ValidDate = 0;
                  if (!client.Player.AddItem(fromTemplate))
                  {
                    GamePlayer player = client.Player;
                    List<SqlDataProvider.Data.ItemInfo> items = new List<SqlDataProvider.Data.ItemInfo>();
                    items.Add(fromTemplate);
                    string content = "Mochila cheia.";
                    string title = "Mochila cheia";
                    int num4 = 8;
                    player.SendItemsToMail(items, content, title, (eMailType) num4);
                  }
                }
                itemAt1.Hole5 = itemAt2.TemplateID;
                str2 = str1 + "," + (object) itemAt2.ItemID + itemAt2.Template.Name;
                flag = true;
                break;
              }
              break;
            case 6:
              if (Convert.ToInt32(strArray[5].Split(',')[1]) == itemAt2.Template.Property2)
              {
                if ((uint) itemAt1.Hole6 > 0U)
                {
                  SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemAt1.Hole6), 1, 102);
                  fromTemplate.IsBinds = true;
                  fromTemplate.ValidDate = 0;
                  if (!client.Player.AddItem(fromTemplate))
                  {
                    GamePlayer player = client.Player;
                    List<SqlDataProvider.Data.ItemInfo> items = new List<SqlDataProvider.Data.ItemInfo>();
                    items.Add(fromTemplate);
                    string content = "Mochila cheia.";
                    string title = "Mochila cheia";
                    int num4 = 8;
                    player.SendItemsToMail(items, content, title, (eMailType) num4);
                  }
                }
                itemAt1.Hole6 = itemAt2.TemplateID;
                str2 = str1 + "," + (object) itemAt2.ItemID + itemAt2.Template.Name;
                flag = true;
                break;
              }
              break;
          }
          if (flag)
          {
            pkg.WriteInt(0);
            --itemAt2.Count;
            client.Player.UpdateItem(itemAt2);
            client.Player.UpdateItem(itemAt1);
          }
          else
            client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.InlayItem.Msg1"));
        }
        else
        {
          pkg.WriteByte((byte) 1);
          client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemInlayHandle.NoPlace"));
        }
        client.Player.SendTCP(pkg);
        client.Player.SaveIntoDatabase();
      }
      else
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("UserBuyItemHandler.NoMoney"));
      return 0;
    }
  }
}
