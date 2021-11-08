// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ItemComposeHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler(58, "物品合成")]
    public class ItemComposeHandler : IPacketHandler
    {
        public static ThreadSafeRandom random = new ThreadSafeRandom();
        private static readonly double[] composeRate = new double[5]
        {
          0.8,
          0.5,
          0.3,
          0.1,
          0.05
        };

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            GSPacketIn packet1 = new GSPacketIn(58, client.Player.PlayerCharacter.ID);
            StringBuilder stringBuilder = new StringBuilder();
            int priceComposeGold = GameProperties.PRICE_COMPOSE_GOLD;
            int num1;
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
                num1 = 0;
            }
            else if (client.Player.PlayerCharacter.Gold < priceComposeGold)
            {
                client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("ItemComposeHandler.NoMoney"));
                num1 = 0;
            }
            else
            {
                int slot = -1;
                int num2 = -1;
                bool flag1 = false;
                bool flag2 = packet.ReadBoolean();
                SqlDataProvider.Data.ItemInfo itemAt1 = client.Player.StoreBag.GetItemAt(1);
                SqlDataProvider.Data.ItemInfo itemAt2 = client.Player.StoreBag.GetItemAt(2);
                SqlDataProvider.Data.ItemInfo itemInfo1 = (SqlDataProvider.Data.ItemInfo)null;
                SqlDataProvider.Data.ItemInfo itemInfo2 = (SqlDataProvider.Data.ItemInfo)null;
                if (itemAt2 == null || itemAt1 == null || itemAt2.Count <= 0)
                {
                    client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("Coloque níveis de pedra e equipamentos para incorporar!"));
                    num1 = 0;
                }
                else
                {
                    string Property = null;
                    string str = null;
                    using (ItemRecordBussiness itemRecordBussiness = new ItemRecordBussiness())
                        itemRecordBussiness.PropertyString(itemAt1, ref Property);
                    if (itemAt1 != null && itemAt2 != null && itemAt1.Template.CanCompose && (itemAt1.Template.CategoryID < 10 || itemAt2.Template.CategoryID == 11 && itemAt2.Template.Property1 == 1))
                    {
                        bool flag3 = flag1 || itemAt1.IsBinds || itemAt2.IsBinds;
                        stringBuilder.Append(itemAt1.ItemID.ToString() + ":" + (object)itemAt1.TemplateID + "," + (object)itemAt2.ItemID + ":" + (object)itemAt2.TemplateID + ",");
                        bool flag4 = false;
                        byte val = 1;
                        double num3 = ItemComposeHandler.composeRate[itemAt2.Template.Quality - 1] * 100.0;
                        if (client.Player.StoreBag.GetItemAt(0) != null)
                        {
                            itemInfo1 = client.Player.StoreBag.GetItemAt(0);
                            if (itemInfo1 != null && itemInfo1.Template.CategoryID == 11 && itemInfo1.Template.Property1 == 3)
                            {
                                flag3 = flag3 || itemInfo1.IsBinds;
                                str = str + "|" + (object)itemInfo1.ItemID + ":" + itemInfo1.Template.Name + "|" + (object)itemAt2.ItemID + ":" + itemAt2.Template.Name;
                                stringBuilder.Append(itemInfo1.ItemID.ToString() + ":" + (object)itemInfo1.TemplateID + ",");
                                num3 += num3 * (double)itemInfo1.Template.Property2 / 100.0;
                            }
                        }
                        else
                            num3 += num3 * 1.0 / 100.0;
                        if (num2 != -1)
                        {
                            itemInfo2 = client.Player.PropBag.GetItemAt(slot);
                            if (itemInfo2 != null && itemInfo2.Template.CategoryID == 11 && itemInfo2.Template.Property1 == 7)
                            {
                                bool flag5 = flag1 || itemInfo2.IsBinds;
                                stringBuilder.Append(itemInfo2.ItemID.ToString() + ":" + (object)itemInfo2.TemplateID + ",");
                                object obj2 = str;
                                str = string.Concat(new object[] { obj2, ",", itemInfo2.ItemID, ":", itemInfo2.Template.Name });
                            }
                            else
                                itemInfo2 = (SqlDataProvider.Data.ItemInfo)null;
                        }
                        if (flag2)
                        {
                            ConsortiaInfo consortiaInfo = ConsortiaMgr.FindConsortiaInfo(client.Player.PlayerCharacter.ConsortiaID);
                            ConsortiaEquipControlInfo consortiaEuqipRiches = new ConsortiaBussiness().GetConsortiaEuqipRiches(client.Player.PlayerCharacter.ConsortiaID, 0, 2);
                            if (consortiaInfo == null)
                            {
                                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemStrengthenHandler.Fail"));
                            }
                            else
                            {
                                if (client.Player.PlayerCharacter.Riches < consortiaEuqipRiches.Riches)
                                {
                                    client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("ItemStrengthenHandler.FailbyPermission"));
                                    return 1;
                                }
                                num3 *= 1.0 + 0.1 * (double)consortiaInfo.SmithLevel;
                            }
                        }
                        double num4 = Math.Floor(num3 * 10.0) / 10.0;
                        int num5 = ItemComposeHandler.random.Next(100);
                        switch (itemAt2.Template.Property3)
                        {
                            case 1:
                                if (itemAt2.Template.Property4 > itemAt1.AttackCompose)
                                {
                                    flag4 = true;
                                    if (num4 > (double)num5)
                                    {
                                        val = (byte)0;
                                        itemAt1.AttackCompose = itemAt2.Template.Property4;
                                        break;
                                    }
                                    break;
                                }
                                else
                                {
                                    client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemComposeHandler.NoCondition"));
                                }
                                break;
                            case 2:
                                if (itemAt2.Template.Property4 > itemAt1.DefendCompose)
                                {
                                    flag4 = true;
                                    if (num4 > (double)num5)
                                    {
                                        val = (byte)0;
                                        itemAt1.DefendCompose = itemAt2.Template.Property4;
                                        break;
                                    }
                                    break;
                                }
                                else
                                {
                                    client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemComposeHandler.NoCondition"));
                                }
                                break;
                            case 3:
                                if (itemAt2.Template.Property4 > itemAt1.AgilityCompose)
                                {
                                    flag4 = true;
                                    if (num4 > (double)num5)
                                    {
                                        val = (byte)0;
                                        itemAt1.AgilityCompose = itemAt2.Template.Property4;
                                        break;
                                    }
                                    break;
                                }
                                else
                                {
                                    client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemComposeHandler.NoCondition"));
                                }
                                break;
                            case 4:
                                if (itemAt2.Template.Property4 > itemAt1.LuckCompose)
                                {
                                    flag4 = true;
                                    if (num4 > (double)num5)
                                    {
                                        val = (byte)0;
                                        itemAt1.LuckCompose = itemAt2.Template.Property4;
                                        break;
                                    }
                                    break;
                                }
                                else
                                {
                                    client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemComposeHandler.NoCondition"));
                                }
                                break;
                        }
                        if (flag4)
                        {
                            itemAt1.IsBinds = flag3;
                            if (val > (byte)0)
                            {
                                stringBuilder.Append("false!");
                            }
                            else
                            {
                                stringBuilder.Append("true!");
                                client.Player.OnItemCompose(itemAt2.TemplateID);
                            }
                            client.Player.StoreBag.RemoveTemplate(itemAt2.TemplateID, 1);
                            if (itemInfo1 != null)
                                client.Player.StoreBag.RemoveTemplate(itemInfo1.TemplateID, 1);
                            if (itemInfo2 != null)
                                client.Player.RemoveItem(itemInfo2);
                            client.Player.RemoveGold(priceComposeGold);
                            client.Player.StoreBag.UpdateItem(itemAt1);
                            packet1.WriteByte(val);
                            client.Out.SendTCP(packet1);
                            if (slot < 31)
                                client.Player.EquipBag.UpdatePlayerProperties();
                        }

                    }
                    else
                        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemComposeHandler.Fail"));
                    num1 = 0;
                }
            }
            return num1;
        }

        
    }
}
