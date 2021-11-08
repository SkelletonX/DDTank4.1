// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.AwardMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Server.Buffer;
using Game.Server.GameObjects;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Game.Server.Managers
{
  public class AwardMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, DailyAwardInfo> _dailyAward;
    private static bool _dailyAwardState;
    private static Dictionary<int, SearchGoodsTempInfo> _searchGoodsTemp;
    private static ReaderWriterLock m_lock;

    public static bool AddDailyAward(GamePlayer player)
    {
      if (DateTime.Now.Date != player.PlayerCharacter.LastAward.Date)
      {
        ++player.PlayerCharacter.DayLoginCount;
        player.PlayerCharacter.LastAward = DateTime.Now;
        foreach (DailyAwardInfo dailyAwardInfo in AwardMgr.GetAllAwardInfo())
        {
          if (dailyAwardInfo.Type == 0)
          {
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(dailyAwardInfo.TemplateID);
            if (itemTemplate != null)
            {
              BufferList.CreateBufferMinutes(itemTemplate, dailyAwardInfo.ValidDate).Start(player);
              return true;
            }
          }
        }
      }
      return false;
    }

    public static bool AddSignAwards(GamePlayer player, int DailyLog)
    {
      AwardMgr.GetAllAwardInfo();
      DailyAwardInfo[] singleDailyAward = new ProduceBussiness().GetSingleDailyAward(DailyLog);
      StringBuilder stringBuilder = new StringBuilder();
      string str = string.Empty;
      bool flag1 = false;
      int templateId = 0;
      int num1 = 1;
      int num2 = 0;
      bool flag2 = true;
      bool flag3 = false;
      for (int index1 = 0; index1 < singleDailyAward.Length; ++index1)
      {
        DailyAwardInfo dailyAwardInfo = singleDailyAward[index1];
        flag1 = true;
        if (dailyAwardInfo.AwardDays == DailyLog && dailyAwardInfo.Type == 7)
        {
          player.AddGiftToken(dailyAwardInfo.Count);
          flag3 = true;
        }
        switch (DailyLog)
        {
          case 3:
            if (dailyAwardInfo.AwardDays == DailyLog && dailyAwardInfo.Type != 7)
            {
              templateId = dailyAwardInfo.TemplateID;
              num1 = dailyAwardInfo.Count;
              num2 = dailyAwardInfo.ValidDate;
              flag2 = dailyAwardInfo.IsBinds;
              flag3 = true;
              break;
            }
            break;
          case 6:
            if (dailyAwardInfo.AwardDays == DailyLog && dailyAwardInfo.Type != 7)
            {
              templateId = dailyAwardInfo.TemplateID;
              num1 = dailyAwardInfo.Count;
              num2 = dailyAwardInfo.ValidDate;
              flag2 = dailyAwardInfo.IsBinds;
              flag3 = true;
              break;
            }
            break;
          case 12:
            if (dailyAwardInfo.AwardDays == DailyLog && dailyAwardInfo.Type != 7)
            {
              templateId = dailyAwardInfo.TemplateID;
              num1 = dailyAwardInfo.Count;
              num2 = dailyAwardInfo.ValidDate;
              flag2 = dailyAwardInfo.IsBinds;
              flag3 = true;
              break;
            }
            break;
          case 18:
            if (dailyAwardInfo.AwardDays == DailyLog && dailyAwardInfo.Type != 7)
            {
              templateId = dailyAwardInfo.TemplateID;
              num1 = dailyAwardInfo.Count;
              num2 = dailyAwardInfo.ValidDate;
              flag2 = dailyAwardInfo.IsBinds;
              flag3 = true;
              break;
            }
            break;
        }
        ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(templateId);
        if (itemTemplate != null)
        {
          int num3 = num1;
          for (int index2 = 0; index2 < num3; index2 += itemTemplate.MaxCount)
          {
            int count = index2 + itemTemplate.MaxCount > num3 ? num3 - index2 : itemTemplate.MaxCount;
            SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate, count, 113);
            fromTemplate.ValidDate = num2;
            fromTemplate.IsBinds = flag2;
            if (!player.AddTemplate(fromTemplate, fromTemplate.Template.BagType, fromTemplate.Count, eGameView.CaddyTypeGet))
            {
              flag1 = true;
              using (PlayerBussiness playerBussiness = new PlayerBussiness())
              {
                fromTemplate.UserID = 0;
                playerBussiness.AddGoods(fromTemplate);
                MailInfo mail = new MailInfo()
                {
                  Annex1 = fromTemplate.ItemID.ToString(),
                  Content = LanguageMgr.GetTranslation("AwardMgr.AddDailyAward.Content", (object) fromTemplate.Template.Name),
                  Gold = 0,
                  Money = 0,
                  Receiver = player.PlayerCharacter.NickName,
                  ReceiverID = player.PlayerCharacter.ID
                };
                mail.Sender = mail.Receiver;
                mail.SenderID = mail.ReceiverID;
                mail.Title = LanguageMgr.GetTranslation("AwardMgr.AddDailyAward.Title", (object) fromTemplate.Template.Name);
                mail.Type = 15;
                playerBussiness.SendMail(mail);
                str = LanguageMgr.GetTranslation("AwardMgr.AddDailyAward.Mail");
              }
            }
          }
        }
      }
      if (flag1 && !string.IsNullOrEmpty(str))
        player.Out.SendMailResponse(player.PlayerCharacter.ID, eMailRespose.Receiver);
      return flag3;
    }

    public static DailyAwardInfo[] GetAllAwardInfo()
    {
      DailyAwardInfo[] dailyAwardInfoArray = (DailyAwardInfo[]) null;
      AwardMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        dailyAwardInfoArray = AwardMgr._dailyAward.Values.ToArray<DailyAwardInfo>();
      }
      catch
      {
      }
      finally
      {
        AwardMgr.m_lock.ReleaseReaderLock();
      }
      return dailyAwardInfoArray ?? new DailyAwardInfo[0];
    }

    public static SearchGoodsTempInfo GetSearchGoodsTempInfo(int starId)
    {
      AwardMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (AwardMgr._searchGoodsTemp.ContainsKey(starId))
          return AwardMgr._searchGoodsTemp[starId];
      }
      catch
      {
      }
      finally
      {
        AwardMgr.m_lock.ReleaseReaderLock();
      }
      return (SearchGoodsTempInfo) null;
    }

    public static bool Init()
    {
      try
      {
        AwardMgr.m_lock = new ReaderWriterLock();
        AwardMgr._dailyAward = new Dictionary<int, DailyAwardInfo>();
        AwardMgr._searchGoodsTemp = new Dictionary<int, SearchGoodsTempInfo>();
        AwardMgr._dailyAwardState = false;
        return AwardMgr.LoadDailyAward(AwardMgr._dailyAward, AwardMgr._searchGoodsTemp);
      }
      catch (Exception ex)
      {
        if (AwardMgr.log.IsErrorEnabled)
          AwardMgr.log.Error((object) nameof (AwardMgr), ex);
        return false;
      }
    }

    private static bool LoadDailyAward(
      Dictionary<int, DailyAwardInfo> awards,
      Dictionary<int, SearchGoodsTempInfo> searchGoodsTemp)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (DailyAwardInfo dailyAwardInfo in produceBussiness.GetAllDailyAward())
        {
          if (!awards.ContainsKey(dailyAwardInfo.ID))
            awards.Add(dailyAwardInfo.ID, dailyAwardInfo);
        }
        foreach (SearchGoodsTempInfo searchGoodsTempInfo in produceBussiness.GetAllSearchGoodsTemp())
        {
          if (!searchGoodsTemp.ContainsKey(searchGoodsTempInfo.StarID))
            searchGoodsTemp.Add(searchGoodsTempInfo.StarID, searchGoodsTempInfo);
        }
      }
      return true;
    }

    public static int MaxStar()
    {
      return AwardMgr._searchGoodsTemp.Count;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, DailyAwardInfo> awards = new Dictionary<int, DailyAwardInfo>();
        Dictionary<int, SearchGoodsTempInfo> searchGoodsTemp = new Dictionary<int, SearchGoodsTempInfo>();
        if (AwardMgr.LoadDailyAward(awards, searchGoodsTemp))
        {
          AwardMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            AwardMgr._dailyAward = awards;
            AwardMgr._searchGoodsTemp = searchGoodsTemp;
            return true;
          }
          catch
          {
          }
          finally
          {
            AwardMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (AwardMgr.log.IsErrorEnabled)
          AwardMgr.log.Error((object) nameof (AwardMgr), ex);
      }
      return false;
    }

    public static bool DailyAwardState
    {
      get
      {
        return AwardMgr._dailyAwardState;
      }
      set
      {
        AwardMgr._dailyAwardState = value;
      }
    }
  }
}
