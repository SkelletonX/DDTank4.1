// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.AchievementData
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class AchievementData : DataObject
  {
    private bool bool_0;
    private DateTime dateTime_0;
    private int int_0;
    private int int_1;

    public AchievementData()
    {
      this.UserID = 0;
      this.AchievementID = 0;
      this.IsComplete = false;
      this.CompletedDate = DateTime.Now;
    }

    public int AchievementID
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

    public DateTime CompletedDate
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

    public bool IsComplete
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
  }
}
