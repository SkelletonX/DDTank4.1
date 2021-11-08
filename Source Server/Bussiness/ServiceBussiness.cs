// Decompiled with JetBrains decompiler
// Type: Bussiness.ServiceBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Bussiness
{
  public class ServiceBussiness : BaseBussiness
  {
    public FightRateInfo[] GetFightRate(int serverId)
    {
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      List<FightRateInfo> fightRateInfoList = new List<FightRateInfo>();
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[1]
        {
          new SqlParameter("@ServerID", (object) serverId)
        };
        this.db.GetReader(ref ResultDataReader, "SP_Fight_Rate", SqlParameters);
        if (ResultDataReader.Read())
        {
          FightRateInfo fightRateInfo = new FightRateInfo()
          {
            ID = (int) ResultDataReader["ID"],
            ServerID = (int) ResultDataReader["ServerID"],
            Rate = (int) ResultDataReader["Rate"],
            BeginDay = (DateTime) ResultDataReader["BeginDay"],
            EndDay = (DateTime) ResultDataReader["EndDay"],
            BeginTime = (DateTime) ResultDataReader["BeginTime"],
            EndTime = (DateTime) ResultDataReader["EndTime"],
            SelfCue = ResultDataReader["SelfCue"] == null ? "" : ResultDataReader["SelfCue"].ToString(),
            EnemyCue = ResultDataReader["EnemyCue"] == null ? "" : ResultDataReader["EnemyCue"].ToString(),
            BoyTemplateID = (int) ResultDataReader["BoyTemplateID"],
            GirlTemplateID = (int) ResultDataReader["GirlTemplateID"],
            Name = ResultDataReader["Name"] == null ? "" : ResultDataReader["Name"].ToString()
          };
          fightRateInfoList.Add(fightRateInfo);
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (GetFightRate), ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return fightRateInfoList.ToArray();
    }

    public string GetGameEdition()
    {
      string empty = string.Empty;
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_Server_Edition");
        if (ResultDataReader.Read())
          return ResultDataReader["value"] == null ? "" : ResultDataReader["value"].ToString();
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return empty;
    }

    public ArrayList GetRate(int serverId)
    {
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        ArrayList arrayList = new ArrayList();
        SqlParameter[] SqlParameters = new SqlParameter[1]
        {
          new SqlParameter("@ServerID", (object) serverId)
        };
        this.db.GetReader(ref ResultDataReader, "SP_Rate", SqlParameters);
        while (ResultDataReader.Read())
        {
          RateInfo rateInfo = new RateInfo()
          {
            ServerID = (int) ResultDataReader["ServerID"],
            Rate = (float) ((Decimal) ResultDataReader["Rate"]),
            BeginDay = (DateTime) ResultDataReader["BeginDay"],
            EndDay = (DateTime) ResultDataReader["EndDay"],
            BeginTime = (DateTime) ResultDataReader["BeginTime"],
            EndTime = (DateTime) ResultDataReader["EndTime"],
            Type = (int) ResultDataReader["Type"]
          };
          arrayList.Add((object) rateInfo);
        }
        arrayList.TrimToSize();
        return arrayList;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "GetRates", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return (ArrayList) null;
    }

    public RateInfo GetRateWithType(int serverId, int type)
    {
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[2]
        {
          new SqlParameter("@ServerID", (object) serverId),
          new SqlParameter("@Type", (object) type)
        };
        this.db.GetReader(ref ResultDataReader, "SP_Rate_WithType", SqlParameters);
        if (ResultDataReader.Read())
          return new RateInfo()
          {
            ServerID = (int) ResultDataReader["ServerID"],
            Type = type,
            Rate = (float) ResultDataReader["Rate"],
            BeginDay = (DateTime) ResultDataReader["BeginDay"],
            EndDay = (DateTime) ResultDataReader["EndDay"],
            BeginTime = (DateTime) ResultDataReader["BeginTime"],
            EndTime = (DateTime) ResultDataReader["EndTime"]
          };
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) ("GetRate type: " + (object) type), ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return (RateInfo) null;
    }

    public RecordInfo GetRecordInfo(DateTime date, int SaveRecordSecond)
    {
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[2]
        {
          new SqlParameter("@Date", (object) date.ToString("yyyy-MM-dd HH:mm:ss")),
          new SqlParameter("@Second", (object) SaveRecordSecond)
        };
        this.db.GetReader(ref ResultDataReader, "SP_Server_Record", SqlParameters);
        if (ResultDataReader.Read())
          return new RecordInfo()
          {
            ActiveExpendBoy = (int) ResultDataReader["ActiveExpendBoy"],
            ActiveExpendGirl = (int) ResultDataReader["ActiveExpendGirl"],
            ActviePayBoy = (int) ResultDataReader["ActviePayBoy"],
            ActviePayGirl = (int) ResultDataReader["ActviePayGirl"],
            ExpendBoy = (int) ResultDataReader["ExpendBoy"],
            ExpendGirl = (int) ResultDataReader["ExpendGirl"],
            OnlineBoy = (int) ResultDataReader["OnlineBoy"],
            OnlineGirl = (int) ResultDataReader["OnlineGirl"],
            TotalBoy = (int) ResultDataReader["TotalBoy"],
            TotalGirl = (int) ResultDataReader["TotalGirl"],
            ActiveOnlineBoy = (int) ResultDataReader["ActiveOnlineBoy"],
            ActiveOnlineGirl = (int) ResultDataReader["ActiveOnlineGirl"]
          };
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return (RecordInfo) null;
    }

    public Dictionary<string, string> GetServerConfig()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_Server_Config");
        while (ResultDataReader.Read())
        {
          if (!dictionary.ContainsKey(ResultDataReader["Name"].ToString()))
            dictionary.Add(ResultDataReader["Name"].ToString(), ResultDataReader["Value"].ToString());
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (GetServerConfig), ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return dictionary;
    }

    public SqlDataProvider.Data.ServerInfo[] GetServerList()
    {
      List<SqlDataProvider.Data.ServerInfo> serverInfoList = new List<SqlDataProvider.Data.ServerInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_Service_List");
        while (ResultDataReader.Read())
        {
          SqlDataProvider.Data.ServerInfo serverInfo = new SqlDataProvider.Data.ServerInfo()
          {
            ID = (int) ResultDataReader["ID"],
            IP = ResultDataReader["IP"].ToString(),
            Name = ResultDataReader["Name"].ToString(),
            Online = (int) ResultDataReader["Online"],
            Port = (int) ResultDataReader["Port"],
            Remark = ResultDataReader["Remark"].ToString(),
            Room = (int) ResultDataReader["Room"],
            State = (int) ResultDataReader["State"],
            Total = (int) ResultDataReader["Total"],
            RSA = ResultDataReader["RSA"].ToString(),
            MustLevel = (int) ResultDataReader["MustLevel"],
            LowestLevel = (int) ResultDataReader["LowestLevel"]
          };
          serverInfoList.Add(serverInfo);
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return serverInfoList.ToArray();
    }

    public ServerProperty GetServerPropertyByKey(string key)
    {
      ServerProperty serverProperty = (ServerProperty) null;
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[1]
        {
          new SqlParameter("@Key", (object) key)
        };
        this.db.GetReader(ref ResultDataReader, "SP_Server_Config_Single", SqlParameters);
        while (ResultDataReader.Read())
          serverProperty = new ServerProperty()
          {
            Key = ResultDataReader["Name"].ToString(),
            Value = ResultDataReader["Value"].ToString()
          };
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "GetServerConfig", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return serverProperty;
    }

    public SqlDataProvider.Data.ServerInfo[] GetServiceByIP(string IP)
    {
      List<SqlDataProvider.Data.ServerInfo> serverInfoList = new List<SqlDataProvider.Data.ServerInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[1]
        {
          new SqlParameter("@IP", SqlDbType.NVarChar, 50)
        };
        SqlParameters[0].Value = (object) IP;
        this.db.GetReader(ref ResultDataReader, "SP_Service_ListByIP", SqlParameters);
        while (ResultDataReader.Read())
        {
          SqlDataProvider.Data.ServerInfo serverInfo = new SqlDataProvider.Data.ServerInfo()
          {
            ID = (int) ResultDataReader["ID"],
            IP = ResultDataReader[nameof (IP)].ToString(),
            Name = ResultDataReader["Name"].ToString(),
            Online = (int) ResultDataReader["Online"],
            Port = (int) ResultDataReader["Port"],
            Remark = ResultDataReader["Remark"].ToString(),
            Room = (int) ResultDataReader["Room"],
            State = (int) ResultDataReader["State"],
            Total = (int) ResultDataReader["Total"],
            RSA = ResultDataReader["RSA"].ToString()
          };
          serverInfoList.Add(serverInfo);
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return serverInfoList.ToArray();
    }

    public SqlDataProvider.Data.ServerInfo GetServiceSingle(int ID)
    {
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[1]
        {
          new SqlParameter("@ID", SqlDbType.Int, 4)
        };
        SqlParameters[0].Value = (object) ID;
        this.db.GetReader(ref ResultDataReader, "SP_Service_Single", SqlParameters);
        if (ResultDataReader.Read())
          return new SqlDataProvider.Data.ServerInfo()
          {
            ID = (int) ResultDataReader[nameof (ID)],
            IP = ResultDataReader["IP"].ToString(),
            Name = ResultDataReader["Name"].ToString(),
            Online = (int) ResultDataReader["Online"],
            Port = (int) ResultDataReader["Port"],
            Remark = ResultDataReader["Remark"].ToString(),
            Room = (int) ResultDataReader["Room"],
            State = (int) ResultDataReader["State"],
            Total = (int) ResultDataReader["Total"],
            RSA = ResultDataReader["RSA"].ToString(),
            NewerServer = (bool) ResultDataReader["NewerServer"],
            ZoneId = (int) ResultDataReader["ZoneId"],
            ZoneName = ResultDataReader["ZoneName"].ToString()
          };
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return (SqlDataProvider.Data.ServerInfo) null;
    }

    public bool UpdateRSA(int ID, string RSA)
    {
      bool flag = false;
      try
      {
        flag = this.db.RunProcedure("SP_Service_UpdateRSA", new SqlParameter[2]
        {
          new SqlParameter("@ID", (object) ID),
          new SqlParameter("@RSA", (object) RSA)
        });
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpdateServerPropertyByKey(string key, string value)
    {
      bool flag = false;
      try
      {
        flag = this.db.RunProcedure("SP_Server_Config_Update", new SqlParameter[2]
        {
          new SqlParameter("@Key", (object) key),
          new SqlParameter("@Value", (object) value)
        });
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpdateService(SqlDataProvider.Data.ServerInfo info)
    {
      bool flag = false;
      try
      {
        flag = this.db.RunProcedure("SP_Service_Update", new SqlParameter[3]
        {
          new SqlParameter("@ID", (object) info.ID),
          new SqlParameter("@Online", (object) info.Online),
          new SqlParameter("@State", (object) info.State)
        });
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }
  }
}
