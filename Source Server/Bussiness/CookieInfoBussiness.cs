// Decompiled with JetBrains decompiler
// Type: Bussiness.CookieInfoBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System;
using System.Data;
using System.Data.SqlClient;

namespace Bussiness
{
  public class CookieInfoBussiness : BaseBussiness
  {
    public bool AddCookieInfo(string bdSigUser, string bdSigPortrait, string bdSigSessionKey)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[4]
        {
          new SqlParameter("@BdSigUser", (object) bdSigUser),
          new SqlParameter("@BdSigPortrait", (object) bdSigPortrait),
          new SqlParameter("@BdSigSessionKey", (object) bdSigSessionKey),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[3].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_Cookie_Info_Insert", SqlParameters);
        flag = (int) SqlParameters[3].Value == 0;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool GetFromDbByUser(
      string bdSigUser,
      ref string bdSigPortrait,
      ref string bdSigSessionKey)
    {
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[1]
        {
          new SqlParameter("@BdSigUser", (object) bdSigUser)
        };
        this.db.GetReader(ref ResultDataReader, "SP_Cookie_Info_QueryByUser", SqlParameters);
        while (ResultDataReader.Read())
        {
          bdSigPortrait = ResultDataReader["BdSigPortrait"] == null ? "" : ResultDataReader["BdSigPortrait"].ToString();
          bdSigSessionKey = ResultDataReader["BdSigSessionKey"] == null ? "" : ResultDataReader["BdSigSessionKey"].ToString();
        }
        return !string.IsNullOrEmpty(bdSigPortrait) && !string.IsNullOrEmpty(bdSigSessionKey);
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
        return false;
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
    }
  }
}
