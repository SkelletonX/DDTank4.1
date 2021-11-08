// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserGetBoxHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler(53, "获取箱子")]
    public class UserGetBoxHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            GamePlayer player = client.Player;
            int num1;
            if (packet.ReadInt() == 0)
            {
                int alreadyBox = packet.ReadInt();
                int totalMinutes = (int)DateTime.Now.Subtract(player.BoxBeginTime).TotalMinutes;
                LoadUserBoxInfo templateByCondition = UserBoxMgr.FindTemplateByCondition(0, player.PlayerCharacter.Grade, player.PlayerCharacter.BoxProgression);
                if (templateByCondition != null && totalMinutes >= alreadyBox && templateByCondition.Condition == alreadyBox)
                {
                    using (PlayerBussiness playerBussiness = new PlayerBussiness())
                    {
                        playerBussiness.UpdateBoxProgression(player.PlayerCharacter.ID, player.PlayerCharacter.BoxProgression, player.PlayerCharacter.GetBoxLevel, player.PlayerCharacter.AddGPLastDate, DateTime.Now, alreadyBox);
                        player.PlayerCharacter.AlreadyGetBox = alreadyBox;
                        player.PlayerCharacter.BoxGetDate = DateTime.Now;
                    }
                }
                num1 = 0;
            }
            else
            {
                int num2 = packet.ReadInt();
                GSPacketIn pkg = packet.Clone();
                pkg.ClearContext();
                bool flag = false;
                bool val = true;
                LoadUserBoxInfo templateByCondition1;
                if (num2 == 0)
                {
                    DateTime now = DateTime.Now;
                    int totalMinutes = (int)now.Subtract(player.BoxBeginTime).TotalMinutes;
                    templateByCondition1 = UserBoxMgr.FindTemplateByCondition(0, player.PlayerCharacter.Grade, player.PlayerCharacter.BoxProgression);
                    if (templateByCondition1 != null && (totalMinutes >= templateByCondition1.Condition || player.PlayerCharacter.AlreadyGetBox == templateByCondition1.Condition))
                    {
                        using (PlayerBussiness playerBussiness1 = new PlayerBussiness())
                        {
                            PlayerBussiness playerBussiness2 = playerBussiness1;
                            int id = player.PlayerCharacter.ID;
                            int condition = templateByCondition1.Condition;
                            int getBoxLevel = player.PlayerCharacter.GetBoxLevel;
                            DateTime addGpLastDate = player.PlayerCharacter.AddGPLastDate;
                            now = DateTime.Now;
                            DateTime date1 = now.Date;
                            if (playerBussiness2.UpdateBoxProgression(id, condition, getBoxLevel, addGpLastDate, date1, 0))
                            {
                                player.PlayerCharacter.BoxProgression = templateByCondition1.Condition;
                                PlayerInfo playerCharacter = player.PlayerCharacter;
                                now = DateTime.Now;
                                DateTime date2 = now.Date;
                                playerCharacter.BoxGetDate = date2;
                                player.PlayerCharacter.AlreadyGetBox = 0;
                                flag = true;
                            }
                        }
                    }
                }
                else
                {
                    templateByCondition1 = UserBoxMgr.FindTemplateByCondition(1, player.PlayerCharacter.GetBoxLevel, Convert.ToInt32(player.PlayerCharacter.Sex));
                    if (templateByCondition1 != null && player.PlayerCharacter.Grade >= templateByCondition1.Level)
                    {
                        using (PlayerBussiness playerBussiness = new PlayerBussiness())
                        {
                            if (playerBussiness.UpdateBoxProgression(player.PlayerCharacter.ID, player.PlayerCharacter.BoxProgression, templateByCondition1.Level, player.PlayerCharacter.AddGPLastDate, player.PlayerCharacter.BoxGetDate, 0))
                            {
                                player.PlayerCharacter.GetBoxLevel = templateByCondition1.Level;
                                flag = true;
                            }
                        }
                    }
                }
                if (flag)
                {
                    if (templateByCondition1 != null)
                    {
                        List<SqlDataProvider.Data.ItemInfo> items = new List<SqlDataProvider.Data.ItemInfo>();
                        List<SqlDataProvider.Data.ItemInfo> itemInfos = new List<SqlDataProvider.Data.ItemInfo>();
                        int gold = 0;
                        int point = 0;
                        int giftToken = 0;
                        int exp = 0;
                        ItemBoxMgr.CreateItemBox(Convert.ToInt32(templateByCondition1.TemplateID), itemInfos, ref gold, ref point, ref giftToken, ref exp);
                        if (gold > 0)
                            player.AddGold(gold);
                        if (point > 0)
                            player.AddMoney(point);
                        if (giftToken > 0)
                            player.AddGiftToken(giftToken);
                        if (exp > 0)
                            player.AddGP(exp);
                        foreach (SqlDataProvider.Data.ItemInfo itemInfo in itemInfos)
                        {
                            itemInfo.RemoveType = 120;
                            if (!player.AddItem(itemInfo))
                                items.Add(itemInfo);
                        }
                        if (num2 == 0)
                        {
                            player.BoxBeginTime = DateTime.Now;
                            LoadUserBoxInfo templateByCondition2 = UserBoxMgr.FindTemplateByCondition(0, player.PlayerCharacter.Grade, player.PlayerCharacter.BoxProgression);
                            if (templateByCondition2 != null)
                                player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("UserGetTimeBoxHandler.success", (object)templateByCondition2.Condition));
                            else
                                player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("UserGetTimeBoxHandler.todayOver"));
                        }
                        else
                        {
                            LoadUserBoxInfo templateByCondition2 = UserBoxMgr.FindTemplateByCondition(1, player.PlayerCharacter.GetBoxLevel, Convert.ToInt32(player.PlayerCharacter.Sex));
                            if (templateByCondition2 != null)
                                player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("UserGetTimeBoxHandler.level", (object)templateByCondition2.Level));
                            else
                                player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("UserGetTimeBoxHandler.over"));
                        }
                        if (items.Count > 0 && player.SendItemsToMail(items, LanguageMgr.GetTranslation("UserGetTimeBoxHandler.mail"), LanguageMgr.GetTranslation("UserGetTimeBoxHandler.title"), eMailType.OpenUpArk))
                        {
                            player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("UserGetTimeBixHandler..full"));
                            val = true;
                            player.Out.SendMailResponse(player.PlayerCharacter.ID, eMailRespose.Receiver);
                        }
                    }
                    else
                        val = false;
                }
                else
                    player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("UserGetTimeBoxHandler.fail"));
                if (num2 == 0)
                {
                    pkg.WriteBoolean(val);
                    pkg.WriteInt(player.PlayerCharacter.BoxProgression);
                    player.SendTCP(pkg);
                }
                num1 = 0;
            }
            return num1;
        }
    }
}
