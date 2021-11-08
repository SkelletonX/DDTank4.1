// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.SubActiveInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class SubActiveInfo
  {
    public int ActiveID { get; set; }

    public string ActiveInfo { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime EndTime { get; set; }

    public int ID { get; set; }

    public bool IsContinued { get; set; }

    public bool IsOpen { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime StartTime { get; set; }

    public int SubID { get; set; }
  }
}
