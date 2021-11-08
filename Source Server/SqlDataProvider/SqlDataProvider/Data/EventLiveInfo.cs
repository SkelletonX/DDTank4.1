// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.EventLiveInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class EventLiveInfo : DataObject
  {
    public int EventID { get; set; }

    public string Description { get; set; }

    public int CondictionType { get; set; }

    public int Condiction_Para1 { get; set; }

    public int Condiction_Para2 { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
  }
}
