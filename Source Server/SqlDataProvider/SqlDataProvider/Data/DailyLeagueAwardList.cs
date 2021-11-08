// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.DailyLeagueAwardList
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System.Collections.Generic;

namespace SqlDataProvider.Data
{
  public class DailyLeagueAwardList
  {
    public int Class { get; set; }

    public List<DailyLeagueAwardItems> AwardLists { get; set; }

    public int Grade { get; set; }

    public int Score { get; set; }

    public int Rank { get; set; }
  }
}
