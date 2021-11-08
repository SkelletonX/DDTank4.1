// Decompiled with JetBrains decompiler
// Type: Game.Server.GameUtils.PlayerBattle
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Server.GameObjects;
using Game.Server.Managers;
using log4net;
using SqlDataProvider.Data;
using System.Reflection;

namespace Game.Server.GameUtils
{
  public class PlayerBattle
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public readonly int Agility = 1600;
    public readonly int Attack = 1700;
    public readonly int Blood = 25000;
    public readonly int Damage = 1000;
    public readonly int Defend = 1500;
    public readonly int Energy = 293;
    public readonly int fairBattleDayPrestige = 2000;
    public readonly int Guard = 500;
    public readonly int LevelLimit = 15;
    public readonly int Lucky = 1500;
    protected object m_lock = new object();
    public readonly int maxCount = 30;
    private UserMatchInfo m_matchInfo;
    protected GamePlayer m_player;
    private bool m_saveToDb;

    public PlayerBattle(GamePlayer player, bool saveTodb)
    {
      this.m_player = player;
      this.m_saveToDb = saveTodb;
    }

    public void AddPrestige(bool isWin)
    {
      FairBattleRewardInfo battleDataByPrestige = FairBattleRewardMgr.GetBattleDataByPrestige(this.m_matchInfo.totalPrestige);
      if (battleDataByPrestige == null)
      {
        this.Player.SendMessage(LanguageMgr.GetTranslation("PVPGame.SendGameOVer.Msg5"));
      }
      else
      {
        int num = battleDataByPrestige.PrestigeForWin;
        string translation = LanguageMgr.GetTranslation("PVPGame.SendGameOVer.Msg3", (object) num);
        if (isWin)
          ++this.m_matchInfo.dailyWinCount;
        if (!isWin)
        {
          num = battleDataByPrestige.PrestigeForLose;
          translation = LanguageMgr.GetTranslation("PVPGame.SendGameOVer.Msg4", (object) num);
        }
        if (this.m_matchInfo.addDayPrestge < this.fairBattleDayPrestige)
        {
          this.m_matchInfo.addDayPrestge += num;
          this.m_matchInfo.totalPrestige += num;
        }
        this.Player.SendMessage(translation);
      }
      ++this.m_matchInfo.dailyGameCount;
      ++this.m_matchInfo.weeklyGameCount;
    }

    public void CreateInfo(int UserID)
    {
      this.m_matchInfo = new UserMatchInfo();
      this.m_matchInfo.ID = 0;
      this.m_matchInfo.UserID = UserID;
      this.m_matchInfo.dailyScore = 0;
      this.m_matchInfo.dailyWinCount = 0;
      this.m_matchInfo.dailyGameCount = 0;
      this.m_matchInfo.DailyLeagueFirst = true;
      this.m_matchInfo.DailyLeagueLastScore = 0;
      this.m_matchInfo.weeklyScore = 0;
      this.m_matchInfo.weeklyGameCount = 0;
      this.m_matchInfo.weeklyRanking = 0;
      this.m_matchInfo.addDayPrestge = 0;
      this.m_matchInfo.totalPrestige = 0;
      this.m_matchInfo.restCount = 30;
      this.m_matchInfo.leagueGrade = 0;
      this.m_matchInfo.leagueItemsGet = 0;
      this.m_matchInfo.maxCount = this.maxCount;
    }

    public int GetRank()
    {
      UserMatchInfo rank = RankMgr.FindRank(this.Player.PlayerCharacter.ID);
      if (rank != null)
        return rank.rank;
      return 0;
    }

    public virtual void LoadFromDatabase()
    {
      if (!this.m_saveToDb)
        return;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        this.m_matchInfo = playerBussiness.GetSingleUserMatchInfo(this.Player.PlayerCharacter.ID);
        if (this.m_matchInfo == null)
          this.CreateInfo(this.Player.PlayerCharacter.ID);
        this.m_matchInfo.maxCount = this.maxCount;
      }
    }

    public void Reset()
    {
      this.m_matchInfo.dailyScore = 0;
      this.m_matchInfo.dailyWinCount = 0;
      this.m_matchInfo.dailyGameCount = 0;
      this.m_matchInfo.addDayPrestge = 0;
      this.m_matchInfo.restCount = 30;
      this.m_matchInfo.leagueItemsGet = 0;
      this.m_saveToDb = true;
      this.SaveToDatabase();
    }

    public virtual void SaveToDatabase()
    {
      if (!this.m_saveToDb)
        return;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        lock (this.m_lock)
        {
          if (this.m_matchInfo == null || !this.m_matchInfo.IsDirty)
            return;
          if (this.m_matchInfo.ID > 0)
            playerBussiness.UpdateUserMatchInfo(this.m_matchInfo);
          else
            playerBussiness.AddUserMatchInfo(this.m_matchInfo);
        }
      }
    }

    public void UpdateLeagueGrade()
    {
      if (this.Player.PlayerCharacter.Grade < 30)
        this.Player.MatchInfo.leagueGrade = 20;
      else if (this.Player.PlayerCharacter.Grade < 40)
        this.Player.MatchInfo.leagueGrade = 30;
      else if (this.Player.PlayerCharacter.Grade <= 50)
        this.Player.MatchInfo.leagueGrade = 40;
      this.m_saveToDb = true;
      this.SaveToDatabase();
    }

    public UserMatchInfo MatchInfo
    {
      get
      {
        return this.m_matchInfo;
      }
      set
      {
        this.m_matchInfo = value;
      }
    }

    public GamePlayer Player
    {
      get
      {
        return this.m_player;
      }
    }
  }
}
