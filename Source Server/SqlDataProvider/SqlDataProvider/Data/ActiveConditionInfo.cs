// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ActiveConditionInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class ActiveConditionInfo
  {
    public int ActiveID { get; set; }

    public string AwardId { get; set; }

    public int Condition { get; set; }

    public int Conditiontype { get; set; }

    public DateTime EndTime { get; set; }

    public int ID { get; set; }

    public bool IsMult { get; set; }

    public string LimitGrade { get; set; }

    public DateTime StartTime { get; set; }
  }
}
