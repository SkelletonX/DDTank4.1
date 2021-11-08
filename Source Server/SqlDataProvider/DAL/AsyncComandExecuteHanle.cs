// Decompiled with JetBrains decompiler
// Type: DAL.AsyncComandExecuteHanle
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;
using System.Data.SqlClient;

namespace DAL
{
  public delegate void AsyncComandExecuteHanle(SqlCommand cmd, IAsyncResult result, object state);
}
