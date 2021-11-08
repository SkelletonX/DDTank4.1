// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ConsortiaBossInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class ConsortiaBossInfo
  {
    public int ConsortiaID { get; set; }

    public int typeBoss { get; set; }

    public int callBossCount { get; set; }

    public int BossLevel { get; set; }

    public int Blood { get; set; }

    public DateTime LastOpenBoss { get; set; }

    public DateTime BossOpenTime { get; set; }

    public int powerPoint { get; set; }
  }
}
