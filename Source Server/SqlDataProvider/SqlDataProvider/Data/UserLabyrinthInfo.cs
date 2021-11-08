// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserLabyrinthInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class UserLabyrinthInfo : DataObject
  {
    private int int_0;
    private int int_1;
    private int int_2;
    private int int_3;
    private bool bool_0;
    private bool bool_1;
    private int int_4;
    private int int_5;
    private int int_6;
    private int int_7;
    private int int_8;
    private int int_9;
    private bool bool_2;
    private bool bool_3;
    private bool bool_4;
    private bool bool_5;
    private DateTime dateTime_0;
    private string string_0;

    public int UserID
    {
      get
      {
        return this.int_0;
      }
      set
      {
        this.int_0 = value;
        this._isDirty = true;
      }
    }

    public int sType
    {
      get
      {
        return this.int_1;
      }
      set
      {
        this.int_1 = value;
        this._isDirty = true;
      }
    }

    public int myProgress
    {
      get
      {
        return this.int_2;
      }
      set
      {
        this.int_2 = value;
        this._isDirty = true;
      }
    }

    public int myRanking
    {
      get
      {
        return this.int_3;
      }
      set
      {
        this.int_3 = value;
        this._isDirty = true;
      }
    }

    public bool completeChallenge
    {
      get
      {
        return this.bool_0;
      }
      set
      {
        this.bool_0 = value;
        this._isDirty = true;
      }
    }

    public bool isDoubleAward
    {
      get
      {
        return this.bool_1;
      }
      set
      {
        this.bool_1 = value;
        this._isDirty = true;
      }
    }

    public int currentFloor
    {
      get
      {
        return this.int_4;
      }
      set
      {
        this.int_4 = value;
        this._isDirty = true;
      }
    }

    public int accumulateExp
    {
      get
      {
        return this.int_5;
      }
      set
      {
        this.int_5 = value;
        this._isDirty = true;
      }
    }

    public int remainTime
    {
      get
      {
        return this.int_6;
      }
      set
      {
        this.int_6 = value;
        this._isDirty = true;
      }
    }

    public int currentRemainTime
    {
      get
      {
        return this.int_7;
      }
      set
      {
        this.int_7 = value;
        this._isDirty = true;
      }
    }

    public int cleanOutAllTime
    {
      get
      {
        return this.int_8;
      }
      set
      {
        this.int_8 = value;
        this._isDirty = true;
      }
    }

    public int cleanOutGold
    {
      get
      {
        return this.int_9;
      }
      set
      {
        this.int_9 = value;
        this._isDirty = true;
      }
    }

    public bool tryAgainComplete
    {
      get
      {
        return this.bool_2;
      }
      set
      {
        this.bool_2 = value;
        this._isDirty = true;
      }
    }

    public bool isInGame
    {
      get
      {
        return this.bool_3;
      }
      set
      {
        this.bool_3 = value;
        this._isDirty = true;
      }
    }

    public bool isCleanOut
    {
      get
      {
        return this.bool_4;
      }
      set
      {
        this.bool_4 = value;
        this._isDirty = true;
      }
    }

    public bool serverMultiplyingPower
    {
      get
      {
        return this.bool_5;
      }
      set
      {
        this.bool_5 = value;
        this._isDirty = true;
      }
    }

    public DateTime LastDate
    {
      get
      {
        return this.dateTime_0;
      }
      set
      {
        this.dateTime_0 = value;
        this._isDirty = true;
      }
    }

    public string ProcessAward
    {
      get
      {
        return this.string_0;
      }
      set
      {
        this.string_0 = value;
        this._isDirty = true;
      }
    }

    public bool isValidDate()
    {
      return this.dateTime_0.Date < DateTime.Now.Date;
    }
  }
}
