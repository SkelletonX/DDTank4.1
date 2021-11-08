// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.WorldEventMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using log4net;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Bussiness.Managers
{
  public class WorldEventMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ThreadSafeRandom random = new ThreadSafeRandom();
    private static ReaderWriterLock m_lock;

    public static bool SendItemsToMail(
      List<ItemInfo> infos,
      int PlayerId,
      string Nickname,
      string title,
      string content = null)
    {
      bool flag = false;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        List<ItemInfo> itemInfoList = new List<ItemInfo>();
        foreach (ItemInfo info in infos)
        {
          if (info.Template.MaxCount == 1)
          {
            for (int index = 0; index < info.Count; ++index)
            {
              ItemInfo itemInfo = ItemInfo.CloneFromTemplate(info.Template, info);
              itemInfo.Count = 1;
              itemInfoList.Add(itemInfo);
            }
          }
          else
            itemInfoList.Add(info);
        }
        for (int index1 = 0; index1 < itemInfoList.Count; index1 += 5)
        {
          MailInfo mail = new MailInfo()
          {
            Title = title,
            Content = content,
            Gold = 0,
            IsExist = true,
            Money = 0,
            Receiver = Nickname,
            ReceiverID = PlayerId,
            Sender = "Administrador do Sistema",
            SenderID = 0,
            Type = 9,
            GiftToken = 0
          };
          StringBuilder stringBuilder1 = new StringBuilder();
          StringBuilder stringBuilder2 = new StringBuilder();
          stringBuilder1.Append(LanguageMgr.GetTranslation("Game.Server.GameUtils.CommonBag.AnnexRemark"));
          int index2 = index1;
          int itemId;
          if (itemInfoList.Count > index2)
          {
            ItemInfo itemInfo = itemInfoList[index2];
            if (itemInfo.ItemID == 0)
              playerBussiness.AddGoods(itemInfo);
            MailInfo mailInfo = mail;
            itemId = itemInfo.ItemID;
            string str = itemId.ToString();
            mailInfo.Annex1 = str;
            mail.Annex1Name = itemInfo.Template.Name;
            stringBuilder1.Append("1、" + mail.Annex1Name + "x" + (object) itemInfo.Count + ";");
            stringBuilder2.Append("1、" + mail.Annex1Name + "x" + (object) itemInfo.Count + ";");
          }
          int index3 = index1 + 1;
          if (itemInfoList.Count > index3)
          {
            ItemInfo itemInfo = itemInfoList[index3];
            if (itemInfo.ItemID == 0)
              playerBussiness.AddGoods(itemInfo);
            MailInfo mailInfo = mail;
            itemId = itemInfo.ItemID;
            string str = itemId.ToString();
            mailInfo.Annex2 = str;
            mail.Annex2Name = itemInfo.Template.Name;
            stringBuilder1.Append("2、" + mail.Annex2Name + "x" + (object) itemInfo.Count + ";");
            stringBuilder2.Append("2、" + mail.Annex2Name + "x" + (object) itemInfo.Count + ";");
          }
          int index4 = index1 + 2;
          if (itemInfoList.Count > index4)
          {
            ItemInfo itemInfo = itemInfoList[index4];
            if (itemInfo.ItemID == 0)
              playerBussiness.AddGoods(itemInfo);
            MailInfo mailInfo = mail;
            itemId = itemInfo.ItemID;
            string str = itemId.ToString();
            mailInfo.Annex3 = str;
            mail.Annex3Name = itemInfo.Template.Name;
            stringBuilder1.Append("3、" + mail.Annex3Name + "x" + (object) itemInfo.Count + ";");
            stringBuilder2.Append("3、" + mail.Annex3Name + "x" + (object) itemInfo.Count + ";");
          }
          int index5 = index1 + 3;
          if (itemInfoList.Count > index5)
          {
            ItemInfo itemInfo = itemInfoList[index5];
            if (itemInfo.ItemID == 0)
              playerBussiness.AddGoods(itemInfo);
            MailInfo mailInfo = mail;
            itemId = itemInfo.ItemID;
            string str = itemId.ToString();
            mailInfo.Annex4 = str;
            mail.Annex4Name = itemInfo.Template.Name;
            stringBuilder1.Append("4、" + mail.Annex4Name + "x" + (object) itemInfo.Count + ";");
            stringBuilder2.Append("4、" + mail.Annex4Name + "x" + (object) itemInfo.Count + ";");
          }
          int index6 = index1 + 4;
          if (itemInfoList.Count > index6)
          {
            ItemInfo itemInfo = itemInfoList[index6];
            if (itemInfo.ItemID == 0)
              playerBussiness.AddGoods(itemInfo);
            MailInfo mailInfo = mail;
            itemId = itemInfo.ItemID;
            string str = itemId.ToString();
            mailInfo.Annex5 = str;
            mail.Annex5Name = itemInfo.Template.Name;
            stringBuilder1.Append("5、" + mail.Annex5Name + "x" + (object) itemInfo.Count + ";");
            stringBuilder2.Append("5、" + mail.Annex5Name + "x" + (object) itemInfo.Count + ";");
          }
          mail.AnnexRemark = stringBuilder1.ToString();
          mail.Content = mail.Content ?? stringBuilder2.ToString();
          flag = playerBussiness.SendMail(mail);
        }
      }
      return flag;
    }

    public static bool SendItemToMail(
      ItemInfo info,
      int PlayerId,
      string Nickname,
      int zoneId,
      AreaConfigInfo areaConfig,
      string title)
    {
      return WorldEventMgr.SendItemsToMail(new List<ItemInfo>()
      {
        info
      }, PlayerId, Nickname, title, (string) null);
    }

    public static bool SendItemsToMails(
      List<ItemInfo> infos,
      int PlayerId,
      string Nickname,
      int zoneId,
      AreaConfigInfo areaConfig,
      string title)
    {
      return WorldEventMgr.SendItemsToMail(infos, PlayerId, Nickname, title, (string) null);
    }

    public static bool SendItemsToMails(
      List<ItemInfo> infos,
      int PlayerId,
      string Nickname,
      int zoneId,
      AreaConfigInfo areaConfig,
      string title,
      string content)
    {
      return WorldEventMgr.SendItemsToMail(infos, PlayerId, Nickname, title, (string) null);
    }

    public static bool SendItemToMail(
      ItemInfo info,
      int PlayerId,
      string Nickname,
      int zoneId,
      AreaConfigInfo areaConfig,
      string title,
      string sender)
    {
      return WorldEventMgr.SendItemsToMail(new List<ItemInfo>()
      {
        info
      }, PlayerId, Nickname, title, (string) null);
    }

    public static bool SendItemsToMail(
      List<ItemInfo> infos,
      int PlayerId,
      string Nickname,
      int zoneId,
      AreaConfigInfo areaConfig,
      string title,
      int type,
      string sender)
    {
      return WorldEventMgr.SendItemsToMail(infos, PlayerId, Nickname, title, (string) null);
    }

    public static bool SendItemsToMail(
      List<ItemInfo> infos,
      int PlayerId,
      string Nickname,
      int zoneId,
      AreaConfigInfo areaConfig,
      string title,
      string content)
    {
      return WorldEventMgr.SendItemsToMail(infos, PlayerId, Nickname, title, (string) null);
    }
  }
}
