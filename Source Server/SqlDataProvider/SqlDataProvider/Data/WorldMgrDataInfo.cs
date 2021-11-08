// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.WorldMgrDataInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using ProtoBuf;
using System.Collections.Generic;

namespace SqlDataProvider.Data
{
  [ProtoContract]
  public class WorldMgrDataInfo
  {
    [ProtoMember(1)]
    public Dictionary<int, ShopFreeCountInfo> ShopFreeCount;
  }
}
