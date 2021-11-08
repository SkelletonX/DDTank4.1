// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.LogFight
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class LogFight
  {
    public int ApplicationId { get; set; }

    public int FightType { get; set; }

    public int LineId { get; set; }

    public int MapId { get; set; }

    public DateTime PlayBegin { get; set; }

    public DateTime PlayEnd { get; set; }

    public string PlayResult { get; set; }

    public int RoomType { get; set; }

    public int SubId { get; set; }

    public int UserCount { get; set; }

    public string Users { get; set; }
  }
}
