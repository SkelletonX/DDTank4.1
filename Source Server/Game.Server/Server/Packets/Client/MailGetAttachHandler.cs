// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MailGetAttachHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler(113, "获取邮件到背包")]
  public class MailGetAttachHandler : IPacketHandler
  {
    public bool GetAnnex(
      string value,
      GamePlayer player,
      ref string msg,
      ref bool result,
      ref eMessageType eMsg)
    {
      int itemID = int.Parse(value);
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        SqlDataProvider.Data.ItemInfo userItemSingle = playerBussiness.GetUserItemSingle(itemID);
        if (userItemSingle != null)
        {
          if (player.AddTemplate(userItemSingle))
          {
            eMsg = eMessageType.GM_NOTICE;
            return true;
          }
        }
      }
      return false;
    }

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num1 = packet.ReadInt();
      byte num2 = packet.ReadByte();
      List<int> intList = new List<int>();
      List<string> stringList = new List<string>();
      int num3 = 0;
      int num4 = 0;
      int num5 = 0;
      string msg = "";
      eMessageType eMsg = eMessageType.GM_NOTICE;
      if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
        return 0;
      }
      GSPacketIn packet1 = new GSPacketIn((short) 113, client.Player.PlayerCharacter.ID);
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        client.Player.LastAttachMail = DateTime.Now;
        MailInfo mailSingle = playerBussiness.GetMailSingle(client.Player.PlayerCharacter.ID, num1);
        if (mailSingle != null)
        {
          bool result = true;
          int money = mailSingle.Money;
          if (mailSingle.Type > 100 && !client.Player.MoneyDirect(money, true))
            return 0;
          GamePlayer playerById = WorldMgr.GetPlayerById(mailSingle.ReceiverID);
          if (!mailSingle.IsRead)
          {
            mailSingle.IsRead = true;
            mailSingle.ValidDate = 72;
            mailSingle.SendTime = DateTime.Now;
          }
          if (result && (num2 == (byte) 0 || num2 == (byte) 1) && !string.IsNullOrEmpty(mailSingle.Annex1))
          {
            intList.Add(1);
            stringList.Add(mailSingle.Annex1);
            mailSingle.Annex1 = (string) null;
          }
          if (result && (num2 == (byte) 0 || num2 == (byte) 2) && !string.IsNullOrEmpty(mailSingle.Annex2))
          {
            intList.Add(2);
            stringList.Add(mailSingle.Annex2);
            mailSingle.Annex2 = (string) null;
          }
          if (result && (num2 == (byte) 0 || num2 == (byte) 3) && !string.IsNullOrEmpty(mailSingle.Annex3))
          {
            intList.Add(3);
            stringList.Add(mailSingle.Annex3);
            mailSingle.Annex3 = (string) null;
          }
          if (result && (num2 == (byte) 0 || num2 == (byte) 4) && !string.IsNullOrEmpty(mailSingle.Annex4))
          {
            intList.Add(4);
            stringList.Add(mailSingle.Annex4);
            mailSingle.Annex4 = (string) null;
          }
          if (result && (num2 == (byte) 0 || num2 == (byte) 5) && !string.IsNullOrEmpty(mailSingle.Annex5))
          {
            intList.Add(5);
            stringList.Add(mailSingle.Annex5);
            mailSingle.Annex5 = (string) null;
          }
          if ((num2 == (byte) 0 || num2 == (byte) 6) && mailSingle.Gold > 0)
          {
            intList.Add(6);
            num4 = mailSingle.Gold;
            mailSingle.Gold = 0;
          }
          if ((num2 == (byte) 0 || num2 == (byte) 7) && (mailSingle.Type < 100 && mailSingle.Money > 0))
          {
            intList.Add(7);
            num3 = mailSingle.Money;
            mailSingle.Money = 0;
          }
          if (mailSingle.Type > 100 && mailSingle.GiftToken > 0)
          {
            intList.Add(8);
            num5 = mailSingle.GiftToken;
            mailSingle.GiftToken = 0;
          }
          if (mailSingle.Type > 100 && mailSingle.Money > 0)
          {
            mailSingle.Money = 0;
            msg = LanguageMgr.GetTranslation("MailGetAttachHandler.Deduct") + (string.IsNullOrEmpty(msg) ? LanguageMgr.GetTranslation("MailGetAttachHandler.Success") : msg);
          }
          if (playerBussiness.UpdateMail(mailSingle, money))
          {
            if (mailSingle.Type > 100 && money > 0)
            {
              client.Out.SendMailResponse(mailSingle.SenderID, eMailRespose.Receiver);
              client.Out.SendMailResponse(mailSingle.ReceiverID, eMailRespose.Send);
            }
            playerById.AddMoney(num3);
            playerById.AddGold(num4);
            playerById.AddGiftToken(num5);
            foreach (string str in stringList)
              this.GetAnnex(str, client.Player, ref msg, ref result, ref eMsg);
          }
          packet1.WriteInt(num1);
          packet1.WriteInt(intList.Count);
          foreach (int val in intList)
            packet1.WriteInt(val);
          client.Out.SendTCP(packet1);
          client.Out.SendMessage(eMsg, string.IsNullOrEmpty(msg) ? LanguageMgr.GetTranslation("MailGetAttachHandler.Success") : msg);
        }
        else
          client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("MailGetAttachHandler.Falied"));
      }
      return 0;
    }
  }
}
