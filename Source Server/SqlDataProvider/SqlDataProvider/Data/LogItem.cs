// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.LogItem
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class LogItem
  {
    public int ApplicationId { get; set; }

    public string BeginProperty { get; set; }

    public string EndProperty { get; set; }

    public DateTime EnterTime { get; set; }

    public int ItemID { get; set; }

    public string ItemName { get; set; }

    public int LineId { get; set; }

    public int Operation { get; set; }

    public int Result { get; set; }

    public int SubId { get; set; }

    public int UserId { get; set; }
  }
}
