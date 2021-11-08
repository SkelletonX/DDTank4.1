// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ConsortiaEventInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class ConsortiaEventInfo
  {
    public int ID { get; set; }

    public int ConsortiaID { get; set; }

    public DateTime Date { get; set; }

    public int Type { get; set; }

    public string NickName { get; set; }

    public int EventValue { get; set; }

    public string ManagerName { get; set; }

    public bool IsExist { get; set; }
  }
}
