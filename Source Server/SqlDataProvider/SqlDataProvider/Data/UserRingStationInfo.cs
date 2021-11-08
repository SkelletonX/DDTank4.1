// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserRingStationInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class UserRingStationInfo : DataObject
  {
    private int _BaseDamage;
    private int _BaseEnergy;
    private int _BaseGuard;
    private int _buyCount;
    private int _ChallengeNum;
    private DateTime _ChallengeTime;
    private int _ID;
    private DateTime _LastDate;
    private bool _OnFight;
    private PlayerInfo _playerInfo;
    private int _Rank;
    private string _signMsg;
    private int _Total;
    private int _UserID;
    private int _WeaponID;

    public bool CanChallenge()
    {
      return (600000 - (int) (DateTime.Now - this._ChallengeTime).TotalMilliseconds) / 10 / 60 / 1000 <= 0;
    }

    public int BaseDamage
    {
      get
      {
        return this._BaseDamage;
      }
      set
      {
        this._BaseDamage = value;
        this._isDirty = true;
      }
    }

    public int BaseEnergy
    {
      get
      {
        return this._BaseEnergy;
      }
      set
      {
        this._BaseEnergy = value;
        this._isDirty = true;
      }
    }

    public int BaseGuard
    {
      get
      {
        return this._BaseGuard;
      }
      set
      {
        this._BaseGuard = value;
        this._isDirty = true;
      }
    }

    public int buyCount
    {
      get
      {
        return this._buyCount;
      }
      set
      {
        this._buyCount = value;
        this._isDirty = true;
      }
    }

    public int ChallengeNum
    {
      get
      {
        return this._ChallengeNum;
      }
      set
      {
        this._ChallengeNum = value;
        this._isDirty = true;
      }
    }

    public DateTime ChallengeTime
    {
      get
      {
        return this._ChallengeTime;
      }
      set
      {
        this._ChallengeTime = value;
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

    public PlayerInfo Info
    {
      get
      {
        return this._playerInfo;
      }
      set
      {
        this._playerInfo = value;
      }
    }

    public DateTime LastDate
    {
      get
      {
        return this._LastDate;
      }
      set
      {
        this._LastDate = value;
        this._isDirty = true;
      }
    }

    public bool OnFight
    {
      get
      {
        return this._OnFight;
      }
      set
      {
        this._OnFight = value;
      }
    }

    public int Rank
    {
      get
      {
        return this._Rank;
      }
      set
      {
        this._Rank = value;
        this._isDirty = true;
      }
    }

    public string signMsg
    {
      get
      {
        return this._signMsg;
      }
      set
      {
        this._signMsg = value;
        this._isDirty = true;
      }
    }

    public int Total
    {
      get
      {
        return this._Total;
      }
      set
      {
        this._Total = value;
        this._isDirty = true;
      }
    }

    public int UserID
    {
      get
      {
        return this._UserID;
      }
      set
      {
        this._UserID = value;
        this._isDirty = true;
      }
    }

    public int WeaponID
    {
      get
      {
        return this._WeaponID;
      }
      set
      {
        this._WeaponID = value;
        this._isDirty = true;
      }
    }
  }
}
