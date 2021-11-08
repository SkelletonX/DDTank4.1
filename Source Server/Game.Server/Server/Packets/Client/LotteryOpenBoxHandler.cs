// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.LotteryOpenBoxHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.GameUtils;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.LOTTERY_OPEN_BOX, "LotteryBox")]
    public class LotteryOpenBoxHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            ProduceBussiness produceBussiness = new ProduceBussiness();
            if (client.Lottery != -1)
            {
                client.Out.SendMessage(eMessageType.GM_NOTICE, "Roleta ativada!");
                return 1;
            }
            int num1 = (int)packet.ReadByte();//Bag
            int slot = packet.ReadInt();//slot
            int num2 = packet.ReadInt();//ID
            List<SqlDataProvider.Data.ItemInfo> list = new List<SqlDataProvider.Data.ItemInfo>();
            PlayerInventory inventory = client.Player.GetInventory((eBageType)num1);
            if (num2 == -1)
            {
                SqlDataProvider.Data.ItemInfo itemAt = inventory.GetItemAt(slot);
                if (itemAt != null && (itemAt.TemplateID == 112019 || itemAt.TemplateID == 190000))
                {
                    this.SendOpenBox(client.Player, itemAt.TemplateID);
                    inventory.RemoveCountFromStack(itemAt, 1);
                }
                return 0;
            }
            string nickName = client.Player.PlayerCharacter.NickName;
            if (inventory.FindFirstEmptySlot() == -1)
            {
                client.Out.SendMessage(eMessageType.GM_NOTICE, "Não identificado!");
                client.Out.SendTCP(this.CaddyGetAward(list, nickName, client.Player.ZoneId));
                return 1;
            }

            int templateId = 11456;
            SqlDataProvider.Data.ItemInfo itemByTemplateId1 = client.Player.GetItemByTemplateID(11456);
            SqlDataProvider.Data.ItemInfo itemByTemplateId2 = client.Player.GetItemByTemplateID(num2);
            if (itemByTemplateId2 != null && itemByTemplateId2.Count >= 1)
            {
                List<SqlDataProvider.Data.ItemInfo> itemInfos = new List<SqlDataProvider.Data.ItemInfo>();

                StringBuilder stringBuilder = new StringBuilder();
                int gold = 0;
                int giftToken = 0;
                int point = 0;
                int exp = 0;
                if (!ItemBoxMgr.CreateItemBox(itemByTemplateId2.TemplateID, itemInfos, ref gold, ref point, ref giftToken, ref exp))
                {
                    client.Out.SendTCP(this.CaddyGetAward(list, nickName, client.Player.ZoneId));
                    client.Player.SendMessage("Não há itens para ganhar.");
                    return 0;
                }
                if ((uint)gold > 0U)
                {
                    stringBuilder.Append(gold.ToString() + LanguageMgr.GetTranslation("OpenUpArkHandler.Gold"));
                    client.Player.AddGold(gold);
                }
                if ((uint)giftToken > 0U)
                {
                    stringBuilder.Append(giftToken.ToString() + LanguageMgr.GetTranslation("OpenUpArkHandler.GiftToken"));
                    client.Player.AddGiftToken(giftToken);
                }
                if (itemInfos.Count > 0)
                {
                    SqlDataProvider.Data.ItemInfo itemInfo = itemInfos[0];
                    if (num2 == 112047 || num2 == 112100 || num2 == 112101)
                    {
                        int num3 = 4;
                        if (itemByTemplateId1.Count >= 2 && client.Player.UsePayBuff(BuffType.Caddy_Good))
                            num3 = 2;
                        if (itemByTemplateId1.Count < num3)
                        {
                            client.Out.SendMessage(eMessageType.GM_NOTICE, string.Format("Quantidade insuficiente para abrir o {0}", (object)itemByTemplateId1.Template.Name));
                            client.Out.SendTCP(this.CaddyGetAward(list, nickName, client.Player.ZoneId));
                            return 0;
                        }
                        client.Player.RemoveTemplate(templateId, 4);
                        client.Player.AddBadLuckCaddy(1);
                    }
                    inventory.AddTemplate(itemInfo);
                    client.Player.RemoveTemplate(num2, 1);
                }

                List<SqlDataProvider.Data.ItemInfo> items = client.Player.CaddyBag.GetItems();
                client.Out.SendTCP(this.CaddyGetAward(items, nickName, client.Player.ZoneId));
                client.Lottery = -1;
                if (stringBuilder != null)
                    client.Out.SendTCP(this.CaddyGetAward(items, nickName, client.Player.ZoneId));
                return 1;
            }
            List<SqlDataProvider.Data.ItemInfo> items1 = client.Player.CaddyBag.GetItems();
            client.Out.SendTCP(this.CaddyGetAward(items1, nickName, client.Player.ZoneId));
            return 1;
        }

        private GSPacketIn CaddyGetAward(List<SqlDataProvider.Data.ItemInfo> list, string name, int zoneID)
        {
            GSPacketIn gsPacketIn = new GSPacketIn((short)245);
            gsPacketIn.WriteBoolean(true);
            gsPacketIn.WriteInt(list.Count);
            foreach (SqlDataProvider.Data.ItemInfo itemInfo in list)
            {
                gsPacketIn.WriteString(name);
                gsPacketIn.WriteInt(itemInfo.TemplateID);
                gsPacketIn.WriteInt(zoneID);
                gsPacketIn.WriteBoolean(false);
            }
            return gsPacketIn;
        }

        private void SendOpenBox(GamePlayer player, int templateid)
        {
            List<ItemBoxInfo> lotteryItemBoxByRand = ItemBoxMgr.FindLotteryItemBoxByRand(templateid, 18);
            player.Lottery = 0;
            player.LotteryItems = lotteryItemBoxByRand;
            player.LotteryID = templateid;
            GSPacketIn pkg = new GSPacketIn((short)29, player.PlayerCharacter.ID);
            pkg.WriteInt(templateid);
            for (int index = 0; index < 18; ++index)
            {
                pkg.WriteInt(lotteryItemBoxByRand[index].TemplateId);
                pkg.WriteBoolean(lotteryItemBoxByRand[index].IsBind);
                pkg.WriteByte((byte)lotteryItemBoxByRand[index].ItemCount);
                pkg.WriteByte((byte)lotteryItemBoxByRand[index].ItemValid);
            }
            player.SendTCP(pkg);
        }

        public void OpenUpItem(
          string data,
          List<SqlDataProvider.Data.ItemInfo> infos,
          ref int gold,
          ref int money,
          ref int giftToken,
          ref int exp)
        {
            if (string.IsNullOrEmpty(data))
                return;
            ItemBoxMgr.CreateItemBox(Convert.ToInt32(data), infos, ref gold, ref money, ref giftToken, ref exp);
        }
    }
}
