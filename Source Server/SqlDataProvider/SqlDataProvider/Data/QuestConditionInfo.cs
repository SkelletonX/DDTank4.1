// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.QuestConditionInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class QuestConditionInfo
  {
    public int Tagert()
    {
      if (this.CondictionType == 20 && this.Para1 != 3)
        return 0;
      return this.Para2;
    }

    public int CondictionID { get; set; }

    public string CondictionTitle { get; set; }

    public int CondictionType { get; set; }

    public bool isOpitional { get; set; }

    public int Para1 { get; set; }

    public int Para2 { get; set; }

    public int QuestID { get; set; }
  }
}
