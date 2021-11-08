// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.HotSpringRoomInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class HotSpringRoomInfo
  {
    public bool CanEnter()
    {
      return this.curCount < this.maxCount;
    }

    public int curCount { get; set; }

    public int effectiveTime { get; set; }

    public DateTime endTime { get; set; }

    public int maxCount { get; set; }

    public int playerID { get; set; }

    public string playerName { get; set; }

    public int roomID { get; set; }

    public string roomIntroduction { get; set; }

    public string roomName { get; set; }

    public int roomNumber { get; set; }

    public string roomPassword { get; set; }

    public int roomType { get; set; }

    public DateTime startTime { get; set; }
  }
}
