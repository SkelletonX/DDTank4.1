// Decompiled with JetBrains decompiler
// Type: Bussiness.BaseCrossBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using log4net;
using SqlDataProvider.BaseClass;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Bussiness
{
  public class BaseCrossBussiness : IDisposable
  {
    protected readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected Sql_DbObject db = new Sql_DbObject("AppConfig", "crosszoneString");

    public DataTable GetPage(
      string queryStr,
      string queryWhere,
      int pageCurrent,
      int pageSize,
      string fdShow,
      string fdOreder,
      string fdKey,
      ref int total)
    {
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[8]
        {
          new SqlParameter("@QueryStr", (object) queryStr),
          new SqlParameter("@QueryWhere", (object) queryWhere),
          new SqlParameter("@PageSize", (object) pageSize),
          new SqlParameter("@PageCurrent", (object) pageCurrent),
          new SqlParameter("@FdShow", (object) fdShow),
          new SqlParameter("@FdOrder", (object) fdOreder),
          new SqlParameter("@FdKey", (object) fdKey),
          new SqlParameter("@TotalRow", (object) total)
        };
        SqlParameters[7].Direction = ParameterDirection.Output;
        DataTable dataTable = this.db.GetDataTable(queryStr, "SP_CustomPage", SqlParameters, 120);
        total = (int) SqlParameters[7].Value;
        return dataTable;
      }
      catch (Exception ex)
      {
        if (this.log.IsErrorEnabled)
          this.log.Error((object) "Init", ex);
      }
      return new DataTable(queryStr);
    }

    public void Dispose()
    {
      this.db.Dispose();
      GC.SuppressFinalize((object) this);
    }
  }
}
