using System.Collections.Generic;
using System.Reflection;
using Game.Base.Packets;
using Game.Server.GameUtils;
using log4net;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte) ePackageType.CHANGE_PLACE_GOODS_ALL, "物品比较")]
    public class MoveGoodsAllHandler : IPacketHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            bool isStack = packet.ReadBoolean();
            int count = packet.ReadInt();
            int bagType = packet.ReadInt();
            PlayerInventory inventory = client.Player.GetInventory((eBageType) bagType);
            int capalility = inventory.Capalility;
            if (inventory.BagType == (int) eBageType.EquipBag)
            {
                capalility = 80;
            }

            List<ItemInfo> recoverItems = inventory.GetItems(inventory.BeginSlot, capalility);
            if (count == recoverItems.Count)
            {
                inventory.BeginChanges();
                try
                {
                    if (inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility) != -1)
                    {
                        for (int k = 1;
                            inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility) <
                            recoverItems[recoverItems.Count - k].Place;
                            k++)
                        {
                            inventory.MoveItem(recoverItems[recoverItems.Count - k].Place,
                                inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility),
                                recoverItems[recoverItems.Count - k].Count);
                        }
                    }
                }
                finally
                {
                    if (isStack)
                    {
                        try
                        {
                            recoverItems = inventory.GetItems(inventory.BeginSlot, capalility);
                            List<int> indexs = new List<int>();
                            for (int i = 0; i < recoverItems.Count; i++)
                            {
                                if (!indexs.Contains(i))
                                {
                                    for (int j = recoverItems.Count - 1; j > i; j--)
                                    {
                                        if (!indexs.Contains(j) &&
                                            (recoverItems[i].TemplateID == recoverItems[j].TemplateID &&
                                             recoverItems[i].CanStackedTo(recoverItems[j])))
                                        {
                                            inventory.MoveItem(recoverItems[j].Place, recoverItems[i].Place,
                                                recoverItems[j].Count);
                                            indexs.Add(j);
                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                            recoverItems = inventory.GetItems(inventory.BeginSlot, capalility);
                            if (inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility) != -1)
                            {
                                for (int k = 1;
                                    inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility) <
                                    recoverItems[recoverItems.Count - k].Place;
                                    k++)
                                {
                                    inventory.MoveItem(recoverItems[recoverItems.Count - k].Place,
                                        inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility),
                                        recoverItems[recoverItems.Count - k].Count);
                                }
                            }
                        }
                    }

                    inventory.CommitChanges();
                }
            }

            return 0;
        }
    }
}