using Bussiness;
using Game.Base;
using Game.Base.Packets;
using Game.Server;
using Game.Server.GameObjects;
using Game.Server.GameUtils;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Statics;
using SqlDataProvider.Data;
using System;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler(116, "发送邮件")]
    public class UserSendMailHandler : IPacketHandler
    {
        public UserSendMailHandler()
        {


        }

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num;
            PlayerInfo playerInfo;
            GSPacketIn gSPacketIn;
            GSPacketIn gSPacketIn1 = new GSPacketIn(116, client.Player.PlayerCharacter.ID);
            if (client.Player.PlayerCharacter.Gold < 100)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("UserSendMailHandler.GoldNotEnought"));
                gSPacketIn1.WriteBoolean(false);
                client.Out.SendTCP(gSPacketIn1);
                return 1;
            }
            //if (client.Player.IsLimitMail())
            //{
            //    client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("UserSendMailHandler.IsLimitMail", Array.Empty<object>()));
            //    gSPacketIn1.WriteBoolean(false);
            //    client.Out.SendTCP(gSPacketIn1);
            //    return 1;
            //}
            string str = "UserSendMailHandler.Success";
            eMessageType _eMessageType = eMessageType.Normal;
            ItemInfo itemAt = null;
            string str1 = packet.ReadString();
            string str2 = packet.ReadString();
            string str3 = packet.ReadString();
            bool flag = packet.ReadBoolean();
            int num1 = packet.ReadInt();
            int num2 = packet.ReadInt();
            eBageType _eBageType = (eBageType)packet.ReadByte();
            int num3 = packet.ReadInt();
            int num4 = packet.ReadInt();
            if (client.Player.IsLimitMoney(num2))
            {
                gSPacketIn1.WriteBoolean(false);
                client.Out.SendTCP(gSPacketIn1);
                return 1;
            }
            int num5 = GameProperties.LimitLevel(0);
            if (num3 != -1 && client.Player.PlayerCharacter.Grade < num5)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("UserSendMailHandler.Msg4", new object[] { num5 }));
                gSPacketIn1.WriteBoolean(false);
                client.Out.SendTCP(gSPacketIn1);
                return 0;
            }
            if (_eBageType == eBageType.EquipBag && num3 != -1 && num3 < client.Player.EquipBag.BeginSlot)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("UserSendMailHandler.WrongPlace"));
                gSPacketIn1.WriteBoolean(false);
                client.Out.SendTCP(gSPacketIn1);
                return 0;
            }
            if ((num2 != 0 || num3 != -1) && client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                gSPacketIn1.WriteBoolean(false);
                client.Out.SendTCP(gSPacketIn1);
                return 1;
            }
            ItemInfo itemInfo = null;
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                GamePlayer clientByPlayerNickName = WorldMgr.GetClientByPlayerNickName(str1);
                playerInfo = (clientByPlayerNickName != null ? clientByPlayerNickName.PlayerCharacter : playerBussiness.GetUserSingleByNickName(str1));
                if (playerInfo == null || string.IsNullOrEmpty(str1))
                {
                    _eMessageType = eMessageType.ERROR;
                    str = "UserSendMailHandler.Failed2";
                    gSPacketIn1.WriteBoolean(false);
                    gSPacketIn = client.Out.SendMessage(_eMessageType, LanguageMgr.GetTranslation(str));
                    client.Out.SendTCP(gSPacketIn1);
                    return 0;
                }
                else if (playerInfo.Grade < num5)
                {
                    client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("UserSendMailHandler.Msg3", new object[] { num5 }));
                    gSPacketIn1.WriteBoolean(false);
                    client.Out.SendTCP(gSPacketIn1);
                    num = 0;
                }
                else if (playerInfo.NickName == client.Player.PlayerCharacter.NickName)
                {
                    str = "UserSendMailHandler.Failed1";
                    gSPacketIn1.WriteBoolean(false);
                    gSPacketIn = client.Out.SendMessage(_eMessageType, LanguageMgr.GetTranslation(str));
                    client.Out.SendTCP(gSPacketIn1);
                    return 0;
                }
                else
                {
                    MailInfo mailInfo = new MailInfo()
                    {
                        SenderID = client.Player.PlayerCharacter.ID,
                        Sender = client.Player.PlayerCharacter.NickName,
                        ReceiverID = playerInfo.ID,
                        Receiver = playerInfo.NickName,
                        IsExist = true,
                        Gold = 0,
                        Money = 0,
                        Title = str2,
                        Content = str3
                    };
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(LanguageMgr.GetTranslation("UserSendMailHandler.AnnexRemark"));
                    int num6 = 0;
                    if (num3 != -1)
                    {
                        itemAt = client.Player.GetItemAt(_eBageType, num3);
                        if (itemAt == null)
                        {
                            client.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("UserSendMailHandler.Msg1"));
                            num = 0;
                            return num;
                        }
                        else if (!itemAt.IsBinds && itemAt != null)
                        {
                            //if (itemAt.Count < num4 || num4 < 0)
                            //{
                            //    client.Out.SendMessage(_eMessageType, LanguageMgr.GetTranslation("UserSendMailHandler.Msg2"));
                            //    num = 0;
                            //    return num;
                            //}
                            //else if (!client.Player.IsLimitCount(num4))
                            //{
                                itemInfo = ItemInfo.CloneFromTemplate(itemAt.Template, itemAt);
                                ItemInfo itemInfo1 = ItemInfo.CloneFromTemplate(itemAt.Template, itemAt);
                                //itemInfo1.Count = num4;
                                if (itemInfo1.ItemID == 0)
                                {
                                    playerBussiness.AddGoods(itemInfo1);
                                }
                                mailInfo.Annex1Name = itemInfo1.Template.Name;
                                mailInfo.Annex1 = itemInfo1.ItemID.ToString();
                                num6++;
                                stringBuilder.Append(num6);
                                stringBuilder.Append("、");
                                stringBuilder.Append(mailInfo.Annex1Name);
                                stringBuilder.Append("x");
                                stringBuilder.Append(itemInfo1.Count);
                                stringBuilder.Append(";");
                            }
                            else
                            {
                                num = 0;
                                return num;
                            }
                        //}
                    }
                    if (!flag)
                    {
                        mailInfo.Type = 1;
                        if (client.Player.PlayerCharacter.Money >= num2 && num2 > 0)
                        {
                            mailInfo.Money = num2;
                            LogMgr.LogMoneyAdd(LogMoneyType.Mail, LogMoneyType.Mail_Send, client.Player.PlayerCharacter.ID, num2, client.Player.PlayerCharacter.Money, 0, 0, 0, "", "", "");
                            client.Player.RemoveMoney(num2);
                            num6++;
                            stringBuilder.Append(num6);
                            stringBuilder.Append("、");
                            stringBuilder.Append(LanguageMgr.GetTranslation("UserSendMailHandler.Money"));
                            stringBuilder.Append(num2);
                            stringBuilder.Append(";");
                        }
                    }
                    else if (num2 <= 0 || string.IsNullOrEmpty(mailInfo.Annex1) && string.IsNullOrEmpty(mailInfo.Annex2) && string.IsNullOrEmpty(mailInfo.Annex3) && string.IsNullOrEmpty(mailInfo.Annex4))
                    {
                        num = 1;
                        return num;
                    }
                    else
                    {
                        mailInfo.ValidDate = (num1 == 1 ? 1 : 6);
                        mailInfo.Type = 101;
                        if (num2 > 0)
                        {
                            mailInfo.Money = num2;
                            num6++;
                            stringBuilder.Append(num6);
                            stringBuilder.Append("、");
                            stringBuilder.Append(LanguageMgr.GetTranslation("UserSendMailHandler.PayMoney"));
                            stringBuilder.Append(num2);
                            stringBuilder.Append(";");
                        }
                    }
                    if (stringBuilder.Length > 1)
                    {
                        mailInfo.AnnexRemark = stringBuilder.ToString();
                    }
                    if (playerBussiness.SendMail(mailInfo))
                    {
                        client.Player.RemoveGold(100);
                        if (itemAt != null)
                        {
                            int count = itemAt.Count - num4;
                            client.Player.RemoveItem(itemAt);
                            if (count > 0)
                            {
                                itemInfo.Count = count;
                                client.Player.AddTemplate(itemInfo, _eBageType, count, eGameView.RouletteTypeGet);
                            }
                            client.Player.OnSendGiftmail(itemAt.TemplateID, num4);

                        }
                    }
                    gSPacketIn1.WriteBoolean(true);
                    if (clientByPlayerNickName != null)
                    {
                        client.Player.Out.SendMailResponse(playerInfo.ID, eMailRespose.Receiver);
                    }
                    client.Player.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Send);
                    gSPacketIn = client.Out.SendMessage(_eMessageType, LanguageMgr.GetTranslation(str));
                    client.Out.SendTCP(gSPacketIn1);
                    return 0;
                }
            }
            return num;
        }
    }
}