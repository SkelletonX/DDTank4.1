// Decompiled with JetBrains decompiler
// Type: Bussiness.UserInfoBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System;
using System.Data;
using System.Data.SqlClient;

namespace Bussiness
{
  public class UserInfoBussiness : BaseBussiness
  {
    public bool AddUserInfo(string uid, string userName, string portrait)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[4]
        {
          new SqlParameter("@Uid", (object) uid),
          new SqlParameter("@UserName", (object) userName),
          new SqlParameter("@Portrait", (object) portrait),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[3].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_User_Info_Insert", SqlParameters);
        flag = (int) SqlParameters[3].Value == 0;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool GetFromDbByUid(string uid, ref string userName, ref string portrait)
    {
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[1]
        {
          new SqlParameter("@Uid", (object) uid)
        };
        this.db.GetReader(ref ResultDataReader, "SP_User_Info_QueryByUid", SqlParameters);
        while (ResultDataReader.Read())
        {
          userName = ResultDataReader["UserName"] == null ? "" : ResultDataReader["UserName"].ToString();
          portrait = ResultDataReader["Portrait"] == null ? "" : ResultDataReader["Portrait"].ToString();
        }
        return !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(portrait);
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
