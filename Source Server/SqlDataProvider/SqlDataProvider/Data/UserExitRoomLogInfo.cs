// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserExitRoomLogInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class UserExitRoomLogInfo
  {
    public int UserID { get; set; }

    public int TotalExitTime { get; set; }

    public DateTime LastLogout { get; set; }

    public DateTime TimeBlock { get; set; }

    public bool IsNoticed { get; set; }

    public bool CanNotice
    {
      get
      {
        if (this.TotalExitTime <= 3 || this.IsNoticed)
          return false;
        this.IsNoticed = true;
        return true;
      }
    }
  }
}
