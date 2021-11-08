// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.DropInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class DropInfo
  {
    public DropInfo(int id, int time, int count, int maxCount)
    {
      this.ID = id;
      this.Time = time;
      this.Count = count;
      this.MaxCount = maxCount;
    }

    public int Count { get; set; }

    public int ID { get; set; }

    public int MaxCount { get; set; }

    public int Time { get; set; }
  }
}
