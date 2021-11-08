// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.RateInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class RateInfo
  {
    public DateTime BeginDay { get; set; }

    public DateTime BeginTime { get; set; }

    public DateTime EndDay { get; set; }

    public DateTime EndTime { get; set; }

    public float Rate { get; set; }

    public int ServerID { get; set; }

    public int Type { get; set; }
  }
}
