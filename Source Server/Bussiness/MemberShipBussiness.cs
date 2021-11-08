// Decompiled with JetBrains decompiler
// Type: Bussiness.MemberShipBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using log4net;
using SqlDataProvider.BaseClass;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Bussiness
{
  public class MemberShipBussiness : IDisposable
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected Sql_DbObject db = new Sql_DbObject("AppConfig", "membershipDb");

    public bool CreateUsername(
      string applicationname,
      string username,
      string password,
      string email,
      string passwordformat,
      string passwordsalt,
      bool usersex)
    {
      SqlParameter[] SqlParameters = new SqlParameter[8]
      {
        new SqlParameter("@ApplicationName", (object) applicationname),
        new SqlParameter("@UserName", (object) username),
        new SqlParameter("@password", (object) password),
        new SqlParameter("@email", (object) email),
        new SqlParameter("@PasswordFormat", (object) passwordformat),
        new SqlParameter("@PasswordSalt", (object) passwordsalt),
        new SqlParameter("@UserSex", (object) usersex),
        new SqlParameter("@UserId", SqlDbType.Int)
      };
      SqlParameters[7].Direction = ParameterDirection.Output;
      bool flag = this.db.RunProcedure("Mem_Users_CreateUser", SqlParameters);
      if (flag)
        flag = (int) SqlParameters[7].Value > 0;
      return flag;
    }

    public bool CheckAdmin(string username)
    {
      SqlParameter[] SqlParameters = new SqlParameter[2]
      {
        new SqlParameter("@UserName", (object) username),
        new SqlParameter("@UserCOUNT", SqlDbType.Int)
      };
      SqlParameters[1].Direction = ParameterDirection.Output;
      this.db.RunProcedure("Mem_UserInfo_Addmin", SqlParameters);
      return (int) SqlParameters[1].Value > 0;
    }

    public int CheckPoint(string username)
    {
      SqlParameter[] SqlParameters = new SqlParameter[2]
      {
        new SqlParameter("@UserName", (object) username),
        new SqlParameter("@Point", SqlDbType.Int)
      };
      SqlParameters[1].Direction = ParameterDirection.Output;
      this.db.RunProcedure("Mem_User_Point", SqlParameters);
      return (int) SqlParameters[1].Value;
    }

    public bool CheckUsername(string username, string password)
    {
      SqlParameter[] SqlParameters = new SqlParameter[3]
      {
        new SqlParameter("@UserName", (object) username),
        new SqlParameter("@password", (object) password),
        new SqlParameter("@UserId", SqlDbType.Int)
      };
      SqlParameters[2].Direction = ParameterDirection.Output;
      this.db.RunProcedure("Check_User", SqlParameters);
      int result = 0;
      int.TryParse(SqlParameters[2].Value.ToString(), out result);
      return result > 0;
    }

    public void Dispose()
    {
      this.db.Dispose();
      GC.SuppressFinalize((object) this);
    }

    public bool ExistsUsername(string username)
    {
      SqlParameter[] SqlParameters = new SqlParameter[2]
      {
        new SqlParameter("@UserName", (object) username),
        new SqlParameter("@UserCOUNT", SqlDbType.Int)
      };
      SqlParameters[1].Direction = ParameterDirection.Output;
      this.db.RunProcedure("Mem_UserInfo_SearchName", SqlParameters);
      return (int) SqlParameters[1].Value > 0;
    }

    public eStoreInfo[] GetAlleStore()
    {
      List<eStoreInfo> eStoreInfoList = new List<eStoreInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_eStore_All");
        while (ResultDataReader.Read())
        {
          eStoreInfo eStoreInfo = new eStoreInfo()
          {
            StoreID = (int) ResultDataReader["StoreID"],
            TemplateID = (int) ResultDataReader["TemplateID"],
            PriceValue = (int) ResultDataReader["PriceValue"],
            StrengthenLevel = (int) ResultDataReader["StrengthenLevel"],
            AttackCompose = (int) ResultDataReader["AttackCompose"],
            AgilityCompose = (int) ResultDataReader["AgilityCompose"],
            DefendCompose = (int) ResultDataReader["DefendCompose"],
            LuckCompose = (int) ResultDataReader["LuckCompose"],
            IsBinds = (bool) ResultDataReader["IsBinds"],
            ValidDate = (int) ResultDataReader["ValidDate"]
          };
          eStoreInfoList.Add(eStoreInfo);
        }
      }
      catch (Exception ex)
      {
        if (MemberShipBussiness.log.IsErrorEnabled)
          MemberShipBussiness.log.Error((object) "eStore", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return eStoreInfoList.ToArray();
    }

    public eStoreInfo[] GetAlleStoreByDesc()
    {
      List<eStoreInfo> eStoreInfoList = new List<eStoreInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_eStore_Desc");
        while (ResultDataReader.Read())
        {
          eStoreInfo eStoreInfo = new eStoreInfo()
          {
            StoreID = (int) ResultDataReader["StoreID"],
            TemplateID = (int) ResultDataReader["TemplateID"],
            PriceValue = (int) ResultDataReader["PriceValue"],
            StrengthenLevel = (int) ResultDataReader["StrengthenLevel"],
            AttackCompose = (int) ResultDataReader["AttackCompose"],
            AgilityCompose = (int) ResultDataReader["AgilityCompose"],
            DefendCompose = (int) ResultDataReader["DefendCompose"],
            LuckCompose = (int) ResultDataReader["LuckCompose"],
            IsBinds = (bool) ResultDataReader["IsBinds"],
            ValidDate = (int) ResultDataReader["ValidDate"]
          };
          eStoreInfoList.Add(eStoreInfo);
        }
      }
      catch (Exception ex)
      {
        if (MemberShipBussiness.log.IsErrorEnabled)
          MemberShipBussiness.log.Error((object) "eStore", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return eStoreInfoList.ToArray();
    }

    public eStoreInfo[] GetAlleStoreSale()
    {
      List<eStoreInfo> eStoreInfoList = new List<eStoreInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_eStore_Desc_Sale");
        while (ResultDataReader.Read())
        {
          eStoreInfo eStoreInfo = new eStoreInfo()
          {
            StoreID = (int) ResultDataReader["StoreID"],
            TemplateID = (int) ResultDataReader["TemplateID"],
            PriceValue = (int) ResultDataReader["PriceValue1"],
            StrengthenLevel = (int) ResultDataReader["StrengthenLevel"],
            AttackCompose = (int) ResultDataReader["AttackCompose"],
            AgilityCompose = (int) ResultDataReader["AgilityCompose"],
            DefendCompose = (int) ResultDataReader["DefendCompose"],
            LuckCompose = (int) ResultDataReader["LuckCompose"],
            IsBinds = (bool) ResultDataReader["IsBinds"],
            ValidDate = (int) ResultDataReader["ValidDate"],
            OldPriceValue = (int) ResultDataReader["PriceValue"]
          };
          eStoreInfoList.Add(eStoreInfo);
        }
      }
      catch (Exception ex)
      {
        if (MemberShipBussiness.log.IsErrorEnabled)
          MemberShipBussiness.log.Error((object) "SP_eStore_Desc_Sale", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return eStoreInfoList.ToArray();
    }

    public eStoreInfo[] GetAlleStoreTopBuy()
    {
      List<eStoreInfo> eStoreInfoList = new List<eStoreInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_eStore_Desc_Buy");
        while (ResultDataReader.Read())
        {
          eStoreInfo eStoreInfo = new eStoreInfo()
          {
            StoreID = (int) ResultDataReader["StoreID"],
            TemplateID = (int) ResultDataReader["TemplateID"],
            PriceValue = (int) ResultDataReader["PriceValue"],
            StrengthenLevel = (int) ResultDataReader["StrengthenLevel"],
            AttackCompose = (int) ResultDataReader["AttackCompose"],
            AgilityCompose = (int) ResultDataReader["AgilityCompose"],
            DefendCompose = (int) ResultDataReader["DefendCompose"],
            LuckCompose = (int) ResultDataReader["LuckCompose"],
            IsBinds = (bool) ResultDataReader["IsBinds"],
            ValidDate = (int) ResultDataReader["ValidDate"]
          };
          eStoreInfoList.Add(eStoreInfo);
        }
      }
      catch (Exception ex)
      {
        if (MemberShipBussiness.log.IsErrorEnabled)
          MemberShipBussiness.log.Error((object) "SP_eStore_Desc_Buy", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return eStoreInfoList.ToArray();
    }

    public bool RemovePoint(string username, int Point)
    {
      SqlParameter[] SqlParameters = new SqlParameter[3]
      {
        new SqlParameter("@UserName", (object) username),
        new SqlParameter("@Point", (object) Point),
        new SqlParameter("@Result", SqlDbType.Int)
      };
      SqlParameters[2].Direction = ParameterDirection.ReturnValue;
      this.db.RunProcedure("Mem_User_Remove_Point", SqlParameters);
      return (int) SqlParameters[2].Value == 0;
    }
  }
}
