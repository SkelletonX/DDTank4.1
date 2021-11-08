// Decompiled with JetBrains decompiler
// Type: Bussiness.AreaBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Bussiness
{
  public class AreaBussiness : BaseCrossBussiness
  {
    public AreaConfigInfo[] GetAllAreaConfig()
    {
      List<AreaConfigInfo> areaConfigInfoList = new List<AreaConfigInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_AreaConfig_All");
        while (ResultDataReader.Read())
          areaConfigInfoList.Add(this.InitAreaConfigInfo(ResultDataReader));
      }
      catch (Exception ex)
      {
        if (this.log.IsErrorEnabled)
          this.log.Error((object) "InitAreaConfigInfo", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return areaConfigInfoList.ToArray();
    }

    public AreaConfigInfo InitAreaConfigInfo(SqlDataReader dr)
    {
      return new AreaConfigInfo()
      {
        AreaID = (int) dr["AreaID"],
        AreaServer = dr["AreaServer"] == null ? "" : dr["AreaServer"].ToString(),
        AreaName = dr["AreaName"] == null ? "" : dr["AreaName"].ToString(),
        DataSource = dr["DataSource"] == null ? "" : dr["DataSource"].ToString(),
        Catalog = dr["Catalog"] == null ? "" : dr["Catalog"].ToString(),
        UserID = dr["UserID"] == null ? "" : dr["UserID"].ToString(),
        Password = dr["Password"] == null ? "" : dr["Password"].ToString(),
        RequestUrl = dr["RequestUrl"] == null ? "" : dr["RequestUrl"].ToString(),
        CrossChatAllow = (bool) dr["CrossChatAllow"],
        CrossPrivateChat = (bool) dr["CrossPrivateChat"],
        Version = dr["Version"] == null ? "" : dr["Version"].ToString()
      };
    }
  }
}
