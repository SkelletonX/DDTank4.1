using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte) ePackageType.CHANGE_PLACE_GOODS, "改变物品位置")]
    public class UserChangeItemPlaceHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            eBageType eBageType = (eBageType)packet.ReadByte();
            int num = packet.ReadInt();
            eBageType eBageType2 = (eBageType)packet.ReadByte();
            int num2 = packet.ReadInt();
            int num3 = packet.ReadInt();


            packet.ReadBoolean();
            PlayerInventory inventory = client.Player.GetInventory(eBageType);
            PlayerInventory inventory2 = client.Player.GetInventory(eBageType2);
            ItemInfo itemAt = inventory.GetItemAt(num);
            ItemInfo itemAt2 = inventory2.GetItemAt(num2);
            if (inventory != null && itemAt != null)
            {
                if (num3 >= 0 && num3 <= itemAt.Count)
                {
                    if (eBageType == eBageType2 && num == num2)
                    {
                        client.Player.AddLog("Hack/Cheat", $"Hack move same place bagType: {eBageType} | toBagType: {eBageType2} | place: {num} | toPlace: {num2}");
                        return 0;
                    }
                    client.Player.LastMovePlaceItem = DateTime.Now;
                    inventory.BeginChanges();
                    inventory2.BeginChanges();
                    try
                    {
                        if (eBageType2 == eBageType.Consortia)
                        {
                            ConsortiaInfo consortiaInfo = ConsortiaMgr.FindConsortiaInfo(client.Player.PlayerCharacter.ConsortiaID);
                            if (consortiaInfo != null)
                            {
                                inventory2.Capalility = consortiaInfo.StoreLevel * 10;
                            }
                        }
                        if (num2 != -1 && eBageType != eBageType2 && eBageType2 == eBageType.EquipBag && num2 < 31)
                        {
                            return 0;
                        }
                        if (num2 != -1 && eBageType == eBageType2 && eBageType == eBageType.EquipBag && num2 >= 31 && num < 31 && itemAt != null && itemAt2 != null && !itemAt2.IsValidItem())
                        {
                            client.Player.SendMessage("背包已满，请清理背包后再试.");
                            return 0;
                        }
                        if (num2 != -1 && eBageType == eBageType2 && eBageType == eBageType.EquipBag && num2 < 31 && num >= 31 && itemAt != null && !itemAt.IsValidItem())
                        {
                            client.Player.SendMessage("背包已满，请清理背包后再试.");
                            return 0;
                        }
                        if (num2 == -1)
                        {
                            bool flag = false;
                            if (eBageType == eBageType2 && eBageType2 == eBageType.EquipBag)
                            {
                                num2 = inventory2.FindFirstEmptySlot();
                                if (!inventory.MoveItem(num, num2, num3))
                                {
                                    flag = true;
                                }
                            }
                            else if (eBageType == eBageType.Consortia)
                            {
                                num2 = inventory2.FindFirstEmptySlot();
                                if (num2 != -1)
                                {
                                    smethod_1(client, num, num2, inventory, inventory2, itemAt);
                                }
                            }
                            else if (eBageType2 == eBageType.Consortia)
                            {
                                num2 = inventory2.FindFirstEmptySlot();
                                if (num2 != -1)
                                {
                                    smethod_0(num, num2, inventory, inventory2, itemAt);
                                }
                            }
                            else if (eBageType == eBageType.Store)
                            {
                                num2 = inventory2.FindFirstEmptySlot();
                                MoveFromStore(client, inventory, itemAt, num2, inventory2, num3);
                            }
                            else if (eBageType == eBageType2)
                            {
                                if (!inventory2.StackItemToAnother(itemAt) && !inventory2.AddItem(itemAt))
                                {
                                    flag = true;
                                }
                                else
                                {
                                    inventory.TakeOutItem(itemAt);
                                }
                            }
                            else if (eBageType == eBageType.CaddyBag)
                            {
                                if (!inventory2.StackItemToAnother(itemAt) && !inventory2.AddItem(itemAt))
                                {
                                    flag = true;
                                }
                                else
                                {
                                    inventory.TakeOutItem(itemAt);
                                }
                            }
                            else if ((eBageType == eBageType.PropBag || eBageType == eBageType.EquipBag) && eBageType2 == eBageType.Store)
                            {
                                num2 = inventory2.FindFirstEmptySlot();
                                MoveToStore(client, inventory, itemAt, num2, inventory2, num3);
                            }
                            else
                            {
                                client.Out.SendMessage(eMessageType.ERROR, "Error ChangeItem. Contact to GM!");
                            }
                            if (flag)
                            {
                                client.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("UserChangeItemPlaceHandler.full"));
                            }
                        }
                        else if (eBageType == eBageType2)
                        {
                            inventory.MoveItem(num, num2, num3);
                        }
                        else
                        {
                            switch (eBageType)
                            {
                                case eBageType.Store:
                                    MoveFromStore(client, inventory, itemAt, num2, inventory2, num3);
                                    break;
                                case eBageType.Consortia:
                                    if (eBageType2 == eBageType.EquipBag && num2 < 31)
                                    {
                                        return 0;
                                    }
                                    if (itemAt2 != null)
                                    {
                                        num2 = inventory2.FindFirstEmptySlot();
                                        smethod_1(client, num, num2, inventory, inventory2, itemAt);
                                    }
                                    else
                                    {
                                        smethod_1(client, num, num2, inventory, inventory2, itemAt);
                                    }
                                    break;
                                default:
                                    switch (eBageType2)
                                    {
                                        case eBageType.Store:
                                            MoveToStore(client, inventory, itemAt, num2, inventory2, num3);
                                            break;
                                        case eBageType.Consortia:
                                            if (eBageType == eBageType.EquipBag && num < 31)
                                            {
                                                return 0;
                                            }
                                            if (itemAt2 != null)
                                            {
                                                num2 = inventory2.FindFirstEmptySlot();
                                                smethod_0(num, num2, inventory, inventory2, itemAt);
                                            }
                                            else
                                            {
                                                smethod_0(num, num2, inventory, inventory2, itemAt);
                                            }
                                            break;
                                        default:
                                            if (inventory2.AddItemTo(itemAt, num2))
                                            {
                                                inventory.TakeOutItem(itemAt);
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                    finally
                    {
                        inventory.CommitChanges();
                        inventory2.CommitChanges();
                    }
                    return 0;
                }
                Console.WriteLine("--count: " + num3 + " |itemCount: " + itemAt.Count + "|maxCount: " + itemAt.Template.MaxCount);
                return 0;
            }
            return 0;
        }

        public void MoveFromStore(GameClient client, PlayerInventory storeBag, ItemInfo item, int toSlot, PlayerInventory bag, int count)
        {
            if (client.Player == null || item == null || storeBag == null || bag == null || item.Template.BagType != (eBageType)bag.BagType || item.Template.BagType != (eBageType)bag.BagType)
            {
                return;
            }
            if (toSlot < bag.BeginSlot || toSlot > bag.Capalility)
            {
                if (bag.StackItemToAnother(item))
                {
                    storeBag.RemoveItem(item, eItemRemoveType.Stack);
                    return;
                }
                string key = $"temp_place_{item.ItemID}";
                if (client.Player.TempProperties.ContainsKey(key))
                {
                    toSlot = (int)storeBag.Player.TempProperties[key];
                    storeBag.Player.TempProperties.Remove(key);
                }
                else
                {
                    toSlot = bag.FindFirstEmptySlot();
                }
            }
            if (!bag.StackItemToAnother(item) && !bag.AddItemTo(item, toSlot))
            {
                toSlot = bag.FindFirstEmptySlot();
                if (bag.AddItemTo(item, toSlot))
                {
                    storeBag.TakeOutItem(item);
                    return;
                }
                storeBag.TakeOutItem(item);
                client.Player.SendItemToMail(item, LanguageMgr.GetTranslation("UserChangeItemPlaceHandler.full"), LanguageMgr.GetTranslation("UserChangeItemPlaceHandler.full"), eMailType.ItemOverdue);
                client.Player.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
            }
            else
            {
                storeBag.TakeOutItem(item);
            }
        }

        public void MoveToStore(GameClient client, PlayerInventory bag, ItemInfo item, int toSlot, PlayerInventory storeBag, int count)
        {
            if (client.Player == null || bag == null || item == null || storeBag == null)
            {
                return;
            }
            int place = item.Place;
            ItemInfo itemAt = storeBag.GetItemAt(toSlot);
            if (itemAt != null)
            {
                if (item.Count == 1 && item.BagType == itemAt.BagType)
                {
                    bag.TakeOutItem(item);
                    storeBag.TakeOutItem(itemAt);
                    bag.AddItemTo(itemAt, place);
                    storeBag.AddItemTo(item, toSlot);
                    return;
                }
                string key = $"temp_place_{itemAt.ItemID}";
                PlayerInventory itemInventory = client.Player.GetItemInventory(itemAt.Template);
                if (client.Player.TempProperties.ContainsKey(key) && itemInventory.BagType == 0)
                {
                    int place2 = (int)client.Player.TempProperties[key];
                    client.Player.TempProperties.Remove(key);
                    if (itemInventory.AddItemTo(itemAt, place2))
                    {
                        storeBag.TakeOutItem(itemAt);
                    }
                }
                else if (itemInventory.StackItemToAnother(itemAt))
                {
                    storeBag.RemoveItem(itemAt, eItemRemoveType.Stack);
                }
                else if (itemInventory.AddItem(itemAt))
                {
                    storeBag.TakeOutItem(itemAt);
                }
                else
                {
                    client.Player.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("UserChangeItemPlaceHandler.full"));
                }
            }
            if (!storeBag.IsEmpty(toSlot))
            {
                return;
            }
            if (item.Count == 1)
            {
                if (!storeBag.AddItemTo(item, toSlot))
                {
                    return;
                }
                bag.TakeOutItem(item);
                if (item.Template.BagType == eBageType.EquipBag && place < 31)
                {
                    string key = $"temp_place_{item.ItemID}";
                    if (client.Player.TempProperties.ContainsKey(key))
                    {
                        client.Player.TempProperties[key] = place;
                    }
                    else
                    {
                        client.Player.TempProperties.Add(key, place);
                    }
                }
            }
            else
            {
                ItemInfo itemInfo = item.Clone();
                itemInfo.Count = count;
                if (bag.RemoveCountFromStack(item, count, eItemRemoveType.Stack) && !storeBag.AddItemTo(itemInfo, toSlot))
                {
                    bag.AddCountToStack(item, count);
                }
            }
        }

        private static void smethod_0(int int_0, int int_1, AbstractInventory abstractInventory_0, AbstractInventory abstractInventory_1, ItemInfo itemInfo_0)
        {
            if (abstractInventory_0 == null || itemInfo_0 == null || abstractInventory_0 == null)
            {
                return;
            }
            ItemInfo itemAt = abstractInventory_1.GetItemAt(int_1);
            if (itemAt != null)
            {
                if (itemInfo_0.CanStackedTo(itemAt) && itemInfo_0.Count + itemAt.Count <= itemInfo_0.Template.MaxCount)
                {
                    if (abstractInventory_1.AddCountToStack(itemAt, itemInfo_0.Count))
                    {
                        abstractInventory_0.RemoveCountFromStack(itemInfo_0, itemInfo_0.Count);
                    }
                }
                else if (itemAt.Template.BagType == (eBageType)abstractInventory_0.BagType)
                {
                    abstractInventory_0.TakeOutItem(itemInfo_0);
                    abstractInventory_1.TakeOutItem(itemAt);
                    abstractInventory_0.AddItemTo(itemAt, int_0);
                    abstractInventory_1.AddItemTo(itemInfo_0, int_1);
                }
            }
            else if (abstractInventory_1.AddItemTo(itemInfo_0, int_1))
            {
                abstractInventory_0.TakeOutItem(itemInfo_0);
            }
        }

        private static void smethod_1(GameClient gameClient_0, int int_0, int int_1, AbstractInventory abstractInventory_0, object object_0, ItemInfo itemInfo_0)
        {
            if (itemInfo_0 == null)
            {
                return;
            }
            PlayerInventory itemInventory = gameClient_0.Player.GetItemInventory(itemInfo_0.Template);
            if (itemInventory == object_0)
            {
                ItemInfo itemAt = itemInventory.GetItemAt(int_1);
                if (itemAt == null)
                {
                    if (itemInventory.AddItemTo(itemInfo_0, int_1))
                    {
                        abstractInventory_0.TakeOutItem(itemInfo_0);
                    }
                }
                else if (itemInfo_0.CanStackedTo(itemAt) && itemInfo_0.Count + itemAt.Count <= itemInfo_0.Template.MaxCount)
                {
                    if (itemInventory.AddCountToStack(itemAt, itemInfo_0.Count))
                    {
                        abstractInventory_0.RemoveCountFromStack(itemInfo_0, itemInfo_0.Count);
                    }
                }
                else
                {
                    itemInventory.TakeOutItem(itemAt);
                    abstractInventory_0.TakeOutItem(itemInfo_0);
                    itemInventory.AddItemTo(itemInfo_0, int_1);
                    abstractInventory_0.AddItemTo(itemAt, int_0);
                }
            }
            else if (itemInventory.AddItem(itemInfo_0))
            {
                abstractInventory_0.TakeOutItem(itemInfo_0);
            }
        }

        public UserChangeItemPlaceHandler()
        {


        }
    }
}
