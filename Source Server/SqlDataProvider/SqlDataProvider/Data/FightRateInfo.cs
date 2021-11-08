// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.FightRateInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class FightRateInfo
  {
    public DateTime BeginDay { get; set; }

    public DateTime BeginTime { get; set; }

    public int BoyTemplateID { get; set; }

    public DateTime EndDay { get; set; }

    public DateTime EndTime { get; set; }

    public string EnemyCue { get; set; }

    public int GirlTemplateID { get; set; }

    public int ID { get; set; }

    public string Name { get; set; }

    public int Rate { get; set; }

    public string SelfCue { get; set; }

    public int ServerID { get; set; }
  }
}
