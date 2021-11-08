// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.CommunalActiveInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class CommunalActiveInfo
  {
    public int ActiveID { get; set; }

    public string AddPropertyByMoney { get; set; }

    public string AddPropertyByProp { get; set; }

    public DateTime BeginTime { get; set; }

    public int DayMaxScore { get; set; }

    public DateTime EndTime { get; set; }

    public bool IsReset { get; set; }

    public bool IsSendAward { get; set; }

    public int LimitGrade { get; set; }

    public int MinScore { get; set; }
  }
}
