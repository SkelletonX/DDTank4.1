// Decompiled with JetBrains decompiler
// Type: Bussiness.ActiveBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Bussiness
{
  public class ActiveBussiness : BaseCrossBussiness
  {
    public ActiveInfo[] GetAllActives()
    {
      List<ActiveInfo> activeInfoList = new List<ActiveInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_Active_All");
        while (ResultDataReader.Read())
          activeInfoList.Add(this.InitActiveInfo(ResultDataReader));
      }
      catch (Exception ex)
      {
        if (this.log.IsErrorEnabled)
          this.log.Error((object) "Init", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return activeInfoList.ToArray();
    }

    public ActiveConvertItemInfo[] GetSingleActiveConvertItems(int activeID)
    {
      List<ActiveConvertItemInfo> activeConvertItemInfoList = new List<ActiveConvertItemInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[1]
        {
          new SqlParameter("@ID", SqlDbType.Int, 4)
        };
        SqlParameters[0].Value = (object) activeID;
        this.db.GetReader(ref ResultDataReader, "SP_Active_Convert_Item_Info_Single", SqlParameters);
        while (ResultDataReader.Read())
          activeConvertItemInfoList.Add(this.InitActiveConvertItemInfo(ResultDataReader));
      }
      catch (Exception ex)
      {
        if (this.log.IsErrorEnabled)
          this.log.Error((object) "Init", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return activeConvertItemInfoList.ToArray();
    }

    public ActiveInfo GetSingleActives(int activeID)
    {
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[1]
        {
          new SqlParameter("@ID", SqlDbType.Int, 4)
        };
        SqlParameters[0].Value = (object) activeID;
        this.db.GetReader(ref ResultDataReader, "SP_Active_Single", SqlParameters);
        if (ResultDataReader.Read())
          return this.InitActiveInfo(ResultDataReader);
      }
      catch (Exception ex)
      {
        if (this.log.IsErrorEnabled)
          this.log.Error((object) "Init", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return (ActiveInfo) null;
    }

    public ActiveConvertItemInfo InitActiveConvertItemInfo(SqlDataReader reader)
    {
      return new ActiveConvertItemInfo()
      {
        ID = (int) reader["ID"],
        ActiveID = (int) reader["ActiveID"],
        TemplateID = (int) reader["TemplateID"],
        ItemType = (int) reader["ItemType"],
        ItemCount = (int) reader["ItemCount"],
        LimitValue = (int) reader["LimitValue"],
        IsBind = (bool) reader["IsBind"],
        ValidDate = (int) reader["ValidDate"]
      };
    }

    public ActiveInfo InitActiveInfo(SqlDataReader reader)
    {
      ActiveInfo activeInfo = new ActiveInfo()
      {
        ActiveID = (int) reader["ActiveID"],
        Description = reader["Description"] == null ? "" : reader["Description"].ToString(),
        Content = reader["Content"] == null ? "" : reader["Content"].ToString(),
        AwardContent = reader["AwardContent"] == null ? "" : reader["AwardContent"].ToString(),
        HasKey = (int) reader["HasKey"]
      };
      if (!string.IsNullOrEmpty(reader["EndDate"].ToString()))
        activeInfo.EndDate = new DateTime?((DateTime) reader["EndDate"]);
      activeInfo.IsOnly = (int) reader["IsOnly"];
      activeInfo.StartDate = (DateTime) reader["StartDate"];
      activeInfo.Title = reader["Title"].ToString();
      activeInfo.Type = (int) reader["Type"];
      activeInfo.ActiveType = (int) reader["ActiveType"];
      activeInfo.ActionTimeContent = reader["ActionTimeContent"] == null ? "" : reader["ActionTimeContent"].ToString();
      activeInfo.IsAdvance = (bool) reader["IsAdvance"];
      activeInfo.GoodsExchangeTypes = reader["GoodsExchangeTypes"] == null ? "" : reader["GoodsExchangeTypes"].ToString();
      activeInfo.GoodsExchangeNum = reader["GoodsExchangeNum"] == null ? "" : reader["GoodsExchangeNum"].ToString();
      activeInfo.limitType = reader["limitType"] == null ? "" : reader["limitType"].ToString();
      activeInfo.limitValue = reader["limitValue"] == null ? "" : reader["limitValue"].ToString();
      activeInfo.IsShow = (bool) reader["IsShow"];
      activeInfo.IconID = (int) reader["IconID"];
      return activeInfo;
    }
  }
}
