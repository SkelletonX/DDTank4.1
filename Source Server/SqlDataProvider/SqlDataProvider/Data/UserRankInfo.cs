// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserRankInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class UserRankInfo : DataObject
  {
    private int _agility;
    private int _attack;
    private DateTime _beginDate;
    private int _damage;
    private int _defence;
    private int _guard;
    private int _hp;
    private int _id;
    private bool _isExit;
    private int _luck;
    private int _userID;
    private string _userRank;
    private int _validate;

    public bool IsValidRank()
    {
      if ((uint) this._validate > 0U)
        return DateTime.Compare(this._beginDate.AddDays((double) this._validate), DateTime.Now) > 0;
      return true;
    }

    public int Agility
    {
      get
      {
        return this._agility;
      }
      set
      {
        this._agility = value;
        this._isDirty = true;
      }
    }

    public int Attack
    {
      get
      {
        return this._attack;
      }
      set
      {
        this._attack = value;
        this._isDirty = true;
      }
    }

    public DateTime BeginDate
    {
      get
      {
        return this._beginDate;
      }
      set
      {
        this._beginDate = value;
        this._isDirty = true;
      }
    }

    public int Damage
    {
      get
      {
        return this._damage;
      }
      set
      {
        this._damage = value;
        this._isDirty = true;
      }
    }

    public int Defence
    {
      get
      {
        return this._defence;
      }
      set
      {
        this._defence = value;
        this._isDirty = true;
      }
    }

    public int Guard
    {
      get
      {
        return this._guard;
      }
      set
      {
        this._guard = value;
        this._isDirty = true;
      }
    }

    public int HP
    {
      get
      {
        return this._hp;
      }
      set
      {
        this._hp = value;
        this._isDirty = true;
      }
    }

    public int ID
    {
      get
      {
        return this._id;
      }
      set
      {
        this._id = value;
        this._isDirty = true;
      }
    }

    public bool IsExit
    {
      get
      {
        return this._isExit;
      }
      set
      {
        this._isExit = value;
        this._isDirty = true;
      }
    }

    public int Luck
    {
      get
      {
        return this._luck;
      }
      set
      {
        this._luck = value;
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

    public string UserRank
    {
      get
      {
        return this._userRank;
      }
      set
      {
        this._userRank = value;
        this._isDirty = true;
      }
    }

    public int Validate
    {
      get
      {
        return this._validate;
      }
      set
      {
        this._validate = value;
        this._isDirty = true;
      }
    }
  }
}
