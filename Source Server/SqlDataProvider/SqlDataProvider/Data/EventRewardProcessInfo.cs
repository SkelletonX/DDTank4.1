// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.EventRewardProcessInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class EventRewardProcessInfo : DataObject
  {
    private int int_0;
    private int int_1;
    private int int_2;
    private int int_3;

    public int ActiveType
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

    public int AwardGot
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

    public int Conditions
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
