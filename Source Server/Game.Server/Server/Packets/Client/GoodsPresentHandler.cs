using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Bussiness;
using SqlDataProvider.Data;
using Game.Server.Managers;
using Game.Base.Packets;
using Game.Server.Statics;


namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.GOODS_PRESENT, "赠送物品")]
    public class GoodsPresentHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {

            int gold = 0;            //表示金币
            int money = 0;           //表示cupons
            int offer = 0;           //表示功勋
            int gifttoken = 0;       //表示礼劵
            StringBuilder payGoods = new StringBuilder();                    //表示支付物品ID
            eMessageType eMsg = eMessageType.Normal;
            string msg = "GoodsPresentHandler.Success";

            string content = packet.ReadString();       //赠送人留言
            string nickName = packet.ReadString();      //购买人昵称
            List<int> needitemsinfo = new List<int>();  //存放兑换物品的templateID,Count

            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 0;
            }

            using (PlayerBussiness db = new PlayerBussiness())
            {
                PlayerInfo receiver = db.GetUserSingleByNickName(nickName);
                if (receiver != null)
                {
                    List<ItemInfo> items = new List<ItemInfo>();
                    StringBuilder types = new StringBuilder();

                    int count = packet.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        int GoodsID = packet.ReadInt();         //序号
                        int type = packet.ReadInt();       //购买方式
                        string color = packet.ReadString();//颜色
                        string skin = packet.ReadString(); //皮肤

                        ShopItemInfo shopItem = Bussiness.Managers.ShopMgr.GetShopItemInfoById(GoodsID);                   //获取商品信息

                        if (shopItem == null)                                                                              //商品不存在
                        {
                            continue;
                        }


                        ItemTemplateInfo goods = Bussiness.Managers.ItemMgr.FindItemTemplate(shopItem.TemplateID);              //查找物品属性
                        ItemInfo item = ItemInfo.CreateFromTemplate(goods, 1, (int)ItemAddType.Gift);                           //构建物品临时属性       

                        if (item == null)
                            continue;

                        //未开始
                        //if (shopitem.AValue1 <= 0 || shopitem.Beat <= 0)
                        //    continue;

                        item.Color = color == null ? "" : color;
                        item.Skin = skin == null ? "" : skin;

                        if (item == null)
                            continue;

                        types.Append(type);
                        types.Append(",");
                        items.Add(item);

                        needitemsinfo = ItemInfo.SetItemType(shopItem, type, ref gold, ref money, ref offer, ref gifttoken);
                    }

                    if (items.Count == 0)
                        return 1;


                    //////////////////////////////////////////////////////////////////////////////////////
                    //玩家背包中是否有兑换物品所需要的物品
                    int icount = client.Player.EquipBag.GetItems().Count;       //获取个数
                    bool result = true;
                    for (int j = 0; j < needitemsinfo.Count; j += 2)
                    {
                        if (client.Player.GetItemCount(needitemsinfo[j]) < needitemsinfo[j + 1])
                        {
                            result = false;
                        }
                    }

                    if (!result)
                    {
                        eMsg = eMessageType.ERROR;
                        msg = "UserBuyItemHandler.NoBuyItem";
                        return 1;
                    }
                    /////////////////////////////////////////////////////////////

                    if (gold <= client.Player.PlayerCharacter.Gold && money <= client.Player.PlayerCharacter.Money && offer <= client.Player.PlayerCharacter.Offer && gifttoken <= client.Player.PlayerCharacter.GiftToken)
                    {

                        types.Remove(types.Length - 1, 1);
                        client.Player.RemoveMoney(money);
                        client.Player.RemoveGold(gold);
                        client.Player.RemoveOffer(offer);
                        client.Player.RemoveGiftToken(gifttoken);
                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //从玩家背包中删除兑换所需要的物品
                        for (int j = 0; j < needitemsinfo.Count; j += 2)
                        {
                            client.Player.RemoveTemplate(needitemsinfo[j], needitemsinfo[j + 1]);
                            payGoods.Append(needitemsinfo[j].ToString() + ":");
                        }
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                        string itemIDs = "";
                        int annexIndex = 0;
                        MailInfo message = new MailInfo();
                        StringBuilder annexRemark = new StringBuilder();
                        annexRemark.Append(LanguageMgr.GetTranslation("GoodsPresentHandler.AnnexRemark"));

                        foreach (ItemInfo item in items)
                        {
                            itemIDs += (itemIDs == "" ? item.TemplateID.ToString() : "," + item.TemplateID.ToString());
                            item.UserID = 0;
                            db.AddGoods(item);

                            annexIndex++;
                            annexRemark.Append(annexIndex);
                            annexRemark.Append("、");
                            annexRemark.Append(item.Template.Name);
                            annexRemark.Append("x");
                            annexRemark.Append(item.Count);
                            annexRemark.Append(";");

                            switch (annexIndex)
                            {
                                case 1:
                                    message.Annex1 = item.ItemID.ToString();
                                    message.Annex1Name = item.Template.Name;
                                    break;
                                case 2:
                                    message.Annex2 = item.ItemID.ToString();
                                    message.Annex2Name = item.Template.Name;
                                    break;
                                case 3:
                                    message.Annex3 = item.ItemID.ToString();
                                    message.Annex3Name = item.Template.Name;
                                    break;
                                case 4:
                                    message.Annex4 = item.ItemID.ToString();
                                    message.Annex4Name = item.Template.Name;
                                    break;
                                case 5:
                                    message.Annex5 = item.ItemID.ToString();
                                    message.Annex5Name = item.Template.Name;
                                    break;
                            }

                            if (annexIndex == 5)
                            {
                                annexIndex = 0;
                                message.AnnexRemark = annexRemark.ToString();
                                annexRemark.Remove(0, annexRemark.Length);
                                annexRemark.Append(LanguageMgr.GetTranslation("GoodsPresentHandler.AnnexRemark"));
                                message.Content = content;
                                message.Gold = 0;
                                message.Money = 0;
                                message.Receiver = receiver.NickName;
                                message.ReceiverID = receiver.ID;
                                message.Sender = client.Player.PlayerCharacter.NickName;
                                message.SenderID = client.Player.PlayerCharacter.ID;
                                message.Title = message.Sender + LanguageMgr.GetTranslation("GoodsPresentHandler.Content") + message.Annex1Name + "]";
                                message.Type = (int)eMailType.PresentItem;
                                db.SendMail(message);

                                message.Revert();
                            }
                        }

                        if (annexIndex > 0)
                        {
                            message.AnnexRemark = annexRemark.ToString();
                            message.Content = content;
                            message.Gold = 0;
                            message.Money = 0;
                            message.Receiver = receiver.NickName;
                            message.ReceiverID = receiver.ID;
                            message.Sender = client.Player.PlayerCharacter.NickName;
                            message.SenderID = client.Player.PlayerCharacter.ID;
                            message.Title = message.Sender + LanguageMgr.GetTranslation("GoodsPresentHandler.Content") + message.Annex1Name + "]";
                            message.Type = (int)eMailType.PresentItem;
                            db.SendMail(message);
                        }
                        LogMgr.LogMoneyAdd(LogMoneyType.Shop, LogMoneyType.Shop_Present, client.Player.PlayerCharacter.ID, money, client.Player.PlayerCharacter.Money, gold, gifttoken, offer, payGoods.ToString(), itemIDs, types.ToString());
                        client.Out.SendMailResponse(receiver.ID, eMailRespose.Receiver);
                        client.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Send);
                    }
                    else
                    {
                        eMsg = eMessageType.ERROR;
                        msg = "GoodsPresentHandler.NoMoney";
                    }
                }
                else
                {
                    eMsg = eMessageType.ERROR;
                    msg = "GoodsPresentHandler.NoUser";
                }
            }
            client.Out.SendMessage(eMsg, LanguageMgr.GetTranslation(msg));
            return 0;
        }
    }
}
