using Bussiness;
using Game.Server.GameObjects;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.GameUtils
{
  public class PlayerRank
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected object m_lock = new object();
    private UserRankInfo m_currentRank;
    protected GamePlayer m_player;
    private List<UserRankInfo> m_rank;
    private bool m_saveToDb;

    public PlayerRank(GamePlayer player, bool saveTodb)
    {
      this.m_player = player;
      this.m_saveToDb = saveTodb;
      this.m_rank = new List<UserRankInfo>();
      this.m_currentRank = this.GetRank(this.m_player.PlayerCharacter.Honor);
    }

    public void AddRank(UserRankInfo info)
    {
      lock (this.m_rank)
        this.m_rank.Add(info);
    }

    public void AddRank(string honor)
    {
      this.AddRank(new UserRankInfo()
      {
        ID = 0,
        UserID = this.m_player.PlayerCharacter.ID,
        UserRank = honor,
        Attack = 0,
        Defence = 0,
        Luck = 0,
        Agility = 0,
        HP = 0,
        Damage = 0,
        Guard = 0,
        BeginDate = DateTime.Now,
        Validate = 0,
        IsExit = true
      });
    }

    public void CreateRank(int UserID)
    {
      List<UserRankInfo> userRankInfoList = new List<UserRankInfo>();
      this.AddRank(new UserRankInfo()
      {
        ID = 0,
        UserID = UserID,
        UserRank = "Super Tank",
        Attack = 0,
        Defence = 0,
        Luck = 0,
        Agility = 0,
        HP = 0,
        Damage = 0,
        Guard = 0,
        BeginDate = DateTime.Now,
        Validate = 0,
        IsExit = true
      });
    }

    public List<UserRankInfo> GetRank()
    {
      List<UserRankInfo> userRankInfoList = new List<UserRankInfo>();
      foreach (UserRankInfo userRankInfo in this.m_rank)
      {
        if (userRankInfo.IsExit)
          userRankInfoList.Add(userRankInfo);
      }
      return userRankInfoList;
    }

    public UserRankInfo GetRank(string honor)
    {
      foreach (UserRankInfo userRankInfo in this.m_rank)
      {
        if (userRankInfo.UserRank == honor)
          return userRankInfo;
      }
      return (UserRankInfo) null;
    }

    public bool IsRank(string honor)
    {
      foreach (UserRankInfo userRankInfo in this.m_rank)
      {
        if (userRankInfo.UserRank == honor)
          return true;
      }
      return false;
    }

    public virtual void LoadFromDatabase()
    {
      if (!this.m_saveToDb)
        return;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        List<UserRankInfo> singleUserRank = playerBussiness.GetSingleUserRank(this.Player.PlayerCharacter.ID);
        if (singleUserRank.Count == 0)
        {
          this.CreateRank(this.Player.PlayerCharacter.ID);
        }
        else
        {
          foreach (UserRankInfo info in singleUserRank)
          {
            if (info.IsValidRank())
              this.AddRank(info);
            else
              this.RemoveRank(info);
          }
        }
      }
    }

    public void RemoveRank(UserRankInfo item)
    {
      item.IsExit = false;
      this.AddRank(item);
    }

    public virtual void SaveToDatabase()
    {
      if (!this.m_saveToDb)
        return;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        lock (this.m_lock)
        {
          for (int index = 0; index < this.m_rank.Count; ++index)
          {
            UserRankInfo userRankInfo = this.m_rank[index];
            if (userRankInfo != null && userRankInfo.IsDirty)
            {
              if (userRankInfo.ID > 0)
                playerBussiness.UpdateUserRank(userRankInfo);
              else
                playerBussiness.AddUserRank(userRankInfo);
            }
          }
        }
      }
    }

    public UserRankInfo CurrentRank
    {
      get
      {
        return this.m_currentRank;
      }
      set
      {
        this.m_currentRank = value;
      }
    }

    public GamePlayer Player
    {
      get
      {
        return this.m_player;
      }
    }

    public List<UserRankInfo> Ranks
    {
      get
      {
        return this.m_rank;
      }
      set
      {
        this.m_rank = value;
      }
    }
  }
}
