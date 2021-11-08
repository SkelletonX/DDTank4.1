// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.RingstationBattleFieldInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class RingstationBattleFieldInfo : DataObject
  {
    public DateTime BattleTime { get; set; }

    public bool DareFlag { get; set; }

    public int ID { get; set; }

    public int Level { get; set; }

    public bool SuccessFlag { get; set; }

    public int UserID { get; set; }

    public string UserName { get; set; }
  }
}
