namespace Game.Server.Packets.Client
{
    using Bussiness;
    using Bussiness.Managers;
    using Game.Base.Packets;
    using Game.Server;
    using Game.Server.Managers;
    using Game.Server.Packets;
    using SqlDataProvider.Data;
    using System;
    using System.Collections.Generic;
    using System.Text;

    [PacketHandler((Byte)ePackageType.BUY_GOODS, "Compra")]
    public class UserBuyItemHandler : IPacketHandler
    {
        private bool AddItemsToStoreBag(GameClient client, SqlDataProvider.Data.ItemInfo item,int ID)
        {
            int slot = 2;

            bool Prevent = false;

            if((item.TemplateID == 11018) && ID == 2)
            {
                slot = 0;
            }
            else if ((item.TemplateID == 11018))
            {
                slot = 4;
                Prevent = true;
            }
            if((item.TemplateID == 11020))
            {
                slot = 3;
                Prevent = true;
            }
          
            SqlDataProvider.Data.ItemInfo itemAt = client.Player.StoreBag.GetItemAt(slot);
            if((itemAt != null && Prevent) || (Prevent && item.Count > 1))
            {
                return client.Player.AddTemplate(item, (eBageType)item.GetBagType, item.Count, eGameView.RouletteTypeGet);
            }


            if (((itemAt != null) && (itemAt.Count < itemAt.Template.MaxCount)) && itemAt.CanStackedTo(item))
            {
                return client.Player.StoreBag.AddTemplateAt(item, item.Count, slot);
            }
            if (itemAt == null)
            {
                return client.Player.StoreBag.AddItemTo(item, slot);
            }
            return client.Player.AddTemplate(item, (eBageType)item.GetBagType, item.Count, eGameView.RouletteTypeGet);
        }


        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int gold = 0;
            int money = 0;
            int offer = 0;
            int gifttoken = 0;
            int medal = 0;
            int damageScore = 0;
            int petScore = 0;
            int iTemplateID = 0;
            int iCount = 0;
            int hardCurrency = 0;
            int leagueMoney = 0;
            int useableScore = 0;




            StringBuilder builder = new StringBuilder();
            eMessageType normal = eMessageType.Normal;
            string translateId = "UserBuyItemHandler.Success";
            GSPacketIn pkg = new GSPacketIn(44, client.Player.PlayerCharacter.ID);
            List<bool> list = new List<bool>();
            List<int> list2 = new List<int>();
            StringBuilder builder2 = new StringBuilder();
            Dictionary<int, SqlDataProvider.Data.ItemInfo> dictionary = new Dictionary<int, SqlDataProvider.Data.ItemInfo>();
            bool isBinds = false;
            ConsortiaInfo info = ConsortiaMgr.FindConsortiaInfo(client.Player.PlayerCharacter.ConsortiaID);
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked", new object[0]));
                return 1;
            }
            int num13 = packet.ReadInt();//indexador
            for (int i = 0; i < num13; i++)
            {

                int iD = packet.ReadInt();//id
                int type = packet.ReadInt();//type
                string str2 = packet.ReadString();//cor
                bool item = packet.ReadBoolean();//item
                string str3 = packet.ReadString();//?
                int num17 = packet.ReadInt();//TemplateID


                ShopItemInfo shopItemInfoById = ShopMgr.GetShopItemInfoById(iD);
                if (((shopItemInfoById != null) && ShopMgr.IsOnShop(shopItemInfoById.ID)) && ShopMgr.CanBuy(shopItemInfoById.ShopID, (info == null) ? 1 : info.ShopLevel, ref isBinds, client.Player.PlayerCharacter.ConsortiaID, client.Player.PlayerCharacter.Riches))
                {
                    SqlDataProvider.Data.ItemInfo info4 = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(shopItemInfoById.TemplateID), 1, 0x66);
                    if (shopItemInfoById.BuyType == 0)
                    {
                        if (1 == type)
                        {
                            info4.ValidDate = shopItemInfoById.AUnit;
                        }
                        if (2 == type)
                        {
                            info4.ValidDate = shopItemInfoById.BUnit;
                        }
                        if (3 == type)
                        {
                            info4.ValidDate = shopItemInfoById.CUnit;
                        }
                    }
                    else
                    {
                        if (1 == type)
                        {
                            info4.Count = shopItemInfoById.AUnit;
                        }
                        if (2 == type)
                        {
                            info4.Count = shopItemInfoById.BUnit;
                        }
                        if (3 == type)
                        {
                            info4.Count = shopItemInfoById.CUnit;
                        }
                    }
                    if (((type <= 3) && (type >= 0)) && ((info4 != null) || (shopItemInfoById != null)))
                    {

                        info4.Color = (str2 == null) ? "" : str2;
                        info4.Skin = (str3 == null) ? "" : str3;


                        

                        ShopMgr.SetItemType(shopItemInfoById, type, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref leagueMoney, ref useableScore);
                        if (ShopCondition.isGiftToken(shopItemInfoById.ShopID) || ShopCondition.isDDTMoney(shopItemInfoById.ShopID) || ShopCondition.isOffer(shopItemInfoById.ShopID) || ShopCondition.isMoney(shopItemInfoById.ShopID) || ShopCondition.isMedal(shopItemInfoById.ShopID))
                        {
                            info4.IsBinds = Convert.ToBoolean(shopItemInfoById.IsBind);
                            builder2.Append(type);
                            builder2.Append(",");
                            list.Add(item);
                            list2.Add(num17);
                            if (!dictionary.ContainsKey(info4.TemplateID))
                            {
                                dictionary.Add(info4.TemplateID, info4);
                            }
                            else
                            {
                                SqlDataProvider.Data.ItemInfo local1 = dictionary[info4.TemplateID];
                                local1.Count += info4.Count;
                            }
                        }
                        else if(ShopCondition.isFree(shopItemInfoById.ShopID))
                        {
                            translateId = "Função Em Desenvolvimento";
                        }

                    }
                }
                else if(shopItemInfoById != null && ShopCondition.isOffer(shopItemInfoById.ShopID))
                {
                    translateId = "UserBuyItemHandler.ConsortiaLevelError";
                }
                else
                {
                    translateId = "UserBuyItemHandler.Error";
                }


            }
           

            int num18 = packet.ReadInt();
            bool flag3 = false;
            int num19 = (((((((gold + money) + offer) + gifttoken) + medal) + damageScore) + petScore) + hardCurrency) + leagueMoney;
            foreach (SqlDataProvider.Data.ItemInfo info5 in dictionary.Values)
 
            if (dictionary.Count == 0)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("UserBuyItemHandler.NoItem", new object[0]));
                return 1;
            }
            
            if ((iTemplateID > 0) && (num19 == 0))
            {
                int itemCount = client.Player.GetItemCount(iTemplateID);
                if ((itemCount <= 0) || (itemCount < iCount))
                {
                    client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("UserBuyItemHandler.FailByPermission", new object[0]));
                }
                else
                {
                    flag3 = client.Player.RemoveTemplate(iTemplateID, iCount);
                }
            }
            else if (((((((((gold + money) + offer) + gifttoken) + medal) + hardCurrency) + damageScore) + petScore) + leagueMoney) > 0)
            {

                if (money > client.Player.PlayerCharacter.Money)
                {
                    translateId = "UserBuyItemHandler.NoMoney";
                }
                else if (gold > client.Player.PlayerCharacter.Gold)
                {
                    translateId = "UserBuyItemHandler.NoGold";
                }
                else if (offer > client.Player.PlayerCharacter.Offer)
                {
                    translateId = "UserBuyItemHandler.NoOffer";
                }
                else if (gifttoken > client.Player.PlayerCharacter.GiftToken) //gifttoken moeda gratis
                {
                    translateId = "UserBuyItemHandler.GiftToken";
                }

                else if (medal > client.Player.GetItemCount(11408))
                {
                    translateId = "UserBuyItemHandler.Medal";
                }

                else if (petScore > client.Player.PlayerCharacter.petScore)
                {

                    translateId = "UserBuyItemHandler.FailByPermission";
                }
                else if (hardCurrency > client.Player.PlayerCharacter.hardCurrency)
                {

                    translateId = "UserBuyItemHandler.FailByPermission";
                }
                else if (useableScore == 0)
                {

                    normal = eMessageType.ERROR;
                    client.Player.RemoveMoney(money);
                    client.Player.RemoveGold(gold);
                    client.Player.RemoveOffer(offer);
                    client.Player.RemoveGiftToken(gifttoken);
                    client.Player.RemoveMedal(medal);
                    client.Player.RemovePetScore(petScore);

                    flag3 = true;
                }
            }
            if (flag3)
            {
                string str4 = "";
                foreach (SqlDataProvider.Data.ItemInfo info7 in dictionary.Values)
                {
                    str4 = str4 + ((str4 == "") ? info7.TemplateID.ToString() : ("," + info7.TemplateID.ToString()));
                    Console.WriteLine(num18);
                    switch (num18)
                    {
                        case 1:
                        case 2:
                            if (!this.AddItemsToStoreBag(client, info7, num18))
                            {
                                client.Player.AddTemplate(info7);
                            }
                            break;

                        default:
                            new List<SqlDataProvider.Data.ItemInfo>();
                            if (info7.Template.MaxCount == 1)
                            {
                                for (int j = 0; j < info7.Count; j++)
                                {
                                    SqlDataProvider.Data.ItemInfo cloneItem = SqlDataProvider.Data.ItemInfo.CloneFromTemplate(info7.Template, info7);
                                    cloneItem.Count = 1;
                                    client.Player.AddTemplate(cloneItem);
                                }
                            }
                            else
                            {
                                int num23 = 0;
                                for (int k = 0; k < info7.Count; k++)
                                {
                                    if (num23 == info7.Template.MaxCount)
                                    {
                                        SqlDataProvider.Data.ItemInfo info9 = SqlDataProvider.Data.ItemInfo.CloneFromTemplate(info7.Template, info7);
                                        info9.Count = num23;
                                        client.Player.AddTemplate(info9);
                                        num23 = 0;
                                    }
                                    num23++;
                                }
                                if (num23 > 0)
                                {
                                    SqlDataProvider.Data.ItemInfo info10 = SqlDataProvider.Data.ItemInfo.CloneFromTemplate(info7.Template, info7);
                                    info10.Count = num23;
                                    client.Player.AddTemplate(info10);
                                    num23 = 0;
                                }
                            }
                            break;
                    }
                }
                client.Player.OnPaid(money, gold, offer, gifttoken, medal, builder.ToString());
            }
            client.Out.SendMessage(normal, LanguageMgr.GetTranslation(translateId, new object[0]));
            pkg.WriteInt(1);
            pkg.WriteInt(3);
            client.Player.SendTCP(pkg);
            return 0;
        }

        private bool MustActiveMoney(int templateID)
        {
            int num = templateID;
            return (num == 0x311e8);
        }
    }
}

