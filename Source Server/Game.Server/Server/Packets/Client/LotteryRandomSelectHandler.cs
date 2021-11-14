// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.LotteryRandomSelectHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler((int)ePackageType.LOTTERY_RANDOM_SELECT, "打开物品")]
  public class LotteryRandomSelectHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int templateId = 11444;
      string str = "Baú da Malásia";
      if (client.Player.LotteryID == 190000)
      {
        templateId = 190001;
        str = "Prato Giratório";
      }
      if (client.Player.Lottery >= 0 && client.Player.LotteryID > 0)
      {
        if (client.Player.Lottery >= 8)
          client.Player.SendMessage(LanguageMgr.GetTranslation("LotteryRandomSelectHandler.Error", (object) str));
        else if (client.Player.PropBag.GetItemCount(templateId) < client.Player.Lottery + 1)
        {
          client.Player.SendMessage(LanguageMgr.GetTranslation("LotteryRandomSelectHandler.NotKeysEnough"));
        }
        else
        {
          ++client.Player.Lottery;
          List<ItemInfo> itemInfos = new List<ItemInfo>();
          SpecialItemDataInfo specialInfo = new SpecialItemDataInfo();
          ItemBoxMgr.CreateItemBox(client.Player.LotteryID, client.Player.LotteryItems, itemInfos, specialInfo);
          if (itemInfos.Count > 0)
          {
            client.Player.LotteryAwardList.Add(itemInfos[0]);
            GSPacketIn pkg = new GSPacketIn((short) 30);
            pkg.WriteBoolean(true);
            pkg.WriteInt(itemInfos[0].TemplateID);
            pkg.WriteInt(itemInfos[0].Template.Quality);
            pkg.WriteInt(itemInfos[0].StrengthenLevel);
            pkg.WriteInt(itemInfos[0].AttackCompose);
            pkg.WriteInt(itemInfos[0].DefendCompose);
            pkg.WriteInt(itemInfos[0].LuckCompose);
            pkg.WriteInt(itemInfos[0].AgilityCompose);
            pkg.WriteBoolean(itemInfos[0].IsBinds);
            pkg.WriteInt(itemInfos[0].ValidDate);
            pkg.WriteByte((byte) itemInfos[0].Count);
            client.Player.SendTCP(pkg);
            client.Player.RemoveLotteryItems(itemInfos[0].TemplateID, itemInfos[0].Count);
            client.Player.PropBag.RemoveTemplate(templateId, client.Player.Lottery);
            if (itemInfos[0].Template.Quality >= 4 && (itemInfos[0].Template.CategoryID == 7 || itemInfos[0].Template.CategoryID == 8 || (itemInfos[0].Template.CategoryID == 9 || itemInfos[0].Template.CategoryID == 13) || (itemInfos[0].Template.CategoryID == 15 || itemInfos[0].Template.CategoryID == 17 || itemInfos[0].Template.CategoryID == 14)))
              GameServer.Instance.LoginServer.SendPacket(WorldMgr.SendSysNotice(eMessageType.ChatNormal, LanguageMgr.GetTranslation("Parabéns, você recebeu o item", (object) client.Player.PlayerCharacter.NickName, (object) str, (object) itemInfos[0].Template.Name), itemInfos[0].ItemID, itemInfos[0].TemplateID, (string) null));
          }
          else
            client.Player.SendMessage(LanguageMgr.GetTranslation("LotteryRandomSelectHandler.Error", (object) str));
        }
      }
      else
        client.Player.SendMessage(LanguageMgr.GetTranslation("LotteryRandomSelectHandler.Error", (object) str));
      return 0;
    }
  }
}
