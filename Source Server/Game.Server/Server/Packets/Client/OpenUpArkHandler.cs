using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.GameUtils;
using Game.Server.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler(63, "打开物品")]
    public class OpenUpArkHandler : IPacketHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num1 = (int)packet.ReadByte();
            int slot = packet.ReadInt();
            PlayerInventory inventory = client.Player.GetInventory((eBageType)num1);
            SqlDataProvider.Data.ItemInfo itemAt = inventory.GetItemAt(slot);
            string str1 = "";
            List<SqlDataProvider.Data.ItemInfo> infos = new List<SqlDataProvider.Data.ItemInfo>();
            if (itemAt != null && itemAt.IsValidItem() && (itemAt.Template.CategoryID == 11 && itemAt.Template.Property1 == 6) && client.Player.PlayerCharacter.Grade >= itemAt.Template.NeedLevel)
            {
                SpecialItemDataInfo specialInfo = new SpecialItemDataInfo();
                int[] bag = new int[3];
                string data = itemAt.Template.Data;
                if (data == null || data == "")
                    data = itemAt.TemplateID.ToString();
                if (itemAt.TemplateID != 112019 && itemAt.TemplateID != 112047 && (itemAt.TemplateID != 112100 && itemAt.TemplateID != 112101) && itemAt.TemplateID != 190000)
                {
                    this.OpenUpItem(data, bag, infos, specialInfo);
                    --bag[itemAt.GetBagTypee()];
                    if (inventory.RemoveCountFromStack(itemAt, 1))
                    {
                        client.Player.AddLog("OpenUpArk", "GoodsID:" + (object)itemAt.TemplateID + "|place:" + (object)slot + "|bagType:" + (object)num1 + "|Name:" + itemAt.Template.Name + "|Count:" + (object)itemAt.Count);
                        StringBuilder stringBuilder1 = new StringBuilder();
                        int num2 = 0;
                        StringBuilder stringBuilder2 = new StringBuilder();
                        stringBuilder2.Append(LanguageMgr.GetTranslation("OpenUpArkHandler.Start"));
                        int num3;
                        if ((uint)specialInfo.Money > 0U)
                        {
                            StringBuilder stringBuilder3 = stringBuilder2;
                            num3 = specialInfo.Money;
                            string str2 = num3.ToString() + LanguageMgr.GetTranslation("OpenUpArkHandler.Money");
                            stringBuilder3.Append(str2);
                            client.Player.AddMoney(specialInfo.Money);
                        }
                        if ((uint)specialInfo.Gold > 0U)
                        {
                            StringBuilder stringBuilder3 = stringBuilder2;
                            num3 = specialInfo.Gold;
                            string str2 = num3.ToString() + LanguageMgr.GetTranslation("OpenUpArkHandler.Gold");
                            stringBuilder3.Append(str2);
                            client.Player.AddGold(specialInfo.Gold);
                        }
                        if ((uint)specialInfo.GiftToken > 0U)
                        {
                            StringBuilder stringBuilder3 = stringBuilder2;
                            num3 = specialInfo.GiftToken;
                            string str2 = num3.ToString() + LanguageMgr.GetTranslation("OpenUpArkHandler.GiftToken");
                            stringBuilder3.Append(str2);
                            client.Player.AddGiftToken(specialInfo.GiftToken);
                        }
                        if ((uint)specialInfo.GP > 0U)
                        {
                            StringBuilder stringBuilder3 = stringBuilder2;
                            num3 = specialInfo.GP;
                            string str2 = num3.ToString() + LanguageMgr.GetTranslation("OpenUpArkHandler.Exp");
                            stringBuilder3.Append(str2);
                            client.Player.AddGP(specialInfo.GP);
                        }
                        if ((uint)specialInfo.myHonor > 0U)
                        {
                            StringBuilder stringBuilder3 = stringBuilder2;
                            num3 = specialInfo.myHonor;
                            string str2 = num3.ToString() + LanguageMgr.GetTranslation("OpenUpArkHandler.Honor");
                            stringBuilder3.Append(str2);
                            client.Player.AddHonor(specialInfo.myHonor);
                        }
                        StringBuilder stringBuilder4 = new StringBuilder();
                        foreach (SqlDataProvider.Data.ItemInfo cloneItem in infos)
                        {
                            cloneItem.IsBinds = true;
                            StringBuilder stringBuilder3 = stringBuilder4;
                            string name = cloneItem.Template.Name;
                            string str2 = "x";
                            num3 = cloneItem.Count;
                            string str3 = num3.ToString();
                            string str4 = ",";
                            string str5 = str2;
                            string str6 = str3;
                            string str7 = str4;
                            string str8 = name + str5 + str6 + str7;
                            stringBuilder3.Append(str8);
                            if (cloneItem.Template.Quality >= itemAt.Template.Property2 & (uint)itemAt.Template.Property2 > 0U)
                            {
                                stringBuilder1.Append(cloneItem.Template.Name + ",");
                                ++num2;
                            }
                            if (!client.Player.AddTemplate(cloneItem, cloneItem.Template.BagType, cloneItem.Count, false))
                            {
                                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                                {
                                    cloneItem.UserID = 0;
                                    playerBussiness.AddGoods(cloneItem);
                                    MailInfo mailInfo1 = new MailInfo();
                                    MailInfo mailInfo2 = mailInfo1;
                                    num3 = cloneItem.ItemID;
                                    string str9 = num3.ToString();
                                    mailInfo2.Annex1 = str9;
                                    mailInfo1.Content = LanguageMgr.GetTranslation("OpenUpArkHandler.Content1") + cloneItem.Template.Name + LanguageMgr.GetTranslation("OpenUpArkHandler.Content2");
                                    mailInfo1.Gold = 0;
                                    mailInfo1.Money = 0;
                                    mailInfo1.Receiver = client.Player.PlayerCharacter.NickName;
                                    mailInfo1.ReceiverID = client.Player.PlayerCharacter.ID;
                                    MailInfo mail = mailInfo1;
                                    mail.Sender = mail.Receiver;
                                    mail.SenderID = mail.ReceiverID;
                                    mail.Title = LanguageMgr.GetTranslation("OpenUpArkHandler.Title") + cloneItem.Template.Name + "]";
                                    mail.Type = 12;
                                    playerBussiness.SendMail(mail);
                                    str1 = LanguageMgr.GetTranslation("OpenUpArkHandler.Mail");
                                }
                            }
                        }
                        if (stringBuilder4.Length > 0)
                        {
                            stringBuilder4.Remove(stringBuilder4.Length - 1, 1);
                            string[] strArray = stringBuilder4.ToString().Split(',');
                            for (int index1 = 0; index1 < strArray.Length; ++index1)
                            {
                                int num4 = 1;
                                for (int index2 = index1 + 1; index2 < strArray.Length; ++index2)
                                {
                                    if (strArray[index1].Contains(strArray[index2]) && strArray[index2].Length == strArray[index1].Length)
                                    {
                                        ++num4;
                                        strArray[index2] = index2.ToString();
                                    }
                                }
                                if (num4 > 1)
                                {
                                    strArray[index1] = strArray[index1].Remove(strArray[index1].Length - 1, 1);
                                    strArray[index1] = strArray[index1] + num4.ToString();
                                }
                                if (strArray[index1] != index1.ToString())
                                {
                                    strArray[index1] = strArray[index1] + ",";
                                    stringBuilder2.Append(strArray[index1]);
                                }
                            }
                        }
                        if ((uint)itemAt.Template.Property2 > 0U & num2 > 0)
                        {
                            string translation = LanguageMgr.GetTranslation("OpenUpArkHandler.Notice", (object)client.Player.PlayerCharacter.NickName, (object)itemAt.Template.Name, (object)stringBuilder1, (object)stringBuilder1.Remove(stringBuilder1.Length - 1, 1));
                            GSPacketIn packet1 = new GSPacketIn((short)10);
                            packet1.WriteInt(2);
                            packet1.WriteString(translation);
                            GameServer.Instance.LoginServer.SendPacket(packet1);
                            foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                            {
                                if (allPlayer != client.Player)
                                    allPlayer.Out.SendTCP(packet1);
                            }
                        }
                        stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
                        stringBuilder2.Append(".");
                        client.Out.SendMessage(eMessageType.GM_NOTICE, str1 + stringBuilder2.ToString());
                        if (!string.IsNullOrEmpty(str1))
                            client.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
                    }
                }
                else
                {
                    client.Player.AddLog("Hack/Cheat", "Hack OpenUp Special Item: " + (object)itemAt.TemplateID + "|place:" + (object)slot);
                    return 0;
                }
            }
            return 1;
        }

        public void OpenUpItem(
          string data,
          int[] bag,
          List<SqlDataProvider.Data.ItemInfo> infos,
          SpecialItemDataInfo specialInfo)
        {
            if (string.IsNullOrEmpty(data))
                return;
            ItemBoxMgr.CreateItemBox(Convert.ToInt32(data), infos, specialInfo);
        }
    }
}
