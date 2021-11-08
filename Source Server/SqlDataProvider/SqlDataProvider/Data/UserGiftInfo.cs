// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserGiftInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class UserGiftInfo
  {
    public int ID { get; set; }

    public int SenderID { get; set; }

    public int ReceiverID { get; set; }

    public int TemplateID { get; set; }

    public int Count { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime LastUpdate { get; set; }
  }
}
