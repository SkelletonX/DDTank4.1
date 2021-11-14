// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.CardUseHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Buffer;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler(183, "OpenCard")]
    public class CardUseHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num1 = packet.ReadInt();
            int num2 = packet.ReadInt();
            SqlDataProvider.Data.ItemInfo itemInfo1 = (SqlDataProvider.Data.ItemInfo)null;
            ShopItemInfo shopItemInfo = new ShopItemInfo();
            List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
            int num3;
            if (DateTime.Compare(client.Player.LastOpenCard.AddSeconds(0.5), DateTime.Now) > 0)
            {
                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("GoSlow"));
                num3 = 0;
            }
            else if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
                num3 = 0;
            }
            else
            {
                if (num1 == -1 && num2 == -1)
                {
                    int num4 = packet.ReadInt();
                    int ID = packet.ReadInt();
                    packet.ReadInt();
                    int num5 = 0;
                    int num6 = 0;
                    for (int index = 0; index < num4; ++index)
                    {
                        ShopItemInfo shopbyId = ShopMgr.FindShopbyID(ID);
                        if (shopbyId != null)
                        {
                            itemInfo1 = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(shopbyId.TemplateID), 1, 102);
                            num6 = shopbyId.AValue1;
                            itemInfo1.ValidDate = shopbyId.AUnit;
                        }
                        if (itemInfo1 != null)
                        {
                            if (num5 <= client.Player.PlayerCharacter.Gold && client.Player.MoneyDirect(num6, false))
                            {
                                BufferList.CreateBuffer(itemInfo1.Template, itemInfo1.ValidDate)?.Start(client.Player);
                                if (itemInfo1.Template.Property5 == 3)
                                {
                                    if (itemInfo1.ValidDate != 30)
                                        itemInfo1.ValidDate = itemInfo1.Template.Property5 * 10;
                                    BufferList.CreateBufferMinutes(itemInfo1.Template, itemInfo1.ValidDate)?.Start(client.Player);
                                }
                                client.Player.RemoveGold(num5);
                                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("CardUseHandler.Success"));
                            }
                        }
                        else
                            client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("CardUseHandler.Fail"));
                    }
                }
                else
                {
                    SqlDataProvider.Data.ItemInfo itemAt = client.Player.GetInventory((eBageType)num1).GetItemAt(num2);
                    if (itemAt != null)
                        itemInfoList.Add(itemAt);
                    string translateId1 = "CardUseHandler.Success";
                    if (itemInfoList.Count > 0)
                    {
                        string translateId2 = string.Empty;
                        foreach (SqlDataProvider.Data.ItemInfo itemInfo2 in itemInfoList)
                        {
                            if (itemInfo2.Template.Property1 == 13 || itemInfo2.Template.Property1 == 11 || (itemInfo2.Template.Property1 == 12 || itemInfo2.Template.Property1 == 26))
                            {
                                AbstractBuffer buffer = BufferList.CreateBuffer(itemInfo2.Template, itemInfo2.ValidDate);
                                if (buffer != null)
                                {
                                    buffer.Start(client.Player);
                                    if (num2 != -1 && num1 != -1)
                                        client.Player.GetInventory((eBageType)num1).RemoveCountFromStack(itemInfo2, 1);
                                }
                                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId1));
                            }
                            else if (itemInfo2.Template.Property5 == 3)
                            {
                                if (itemInfo2.ValidDate != 30)
                                    itemInfo2.ValidDate = itemInfo2.Template.Property5 * 10;
                                AbstractBuffer bufferMinutes = BufferList.CreateBufferMinutes(itemInfo2.Template, itemInfo2.ValidDate);
                                if (bufferMinutes != null)
                                {
                                    bufferMinutes.Start(client.Player);
                                    if (num2 != -1 && num1 != -1)
                                        client.Player.GetInventory((eBageType)num1).RemoveCountFromStack(itemInfo2, 1);
                                }
                                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId1));
                            }
                            else
                            {
                                if (itemInfo2.IsValidItem() && itemInfo2.Template.Property1 == 21)
                                {
                                    if (itemInfo2.TemplateID == 201145)
                                    {
                                        int property2 = itemInfo2.Template.Property2;
                                        int count = itemAt.Count;
                                        translateId2 = "TimerDanUser.Success";
                                    }
                                    else
                                    {
                                        int gp = itemInfo2.Template.Property2 * itemAt.Count;
                                        if (client.Player.Level == LevelMgr.MaxLevel)
                                        {
                                            int num4 = gp / 100;
                                            if (num4 > 0)
                                            {
                                                client.Player.AddOffer(num4);
                                                client.Player.UpdateProperties();
                                                translateId2 = string.Format("Sucesso ao Ativar Agora Seus Meritos Serão Duplicados.!", (object)num4);
                                            }
                                        }
                                        else
                                        {
                                            client.Player.AddGP(gp);
                                            translateId2 = "GPDanUser.Success";
                                            new PlayerBussiness().AddDailyRecord(new DailyRecordInfo()
                                            {
                                                UserID = client.Player.PlayerCharacter.ID,
                                                Type = 11,
                                                Value = gp.ToString()
                                            });
                                        }
                                    }
                                    if (itemInfo2.Template.CanDelete)
                                        client.Player.RemoveAt((eBageType)num1, num2);
                                }
                                else if (itemInfo2.TemplateID == 11992)
                                {
                                    DateTime now = DateTime.Now;
                                    using (PlayerBussiness playerBussiness = new PlayerBussiness())
                                    {
                                        int typeVIP = (int)client.Player.SetTypeVIP(itemInfo2.ValidDate);
                                        playerBussiness.VIPRenewal2(client.Player.PlayerCharacter.NickName, itemInfo2.ValidDate, typeVIP, ref now);
                                        if (itemInfo2.ValidDate == 0)
                                            itemInfo2.ValidDate = 1;
                                        if (client.Player.PlayerCharacter.typeVIP == (byte)0)
                                        {
                                            client.Player.OpenVIP2(itemInfo2.ValidDate, now);
                                            translateId2 = string.Format("O VIP foi aberto por {0} dias", (object)itemInfo2.ValidDate);
                                        }
                                        else
                                        {
                                            client.Player.ContinuousVIP(itemInfo2.ValidDate, now);
                                            translateId2 = string.Format("O VIP foi renovado com sucesso por mais {0} dias!", (object)itemInfo2.ValidDate);
                                        }
                                        client.Out.SendOpenVIP(client.Player);
                                        client.Player.OnVIPUpgrade(client.Player.PlayerCharacter.VIPLevel, client.Player.PlayerCharacter.VIPExp);
                                        if (itemInfo2.Template.CanDelete)
                                            client.Player.GetInventory((eBageType)num1).RemoveCountFromStack(itemInfo2, 1);
                                    }
                                }
                                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId2, (object)(itemAt.Template.Property2 * itemAt.Count)));
                            }
                        }
                    }
                    else
                        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("CardUseHandler.Fail"));
                }
                client.Player.LastOpenCard = DateTime.Now;
                client.Player.SaveIntoDatabase();
                num3 = 0;
            }
            return num3;
        }
    }
}
