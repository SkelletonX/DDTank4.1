using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(106, "场景用户离开")]
  public class WishBeadEquipHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int place1 = packet.ReadInt();
      int num1 = packet.ReadInt();
      int templateId1 = packet.ReadInt();
      int place2 = packet.ReadInt();
      int num2 = packet.ReadInt();
      int templateId2 = packet.ReadInt();
      GSPacketIn packet1 = new GSPacketIn((short) 106, client.Player.PlayerCharacter.ID);
      SqlDataProvider.Data.ItemInfo itemAt1 = client.Player.GetItemAt((eBageType) num1, place1);
      SqlDataProvider.Data.ItemInfo itemAt2 = client.Player.GetItemAt((eBageType) num2, place2);
      if (itemAt2 != null && itemAt1 != null)
      {
        if (itemAt2.Count >= 1 && itemAt2.TemplateID == templateId2)
        {
          if (!this.method_0(itemAt2.TemplateID, itemAt1.Template.CategoryID))
          {
            packet1.WriteInt(5);
            client.Out.SendTCP(packet1);
            return 0;
          }
          double num3 = GameProperties.WishBeadRate * 100.0;
          GoldEquipTemplateInfo goldEquipByTemplate = GoldEquipMgr.FindGoldEquipByTemplate(templateId1);
          itemAt1.IsBinds = true;
          if (goldEquipByTemplate == null && itemAt1.Template.CategoryID == 7)
            packet1.WriteInt(5);
          else if (itemAt1.StrengthenLevel > GameProperties.WishBeadLimitLv && GameProperties.IsWishBeadLimit)
            packet1.WriteInt(5);
          else if (!itemAt1.isGold)
          {
            int num4 = new RandomSafe().Next(10000);
            if (num3 > (double) num4 && num4 > 0)
            {
              itemAt1.goldBeginTime = DateTime.Now;
              itemAt1.goldValidDate = 30;
              if (itemAt1.Template.CategoryID == 7)
              {
                ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(goldEquipByTemplate.NewTemplateId);
                if (itemTemplate != null)
                  itemAt1.GoldEquip = itemTemplate;
              }
              client.Player.UpdateItem(itemAt1);
              packet1.WriteInt(0);
              GameServer.Instance.LoginServer.SendPacket(WorldMgr.SendSysNotice(eMessageType.ChatNormal, LanguageMgr.GetTranslation("WishBeadHandler.congratulation", (object) client.Player.PlayerCharacter.NickName, (object) itemAt1.TemplateID), itemAt1.ItemID, itemAt1.TemplateID, (string) null));
              client.Player.AddLog("ItemWishBead", "id:" + (object) itemAt1.TemplateID + "|name:" + itemAt1.Template.Name + "|rand:" + (object) num4);
            }
            else
              packet1.WriteInt(1);
            if (num4 <= 0)
              client.Player.AddLog("ItemWishBead", "id:" + (object) itemAt1.TemplateID + "|name:" + itemAt1.Template.Name + "|rand:" + (object) num4);
            client.Player.RemoveTemplate(templateId2, 1);
          }
          else
            packet1.WriteInt(6);
          client.Out.SendTCP(packet1);
          return 0;
        }
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("WishBeadEquipHandler.Msg2"));
        packet1.WriteInt(5);
        client.Out.SendTCP(packet1);
        return 0;
      }
      client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("WishBeadEquipHandler.Msg1"));
      packet1.WriteInt(5);
      client.Out.SendTCP(packet1);
      return 0;
    }

    private bool method_0(int int_0, int int_1)
    {
      if (int_0 == 11560 && int_1 == 7 || int_0 == 11561 && int_1 == 5)
        return true;
      if (int_0 == 11562)
        return int_1 == 1;
      return false;
    }
  }
}
