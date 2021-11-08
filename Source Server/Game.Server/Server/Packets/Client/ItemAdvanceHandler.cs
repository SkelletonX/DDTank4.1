// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ItemAdvanceHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler(138, "物品强化")]
    public class ItemAdvanceHandler : IPacketHandler
    {
        public static RandomSafe random = new RandomSafe();

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool flag1 = false;
            GSPacketIn packet1 = new GSPacketIn((short)138, client.Player.PlayerCharacter.ID);
            ItemInfo itemAt = client.Player.StoreBag.GetItemAt(0);
            ItemInfo itemInfo1 = client.Player.StoreBag.GetItemAt(1);
            if (itemAt != null && itemInfo1 != null && itemAt.Count > 0)
            {
                if (itemInfo1.StrengthenLevel >= 15)
                {
                    client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("ItemAdvanceHandler.Msg2"));
                    return 0;
                }
                int count = 1;
                string str1 = "";
                if (itemInfo1 != null && itemInfo1.Template.CanStrengthen && (itemInfo1.Template.CategoryID < 18 && itemInfo1.Count == 1))
                {
                    bool flag2 = flag1 || itemInfo1.IsBinds;
                    stringBuilder.Append(itemInfo1.ItemID.ToString() + ":" + (object)itemInfo1.TemplateID + ",");
                    if (itemAt.TemplateID == 11150)
                    {
                        bool flag3 = flag2 || itemAt.IsBinds;
                        string[] strArray = new string[5]
                        {
              str1,
              ",",
              null,
              null,
              null
                        };
                        int index = 2;
                        int num1 = itemAt.ItemID;
                        string str2 = num1.ToString();
                        strArray[index] = str2;
                        strArray[3] = ":";
                        strArray[4] = itemAt.Template.Name;
                        string.Concat(strArray);
                        int val = itemAt.Template.Property2 < 10 ? 10 : itemAt.Template.Property2;
                        stringBuilder.Append("true");
                        bool flag4 = false;
                        int maxValue = StrengthenMgr.StrengthenExp[itemInfo1.StrengthenLevel + 1] * GameProperties.AdvanceRateValue;
                        int num2 = ItemAdvanceHandler.random.Next(maxValue);
                        if (itemInfo1.StrengthenExp <= num2 && itemInfo1.StrengthenExp < StrengthenMgr.StrengthenExp[itemInfo1.StrengthenLevel + 1])
                        {
                            itemInfo1.StrengthenExp += val;
                            packet1.WriteByte((byte)1);
                            packet1.WriteInt(val);
                        }
                        else
                        {
                            itemInfo1.IsBinds = flag3;
                            num1 = itemInfo1.StrengthenLevel++;
                            itemInfo1.StrengthenExp = 0;
                            packet1.WriteByte((byte)0);
                            packet1.WriteInt(val);
                            flag4 = true;
                            StrengthenGoodsInfo strengthenGoodsInfo = StrengthenMgr.FindStrengthenGoodsInfo(itemInfo1.StrengthenLevel, itemInfo1.TemplateID);
                            if (strengthenGoodsInfo != null && itemInfo1.Template.CategoryID == 7 && strengthenGoodsInfo.GainEquip > itemInfo1.TemplateID)
                            {
                                ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(strengthenGoodsInfo.GainEquip);
                                if (itemTemplate != null)
                                {
                                    ItemInfo itemInfo2 = ItemInfo.CloneFromTemplate(itemTemplate, itemInfo1);
                                    client.Player.StoreBag.RemoveItemAt(1);
                                    client.Player.StoreBag.AddItemTo(itemInfo2, 1);
                                    itemInfo1 = itemInfo2;
                                }
                            }
                        }
                        client.Player.StoreBag.RemoveCountFromStack(itemAt, count);
                        client.Player.StoreBag.UpdateItem(itemInfo1);
                        client.Out.SendTCP(packet1);
                        if (flag4)
                            WorldMgr.SendSysNotice(eMessageType.ChatNormal, LanguageMgr.GetTranslation("ItemStrengthenHandler.congratulation2", (object)client.Player.PlayerCharacter.NickName, (object)itemInfo1.TemplateID, (object)(itemInfo1.StrengthenLevel - 12)), itemInfo1.ItemID, itemInfo1.TemplateID, (string)null);
                        client.Player.AddLog("ItemAdvance", "stone:" + itemAt.Template.Name + "x" + (object)itemAt.Count + "|" + itemInfo1.Template.Name + "-level:" + (object)itemInfo1.StrengthenLevel + "-maxRand:" + (object)num2 + "-needexp:" + (object)maxValue + "----" + flag4.ToString());
                        stringBuilder.Append(itemInfo1.StrengthenLevel);
                    }
                    else
                    {
                        client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("ItemAdvanceHandler.Msg3"));
                        return 0;
                    }
                }
                else
                    client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemStrengthenHandler.Content1") + itemAt.Template.Name + LanguageMgr.GetTranslation("ItemStrengthenHandler.Content2"));
                if (itemInfo1.Place < 31)
                    client.Player.EquipBag.UpdatePlayerProperties();
                return 0;
            }
            client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("ItemAdvanceHandler.Msg1"));
            return 0;
        }
    }
}
