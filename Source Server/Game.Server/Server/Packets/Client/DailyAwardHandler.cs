using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler(13, "场景用户离开")]
    public class DailyAwardHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int type = packet.ReadInt();
            int point = 0;
            int gold = 0;
            int giftToken = 0;
            int medal = 0;
            int exp = 0;
            int myHonor = 0;
            StringBuilder stringBuilder1 = new StringBuilder();
            List<SqlDataProvider.Data.ItemInfo> itemInfos = new List<SqlDataProvider.Data.ItemInfo>();
            string str1 = "";
            switch (type)
            {
                case 0:
                    if (AwardMgr.AddDailyAward(client.Player))
                    {
                        using (PlayerBussiness playerBussiness = new PlayerBussiness())
                        {
                            if (playerBussiness.UpdatePlayerLastAward(client.Player.PlayerCharacter.ID, type))
                            {
                                stringBuilder1.Append(LanguageMgr.GetTranslation("GameUserDailyAward.Success"));
                                break;
                            }
                            stringBuilder1.Append(LanguageMgr.GetTranslation("GameUserDailyAward.Fail"));
                            break;
                        }
                    }
                    else
                    {
                        stringBuilder1.Append(LanguageMgr.GetTranslation("GameUserDailyAward.Fail1"));
                        break;
                    }
                case 2:
                    using (PlayerBussiness playerBussiness = new PlayerBussiness())
                        playerBussiness.UpdatePlayerLastAward(client.Player.PlayerCharacter.ID, type);
                    if (!ItemBoxMgr.CreateItemBox(ItemMgr.FindItemTemplate(112059).TemplateID, itemInfos, ref gold, ref point, ref giftToken, ref medal, ref exp, ref myHonor))
                    {
                        client.Player.SendMessage(LanguageMgr.GetTranslation("Error.ChangeChannel"));
                        return 0;
                    }
                    break;
                case 3:
                    if (client.Player.PlayerCharacter.CanTakeVipReward)
                    {
                        int vipLevel = client.Player.PlayerCharacter.VIPLevel;
                        client.Player.LastVIPPackTime();
                        if (!ItemBoxMgr.CreateItemBox(ItemMgr.FindItemTemplate(ItemMgr.FindItemBoxTypeAndLv(2, vipLevel).TemplateID).TemplateID, itemInfos, ref gold, ref point, ref giftToken, ref medal, ref exp, ref myHonor))
                        {
                            client.Player.SendMessage(LanguageMgr.GetTranslation("Error.ChangeChannel"));
                            return 0;
                        }
                        using (PlayerBussiness playerBussiness = new PlayerBussiness())
                        {
                            playerBussiness.UpdateLastVIPPackTime(client.Player.PlayerCharacter.ID);
                            break;
                        }
                    }
                    else
                    {
                        stringBuilder1.Append("Bạn đã nhận thưởng 1 lần trong ngày hôm nay!");
                        break;
                    }
                case 5:
                    using (PlayerBussiness playerBussiness = new PlayerBussiness())
                    {
                        DailyLogListInfo info = playerBussiness.GetDailyLogListSingle(client.Player.PlayerCharacter.ID);
                        if (info == null)
                            info = new DailyLogListInfo()
                            {
                                UserID = client.Player.PlayerCharacter.ID,
                                DayLog = "",
                                UserAwardLog = 0,
                                LastDate = DateTime.Now
                            };
                        string dayLog = info.DayLog;
                        dayLog.Split(',');
                        string str2;
                        if (string.IsNullOrEmpty(dayLog))
                        {
                            str2 = "True";
                            info.UserAwardLog = 0;
                        }
                        else
                            str2 = dayLog + ",True";
                        info.DayLog = str2;
                        ++info.UserAwardLog;
                        playerBussiness.UpdateDailyLogList(info);
                        break;
                    }
            }
            if ((uint)point > 0U)
            {
                stringBuilder1.Append(point.ToString() + LanguageMgr.GetTranslation("OpenUpArkHandler.Money"));
                client.Player.AddMoney(point);
            }
            if ((uint)gold > 0U)
            {
                stringBuilder1.Append(gold.ToString() + LanguageMgr.GetTranslation("OpenUpArkHandler.Gold"));
                client.Player.AddGold(gold);
            }
            if ((uint)giftToken > 0U)
            {
                stringBuilder1.Append(giftToken.ToString() + LanguageMgr.GetTranslation("OpenUpArkHandler.GiftToken"));
                client.Player.AddGiftToken(giftToken);
            }
            if ((uint)medal > 0U)
            {
                stringBuilder1.Append(medal.ToString() + LanguageMgr.GetTranslation("OpenUpArkHandler.Medal"));
                client.Player.AddMedal(medal);
            }
            StringBuilder stringBuilder2 = new StringBuilder();
            foreach (SqlDataProvider.Data.ItemInfo cloneItem in itemInfos)
            {
                stringBuilder2.Append(cloneItem.Template.Name + "x" + cloneItem.Count.ToString() + ",");
                if (!client.Player.AddTemplate(cloneItem, cloneItem.Template.BagType, cloneItem.Count, eGameView.RouletteTypeGet))
                {
                    using (PlayerBussiness playerBussiness = new PlayerBussiness())
                    {
                        cloneItem.UserID = 0;
                        playerBussiness.AddGoods(cloneItem);
                        MailInfo mail = new MailInfo()
                        {
                            Annex1 = cloneItem.ItemID.ToString(),
                            Content = LanguageMgr.GetTranslation("OpenUpArkHandler.Content1") + cloneItem.Template.Name + LanguageMgr.GetTranslation("OpenUpArkHandler.Content2"),
                            Gold = 0,
                            Money = 0,
                            Receiver = client.Player.PlayerCharacter.NickName,
                            ReceiverID = client.Player.PlayerCharacter.ID,
                            Sender = "Sistema",
                            SenderID = 1,
                            Title = LanguageMgr.GetTranslation("OpenUpArkHandler.Title") + cloneItem.Template.Name + "]",
                            Type = 12
                        };
                        playerBussiness.SendMail(mail);
                        str1 = LanguageMgr.GetTranslation("OpenUpArkHandler.Mail");
                    }
                }
            }
            if (stringBuilder2.Length > 0)
            {
                stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
                string[] strArray1 = stringBuilder2.ToString().Split(',');
                for (int index1 = 0; index1 < strArray1.Length; ++index1)
                {
                    int num1 = 1;
                    for (int index2 = index1 + 1; index2 < strArray1.Length; ++index2)
                    {
                        if (strArray1[index1].Contains(strArray1[index2]) && strArray1[index2].Length == strArray1[index1].Length)
                        {
                            ++num1;
                            strArray1[index2] = index2.ToString();
                        }
                    }
                    if (num1 > 1)
                    {
                        strArray1[index1] = strArray1[index1].Remove(strArray1[index1].Length - 1, 1);
                        string[] strArray2;
                        IntPtr num2;
                        (strArray2 = strArray1)[(int)(num2 = (IntPtr)index1)] = strArray2[(int)num2] + num1.ToString();
                    }
                    if (strArray1[index1] != index1.ToString())
                    {
                        string[] strArray2;
                        IntPtr num2;
                        (strArray2 = strArray1)[(int)(num2 = (IntPtr)index1)] = strArray2[(int)num2] + ",";
                        stringBuilder1.Append(strArray1[index1]);
                    }
                }
            }
            if (stringBuilder1.Length - 1 > 0)
            {
                stringBuilder1.Remove(stringBuilder1.Length - 1, 1);
                stringBuilder1.Append(".");
            }
            client.Out.SendMessage(eMessageType.GM_NOTICE, str1 + stringBuilder1.ToString());
            if (!string.IsNullOrEmpty(str1))
                client.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
            return 2;
        }
    }
}
