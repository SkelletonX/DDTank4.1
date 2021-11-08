
using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.GameUtils;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler(78, "熔化")]
    public class ItemFusionHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num1 = (int)packet.ReadByte();
            int MinValid = int.MaxValue;
            List<SqlDataProvider.Data.ItemInfo> Items = new List<SqlDataProvider.Data.ItemInfo>();
            List<SqlDataProvider.Data.ItemInfo> AppendItems = new List<SqlDataProvider.Data.ItemInfo>();
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 1;
            }
            if (MinValid == int.MaxValue)
            {
                MinValid = 0;
                Items.Clear();
            }
            PlayerInventory storeBag = client.Player.StoreBag;
            for (int slot = 1; slot <= 4; ++slot)
            {
                SqlDataProvider.Data.ItemInfo itemAt = storeBag.GetItemAt(slot);
                if (itemAt != null)
                    Items.Add(itemAt);
            }
            if (Items.Count != 4)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemFusionHandler.ItemNotEnough"));
                return 0;
            }
            if (num1 == 0)
            {
                bool isBind = false;
                Dictionary<int, double> previewItemList = FusionMgr.FusionPreview(Items, AppendItems, ref isBind);
                if (previewItemList != null)
                {
                    if ((uint)previewItemList.Count > 0U)
                        client.Out.SendFusionPreview(client.Player, previewItemList, isBind, MinValid);
                    else
                        client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.Fustion.Msg1"));
                }
                else
                    Console.WriteLine("previewItemList is NULL");
            }
            else
            {
                int num2 = 400;
                if (client.Player.PlayerCharacter.Gold < 400)
                {
                    client.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("ItemFusionHandler.NoMoney"));
                    return 0;
                }
                bool isBind = false;
                bool result = false;
                ItemTemplateInfo goods = FusionMgr.Fusion(Items, AppendItems, ref isBind, ref result);
                if (goods != null)
                {
                    SqlDataProvider.Data.ItemInfo itemAt = storeBag.GetItemAt(0);
                    if (itemAt != null)
                    {
                        if (!client.Player.StackItemToAnother(itemAt) && !client.Player.AddItem(itemAt))
                            client.Player.SendItemsToMail(new List<SqlDataProvider.Data.ItemInfo>()
              {
                itemAt
              }, "Os itens de exercício retornam e - mails com êxito devido à Mochila cheia", eMailType.StoreCanel);
                        storeBag.TakeOutItemAt(0);
                    }
                    client.Player.RemoveGold(num2);
                    for (int index = 0; index < Items.Count; ++index)
                    {
                        --Items[index].Count;
                        client.Player.UpdateItem(Items[index]);
                    }
                    for (int index = 0; index < AppendItems.Count; ++index)
                    {
                        --AppendItems[index].Count;
                        client.Player.UpdateItem(AppendItems[index]);
                    }
                    if (result)
                    {
                        if (goods.BagType == eBageType.EquipBag)
                            MinValid = 7;
                        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(goods, 1, 105);
                        if (fromTemplate == null)
                            return 0;
                        fromTemplate.IsBinds = isBind;
                        fromTemplate.ValidDate = MinValid;
                        client.Player.OnItemFusion(fromTemplate.Template.FusionType);
                        client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemFusionHandler.Succeed1") + fromTemplate.Template.Name);
                        int templateId;
                        if (fromTemplate.Template.CategoryID == 7 || fromTemplate.Template.CategoryID == 17 || fromTemplate.Template.CategoryID == 19 || fromTemplate.Template.CategoryID == 16)
                        {
                            client.Player.SaveNewItems();
                            GameServer.Instance.LoginServer.SendPacket(WorldMgr.SendSysNotice(eMessageType.ChatNormal, LanguageMgr.GetTranslation("ItemFusionHandler.Notice", (object)client.Player.PlayerCharacter.NickName, (object)fromTemplate.TemplateID), fromTemplate.ItemID, fromTemplate.TemplateID, (string)null));
                            GamePlayer player = client.Player;
                            templateId = fromTemplate.TemplateID;
                            string content = "TemplateID: " + templateId.ToString() + "|Name: " + fromTemplate.Template.Name;
                            player.AddLog("Fusion", content);
                        }
                        if (!client.Player.StoreBag.AddItemTo(fromTemplate, 0))
                        {
                            client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation(fromTemplate.GetBagName()) + LanguageMgr.GetTranslation("ItemFusionHandler.NoPlace"));
                            GamePlayer player1 = client.Player;
                            string name = fromTemplate.Template.Name;
                            templateId = fromTemplate.TemplateID;
                            string str = templateId.ToString();
                            string content = "ItemFusionError" + name + "|TemplateID:" + str;
                            player1.AddLog("Error", content);
                            GamePlayer player2 = client.Player;
                            List<SqlDataProvider.Data.ItemInfo> items = new List<SqlDataProvider.Data.ItemInfo>();
                            items.Add(fromTemplate);
                            string translation1 = LanguageMgr.GetTranslation("GameServer.Fustion.msg2", (object)fromTemplate.TemplateID);
                            string translation2 = LanguageMgr.GetTranslation("GameServer.Fustion.Msg3");
                            player2.SendItemsToMail(items, translation1, translation2, eMailType.BuyItem);
                        }
                        client.Out.SendFusionResult(client.Player, result);
                    }
                    else
                    {
                        client.Out.SendFusionResult(client.Player, result);
                        client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemFusionHandler.Failed"));
                    }
                    client.Player.SaveIntoDatabase();
                }
                else
                    client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemFusionHandler.NoCondition"));
            }
            return 0;
        }
    }
}
