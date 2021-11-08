// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ItemStrengthenHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler((byte)ePackageType.ITEM_STRENGTHEN, "物品强化")]
  public class ItemStrengthenHandler : IPacketHandler
  {
    public static int countConnect = 0;
    private static RandomSafe random = new RandomSafe();

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GSPacketIn packet1 = packet.Clone();
      packet1.ClearContext();
      bool flag1 = packet.ReadBoolean();
      List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
      SqlDataProvider.Data.ItemInfo mainItem = client.Player.StoreBag.GetItemAt(5);
      SqlDataProvider.Data.ItemInfo itemInfo1 = (SqlDataProvider.Data.ItemInfo) null;
      SqlDataProvider.Data.ItemInfo itemInfo2 = (SqlDataProvider.Data.ItemInfo) null;
      bool flag2 = false;
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = 0.0;
      if (mainItem != null && mainItem.Template.CanStrengthen && mainItem.Count == 1)
      {
        bool flag3 = mainItem.IsBinds;
        SqlDataProvider.Data.ItemInfo itemAt1 = client.Player.StoreBag.GetItemAt(0);
        if (itemAt1 != null && itemAt1.Template.CategoryID == 11 && (itemAt1.Template.Property1 == 2 || itemAt1.Template.Property1 == 35) && !itemInfoList.Contains(itemAt1))
        {
          itemInfoList.Add(itemAt1);
          num1 += StrengthenMgr.RateItems[itemAt1.Template.Level - 1];
        }
        SqlDataProvider.Data.ItemInfo itemAt2 = client.Player.StoreBag.GetItemAt(1);
        if (itemAt2 != null && itemAt2.Template.CategoryID == 11 && (itemAt2.Template.Property1 == 2 || itemAt2.Template.Property1 == 35) && !itemInfoList.Contains(itemAt2))
        {
          itemInfoList.Add(itemAt2);
          num1 += StrengthenMgr.RateItems[itemAt2.Template.Level - 1];
        }
        SqlDataProvider.Data.ItemInfo itemAt3 = client.Player.StoreBag.GetItemAt(2);
        if (itemAt3 != null && itemAt3.Template.CategoryID == 11 && (itemAt3.Template.Property1 == 2 || itemAt3.Template.Property1 == 35) && !itemInfoList.Contains(itemAt3))
        {
          itemInfoList.Add(itemAt3);
          num1 += StrengthenMgr.RateItems[itemAt3.Template.Level - 1];
        }
        if (client.Player.StoreBag.GetItemAt(4) != null)
        {
          itemInfo1 = client.Player.StoreBag.GetItemAt(4);
          if (itemInfo1 != null && itemInfo1.Template.CategoryID == 11 && itemInfo1.Template.Property1 == 3)
            num2 += num1 + (double) (itemInfo1.Template.Property2 / 100);
          else
            itemInfo1 = (SqlDataProvider.Data.ItemInfo) null;
        }
        if (client.Player.StoreBag.GetItemAt(3) != null)
        {
          itemInfo2 = client.Player.StoreBag.GetItemAt(3);
          if (itemInfo2 != null && itemInfo2.Template.CategoryID == 11 && itemInfo2.Template.Property1 == 7)
            flag2 = true;
          else
            itemInfo2 = (SqlDataProvider.Data.ItemInfo) null;
        }
        double num5 = num1 * 100.0 / (double) StrengthenMgr.GetNeedRate(mainItem);
        double num6 = num2 * 100.0 / (double) StrengthenMgr.GetNeedRate(mainItem);
        if (itemAt1 != null && itemAt1.IsBinds || itemAt2 != null && itemAt2.IsBinds || (itemAt3 != null && itemAt3.IsBinds || itemInfo1 != null && itemInfo1.IsBinds) || itemInfo2 != null && itemInfo2.IsBinds)
          flag3 = true;
        if (flag1)
        {
          ConsortiaInfo consortiaInfo = ConsortiaMgr.FindConsortiaInfo(client.Player.PlayerCharacter.ConsortiaID);
          ConsortiaEquipControlInfo consortiaEuqipRiches = new ConsortiaBussiness().GetConsortiaEuqipRiches(client.Player.PlayerCharacter.ConsortiaID, 0, 2);
          if (consortiaInfo == null)
            client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemStrengthenHandler.Fail"));
          else if (client.Player.PlayerCharacter.Riches < consortiaEuqipRiches.Riches)
            client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("ItemStrengthenHandler.FailbyPermission"));
          else
            num4 = num5 * (0.1 * (double) consortiaInfo.SmithLevel);
        }
        if (client.Player.PlayerCharacter.typeVIP > (byte) 0)
          num3 += StrengthenMgr.VIPStrengthenEx * num5;
        if (itemInfoList.Count >= 1)
        {
          ++mainItem.StrengthenTimes;
          mainItem.IsBinds = flag3;
          client.Player.StoreBag.ClearBag();
          if (Math.Floor((num5 + num6 + num4 + num3) * 100.0) > (double) ItemStrengthenHandler.random.Next(10000))
          {
            packet1.WriteByte((byte) 0);
            packet1.WriteBoolean(true);
            ++mainItem.StrengthenLevel;
            StrengthenGoodsInfo strengthenGoodsInfo = StrengthenMgr.FindStrengthenGoodsInfo(mainItem.StrengthenLevel, mainItem.TemplateID);
            if (strengthenGoodsInfo != null && mainItem.Template.CategoryID == 7 && strengthenGoodsInfo.GainEquip > mainItem.TemplateID)
            {
              ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(strengthenGoodsInfo.GainEquip);
              if (itemTemplate != null)
              {
                SqlDataProvider.Data.ItemInfo itemInfo3 = SqlDataProvider.Data.ItemInfo.CloneFromTemplate(itemTemplate, mainItem);
                client.Player.StoreBag.RemoveItemAt(5);
                mainItem = itemInfo3;
              }
            }
            SqlDataProvider.Data.ItemInfo.OpenHole(ref mainItem);
            client.Player.StoreBag.AddItemTo(mainItem, 5);
            client.Player.OnItemStrengthen(mainItem.Template.CategoryID, mainItem.StrengthenLevel);
            client.Player.SaveIntoDatabase();
            if (mainItem.StrengthenLevel >= 9)
              GameServer.Instance.LoginServer.SendPacket(WorldMgr.SendSysNotice(eMessageType.ChatNormal, LanguageMgr.GetTranslation("ItemStrengthenHandler.congratulation", (object) client.Player.PlayerCharacter.NickName, (object) mainItem.TemplateID, (object) mainItem.StrengthenLevel), mainItem.ItemID, mainItem.TemplateID, (string) null));
            if (mainItem.Template.CategoryID == 7 && client.Player.Extra.CheckNoviceActiveOpen(NoviceActiveType.STRENGTHEN_WEAPON_ACTIVE))
              client.Player.Extra.UpdateEventCondition(2, mainItem.StrengthenLevel);
          }
          else
          {
            packet1.WriteByte((byte) 1);
            packet1.WriteBoolean(false);
            if (!flag2)
            {
              if (mainItem.Template.Level == 3)
              {
                mainItem.StrengthenLevel = mainItem.StrengthenLevel < 5 ? mainItem.StrengthenLevel : mainItem.StrengthenLevel - 1;
                StrengthenGoodsInfo strengthenGoodInfo = StrengthenMgr.FindRealStrengthenGoodInfo(mainItem.StrengthenLevel, mainItem.TemplateID);
                if (strengthenGoodInfo != null && mainItem.Template.CategoryID == 7 && mainItem.TemplateID != strengthenGoodInfo.GainEquip)
                {
                  ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(strengthenGoodInfo.GainEquip);
                  if (itemTemplate != null)
                  {
                    SqlDataProvider.Data.ItemInfo itemInfo3 = SqlDataProvider.Data.ItemInfo.CloneFromTemplate(itemTemplate, mainItem);
                    client.Player.StoreBag.RemoveItemAt(5);
                    mainItem = itemInfo3;
                  }
                }
                client.Player.StoreBag.AddItemTo(mainItem, 5);
              }
              else
              {
                --mainItem.Count;
                client.Player.StoreBag.AddItemTo(mainItem, 5);
              }
            }
            else
              client.Player.StoreBag.AddItemTo(mainItem, 5);
            SqlDataProvider.Data.ItemInfo.OpenHole(ref mainItem);
            client.Player.SaveIntoDatabase();
          }
          client.Out.SendTCP(packet1);
          if (mainItem.Place < 31)
            client.Player.EquipBag.UpdatePlayerProperties();
        }
        else
          client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemStrengthenHandler.Content1") + (object) 1 + LanguageMgr.GetTranslation("ItemStrengthenHandler.Content2"));
      }
      else
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemStrengthenHandler.Success"));
      return 0;
    }
  }
}
