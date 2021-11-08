// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.AchievementProcessInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class AchievementProcessInfo : DataObject
  {
    private int int_0;
    private int int_1;

    public AchievementProcessInfo()
    {
    }

    public AchievementProcessInfo(int type, int value)
    {
      this.CondictionType = type;
      this.Value = value;
    }

    public int CondictionType
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

    public int Value
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
  }
}
