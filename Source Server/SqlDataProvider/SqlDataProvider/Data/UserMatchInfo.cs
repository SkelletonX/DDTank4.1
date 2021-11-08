// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserMatchInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class UserMatchInfo : DataObject
  {
    private int _addDayPrestge;
    private int _dailyGameCount;
    private bool _DailyLeagueFirst;
    private int _DailyLeagueLastScore;
    private int _dailyScore;
    private int _dailyWinCount;
    private int _ID;
    private int _maxCount;
    private int _rank;
    private int _restCount;
    private int _totalPrestige;
    private int _userID;
    private int _weeklyGameCount;
    private int _weeklyRanking;
    private int _weeklyScore;
    private int _leagueGrade;
    private int _leagueItemsGet;

    public int addDayPrestge
    {
      get
      {
        return this._addDayPrestge;
      }
      set
      {
        this._addDayPrestge = value;
        this._isDirty = true;
      }
    }

    public int dailyGameCount
    {
      get
      {
        return this._dailyGameCount;
      }
      set
      {
        this._dailyGameCount = value;
        this._isDirty = true;
      }
    }

    public bool DailyLeagueFirst
    {
      get
      {
        return this._DailyLeagueFirst;
      }
      set
      {
        this._DailyLeagueFirst = value;
        this._isDirty = true;
      }
    }

    public int DailyLeagueLastScore
    {
      get
      {
        return this._DailyLeagueLastScore;
      }
      set
      {
        this._DailyLeagueLastScore = value;
        this._isDirty = true;
      }
    }

    public int dailyScore
    {
      get
      {
        return this._dailyScore;
      }
      set
      {
        this._dailyScore = value;
        this._isDirty = true;
      }
    }

    public int dailyWinCount
    {
      get
      {
        return this._dailyWinCount;
      }
      set
      {
        this._dailyWinCount = value;
        this._isDirty = true;
      }
    }

    public int ID
    {
      get
      {
        return this._ID;
      }
      set
      {
        this._ID = value;
        this._isDirty = true;
      }
    }

    public int maxCount
    {
      get
      {
        return 30;
      }
      set
      {
        this._maxCount = 30;
        this._isDirty = true;
      }
    }

    public int rank
    {
      get
      {
        return this._rank;
      }
      set
      {
        this._rank = value;
        this._isDirty = true;
      }
    }

    public int restCount
    {
      get
      {
        return this._restCount;
      }
      set
      {
        this._restCount = value;
        this._isDirty = true;
      }
    }

    public int totalPrestige
    {
      get
      {
        return this._totalPrestige;
      }
      set
      {
        this._totalPrestige = value;
        this._isDirty = true;
      }
    }

    public int UserID
    {
      get
      {
        return this._userID;
      }
      set
      {
        this._userID = value;
        this._isDirty = true;
      }
    }

    public int weeklyGameCount
    {
      get
      {
        return this._weeklyGameCount;
      }
      set
      {
        this._weeklyGameCount = value;
        this._isDirty = true;
      }
    }

    public int weeklyRanking
    {
      get
      {
        return this._weeklyRanking;
      }
      set
      {
        this._weeklyRanking = value;
        this._isDirty = true;
      }
    }

    public int weeklyScore
    {
      get
      {
        return this._weeklyScore;
      }
      set
      {
        this._weeklyScore = value;
        this._isDirty = true;
      }
    }

    public int leagueGrade
    {
      get
      {
        return this._leagueGrade;
      }
      set
      {
        this._leagueGrade = value;
        this._isDirty = true;
      }
    }

    public int leagueItemsGet
    {
      get
      {
        return this._leagueItemsGet;
      }
      set
      {
        this._leagueItemsGet = value;
        this._isDirty = true;
      }
    }
  }
}
