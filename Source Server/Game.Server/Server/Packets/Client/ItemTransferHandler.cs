// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ItemTransferHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System.Text;

namespace Game.Server.Packets.Client
{
  [PacketHandler(61, "物品转移")]
  public class ItemTransferHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GSPacketIn pkg = packet.Clone();
      pkg.ClearContext();
      StringBuilder stringBuilder = new StringBuilder();
      int num1 = 10000;
      bool tranHole = packet.ReadBoolean();
      bool tranHoleFivSix = packet.ReadBoolean();
      ItemInfo itemAt1 = client.Player.StoreBag.GetItemAt(0);
      ItemInfo itemAt2 = client.Player.StoreBag.GetItemAt(1);
      if (itemAt1 != null && itemAt2 != null && (itemAt1.Template.CategoryID == itemAt2.Template.CategoryID && itemAt2.Count == 1) && itemAt1.Count == 1)
      {
        if (client.Player.PlayerCharacter.Gold < num1)
        {
          client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemTransferHandler.NoGold"));
          return 1;
        }
        client.Player.RemoveGold(num1);
        StrengthenMgr.InheritTransferProperty(ref itemAt1, ref itemAt2, tranHole, tranHoleFivSix);
        int num2 = this.method_0(itemAt1);
        int num3 = this.method_0(itemAt2);
        ItemTemplateInfo goods1 = (ItemTemplateInfo) null;
        ItemTemplateInfo goods2 = (ItemTemplateInfo) null;
        if (num2 > 0)
          goods1 = ItemMgr.FindItemTemplate(this.method_1(itemAt1));
        if (num3 > 0)
          goods2 = ItemMgr.FindItemTemplate(this.method_1(itemAt2));
        if (this.TransferCondition(itemAt2, itemAt1) && goods1 != null && goods2 != null)
        {
          ItemInfo itemInfo1 = ItemInfo.CloneFromTemplate(goods1, itemAt1);
          ItemInfo.OpenHole(ref itemInfo1);
          if (itemInfo1.isGold)
          {
            GoldEquipTemplateInfo goldEquipByTemplate = GoldEquipMgr.FindGoldEquipByTemplate(goods1.TemplateID);
            if (goldEquipByTemplate != null)
            {
              ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(goldEquipByTemplate.NewTemplateId);
              if (itemTemplate != null)
                itemInfo1.GoldEquip = itemTemplate;
            }
          }
          client.Player.StoreBag.RemoveItemAt(0);
          client.Player.StoreBag.AddItemTo(itemInfo1, 0);
          ItemInfo itemInfo2 = ItemInfo.CloneFromTemplate(goods2, itemAt2);
          ItemInfo.OpenHole(ref itemInfo2);
          if (itemInfo2.isGold)
          {
            GoldEquipTemplateInfo goldEquipByTemplate = GoldEquipMgr.FindGoldEquipByTemplate(goods2.TemplateID);
            if (goldEquipByTemplate != null)
            {
              ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(goldEquipByTemplate.NewTemplateId);
              if (itemTemplate != null)
                itemInfo2.GoldEquip = itemTemplate;
            }
          }
          client.Player.StoreBag.RemoveItemAt(1);
          if(client.Player.EquipBag.CountTotalFullItemSlots()>=48)
          {
                        client.Player.BagFullSendToMail(itemInfo2);
          }
          else
          {
              client.Player.StoreBag.AddItemTo(itemInfo2, 1);
          }
        }
        else
        {
          client.Player.StoreBag.UpdateItem(itemAt1);
          client.Player.StoreBag.UpdateItem(itemAt2);
        }


        pkg.WriteByte((byte) 0);
        client.SendTCP(pkg);
      }
      else
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("itemtransferhandler.nocondition"));
      return 0;
    }

    public bool TransferCondition(ItemInfo itemAtZero, ItemInfo itemAtOne)
    {
      if (itemAtZero.Template.CategoryID != 7 && itemAtOne.Template.CategoryID != 7)
        return false;
      if (itemAtZero.StrengthenLevel < 10)
        return itemAtOne.StrengthenLevel >= 10;
      return true;
    }

    private int method_0(ItemInfo itemInfo_0)
    {
      StrengthenGoodsInfo transferInfo = StrengthenMgr.FindTransferInfo(itemInfo_0.TemplateID);
      if (transferInfo == null)
      {
        GoldEquipTemplateInfo equipOldTemplate = GoldEquipMgr.FindGoldEquipOldTemplate(itemInfo_0.TemplateID);
        if (equipOldTemplate == null)
          return 0;
        transferInfo = StrengthenMgr.FindTransferInfo(equipOldTemplate.OldTemplateId);
      }
      return transferInfo.OrginEquip;
    }

    private int method_1(ItemInfo itemInfo_0)
    {
      if (itemInfo_0.StrengthenLevel >= 10)
      {
        StrengthenGoodsInfo transferInfo = StrengthenMgr.FindTransferInfo(itemInfo_0.StrengthenLevel, itemInfo_0.TemplateID);
        if (transferInfo == null)
          return -1;
        return transferInfo.GainEquip;
      }
      StrengthenGoodsInfo transferInfo1 = StrengthenMgr.FindTransferInfo(itemInfo_0.TemplateID);
      if (transferInfo1 == null)
        return -1;
      return transferInfo1.OrginEquip;
    }


  }
}
