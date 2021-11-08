// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.AchievementInventory
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.Achievement
{
  public class AchievementInventory
  {
    private object m_lock;
    protected List<AchievementDataInfo> m_data;
    protected List<UsersRecordInfo> m_userRecord;
    private GamePlayer m_player;

    public AchievementInventory(GamePlayer player)
    {
      this.m_player = player;
      this.m_lock = new object();
      this.m_userRecord = new List<UsersRecordInfo>();
      this.m_data = new List<AchievementDataInfo>();
    }

    public void LoadFromDatabase(int playerId)
    {
      lock (this.m_lock)
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          this.m_userRecord = playerBussiness.GetUserRecord(this.m_player.PlayerId);
          this.m_data = playerBussiness.GetUserAchievementData(this.m_player.PlayerId);
          this.InitUserRecord();
          if (this.m_userRecord != null && this.m_userRecord.Count > 0)
            this.m_player.Out.SendInitAchievements(this.m_userRecord);
          if (this.m_data != null)
          {
            if (this.m_data.Count > 0)
              this.m_player.Out.SendUpdateAchievementData(this.m_data);
          }
        }
        BaseUserRecord.CreateCondition(AchievementMgr.ItemRecordType, this.m_player);
      }
    }

    public List<AchievementDataInfo> GetSuccessAchievement()
    {
      lock (this.m_data)
        return this.m_data.ToList<AchievementDataInfo>();
    }

    public List<UsersRecordInfo> GetProccessAchievement()
    {
      lock (this.m_userRecord)
        return this.m_userRecord.ToList<UsersRecordInfo>();
    }

    public void InitUserRecord()
    {
      Hashtable itemRecordType = AchievementMgr.ItemRecordType;
      lock (this.m_userRecord)
      {
        if (this.m_userRecord.Count >= itemRecordType.Count)
          return;
        foreach (DictionaryEntry dictionaryEntry in itemRecordType)
        {
          DictionaryEntry de = dictionaryEntry;
          UsersRecordInfo usersRecordInfo = new UsersRecordInfo();
          usersRecordInfo.UserID = this.m_player.PlayerId;
          usersRecordInfo.RecordID = int.Parse(de.Key.ToString());
          usersRecordInfo.Total = 0;
          usersRecordInfo.IsDirty = true;
          if (this.m_userRecord.Where<UsersRecordInfo>((Func<UsersRecordInfo, bool>) (s => s.RecordID == int.Parse(de.Key.ToString()))).ToList<UsersRecordInfo>().Count <= 0)
            this.m_userRecord.Add(usersRecordInfo);
        }
      }
    }

    public int UpdateUserAchievement(int type, int value)
    {
      lock (this.m_userRecord)
      {
        foreach (UsersRecordInfo info in this.m_userRecord)
        {
          if (info.RecordID == type)
          {
            info.Total += value;
            info.IsDirty = true;
            this.m_player.Out.SendUpdateAchievements(info);
          }
        }
      }
      return 0;
    }

    public int UpdateUserAchievement(int type, int value, int mode)
    {
      lock (this.m_userRecord)
      {
        foreach (UsersRecordInfo info in this.m_userRecord)
        {
          if (info.RecordID == type && info.Total < value)
          {
            info.Total = value;
            info.IsDirty = true;
            this.m_player.Out.SendUpdateAchievements(info);
          }
        }
      }
      return 0;
    }

    public bool Finish(AchievementInfo achievementInfo)
    {
      bool flag;
      if (!this.CanCompleted(achievementInfo))
      {
        flag = false;
      }
      else
      {
        this.AddAchievementData(achievementInfo);
        this.SendReward(achievementInfo);
        flag = true;
      }
      return flag;
    }

    private bool CheckAchievementData(AchievementInfo info)
    {
      bool flag;
      if (info.EndDate < DateTime.Now)
        flag = false;
      else if (info.NeedMaxLevel < this.m_player.Level)
        flag = false;
      else if (info.IsOther == 1 && this.m_player.PlayerCharacter.ConsortiaID <= 0)
        flag = false;
      else if (info.IsOther == 2 && this.m_player.PlayerCharacter.SpouseID <= 0)
        flag = false;
      else if (info.PreAchievementID != "0,")
      {
        string preAchievementId = info.PreAchievementID;
        char[] chArray = new char[1]{ ',' };
        foreach (string str in preAchievementId.Split(chArray))
        {
          if (!this.IsAchievementFinish(AchievementMgr.GetSingleAchievement(Convert.ToInt32(str))))
            return false;
        }
        flag = true;
      }
      else
        flag = true;
      return flag;
    }

    public bool CanCompleted(AchievementInfo achievementInfo)
    {
      int num = 0;
      List<AchievementConditionInfo> achievementCondition = AchievementMgr.GetAchievementCondition(achievementInfo);
      if (achievementCondition != null && achievementCondition.Count > 0)
      {
        foreach (AchievementConditionInfo achievementConditionInfo in achievementCondition)
        {
          foreach (UsersRecordInfo usersRecordInfo in this.m_userRecord)
          {
            if (achievementConditionInfo.CondictionType == usersRecordInfo.RecordID && achievementConditionInfo.Condiction_Para2 <= usersRecordInfo.Total)
              ++num;
          }
        }
      }
      return num == achievementCondition.Count;
    }

    public bool SendReward(AchievementInfo achievementInfo)
    {
      string str1 = "";
      List<AchievementRewardInfo> achievementReward = AchievementMgr.GetAchievementReward(achievementInfo);
      List<SqlDataProvider.Data.ItemInfo> itemInfoList1 = new List<SqlDataProvider.Data.ItemInfo>();
      List<SqlDataProvider.Data.ItemInfo> itemInfoList2 = new List<SqlDataProvider.Data.ItemInfo>();
      foreach (AchievementRewardInfo achievementRewardInfo in achievementReward)
      {
        if (achievementRewardInfo.RewardType == 1)
        {
          this.m_player.Rank.AddRank(achievementRewardInfo.RewardPara);
          this.m_player.Out.SendUserRanks(this.m_player.Rank.GetRank());
        }
      }
      if ((uint) achievementInfo.AchievementPoint > 0U)
      {
        this.m_player.AddAchievementPoint(achievementInfo.AchievementPoint);
        string str2 = str1 + LanguageMgr.GetTranslation("Game.Server.Achievement.FinishAchievement.AchievementPoint", (object) achievementInfo.AchievementPoint) + " ";
      }
      return true;
    }

    public AchievementInfo FindAchievement(int id)
    {
      foreach (KeyValuePair<int, AchievementInfo> keyValuePair in AchievementMgr.Achievement)
      {
        if (keyValuePair.Value.ID == id)
          return keyValuePair.Value;
      }
      return (AchievementInfo) null;
    }

    public bool AddAchievementData(AchievementInfo achievementInfo)
    {
      bool flag;
      if (!this.IsAchievementFinish(achievementInfo))
      {
        AchievementDataInfo achievementDataInfo = new AchievementDataInfo();
        achievementDataInfo.UserID = this.m_player.PlayerId;
        achievementDataInfo.AchievementID = achievementInfo.ID;
        achievementDataInfo.IsComplete = true;
        achievementDataInfo.CompletedDate = DateTime.Now;
        achievementDataInfo.IsDirty = true;
        lock (this.m_data)
          this.m_data.Add(achievementDataInfo);
        flag = true;
      }
      else
        flag = false;
      return flag;
    }

    private bool IsAchievementFinish(AchievementInfo achievementInfo)
    {
      IEnumerable<AchievementDataInfo> source = this.m_data.Where<AchievementDataInfo>((Func<AchievementDataInfo, bool>) (s => s.AchievementID == achievementInfo.ID));
      if (source != null)
        return source.ToList<AchievementDataInfo>().Count > 0;
      return false;
    }

    public void SaveToDatabase()
    {
      if (this.m_userRecord != null && this.m_userRecord.Count > 0)
      {
        lock (this.m_userRecord)
        {
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            foreach (UsersRecordInfo info in this.m_userRecord)
            {
              if (info.IsDirty)
                playerBussiness.UpdateDbUserRecord(info);
            }
          }
        }
      }
      if (this.m_data == null || this.m_data.Count <= 0)
        return;
      lock (this.m_data)
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          foreach (AchievementDataInfo info in this.m_data)
          {
            if (info.IsDirty)
              playerBussiness.UpdateDbAchievementDataInfo(info);
          }
        }
      }
    }
  }
}
