// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ShopFreeCountInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using ProtoBuf;
using System;

namespace SqlDataProvider.Data
{
  [ProtoContract]
  public class ShopFreeCountInfo
  {
    [ProtoMember(1)]
    public int ShopID { get; set; }

    [ProtoMember(2)]
    public int Count { get; set; }

    [ProtoMember(3)]
    public DateTime CreateDate { get; set; }
  }
}
