// Decompiled with JetBrains decompiler
// Type: Bussiness.ManageBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using Bussiness.CenterService;
using SqlDataProvider.Data;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Bussiness
{
  public class ManageBussiness : BaseBussiness
  {
    private bool ForbidPlayer(
      string userName,
      string nickName,
      int userID,
      DateTime forbidDate,
      bool isExist)
    {
      return this.ForbidPlayer(userName, nickName, userID, forbidDate, isExist, "");
    }

    private bool ForbidPlayer(
      string userName,
      string nickName,
      int userID,
      DateTime forbidDate,
      bool isExist,
      string ForbidReason)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[6]
        {
          new SqlParameter("@UserName", (object) userName),
          new SqlParameter("@NickName", (object) nickName),
          new SqlParameter("@UserID", (object) userID),
          null,
          null,
          null
        };
        SqlParameters[2].Direction = ParameterDirection.InputOutput;
        SqlParameters[3] = new SqlParameter("@ForbidDate", (object) forbidDate);
        SqlParameters[4] = new SqlParameter("@IsExist", (object) isExist);
        SqlParameters[5] = new SqlParameter("@ForbidReason", (object) ForbidReason);
        this.db.RunProcedure("SP_Admin_ForbidUser", SqlParameters);
        userID = (int) SqlParameters[2].Value;
        if (userID <= 0)
          return flag;
        flag = true;
        if (!isExist)
          this.KitoffUser(userID, "Você Foi Banido!");
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool ForbidPlayerByNickName(string nickName, DateTime date, bool isExist)
    {
      return this.ForbidPlayer("", nickName, 0, date, isExist);
    }

    public bool ForbidPlayerByNickName(
      string nickName,
      DateTime date,
      bool isExist,
      string ForbidReason)
    {
      return this.ForbidPlayer("", nickName, 0, date, isExist, ForbidReason);
    }

    public bool ForbidPlayerByUserID(int userID, DateTime date, bool isExist)
    {
      return this.ForbidPlayer("", "", userID, date, isExist);
    }

    public bool ForbidPlayerByUserID(int userID, DateTime date, bool isExist, string ForbidReason)
    {
      return this.ForbidPlayer("", "", userID, date, isExist, ForbidReason);
    }

    public bool ForbidPlayerByUserName(string userName, DateTime date, bool isExist)
    {
      return this.ForbidPlayer(userName, "", 0, date, isExist);
    }

    public bool ForbidPlayerByUserName(
      string userName,
      DateTime date,
      bool isExist,
      string ForbidReason)
    {
      return this.ForbidPlayer(userName, "", 0, date, isExist, ForbidReason);
    }

    public int GetConfigState(int type)
    {
      try
      {
        using (CenterServiceClient centerServiceClient = new CenterServiceClient())
          return centerServiceClient.GetConfigState(type);
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (GetConfigState), ex);
      }
      return 2;
    }

    public int KitoffUser(int id, string msg)
    {
      try
      {
        using (CenterServiceClient centerServiceClient = new CenterServiceClient())
          return centerServiceClient.KitoffUser(id, msg) ? 0 : 3;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (KitoffUser), ex);
        return 1;
      }
    }

    public int KitoffUserByNickName(string name, string msg)
    {
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        PlayerInfo singleByNickName = playerBussiness.GetUserSingleByNickName(name);
        if (singleByNickName == null)
          return 2;
        return this.KitoffUser(singleByNickName.ID, msg);
      }
    }

    public int KitoffUserByUserName(string name, string msg)
    {
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        PlayerInfo singleByUserName = playerBussiness.GetUserSingleByUserName(name);
        if (singleByUserName == null)
          return 2;
        return this.KitoffUser(singleByUserName.ID, msg);
      }
    }

    public bool Reload(string type)
    {
      try
      {
        using (CenterServiceClient centerServiceClient = new CenterServiceClient())
          return centerServiceClient.Reload(type);
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (Reload), ex);
      }
      return false;
    }

    public bool ReLoadServerList()
    {
      bool flag = false;
      try
      {
        using (CenterServiceClient centerServiceClient = new CenterServiceClient())
        {
          if (centerServiceClient.ReLoadServerList())
            flag = true;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (ReLoadServerList), ex);
      }
      return flag;
    }

    public bool SystemNotice(string msg)
    {
      bool flag = false;
      try
      {
        if (string.IsNullOrEmpty(msg))
          return flag;
        using (CenterServiceClient centerServiceClient = new CenterServiceClient())
        {
          if (centerServiceClient.SystemNotice(msg))
            flag = true;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (SystemNotice), ex);
      }
      return flag;
    }

    public bool UpdateConfigState(int type, bool state)
    {
      try
      {
        using (CenterServiceClient centerServiceClient = new CenterServiceClient())
          return centerServiceClient.UpdateConfigState(type, state);
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (UpdateConfigState), ex);
      }
      return false;
    }
  }
}
