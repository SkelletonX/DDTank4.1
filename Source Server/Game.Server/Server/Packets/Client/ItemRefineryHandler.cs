using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Base.Packets;
using SqlDataProvider.Data;
using Bussiness;
using Game.Server.Statics;
using Game.Server.GameUtils;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.ITEM_REFINERY, "物品炼化")]

    public class ItemRefineryHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            GSPacketIn pkg = packet.Clone();
            pkg.ClearContext();
            bool IsBinds = false;
            int OpertionType = packet.ReadInt();
            int Count = packet.ReadInt();

            List<ItemInfo> Items = new List<ItemInfo>();
            List<ItemInfo> AppendItems = new List<ItemInfo>();
            List<eBageType> BagTypes = new List<eBageType>();

            StringBuilder str = new StringBuilder();

            int defaultprobability = 25;

            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.locked"));
                return 1;
            }



            for (int i = 0; i < Count; i++)
            {
                eBageType bagType = (eBageType)packet.ReadInt();
                int place = packet.ReadInt();

                ItemInfo info = client.Player.GetItemAt(bagType, place);
                if (info != null)
                {
                    if (!Items.Contains(info))
                    {
                        if (info.IsBinds == true)
                        {
                            IsBinds = true;
                        }
                        str.Append(info.ItemID + ":" + info.TemplateID + ",");
                        Items.Add(info);
                        BagTypes.Add(bagType);
                    }
                    else
                    {
                        client.Out.SendMessage(eMessageType.Normal, "Bad Input");
                        return 1;
                    }
                }

            }

            //          ItemInfo MainMaterial = client.Player.GetItemAt(packet.ReadInt(), packet.ReadInt());


            eBageType BagType = (eBageType)packet.ReadInt();
            int Place = packet.ReadInt();

            ItemInfo Item = client.Player.GetItemAt(BagType, Place);
            if (Item != null)
            {
                str.Append(Item.ItemID + ":" + Item.TemplateID + ",");
            }

            eBageType LuckBagType = (eBageType)packet.ReadInt();
            int LuckPlace = packet.ReadInt();

            ItemInfo LuckItem = client.Player.GetItemAt(LuckBagType, LuckPlace); ;

            bool Luck = LuckItem == null ? false : true;

            //if (IsBinds != Item.IsBinds && IsBinds == true)
            //{
            //    client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemRefineryHandler.IsBinds"));
            //  //  return 1;
            //}

            if (Count != 4 || Item == null)
            {

                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemRefineryHandler.ItemNotEnough"));
                return 1;
            }

            bool result = false;
            bool IsFormula = false;


            if (OpertionType == 0)  //预览模式
            {
                ItemTemplateInfo Template = Managers.RefineryMgr.Refinery(client.Player, Items, Item, Luck, OpertionType, ref result, ref defaultprobability, ref IsFormula);

                if (Template != null)
                {
                    client.Out.SendRefineryPreview(client.Player, Template.TemplateID, IsBinds, Item);
                }

                return 0;

            }
            else                     //玩家炼化
            {

                int MustGold = 10000;


                if (client.Player.PlayerCharacter.Gold > MustGold)
                {

                    client.Player.RemoveGold(MustGold);
                    ItemTemplateInfo RewardItem = Managers.RefineryMgr.Refinery(client.Player, Items, Item, Luck, OpertionType, ref result, ref defaultprobability, ref IsFormula);
                    if (RewardItem != null && IsFormula && result)
                    {
                        str.Append("Success");
                        result = true;

                        if (result)
                        {
                            ItemInfo item = ItemInfo.CreateFromTemplate(RewardItem, 1, (int)ItemAddType.Refinery);

                            if (item != null)
                            {
                                client.Player.OnItemMelt(Item.Template.CategoryID);
                              //  Managers.RefineryMgr.InheritProperty(Item, ref item);
                                item.IsBinds = IsBinds;

                                AbstractInventory bg = client.Player.GetItemInventory(RewardItem);

                                if (!bg.AddItem(item, bg.BeginSlot))
                                {
                                    str.Append("NoPlace");
                                    client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation(item.GetBagName()) + LanguageMgr.GetTranslation("ItemFusionHandler.NoPlace"));
                                }

                                pkg.WriteByte(0);

                                Item.Count--;
                                client.Player.UpdateItem(Item);
                            }
                        }

                        else
                        {
                            str.Append("false");

                        }                        
                        
                    }
                    else
                    {
                        pkg.WriteByte(1);
                    }



                    if (Luck)
                    {
                        LuckItem.Count--;
                        client.Player.UpdateItem(LuckItem);
                    }

                    for (int i = 0; i < Items.Count; i++)
                    {
                        client.Player.UpdateItem(Items[i]);
                        if (Items[i].Count <= 0)
                        {
                            client.Player.RemoveItem(Items[i]);                            
                        }
                    }
                    client.Player.RemoveItem(Items[Items.Count - 1]);
                    client.Player.Out.SendTCP(pkg);

                }
                else
                {
                    client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemRefineryHandler.NoGold"));
                }


                return 1;
            }
        }


    }
}