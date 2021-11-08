// Decompiled with JetBrains decompiler
// Type: Bussiness.ConsortiaBussiness
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
  public class ConsortiaBussiness : BaseBussiness
  {
    public bool AddAndUpdateConsortiaEuqipControl(
      ConsortiaEquipControlInfo info,
      int userID,
      ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[6]
        {
          new SqlParameter("@ConsortiaID", (object) info.ConsortiaID),
          new SqlParameter("@Level", (object) info.Level),
          new SqlParameter("@Type", (object) info.Type),
          new SqlParameter("@Riches", (object) info.Riches),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[5].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_Consortia_Equip_Control_Add", SqlParameters);
        int num = (int) SqlParameters[2].Value;
        flag = num == 0;
        if (num == 2)
          msg = "ConsortiaBussiness.AddAndUpdateConsortiaEuqipControl.Msg2";
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool ConsortiaRichRemove(int consortiID, ref int riches)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[3]
        {
          new SqlParameter("@ConsortiaID", (object) consortiID),
          new SqlParameter("@Riches", SqlDbType.Int),
          null
        };
        SqlParameters[1].Direction = ParameterDirection.InputOutput;
        SqlParameters[1].Value = (object) riches;
        SqlParameters[2] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[2].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_Consortia_Riches_Remove", SqlParameters);
        riches = (int) SqlParameters[1].Value;
        flag = (int) SqlParameters[2].Value == 0;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Consortia_Riches_Remove", ex);
      }
      return flag;
    }

    public ConsortiaBossConfigInfo[] GetConsortiaBossConfigAll()
    {
      List<ConsortiaBossConfigInfo> consortiaBossConfigInfoList = new List<ConsortiaBossConfigInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_Consortia_Boss_Config_All");
        while (ResultDataReader.Read())
          consortiaBossConfigInfoList.Add(new ConsortiaBossConfigInfo()
          {
            Level = (int) ResultDataReader["Level"],
            NpcID = (int) ResultDataReader["NpcID"],
            MissionID = (int) ResultDataReader["MissionID"],
            AwardID = (int) ResultDataReader["AwardID"],
            CostRich = (int) ResultDataReader["CostRich"],
            ProlongRich = (int) ResultDataReader["ProlongRich"],
            BossLevel = (int) ResultDataReader["BossLevel"]
          });
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
      return consortiaBossConfigInfoList.ToArray();
    }

    public ConsortiaBuffTempInfo[] GetAllConsortiaBuffTemp()
    {
      List<ConsortiaBuffTempInfo> consortiaBuffTempInfoList = new List<ConsortiaBuffTempInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_Consortia_Buff_Temp_All");
        while (ResultDataReader.Read())
          consortiaBuffTempInfoList.Add(new ConsortiaBuffTempInfo()
          {
            id = (int) ResultDataReader["id"],
            name = (string) ResultDataReader["name"],
            descript = (string) ResultDataReader["descript"],
            type = (int) ResultDataReader["type"],
            level = (int) ResultDataReader["level"],
            value = (int) ResultDataReader["value"],
            riches = (int) ResultDataReader["riches"],
            metal = (int) ResultDataReader["metal"],
            pic = (int) ResultDataReader["pic"],
            group = (int) ResultDataReader["group"]
          });
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (GetAllConsortiaBuffTemp), ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return consortiaBuffTempInfoList.ToArray();
    }

    public bool AddConsortia(ConsortiaInfo info, ref string msg, ref ConsortiaDutyInfo dutyInfo)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[23];
        SqlParameters[0] = new SqlParameter("@ConsortiaID", (object) info.ConsortiaID);
        SqlParameters[0].Direction = ParameterDirection.InputOutput;
        SqlParameters[1] = new SqlParameter("@BuildDate", (object) info.BuildDate);
        SqlParameters[2] = new SqlParameter("@CelebCount", (object) info.CelebCount);
        SqlParameters[3] = new SqlParameter("@ChairmanID", (object) info.ChairmanID);
        SqlParameters[4] = new SqlParameter("@ChairmanName", info.ChairmanName == null ? (object) "" : (object) info.ChairmanName);
        SqlParameters[5] = new SqlParameter("@ConsortiaName", info.ConsortiaName == null ? (object) "" : (object) info.ConsortiaName);
        SqlParameters[6] = new SqlParameter("@CreatorID", (object) info.CreatorID);
        SqlParameters[7] = new SqlParameter("@CreatorName", info.CreatorName == null ? (object) "" : (object) info.CreatorName);
        SqlParameters[8] = new SqlParameter("@Description", (object) info.Description);
        SqlParameters[9] = new SqlParameter("@Honor", (object) info.Honor);
        SqlParameters[10] = new SqlParameter("@IP", (object) info.IP);
        SqlParameters[11] = new SqlParameter("@IsExist", (object) info.IsExist);
        SqlParameters[12] = new SqlParameter("@Level", (object) info.Level);
        SqlParameters[13] = new SqlParameter("@MaxCount", (object) info.MaxCount);
        SqlParameters[14] = new SqlParameter("@Placard", (object) info.Placard);
        SqlParameters[15] = new SqlParameter("@Port", (object) info.Port);
        SqlParameters[16] = new SqlParameter("@Repute", (object) info.Repute);
        SqlParameters[17] = new SqlParameter("@Count", (object) info.Count);
        SqlParameters[18] = new SqlParameter("@Riches", (object) info.Riches);
        SqlParameters[19] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[19].Direction = ParameterDirection.ReturnValue;
        SqlParameters[20] = new SqlParameter("@tempDutyLevel", SqlDbType.Int);
        SqlParameters[20].Direction = ParameterDirection.InputOutput;
        SqlParameters[20].Value = (object) dutyInfo.Level;
        SqlParameters[21] = new SqlParameter("@tempDutyName", SqlDbType.NVarChar, 100);
        SqlParameters[21].Direction = ParameterDirection.InputOutput;
        SqlParameters[21].Value = (object) "";
        SqlParameters[22] = new SqlParameter("@tempRight", SqlDbType.Int);
        SqlParameters[22].Direction = ParameterDirection.InputOutput;
        SqlParameters[22].Value = (object) dutyInfo.Right;
        flag = this.db.RunProcedure("SP_Consortia_Add", SqlParameters);
        int num = (int) SqlParameters[19].Value;
        flag = num == 0;
        if (flag)
        {
          info.ConsortiaID = (int) SqlParameters[0].Value;
          dutyInfo.Level = (int) SqlParameters[20].Value;
          dutyInfo.DutyName = SqlParameters[21].Value.ToString();
          dutyInfo.Right = (int) SqlParameters[22].Value;
        }
        if (num == 2)
          msg = "ConsortiaBussiness.AddConsortia.Msg2";
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool AddConsortiaAlly(ConsortiaAllyInfo info, int userID, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[9];
        SqlParameters[0] = new SqlParameter("@ID", (object) info.ID);
        SqlParameters[0].Direction = ParameterDirection.InputOutput;
        SqlParameters[1] = new SqlParameter("@Consortia1ID", (object) info.Consortia1ID);
        SqlParameters[2] = new SqlParameter("@Consortia2ID", (object) info.Consortia2ID);
        SqlParameters[3] = new SqlParameter("@State", (object) info.State);
        SqlParameters[4] = new SqlParameter("@Date", (object) info.Date);
        SqlParameters[5] = new SqlParameter("@ValidDate", (object) info.ValidDate);
        SqlParameters[6] = new SqlParameter("@IsExist", (object) info.IsExist);
        SqlParameters[7] = new SqlParameter("@UserID", (object) userID);
        SqlParameters[8] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[8].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_ConsortiaAlly_Add", SqlParameters);
        int num = (int) SqlParameters[8].Value;
        flag = num == 0;
        if (num != 2)
        {
          if (num != 3)
            return flag;
          msg = "ConsortiaBussiness.AddConsortiaAlly.Msg3";
          return flag;
        }
        msg = "ConsortiaBussiness.AddConsortiaAlly.Msg2";
        return flag;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool AddConsortiaApplyAlly(ConsortiaApplyAllyInfo info, int userID, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[9];
        SqlParameters[0] = new SqlParameter("@ID", (object) info.ID);
        SqlParameters[0].Direction = ParameterDirection.InputOutput;
        SqlParameters[1] = new SqlParameter("@Consortia1ID", (object) info.Consortia1ID);
        SqlParameters[2] = new SqlParameter("@Consortia2ID", (object) info.Consortia2ID);
        SqlParameters[3] = new SqlParameter("@Date", (object) info.Date);
        SqlParameters[4] = new SqlParameter("@Remark", (object) info.Remark);
        SqlParameters[5] = new SqlParameter("@IsExist", (object) info.IsExist);
        SqlParameters[6] = new SqlParameter("@UserID", (object) userID);
        SqlParameters[7] = new SqlParameter("@State", (object) info.State);
        SqlParameters[8] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[8].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_ConsortiaApplyAlly_Add", SqlParameters);
        info.ID = (int) SqlParameters[0].Value;
        int num = (int) SqlParameters[8].Value;
        flag = num == 0;
        switch (num)
        {
          case 2:
            msg = "ConsortiaBussiness.AddConsortiaApplyAlly.Msg2";
            return flag;
          case 3:
            msg = "ConsortiaBussiness.AddConsortiaApplyAlly.Msg3";
            return flag;
          case 4:
            msg = "ConsortiaBussiness.AddConsortiaApplyAlly.Msg4";
            return flag;
          default:
            return flag;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool AddConsortiaApplyUsers(ConsortiaApplyUserInfo info, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[9];
        SqlParameters[0] = new SqlParameter("@ID", (object) info.ID);
        SqlParameters[0].Direction = ParameterDirection.InputOutput;
        SqlParameters[1] = new SqlParameter("@ApplyDate", (object) info.ApplyDate);
        SqlParameters[2] = new SqlParameter("@ConsortiaID", (object) info.ConsortiaID);
        SqlParameters[3] = new SqlParameter("@ConsortiaName", info.ConsortiaName == null ? (object) "" : (object) info.ConsortiaName);
        SqlParameters[4] = new SqlParameter("@IsExist", (object) info.IsExist);
        SqlParameters[5] = new SqlParameter("@Remark", info.Remark == null ? (object) "" : (object) info.Remark);
        SqlParameters[6] = new SqlParameter("@UserID", (object) info.UserID);
        SqlParameters[7] = new SqlParameter("@UserName", info.UserName == null ? (object) "" : (object) info.UserName);
        SqlParameters[8] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[8].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_ConsortiaApplyUser_Add", SqlParameters);
        info.ID = (int) SqlParameters[0].Value;
        int num = (int) SqlParameters[8].Value;
        flag = num == 0;
        switch (num)
        {
          case 2:
            msg = "ConsortiaBussiness.AddConsortiaApplyUsers.Msg2";
            break;
          case 6:
            msg = "ConsortiaBussiness.AddConsortiaApplyUsers.Msg6";
            return flag;
          case 7:
            msg = "ConsortiaBussiness.AddConsortiaApplyUsers.Msg7";
            return flag;
          default:
            return flag;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool AddConsortiaDuty(ConsortiaDutyInfo info, int userID, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[7];
        SqlParameters[0] = new SqlParameter("@DutyID", (object) info.DutyID);
        SqlParameters[0].Direction = ParameterDirection.InputOutput;
        SqlParameters[1] = new SqlParameter("@ConsortiaID", (object) info.ConsortiaID);
        SqlParameters[2] = new SqlParameter("@DutyName", (object) info.DutyName);
        SqlParameters[3] = new SqlParameter("@Level", (object) info.Level);
        SqlParameters[4] = new SqlParameter("@UserID", (object) userID);
        SqlParameters[5] = new SqlParameter("@Right", (object) info.Right);
        SqlParameters[6] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[6].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_ConsortiaDuty_Add", SqlParameters);
        info.DutyID = (int) SqlParameters[0].Value;
        int num = (int) SqlParameters[6].Value;
        flag = num == 0;
        if (num != 2)
        {
          if (num != 3)
            return flag;
          msg = "ConsortiaBussiness.AddConsortiaDuty.Msg3";
          return flag;
        }
        msg = "ConsortiaBussiness.AddConsortiaDuty.Msg2";
        return flag;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool AddConsortiaInviteUsers(ConsortiaInviteUserInfo info, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[11];
        SqlParameters[0] = new SqlParameter("@ID", (object) info.ID);
        SqlParameters[0].Direction = ParameterDirection.InputOutput;
        SqlParameters[1] = new SqlParameter("@ConsortiaID", (object) info.ConsortiaID);
        SqlParameters[2] = new SqlParameter("@ConsortiaName", info.ConsortiaName == null ? (object) "" : (object) info.ConsortiaName);
        SqlParameters[3] = new SqlParameter("@InviteDate", (object) info.InviteDate);
        SqlParameters[4] = new SqlParameter("@InviteID", (object) info.InviteID);
        SqlParameters[5] = new SqlParameter("@InviteName", info.InviteName == null ? (object) "" : (object) info.InviteName);
        SqlParameters[6] = new SqlParameter("@IsExist", (object) info.IsExist);
        SqlParameters[7] = new SqlParameter("@Remark", info.Remark == null ? (object) "" : (object) info.Remark);
        SqlParameters[8] = new SqlParameter("@UserID", (object) info.UserID);
        SqlParameters[8].Direction = ParameterDirection.InputOutput;
        SqlParameters[9] = new SqlParameter("@UserName", info.UserName == null ? (object) "" : (object) info.UserName);
        SqlParameters[10] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[10].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_ConsortiaInviteUser_Add", SqlParameters);
        info.ID = (int) SqlParameters[0].Value;
        info.UserID = (int) SqlParameters[8].Value;
        int num = (int) SqlParameters[10].Value;
        flag = num == 0;
        switch (num)
        {
          case 2:
            msg = "ConsortiaBussiness.AddConsortiaInviteUsers.Msg2";
            return flag;
          case 3:
            return flag;
          case 4:
            msg = "ConsortiaBussiness.AddConsortiaInviteUsers.Msg4";
            return flag;
          case 5:
            msg = "ConsortiaBussiness.AddConsortiaInviteUsers.Msg5";
            return flag;
          case 6:
            msg = "ConsortiaBussiness.AddConsortiaInviteUsers.Msg6";
            return flag;
          default:
            return flag;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool BuyBadge(int consortiaID, int userID, ConsortiaInfo info, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[6]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@BadgeID", (object) info.BadgeID),
          new SqlParameter("@ValidDate", (object) info.ValidDate),
          new SqlParameter("@BadgeBuyTime", (object) info.BadgeBuyTime),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[5].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_ConsortiaBadge_Update", SqlParameters);
        int num = (int) SqlParameters[5].Value;
        flag = num == 0;
        if (num == 2)
          msg = "ConsortiaBussiness.BuyBadge.Msg2";
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool ConsortiaFight(
      int consortiWin,
      int consortiaLose,
      int playerCount,
      out int riches,
      int state,
      int totalKillHealth,
      float richesRate)
    {
      bool flag = false;
      riches = 0;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[8]
        {
          new SqlParameter("@ConsortiaWin", (object) consortiWin),
          new SqlParameter("@ConsortiaLose", (object) consortiaLose),
          new SqlParameter("@PlayerCount", (object) playerCount),
          new SqlParameter("@Riches", SqlDbType.Int),
          null,
          null,
          null,
          null
        };
        SqlParameters[3].Direction = ParameterDirection.InputOutput;
        SqlParameters[3].Value = (object) riches;
        SqlParameters[4] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[4].Direction = ParameterDirection.ReturnValue;
        SqlParameters[5] = new SqlParameter("@State", (object) state);
        SqlParameters[6] = new SqlParameter("@TotalKillHealth", (object) totalKillHealth);
        SqlParameters[7] = new SqlParameter("@RichesRate", (object) richesRate);
        flag = this.db.RunProcedure("SP_Consortia_Fight", SqlParameters);
        riches = (int) SqlParameters[3].Value;
        flag = (int) SqlParameters[4].Value == 0;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (ConsortiaFight), ex);
      }
      return flag;
    }

    public bool ConsortiaRichAdd(int consortiID, ref int riches)
    {
      return this.ConsortiaRichAdd(consortiID, ref riches, 0, "");
    }

    public bool ConsortiaRichAdd(int consortiID, ref int riches, int type, string username)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[5]
        {
          new SqlParameter("@ConsortiaID", (object) consortiID),
          new SqlParameter("@Riches", SqlDbType.Int),
          null,
          null,
          null
        };
        SqlParameters[1].Direction = ParameterDirection.InputOutput;
        SqlParameters[1].Value = (object) riches;
        SqlParameters[2] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[2].Direction = ParameterDirection.ReturnValue;
        SqlParameters[3] = new SqlParameter("@Type", (object) type);
        SqlParameters[4] = new SqlParameter("@UserName", (object) username);
        flag = this.db.RunProcedure("SP_Consortia_Riches_Add", SqlParameters);
        riches = (int) SqlParameters[1].Value;
        flag = (int) SqlParameters[2].Value == 0;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (ConsortiaRichAdd), ex);
      }
      return flag;
    }

    public bool DeleteConsortia(int consortiaID, int userID, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[3]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[2].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_Consortia_Delete", SqlParameters);
        int num = (int) SqlParameters[2].Value;
        flag = num == 0;
        if (num != 2)
        {
          if (num != 3)
            return flag;
          msg = "ConsortiaBussiness.DeleteConsortia.Msg3";
          return flag;
        }
        msg = "ConsortiaBussiness.DeleteConsortia.Msg2";
        return flag;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool DeleteConsortiaApplyAlly(int applyID, int userID, int consortiaID, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[4]
        {
          new SqlParameter("@ID", (object) applyID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[3].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_ConsortiaApplyAlly_Delete", SqlParameters);
        int num = (int) SqlParameters[3].Value;
        flag = num == 0;
        if (num == 2)
          msg = "ConsortiaBussiness.DeleteConsortiaApplyAlly.Msg2";
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool DeleteConsortiaApplyUsers(
      int applyID,
      int userID,
      int consortiaID,
      ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[4]
        {
          new SqlParameter("@ID", (object) applyID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[3].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_ConsortiaApplyUser_Delete", SqlParameters);
        int num = (int) SqlParameters[3].Value;
        flag = num == 0 || num == 3;
        if (num == 2)
          msg = "ConsortiaBussiness.DeleteConsortiaApplyUsers.Msg2";
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool DeleteConsortiaDuty(int dutyID, int userID, int consortiaID, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[4]
        {
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@DutyID", (object) dutyID),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[3].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_ConsortiaDuty_Delete", SqlParameters);
        int num = (int) SqlParameters[3].Value;
        flag = num == 0;
        if (num != 2)
        {
          if (num != 3)
            return flag;
          msg = "ConsortiaBussiness.DeleteConsortiaDuty.Msg3";
          return flag;
        }
        msg = "ConsortiaBussiness.DeleteConsortiaDuty.Msg2";
        return flag;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool DeleteConsortiaInviteUsers(int intiveID, int userID)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[3]
        {
          new SqlParameter("@ID", (object) intiveID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[2].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_ConsortiaInviteUser_Delete", SqlParameters);
        flag = (int) SqlParameters[2].Value == 0;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool DeleteConsortiaUser(
      int userID,
      int kickUserID,
      int consortiaID,
      ref string msg,
      ref string nickName)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[5]
        {
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@KickUserID", (object) kickUserID),
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@Result", SqlDbType.Int),
          null
        };
        SqlParameters[3].Direction = ParameterDirection.ReturnValue;
        SqlParameters[4] = new SqlParameter("@NickName", SqlDbType.NVarChar, 200);
        SqlParameters[4].Direction = ParameterDirection.InputOutput;
        SqlParameters[4].Value = (object) nickName;
        this.db.RunProcedure("SP_ConsortiaUser_Delete", SqlParameters);
        int num = (int) SqlParameters[3].Value;
        if (num == 0)
        {
          flag = true;
          nickName = SqlParameters[4].Value.ToString();
        }
        switch (num - 2)
        {
          case 0:
            msg = "ConsortiaBussiness.DeleteConsortiaUser.Msg2";
            return flag;
          case 1:
            msg = "ConsortiaBussiness.DeleteConsortiaUser.Msg3";
            return flag;
          case 2:
            msg = "ConsortiaBussiness.DeleteConsortiaUser.Msg4";
            return flag;
          case 3:
            msg = "ConsortiaBussiness.DeleteConsortiaUser.Msg5";
            return flag;
          default:
            return flag;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public ConsortiaLevelInfo[] GetAllConsortiaLevel()
    {
      List<ConsortiaLevelInfo> consortiaLevelInfoList = new List<ConsortiaLevelInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_Consortia_Level_All");
        while (ResultDataReader.Read())
        {
          ConsortiaLevelInfo consortiaLevelInfo = new ConsortiaLevelInfo()
          {
            Count = (int) ResultDataReader["Count"],
            Deduct = (int) ResultDataReader["Deduct"],
            Level = (int) ResultDataReader["Level"],
            NeedGold = (int) ResultDataReader["NeedGold"],
            NeedItem = (int) ResultDataReader["NeedItem"],
            Reward = (int) ResultDataReader["Reward"],
            Riches = (int) ResultDataReader["Riches"],
            ShopRiches = (int) ResultDataReader["ShopRiches"],
            SmithRiches = (int) ResultDataReader["SmithRiches"],
            StoreRiches = (int) ResultDataReader["StoreRiches"],
            BufferRiches = (int) ResultDataReader["BufferRiches"]
          };
          consortiaLevelInfoList.Add(consortiaLevelInfo);
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (GetAllConsortiaLevel), ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return consortiaLevelInfoList.ToArray();
    }

    public ConsortiaInfo[] GetConsortiaAll()
    {
      List<ConsortiaInfo> consortiaInfoList = new List<ConsortiaInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_Consortia_All");
        while (ResultDataReader.Read())
        {
          ConsortiaInfo consortiaInfo1 = new ConsortiaInfo();
          consortiaInfo1.ConsortiaID = (int) ResultDataReader["ConsortiaID"];
          consortiaInfo1.Honor = (int) ResultDataReader["Honor"];
          consortiaInfo1.Level = (int) ResultDataReader["Level"];
          consortiaInfo1.Riches = (int) ResultDataReader["Riches"];
          consortiaInfo1.MaxCount = (int) ResultDataReader["MaxCount"];
          consortiaInfo1.BuildDate = (DateTime) ResultDataReader["BuildDate"];
          consortiaInfo1.IsExist = (bool) ResultDataReader["IsExist"];
          consortiaInfo1.DeductDate = (DateTime) ResultDataReader["DeductDate"];
          consortiaInfo1.StoreLevel = (int) ResultDataReader["StoreLevel"];
          consortiaInfo1.SmithLevel = (int) ResultDataReader["SmithLevel"];
          consortiaInfo1.ShopLevel = (int) ResultDataReader["ShopLevel"];
          consortiaInfo1.SkillLevel = (int) ResultDataReader["SkillLevel"];
          consortiaInfo1.ConsortiaName = ResultDataReader["ConsortiaName"] == null ? "" : ResultDataReader["ConsortiaName"].ToString();
          consortiaInfo1.IsDirty = false;
          ConsortiaInfo consortiaInfo2 = consortiaInfo1;
          consortiaInfoList.Add(consortiaInfo2);
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
      return consortiaInfoList.ToArray();
    }

    public ConsortiaAllyInfo[] GetConsortiaAllyAll()
    {
      List<ConsortiaAllyInfo> consortiaAllyInfoList = new List<ConsortiaAllyInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_ConsortiaAlly_All");
        while (ResultDataReader.Read())
        {
          ConsortiaAllyInfo consortiaAllyInfo = new ConsortiaAllyInfo()
          {
            Consortia1ID = (int) ResultDataReader["Consortia1ID"],
            Consortia2ID = (int) ResultDataReader["Consortia2ID"],
            Date = (DateTime) ResultDataReader["Date"],
            ID = (int) ResultDataReader["ID"],
            State = (int) ResultDataReader["State"],
            ValidDate = (int) ResultDataReader["ValidDate"],
            IsExist = (bool) ResultDataReader["IsExist"]
          };
          consortiaAllyInfoList.Add(consortiaAllyInfo);
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
      return consortiaAllyInfoList.ToArray();
    }

    public ConsortiaAllyInfo[] GetConsortiaAllyPage(
      int page,
      int size,
      ref int total,
      int order,
      int consortiaID,
      int state,
      string name)
    {
      List<ConsortiaAllyInfo> consortiaAllyInfoList = new List<ConsortiaAllyInfo>();
      string queryWhere = " IsExist=1 and ConsortiaID<>" + (object) consortiaID;
      Dictionary<int, int> consortiaByAlly = this.GetConsortiaByAlly(consortiaID);
      try
      {
        if (state != -1)
        {
          string str1 = string.Empty;
          foreach (int key in consortiaByAlly.Keys)
            str1 = str1 + (object) key + ",";
          string str2 = str1 + (object) 0;
          queryWhere = state != 0 ? queryWhere + " and ConsortiaID in (" + str2 + ") " : queryWhere + " and ConsortiaID not in (" + str2 + ") ";
        }
        if (!string.IsNullOrEmpty(name))
          queryWhere = queryWhere + " and ConsortiaName like '%" + name + "%' ";
        foreach (DataRow row in (InternalDataCollectionBase) this.GetPage("Consortia", queryWhere, page, size, "*", "ConsortiaID", "ConsortiaID", ref total).Rows)
        {
          ConsortiaAllyInfo consortiaAllyInfo = new ConsortiaAllyInfo()
          {
            Consortia1ID = (int) row["ConsortiaID"],
            ConsortiaName1 = row["ConsortiaName"] == null ? "" : row["ConsortiaName"].ToString(),
            ConsortiaName2 = "",
            Count1 = (int) row["Count"],
            Repute1 = (int) row["Repute"],
            ChairmanName1 = row["ChairmanName"] == null ? "" : row["ChairmanName"].ToString(),
            ChairmanName2 = "",
            Level1 = (int) row["Level"],
            Honor1 = (int) row["Honor"],
            Description1 = row["Description"] == null ? "" : row["Description"].ToString(),
            Description2 = "",
            Riches1 = (int) row["Riches"],
            Date = DateTime.Now,
            IsExist = true
          };
          if (consortiaByAlly.ContainsKey(consortiaAllyInfo.Consortia1ID))
            consortiaAllyInfo.State = consortiaByAlly[consortiaAllyInfo.Consortia1ID];
          consortiaAllyInfo.ValidDate = 0;
          consortiaAllyInfoList.Add(consortiaAllyInfo);
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (GetConsortiaAllyPage), ex);
      }
      return consortiaAllyInfoList.ToArray();
    }

    public ConsortiaApplyAllyInfo[] GetConsortiaApplyAllyPage(
      int page,
      int size,
      ref int total,
      int order,
      int consortiaID,
      int applyID,
      int state)
    {
      List<ConsortiaApplyAllyInfo> consortiaApplyAllyInfoList = new List<ConsortiaApplyAllyInfo>();
      try
      {
        string queryWhere = " IsExist=1 ";
        if (consortiaID != -1)
          queryWhere = queryWhere + " and Consortia2ID =" + (object) consortiaID + " ";
        if (applyID != -1)
          queryWhere = queryWhere + " and ID =" + (object) applyID + " ";
        if (state != -1)
          queryWhere = queryWhere + " and State =" + (object) state + " ";
        string str = "ConsortiaName";
        switch (order)
        {
          case 1:
            str = "Repute";
            break;
          case 2:
            str = "ChairmanName";
            break;
          case 3:
            str = "Count";
            break;
          case 4:
            str = "Level";
            break;
          case 5:
            str = "Honor";
            break;
        }
        string fdOreder = str + ",ID ";
        foreach (DataRow row in (InternalDataCollectionBase) this.GetPage("V_Consortia_Apply_Ally", queryWhere, page, size, "*", fdOreder, "ID", ref total).Rows)
        {
          ConsortiaApplyAllyInfo consortiaApplyAllyInfo = new ConsortiaApplyAllyInfo()
          {
            ID = (int) row["ID"],
            CelebCount = (int) row["CelebCount"],
            ChairmanName = row["ChairmanName"].ToString(),
            Consortia1ID = (int) row["Consortia1ID"],
            Consortia2ID = (int) row["Consortia2ID"],
            ConsortiaName = row["ConsortiaName"].ToString(),
            Count = (int) row["Count"],
            Date = (DateTime) row["Date"],
            Honor = (int) row["Honor"],
            IsExist = (bool) row["IsExist"],
            Remark = row["Remark"].ToString(),
            Repute = (int) row["Repute"],
            State = (int) row["State"],
            Level = (int) row["Level"],
            Description = row["Description"] == null ? "" : row["Description"].ToString()
          };
          consortiaApplyAllyInfoList.Add(consortiaApplyAllyInfo);
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return consortiaApplyAllyInfoList.ToArray();
    }

    public ConsortiaApplyUserInfo[] GetConsortiaApplyUserPage(
      int page,
      int size,
      ref int total,
      int order,
      int consortiaID,
      int applyID,
      int userID)
    {
      List<ConsortiaApplyUserInfo> consortiaApplyUserInfoList = new List<ConsortiaApplyUserInfo>();
      try
      {
        string queryWhere = " IsExist=1 ";
        if (consortiaID != -1)
          queryWhere = queryWhere + " and ConsortiaID =" + (object) consortiaID + " ";
        if (applyID != -1)
          queryWhere = queryWhere + " and ID =" + (object) applyID + " ";
        if (userID != -1)
          queryWhere = queryWhere + " and UserID ='" + (object) userID + "' ";
        string fdOreder = "ID";
        switch (order)
        {
          case 1:
            fdOreder = "UserName,ID";
            break;
          case 2:
            fdOreder = "ApplyDate,ID";
            break;
        }
        foreach (DataRow row in (InternalDataCollectionBase) this.GetPage("V_Consortia_Apply_Users", queryWhere, page, size, "*", fdOreder, "ID", ref total).Rows)
        {
          ConsortiaApplyUserInfo consortiaApplyUserInfo = new ConsortiaApplyUserInfo()
          {
            ID = (int) row["ID"],
            ApplyDate = (DateTime) row["ApplyDate"],
            ConsortiaID = (int) row["ConsortiaID"],
            ConsortiaName = row["ConsortiaName"].ToString(),
            ChairmanID = (int) row["ChairmanID"],
            ChairmanName = row["ChairmanName"].ToString(),
            IsExist = (bool) row["IsExist"],
            Remark = row["Remark"].ToString(),
            UserID = (int) row["UserID"],
            UserName = row["UserName"].ToString(),
            UserLevel = (int) row["Grade"],
            typeVIP = (int) row["typeVIP"],
            Win = (int) row["Win"],
            Total = (int) row["Total"],
            Repute = (int) row["Repute"],
            FightPower = (int) row["FightPower"],
            IsOld = (bool) row["IsOldPlayer"],
            Offer = (int) row["Offer"]
          };
          consortiaApplyUserInfoList.Add(consortiaApplyUserInfo);
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return consortiaApplyUserInfoList.ToArray();
    }

    public Dictionary<int, int> GetConsortiaByAlly(int consortiaID)
    {
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[1]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID)
        };
        this.db.GetReader(ref ResultDataReader, "SP_Consortia_Ally_Neutral", SqlParameters);
        while (ResultDataReader.Read())
        {
          if ((int) ResultDataReader["Consortia1ID"] != consortiaID)
            dictionary.Add((int) ResultDataReader["Consortia1ID"], (int) ResultDataReader["State"]);
          else
            dictionary.Add((int) ResultDataReader["Consortia2ID"], (int) ResultDataReader["State"]);
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (GetConsortiaByAlly), ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return dictionary;
    }

    public int[] GetConsortiaByAllyByState(int consortiaID, int state)
    {
      List<int> intList = new List<int>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[2]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@State", (object) state)
        };
        this.db.GetReader(ref ResultDataReader, "SP_Consortia_AllyByState", SqlParameters);
        while (ResultDataReader.Read())
          intList.Add((int) ResultDataReader["Consortia2ID"]);
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
      return intList.ToArray();
    }

    public ConsortiaDutyInfo[] GetConsortiaDutyPage(
      int page,
      int size,
      ref int total,
      int order,
      int consortiaID,
      int dutyID)
    {
      List<ConsortiaDutyInfo> consortiaDutyInfoList = new List<ConsortiaDutyInfo>();
      try
      {
        string queryWhere = " IsExist=1 ";
        if (consortiaID != -1)
          queryWhere = queryWhere + " and ConsortiaID =" + (object) consortiaID + " ";
        if (dutyID != -1)
          queryWhere = queryWhere + " and DutyID =" + (object) dutyID + " ";
        string str = "Level";
        if (order == 1)
          str = "DutyName";
        string fdOreder = str + ",DutyID ";
        foreach (DataRow row in (InternalDataCollectionBase) this.GetPage("Consortia_Duty", queryWhere, page, size, "*", fdOreder, "DutyID", ref total).Rows)
        {
          ConsortiaDutyInfo consortiaDutyInfo = new ConsortiaDutyInfo()
          {
            DutyID = (int) row["DutyID"],
            ConsortiaID = (int) row["ConsortiaID"],
            DutyName = row["DutyName"].ToString(),
            IsExist = (bool) row["IsExist"],
            Right = (int) row["Right"],
            Level = (int) row["Level"]
          };
          consortiaDutyInfoList.Add(consortiaDutyInfo);
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return consortiaDutyInfoList.ToArray();
    }

    public ConsortiaEquipControlInfo[] GetConsortiaEquipControlPage(
      int page,
      int size,
      ref int total,
      int order,
      int consortiaID,
      int level,
      int type)
    {
      List<ConsortiaEquipControlInfo> equipControlInfoList = new List<ConsortiaEquipControlInfo>();
      try
      {
        string queryWhere = " IsExist=1 ";
        if (consortiaID != -1)
          queryWhere = queryWhere + " and ConsortiaID =" + (object) consortiaID + " ";
        if (level != -1)
          queryWhere = queryWhere + " and Level =" + (object) level + " ";
        if (type != -1)
          queryWhere = queryWhere + " and Type =" + (object) type + " ";
        string fdOreder = "ConsortiaID ";
        foreach (DataRow row in (InternalDataCollectionBase) this.GetPage("Consortia_Equip_Control", queryWhere, page, size, "*", fdOreder, "ConsortiaID", ref total).Rows)
        {
          ConsortiaEquipControlInfo equipControlInfo = new ConsortiaEquipControlInfo()
          {
            ConsortiaID = (int) row["ConsortiaID"],
            Level = (int) row["Level"],
            Riches = (int) row["Riches"],
            Type = (int) row["Type"]
          };
          equipControlInfoList.Add(equipControlInfo);
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return equipControlInfoList.ToArray();
    }

    public ConsortiaEquipControlInfo GetConsortiaEuqipRiches(
      int consortiaID,
      int Level,
      int type)
    {
      ConsortiaEquipControlInfo equipControlInfo = (ConsortiaEquipControlInfo) null;
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[3]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@Level", (object) Level),
          new SqlParameter("@Type", (object) type)
        };
        this.db.GetReader(ref ResultDataReader, "SP_Consortia_Equip_Control_Single", SqlParameters);
        if (ResultDataReader.Read())
          return new ConsortiaEquipControlInfo()
          {
            ConsortiaID = (int) ResultDataReader["ConsortiaID"],
            Level = (int) ResultDataReader[nameof (Level)],
            Riches = (int) ResultDataReader["Riches"],
            Type = (int) ResultDataReader["Type"]
          };
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "GetAllConsortiaLevel", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      if (equipControlInfo != null)
        return equipControlInfo;
      return new ConsortiaEquipControlInfo()
      {
        ConsortiaID = consortiaID,
        Level = Level,
        Riches = 100,
        Type = type
      };
    }

    public ConsortiaEventInfo[] GetConsortiaEventPage(
      int page,
      int size,
      ref int total,
      int order,
      int consortiaID)
    {
      List<ConsortiaEventInfo> consortiaEventInfoList = new List<ConsortiaEventInfo>();
      try
      {
        string queryWhere = " IsExist=1 ";
        if (consortiaID != -1)
          queryWhere = queryWhere + " and ConsortiaID =" + (object) consortiaID + " ";
        string fdOreder = "Date desc,ID ";
        foreach (DataRow row in (InternalDataCollectionBase) this.GetPage("Consortia_Event", queryWhere, page, size, "*", fdOreder, "ID", ref total).Rows)
        {
          ConsortiaEventInfo consortiaEventInfo = new ConsortiaEventInfo()
          {
            ID = (int) row["ID"],
            ConsortiaID = (int) row["ConsortiaID"],
            Date = (DateTime) row["Date"],
            Type = (int) row["Type"],
            NickName = row["NickName"].ToString(),
            EventValue = (int) row["EventValue"],
            ManagerName = row["ManagerName"].ToString()
          };
          consortiaEventInfoList.Add(consortiaEventInfo);
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return consortiaEventInfoList.ToArray();
    }

    public ConsortiaInviteUserInfo[] GetConsortiaInviteUserPage(
      int page,
      int size,
      ref int total,
      int order,
      int userID,
      int inviteID)
    {
      List<ConsortiaInviteUserInfo> consortiaInviteUserInfoList = new List<ConsortiaInviteUserInfo>();
      try
      {
        string queryWhere = " IsExist=1 ";
        if (userID != -1)
          queryWhere = queryWhere + " and UserID =" + (object) userID + " ";
        if (inviteID != -1)
          queryWhere = queryWhere + " and UserID =" + (object) inviteID + " ";
        string str = "ConsortiaName";
        switch (order)
        {
          case 1:
            str = "Repute";
            break;
          case 2:
            str = "ChairmanName";
            break;
          case 3:
            str = "Count";
            break;
          case 4:
            str = "CelebCount";
            break;
          case 5:
            str = "Honor";
            break;
        }
        string fdOreder = str + ",ID ";
        foreach (DataRow row in (InternalDataCollectionBase) this.GetPage("V_Consortia_Invite", queryWhere, page, size, "*", fdOreder, "ID", ref total).Rows)
        {
          ConsortiaInviteUserInfo consortiaInviteUserInfo = new ConsortiaInviteUserInfo()
          {
            ID = (int) row["ID"],
            CelebCount = (int) row["CelebCount"],
            ChairmanName = row["ChairmanName"].ToString(),
            ConsortiaID = (int) row["ConsortiaID"],
            ConsortiaName = row["ConsortiaName"].ToString(),
            Count = (int) row["Count"],
            Honor = (int) row["Honor"],
            InviteDate = (DateTime) row["InviteDate"],
            InviteID = (int) row["InviteID"],
            InviteName = row["InviteName"].ToString(),
            IsExist = (bool) row["IsExist"],
            Remark = row["Remark"].ToString(),
            Repute = (int) row["Repute"],
            UserID = (int) row["UserID"],
            UserName = row["UserName"].ToString()
          };
          consortiaInviteUserInfoList.Add(consortiaInviteUserInfo);
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return consortiaInviteUserInfoList.ToArray();
    }

    public ConsortiaInfo[] GetConsortiaPage(
      int page,
      int size,
      ref int total,
      int order,
      string name,
      int consortiaID,
      int level,
      int openApply)
    {
      List<ConsortiaInfo> consortiaInfoList = new List<ConsortiaInfo>();
      try
      {
        string queryWhere = " IsExist=1 ";
        if (!string.IsNullOrEmpty(name))
          queryWhere = queryWhere + " and ConsortiaName like '%" + name + "%' ";
        if (consortiaID != -1)
          queryWhere = queryWhere + " and ConsortiaID =" + (object) consortiaID + " ";
        if (level != -1)
          queryWhere = queryWhere + " and Level =" + (object) level + " ";
        if (openApply != -1)
          queryWhere = queryWhere + " and OpenApply =" + (object) openApply + " ";
        string str = "ConsortiaName";
        switch (order)
        {
          case 1:
            str = "ReputeSort";
            break;
          case 2:
            str = "ChairmanName";
            break;
          case 3:
            str = "Count desc";
            break;
          case 4:
            str = "Level desc";
            break;
          case 5:
            str = "Honor desc";
            break;
          case 10:
            str = "Riches desc";
            break;
          case 11:
            str = "AddDayRiches desc";
            break;
          case 12:
            str = "AddWeekRiches desc";
            break;
          case 13:
            str = "LastDayHonor desc";
            break;
          case 14:
            str = "AddDayHonor desc";
            break;
          case 15:
            str = "AddWeekHonor desc";
            break;
          case 16:
            str = "level desc,LastDayRiches desc";
            break;
        }
        string fdOreder = str + ",ConsortiaID ";
        foreach (DataRow row in (InternalDataCollectionBase) this.GetPage("V_Consortia", queryWhere, page, size, "*", fdOreder, "ConsortiaID", ref total).Rows)
          consortiaInfoList.Add(new ConsortiaInfo()
          {
            ConsortiaID = (int) row["ConsortiaID"],
            BuildDate = (DateTime) row["BuildDate"],
            CelebCount = (int) row["CelebCount"],
            ChairmanID = (int) row["ChairmanID"],
            ChairmanName = row["ChairmanName"].ToString(),
            ConsortiaName = row["ConsortiaName"].ToString(),
            CreatorID = (int) row["CreatorID"],
            CreatorName = row["CreatorName"].ToString(),
            Description = row["Description"].ToString(),
            Honor = (int) row["Honor"],
            IsExist = (bool) row["IsExist"],
            Level = (int) row["Level"],
            MaxCount = (int) row["MaxCount"],
            Placard = row["Placard"].ToString(),
            IP = row["IP"].ToString(),
            Port = (int) row["Port"],
            Repute = (int) row["Repute"],
            Count = (int) row["Count"],
            Riches = (int) row["Riches"],
            DeductDate = (DateTime) row["DeductDate"],
            AddDayHonor = (int) row["AddDayHonor"],
            AddDayRiches = (int) row["AddDayRiches"],
            AddWeekHonor = (int) row["AddWeekHonor"],
            AddWeekRiches = (int) row["AddWeekRiches"],
            LastDayRiches = (int) row["LastDayRiches"],
            OpenApply = (bool) row["OpenApply"],
            StoreLevel = (int) row["StoreLevel"],
            SmithLevel = (int) row["SmithLevel"],
            ShopLevel = (int) row["ShopLevel"],
            SkillLevel = (int) row["SkillLevel"],
            BadgeType = (int) row["BadgeType"],
            BadgeName = row["BadgeName"] == DBNull.Value ? "" : (string) row["BadgeName"],
            BadgeID = (int) row["BadgeID"],
            BadgeBuyTime = row["BadgeBuyTime"] == DBNull.Value ? "" : (string) row["BadgeBuyTime"],
            ValidDate = (int) row["ValidDate"]
          });
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return consortiaInfoList.ToArray();
    }

    public ConsortiaInfo GetConsortiaSingle(int id)
    {
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[1]
        {
          new SqlParameter("@ID", (object) id)
        };
        this.db.GetReader(ref ResultDataReader, "SP_Consortia_Single", SqlParameters);
        if (ResultDataReader.Read())
          return new ConsortiaInfo()
          {
            ConsortiaID = (int) ResultDataReader["ConsortiaID"],
            BuildDate = (DateTime) ResultDataReader["BuildDate"],
            CelebCount = (int) ResultDataReader["CelebCount"],
            ChairmanID = (int) ResultDataReader["ChairmanID"],
            ChairmanName = ResultDataReader["ChairmanName"].ToString(),
            ChairmanTypeVIP = Convert.ToByte(ResultDataReader["typeVIP"]),
            ChairmanVIPLevel = (int) ResultDataReader["VIPLevel"],
            ConsortiaName = ResultDataReader["ConsortiaName"].ToString(),
            CreatorID = (int) ResultDataReader["CreatorID"],
            CreatorName = ResultDataReader["CreatorName"].ToString(),
            Description = ResultDataReader["Description"].ToString(),
            Honor = (int) ResultDataReader["Honor"],
            IsExist = (bool) ResultDataReader["IsExist"],
            Level = (int) ResultDataReader["Level"],
            MaxCount = (int) ResultDataReader["MaxCount"],
            Placard = ResultDataReader["Placard"].ToString(),
            IP = ResultDataReader["IP"].ToString(),
            Port = (int) ResultDataReader["Port"],
            Repute = (int) ResultDataReader["Repute"],
            Count = (int) ResultDataReader["Count"],
            Riches = (int) ResultDataReader["Riches"],
            DeductDate = (DateTime) ResultDataReader["DeductDate"],
            StoreLevel = (int) ResultDataReader["StoreLevel"],
            SmithLevel = (int) ResultDataReader["SmithLevel"],
            ShopLevel = (int) ResultDataReader["ShopLevel"],
            SkillLevel = (int) ResultDataReader["SkillLevel"],
            DateOpenTask = ResultDataReader["DateOpenTask"] == DBNull.Value ? DateTime.Now.AddYears(-1) : (DateTime) ResultDataReader["DateOpenTask"]
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
      return (ConsortiaInfo) null;
    }

    public ConsortiaInfo GetConsortiaSingleByName(string ConsortiaName)
    {
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[1]
        {
          new SqlParameter("@ConsortiaName", SqlDbType.NVarChar, 200)
        };
        SqlParameters[0].Value = (object) ConsortiaName;
        this.db.GetReader(ref ResultDataReader, "SP_Consortia_CheckByName", SqlParameters);
        if (ResultDataReader.Read())
          return new ConsortiaInfo()
          {
            ConsortiaID = (int) ResultDataReader["ConsortiaID"],
            BuildDate = (DateTime) ResultDataReader["BuildDate"],
            CelebCount = (int) ResultDataReader["CelebCount"],
            ChairmanID = (int) ResultDataReader["ChairmanID"],
            ChairmanName = ResultDataReader["ChairmanName"].ToString(),
            ConsortiaName = ResultDataReader[nameof (ConsortiaName)].ToString(),
            CreatorID = (int) ResultDataReader["CreatorID"],
            CreatorName = ResultDataReader["CreatorName"].ToString(),
            Description = ResultDataReader["Description"].ToString(),
            Honor = (int) ResultDataReader["Honor"],
            IsExist = (bool) ResultDataReader["IsExist"],
            Level = (int) ResultDataReader["Level"],
            MaxCount = (int) ResultDataReader["MaxCount"],
            Placard = ResultDataReader["Placard"].ToString(),
            IP = ResultDataReader["IP"].ToString(),
            Port = (int) ResultDataReader["Port"],
            Repute = (int) ResultDataReader["Repute"],
            Count = (int) ResultDataReader["Count"],
            Riches = (int) ResultDataReader["Riches"],
            DeductDate = (DateTime) ResultDataReader["DeductDate"],
            StoreLevel = (int) ResultDataReader["StoreLevel"],
            SmithLevel = (int) ResultDataReader["SmithLevel"],
            ShopLevel = (int) ResultDataReader["ShopLevel"],
            SkillLevel = (int) ResultDataReader["SkillLevel"]
          };
      }
      catch
      {
        throw new Exception();
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return (ConsortiaInfo) null;
    }

    public ConsortiaUserInfo GetConsortiaUsersByUserID(int userID)
    {
      int total = 0;
      ConsortiaUserInfo[] consortiaUsersPage = this.GetConsortiaUsersPage(1, 1, ref total, -1, -1, userID, -1);
      if (consortiaUsersPage.Length == 1)
        return consortiaUsersPage[0];
      return (ConsortiaUserInfo) null;
    }

    public ConsortiaUserInfo[] GetConsortiaUsersPage(
      int page,
      int size,
      ref int total,
      int order,
      int consortiaID,
      int userID,
      int state)
    {
      List<ConsortiaUserInfo> consortiaUserInfoList = new List<ConsortiaUserInfo>();
      try
      {
        string queryWhere = " IsExist=1 ";
        if (consortiaID != -1)
          queryWhere = queryWhere + " and ConsortiaID =" + (object) consortiaID + " ";
        if (userID != -1)
          queryWhere = queryWhere + " and UserID =" + (object) userID + " ";
        if (state != -1)
          queryWhere = queryWhere + " and state =" + (object) state + " ";
        string str = "UserName";
        switch (order)
        {
          case 1:
            str = "DutyID";
            break;
          case 2:
            str = "Grade";
            break;
          case 3:
            str = "Repute";
            break;
          case 4:
            str = "GP";
            break;
          case 5:
            str = "State";
            break;
          case 6:
            str = "Offer";
            break;
        }
        string fdOreder = str + ",ID ";
        foreach (DataRow row in (InternalDataCollectionBase) this.GetPage("V_Consortia_Users", queryWhere, page, size, "*", fdOreder, "ID", ref total).Rows)
        {
          ConsortiaUserInfo consortiaUserInfo = new ConsortiaUserInfo()
          {
            ID = (int) row["ID"],
            ConsortiaID = (int) row["ConsortiaID"],
            DutyID = (int) row["DutyID"],
            DutyName = row["DutyName"].ToString(),
            IsExist = (bool) row["IsExist"],
            RatifierID = (int) row["RatifierID"],
            RatifierName = row["RatifierName"].ToString(),
            Remark = row["Remark"].ToString(),
            UserID = (int) row["UserID"],
            UserName = row["UserName"].ToString(),
            Grade = (int) row["Grade"],
            GP = (int) row["GP"],
            Repute = (int) row["Repute"],
            State = (int) row["State"],
            Right = (int) row["Right"],
            Offer = (int) row["Offer"],
            Colors = row["Colors"].ToString(),
            Style = row["Style"].ToString(),
            Hide = (int) row["Hide"]
          };
          consortiaUserInfo.Skin = row["Skin"] == null ? "" : consortiaUserInfo.Skin;
          consortiaUserInfo.Level = (int) row["Level"];
          consortiaUserInfo.LastDate = (DateTime) row["LastDate"];
          consortiaUserInfo.Sex = (bool) row["Sex"];
          consortiaUserInfo.IsBanChat = (bool) row["IsBanChat"];
          consortiaUserInfo.Win = (int) row["Win"];
          consortiaUserInfo.Total = (int) row["Total"];
          consortiaUserInfo.Escape = (int) row["Escape"];
          consortiaUserInfo.RichesOffer = (int) row["RichesOffer"];
          consortiaUserInfo.RichesRob = (int) row["RichesRob"];
          consortiaUserInfo.AchievementPoint = (int) row["AchievementPoint"];
          consortiaUserInfo.honor = (string) row["Honor"];
          consortiaUserInfo.UseOffer = (int) row["RichesOffer"] + (int) row["RichesRob"];
          consortiaUserInfo.LoginName = row["LoginName"] == null ? "" : row["LoginName"].ToString();
          consortiaUserInfo.Nimbus = (int) row["Nimbus"];
          consortiaUserInfo.FightPower = (int) row["FightPower"];
          consortiaUserInfo.typeVIP = Convert.ToByte(row["typeVIP"]);
          consortiaUserInfo.VIPLevel = (int) row["VIPLevel"];
          consortiaUserInfoList.Add(consortiaUserInfo);
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return consortiaUserInfoList.ToArray();
    }

    public bool PassConsortiaApplyAlly(
      int applyID,
      int userID,
      int consortiaID,
      ref int tempID,
      ref int state,
      ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[6]
        {
          new SqlParameter("@ID", (object) applyID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@tempID", (object) tempID),
          null,
          null
        };
        SqlParameters[3].Direction = ParameterDirection.InputOutput;
        SqlParameters[4] = new SqlParameter("@State", (object) state);
        SqlParameters[4].Direction = ParameterDirection.InputOutput;
        SqlParameters[5] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[5].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_ConsortiaApplyAlly_Pass", SqlParameters);
        int num = (int) SqlParameters[5].Value;
        if (num == 0)
        {
          flag = true;
          tempID = (int) SqlParameters[3].Value;
          state = (int) SqlParameters[4].Value;
        }
        if (num != 2)
        {
          if (num != 3)
            return flag;
          msg = "ConsortiaBussiness.PassConsortiaApplyAlly.Msg3";
          return flag;
        }
        msg = "ConsortiaBussiness.PassConsortiaApplyAlly.Msg2";
        return flag;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool PassConsortiaApplyUsers(
      int applyID,
      int userID,
      string userName,
      int consortiaID,
      ref string msg,
      ConsortiaUserInfo info,
      ref int consortiaRepute)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[24];
        SqlParameters[0] = new SqlParameter("@ID", (object) applyID);
        SqlParameters[1] = new SqlParameter("@UserID", (object) userID);
        SqlParameters[2] = new SqlParameter("@UserName", (object) userName);
        SqlParameters[3] = new SqlParameter("@ConsortiaID", (object) consortiaID);
        SqlParameters[4] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[4].Direction = ParameterDirection.ReturnValue;
        SqlParameters[5] = new SqlParameter("@tempID", SqlDbType.Int);
        SqlParameters[5].Direction = ParameterDirection.InputOutput;
        SqlParameters[5].Value = (object) info.UserID;
        SqlParameters[6] = new SqlParameter("@tempName", SqlDbType.NVarChar, 100);
        SqlParameters[6].Direction = ParameterDirection.InputOutput;
        SqlParameters[6].Value = (object) "";
        SqlParameters[7] = new SqlParameter("@tempDutyID", SqlDbType.Int);
        SqlParameters[7].Direction = ParameterDirection.InputOutput;
        SqlParameters[7].Value = (object) info.DutyID;
        SqlParameters[8] = new SqlParameter("@tempDutyName", SqlDbType.NVarChar, 100);
        SqlParameters[8].Direction = ParameterDirection.InputOutput;
        SqlParameters[8].Value = (object) "";
        SqlParameters[9] = new SqlParameter("@tempOffer", SqlDbType.Int);
        SqlParameters[9].Direction = ParameterDirection.InputOutput;
        SqlParameters[9].Value = (object) info.Offer;
        SqlParameters[10] = new SqlParameter("@tempRichesOffer", SqlDbType.Int);
        SqlParameters[10].Direction = ParameterDirection.InputOutput;
        SqlParameters[10].Value = (object) info.RichesOffer;
        SqlParameters[11] = new SqlParameter("@tempRichesRob", SqlDbType.Int);
        SqlParameters[11].Direction = ParameterDirection.InputOutput;
        SqlParameters[11].Value = (object) info.RichesRob;
        SqlParameters[12] = new SqlParameter("@tempLastDate", SqlDbType.DateTime);
        SqlParameters[12].Direction = ParameterDirection.InputOutput;
        SqlParameters[12].Value = (object) DateTime.Now;
        SqlParameters[13] = new SqlParameter("@tempWin", SqlDbType.Int);
        SqlParameters[13].Direction = ParameterDirection.InputOutput;
        SqlParameters[13].Value = (object) info.Win;
        SqlParameters[14] = new SqlParameter("@tempTotal", SqlDbType.Int);
        SqlParameters[14].Direction = ParameterDirection.InputOutput;
        SqlParameters[14].Value = (object) info.Total;
        SqlParameters[15] = new SqlParameter("@tempEscape", SqlDbType.Int);
        SqlParameters[15].Direction = ParameterDirection.InputOutput;
        SqlParameters[15].Value = (object) info.Escape;
        SqlParameters[16] = new SqlParameter("@tempGrade", SqlDbType.Int);
        SqlParameters[16].Direction = ParameterDirection.InputOutput;
        SqlParameters[16].Value = (object) info.Grade;
        SqlParameters[17] = new SqlParameter("@tempLevel", SqlDbType.Int);
        SqlParameters[17].Direction = ParameterDirection.InputOutput;
        SqlParameters[17].Value = (object) info.Level;
        SqlParameters[18] = new SqlParameter("@tempCUID", SqlDbType.Int);
        SqlParameters[18].Direction = ParameterDirection.InputOutput;
        SqlParameters[18].Value = (object) info.ID;
        SqlParameters[19] = new SqlParameter("@tempState", SqlDbType.Int);
        SqlParameters[19].Direction = ParameterDirection.InputOutput;
        SqlParameters[19].Value = (object) info.State;
        SqlParameters[20] = new SqlParameter("@tempSex", SqlDbType.Bit);
        SqlParameters[20].Direction = ParameterDirection.InputOutput;
        SqlParameters[20].Value = (object) info.Sex;
        SqlParameters[21] = new SqlParameter("@tempDutyRight", SqlDbType.Int);
        SqlParameters[21].Direction = ParameterDirection.InputOutput;
        SqlParameters[21].Value = (object) info.Right;
        SqlParameters[22] = new SqlParameter("@tempConsortiaRepute", SqlDbType.Int);
        SqlParameters[22].Direction = ParameterDirection.InputOutput;
        SqlParameters[22].Value = (object) consortiaRepute;
        SqlParameters[23] = new SqlParameter("@tempLoginName", SqlDbType.NVarChar, 200);
        SqlParameters[23].Direction = ParameterDirection.InputOutput;
        SqlParameters[23].Value = (object) consortiaRepute;
        this.db.RunProcedure("SP_ConsortiaApplyUser_Pass", SqlParameters);
        int num = (int) SqlParameters[4].Value;
        flag = num == 0;
        if (flag)
        {
          info.UserID = (int) SqlParameters[5].Value;
          info.UserName = SqlParameters[6].Value.ToString();
          info.DutyID = (int) SqlParameters[7].Value;
          info.DutyName = SqlParameters[8].Value.ToString();
          info.Offer = (int) SqlParameters[9].Value;
          info.RichesOffer = (int) SqlParameters[10].Value;
          info.RichesRob = (int) SqlParameters[11].Value;
          info.LastDate = (DateTime) SqlParameters[12].Value;
          info.Win = (int) SqlParameters[13].Value;
          info.Total = (int) SqlParameters[14].Value;
          info.Escape = (int) SqlParameters[15].Value;
          info.Grade = (int) SqlParameters[16].Value;
          info.Level = (int) SqlParameters[17].Value;
          info.ID = (int) SqlParameters[18].Value;
          info.State = (int) SqlParameters[19].Value;
          info.Sex = (bool) SqlParameters[20].Value;
          info.Right = (int) SqlParameters[21].Value;
          consortiaRepute = (int) SqlParameters[22].Value;
          info.LoginName = SqlParameters[23].Value.ToString();
        }
        switch (num)
        {
          case 2:
            msg = "ConsortiaBussiness.PassConsortiaApplyUsers.Msg2";
            return flag;
          case 3:
            msg = "ConsortiaBussiness.PassConsortiaApplyUsers.Msg3";
            return flag;
          case 4:
          case 5:
            return flag;
          case 6:
            msg = "ConsortiaBussiness.PassConsortiaApplyUsers.Msg6";
            return flag;
          default:
            return flag;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool PassConsortiaInviteUsers(
      int inviteID,
      int userID,
      string userName,
      ref int consortiaID,
      ref string consortiaName,
      ref string msg,
      ConsortiaUserInfo info,
      ref int tempID,
      ref string tempName,
      ref int consortiaRepute)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[24];
        SqlParameters[0] = new SqlParameter("@ID", (object) inviteID);
        SqlParameters[1] = new SqlParameter("@UserID", (object) userID);
        SqlParameters[2] = new SqlParameter("@UserName", (object) userName);
        SqlParameters[3] = new SqlParameter("@ConsortiaID", (object) consortiaID);
        SqlParameters[3].Direction = ParameterDirection.InputOutput;
        SqlParameters[4] = new SqlParameter("@ConsortiaName", SqlDbType.NVarChar, 100);
        SqlParameters[4].Value = (object) consortiaName;
        SqlParameters[4].Direction = ParameterDirection.InputOutput;
        SqlParameters[5] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[5].Direction = ParameterDirection.ReturnValue;
        SqlParameters[6] = new SqlParameter("@tempName", SqlDbType.NVarChar, 100);
        SqlParameters[6].Direction = ParameterDirection.InputOutput;
        SqlParameters[6].Value = (object) tempName;
        SqlParameters[7] = new SqlParameter("@tempDutyID", SqlDbType.Int);
        SqlParameters[7].Direction = ParameterDirection.InputOutput;
        SqlParameters[7].Value = (object) info.DutyID;
        SqlParameters[8] = new SqlParameter("@tempDutyName", SqlDbType.NVarChar, 100);
        SqlParameters[8].Direction = ParameterDirection.InputOutput;
        SqlParameters[8].Value = (object) "";
        SqlParameters[9] = new SqlParameter("@tempOffer", SqlDbType.Int);
        SqlParameters[9].Direction = ParameterDirection.InputOutput;
        SqlParameters[9].Value = (object) info.Offer;
        SqlParameters[10] = new SqlParameter("@tempRichesOffer", SqlDbType.Int);
        SqlParameters[10].Direction = ParameterDirection.InputOutput;
        SqlParameters[10].Value = (object) info.RichesOffer;
        SqlParameters[11] = new SqlParameter("@tempRichesRob", SqlDbType.Int);
        SqlParameters[11].Direction = ParameterDirection.InputOutput;
        SqlParameters[11].Value = (object) info.RichesRob;
        SqlParameters[12] = new SqlParameter("@tempLastDate", SqlDbType.DateTime);
        SqlParameters[12].Direction = ParameterDirection.InputOutput;
        SqlParameters[12].Value = (object) DateTime.Now;
        SqlParameters[13] = new SqlParameter("@tempWin", SqlDbType.Int);
        SqlParameters[13].Direction = ParameterDirection.InputOutput;
        SqlParameters[13].Value = (object) info.Win;
        SqlParameters[14] = new SqlParameter("@tempTotal", SqlDbType.Int);
        SqlParameters[14].Direction = ParameterDirection.InputOutput;
        SqlParameters[14].Value = (object) info.Total;
        SqlParameters[15] = new SqlParameter("@tempEscape", SqlDbType.Int);
        SqlParameters[15].Direction = ParameterDirection.InputOutput;
        SqlParameters[15].Value = (object) info.Escape;
        SqlParameters[16] = new SqlParameter("@tempID", SqlDbType.Int);
        SqlParameters[16].Direction = ParameterDirection.InputOutput;
        SqlParameters[16].Value = (object) tempID;
        SqlParameters[17] = new SqlParameter("@tempGrade", SqlDbType.Int);
        SqlParameters[17].Direction = ParameterDirection.InputOutput;
        SqlParameters[17].Value = (object) info.Level;
        SqlParameters[18] = new SqlParameter("@tempLevel", SqlDbType.Int);
        SqlParameters[18].Direction = ParameterDirection.InputOutput;
        SqlParameters[18].Value = (object) info.Level;
        SqlParameters[19] = new SqlParameter("@tempCUID", SqlDbType.Int);
        SqlParameters[19].Direction = ParameterDirection.InputOutput;
        SqlParameters[19].Value = (object) info.ID;
        SqlParameters[20] = new SqlParameter("@tempState", SqlDbType.Int);
        SqlParameters[20].Direction = ParameterDirection.InputOutput;
        SqlParameters[20].Value = (object) info.State;
        SqlParameters[21] = new SqlParameter("@tempSex", SqlDbType.Bit);
        SqlParameters[21].Direction = ParameterDirection.InputOutput;
        SqlParameters[21].Value = (object) info.Sex;
        SqlParameters[22] = new SqlParameter("@tempRight", SqlDbType.Int);
        SqlParameters[22].Direction = ParameterDirection.InputOutput;
        SqlParameters[22].Value = (object) info.Right;
        SqlParameters[23] = new SqlParameter("@tempConsortiaRepute", SqlDbType.Int);
        SqlParameters[23].Direction = ParameterDirection.InputOutput;
        SqlParameters[23].Value = (object) consortiaRepute;
        this.db.RunProcedure("SP_ConsortiaInviteUser_Pass", SqlParameters);
        int num = (int) SqlParameters[5].Value;
        flag = num == 0;
        if (flag)
        {
          consortiaID = (int) SqlParameters[3].Value;
          consortiaName = SqlParameters[4].Value.ToString();
          tempName = SqlParameters[6].Value.ToString();
          info.DutyID = (int) SqlParameters[7].Value;
          info.DutyName = SqlParameters[8].Value.ToString();
          info.Offer = (int) SqlParameters[9].Value;
          info.RichesOffer = (int) SqlParameters[10].Value;
          info.RichesRob = (int) SqlParameters[11].Value;
          info.LastDate = (DateTime) SqlParameters[12].Value;
          info.Win = (int) SqlParameters[13].Value;
          info.Total = (int) SqlParameters[14].Value;
          info.Escape = (int) SqlParameters[15].Value;
          tempID = (int) SqlParameters[16].Value;
          info.Grade = (int) SqlParameters[17].Value;
          info.Level = (int) SqlParameters[18].Value;
          info.ID = (int) SqlParameters[19].Value;
          info.State = (int) SqlParameters[20].Value;
          info.Sex = (bool) SqlParameters[21].Value;
          info.Right = (int) SqlParameters[22].Value;
          consortiaRepute = (int) SqlParameters[23].Value;
        }
        switch (num)
        {
          case 3:
            msg = "ConsortiaBussiness.PassConsortiaInviteUsers.Msg3";
            goto label_9;
          case 6:
            msg = "ConsortiaBussiness.PassConsortiaInviteUsers.Msg6";
            break;
        }
        return flag;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
label_9:
      return flag;
    }

    public bool RenameConsortia(int ConsortiaID, string nickName, string newNickName)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[4]
        {
          new SqlParameter("@ConsortiaID", (object) ConsortiaID),
          new SqlParameter("@NickName", (object) nickName),
          new SqlParameter("@NewNickName", (object) newNickName),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[3].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_Users_RenameConsortia", SqlParameters);
        flag = (int) SqlParameters[3].Value == 0;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "RenameNick", ex);
      }
      return flag;
    }

    public bool RenameConsortiaName(
      string userName,
      string nickName,
      string consortiaName,
      ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[4]
        {
          new SqlParameter("@UserName", (object) userName),
          new SqlParameter("@NickName", (object) nickName),
          new SqlParameter("@ConsortiaName", (object) consortiaName),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[3].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_Users_RenameConsortiaName", SqlParameters);
        int num = (int) SqlParameters[3].Value;
        flag = num == 0;
        if ((uint) (num - 4) > 1U)
          return flag;
        msg = LanguageMgr.GetTranslation("PlayerBussiness.SP_Users_RenameConsortiaName.Msg4");
        return flag;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "RenameNick", ex);
      }
      return flag;
    }

    public bool ScanConsortia(ref string noticeID)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[2]
        {
          new SqlParameter("@NoticeID", SqlDbType.NVarChar, 4000),
          null
        };
        SqlParameters[0].Direction = ParameterDirection.Output;
        SqlParameters[1] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[1].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_Consortia_Scan", SqlParameters);
        flag = (int) SqlParameters[1].Value == 0;
        if (flag)
          noticeID = SqlParameters[0].Value.ToString();
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpdateConsortiaChairman(
      string nickName,
      int consortiaID,
      int userID,
      ref string msg,
      ref ConsortiaDutyInfo info,
      ref int tempUserID,
      ref string tempUserName)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[9];
        SqlParameters[0] = new SqlParameter("@NickName", (object) nickName);
        SqlParameters[1] = new SqlParameter("@ConsortiaID", (object) consortiaID);
        SqlParameters[2] = new SqlParameter("@UserID", (object) userID);
        SqlParameters[3] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[3].Direction = ParameterDirection.ReturnValue;
        SqlParameters[4] = new SqlParameter("@tempUserID", SqlDbType.Int);
        SqlParameters[4].Direction = ParameterDirection.InputOutput;
        SqlParameters[4].Value = (object) tempUserID;
        SqlParameters[5] = new SqlParameter("@tempUserName", SqlDbType.NVarChar, 100);
        SqlParameters[5].Direction = ParameterDirection.InputOutput;
        SqlParameters[5].Value = (object) tempUserName;
        SqlParameters[6] = new SqlParameter("@tempDutyLevel", SqlDbType.Int);
        SqlParameters[6].Direction = ParameterDirection.InputOutput;
        SqlParameters[6].Value = (object) info.Level;
        SqlParameters[7] = new SqlParameter("@tempDutyName", SqlDbType.NVarChar, 100);
        SqlParameters[7].Direction = ParameterDirection.InputOutput;
        SqlParameters[7].Value = (object) "";
        SqlParameters[8] = new SqlParameter("@tempRight", SqlDbType.Int);
        SqlParameters[8].Direction = ParameterDirection.InputOutput;
        SqlParameters[8].Value = (object) info.Right;
        flag = this.db.RunProcedure("SP_ConsortiaChangeChairman", SqlParameters);
        int num = (int) SqlParameters[3].Value;
        flag = num == 0;
        if (flag)
        {
          tempUserID = (int) SqlParameters[4].Value;
          tempUserName = SqlParameters[5].Value.ToString();
          info.Level = (int) SqlParameters[6].Value;
          info.DutyName = SqlParameters[7].Value.ToString();
          info.Right = (int) SqlParameters[8].Value;
        }
        if (num != 1)
        {
          if (num != 2)
            return flag;
          msg = "ConsortiaBussiness.UpdateConsortiaChairman.Msg2";
          return flag;
        }
        msg = "ConsortiaBussiness.UpdateConsortiaChairman.Msg3";
        return flag;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpdateConsortiaDescription(
      int consortiaID,
      int userID,
      string description,
      ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[4]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Description", (object) description),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[3].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_ConsortiaDescription_Update", SqlParameters);
        int num = (int) SqlParameters[3].Value;
        flag = num == 0;
        if (num == 2)
          msg = "ConsortiaBussiness.UpdateConsortiaDescription.Msg2";
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpdateConsortiaDuty(
      ConsortiaDutyInfo info,
      int userID,
      int updateType,
      ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[8];
        SqlParameters[0] = new SqlParameter("@DutyID", (object) info.DutyID);
        SqlParameters[0].Direction = ParameterDirection.InputOutput;
        SqlParameters[1] = new SqlParameter("@ConsortiaID", (object) info.ConsortiaID);
        SqlParameters[2] = new SqlParameter("@DutyName", SqlDbType.NVarChar, 100);
        SqlParameters[2].Direction = ParameterDirection.InputOutput;
        SqlParameters[2].Value = (object) info.DutyName;
        SqlParameters[3] = new SqlParameter("@Right", SqlDbType.Int);
        SqlParameters[3].Direction = ParameterDirection.InputOutput;
        SqlParameters[3].Value = (object) info.Right;
        SqlParameters[4] = new SqlParameter("@Level", SqlDbType.Int);
        SqlParameters[4].Direction = ParameterDirection.InputOutput;
        SqlParameters[4].Value = (object) info.Level;
        SqlParameters[5] = new SqlParameter("@UserID", (object) userID);
        SqlParameters[6] = new SqlParameter("@UpdateType", (object) updateType);
        SqlParameters[7] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[7].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_ConsortiaDuty_Update", SqlParameters);
        int num = (int) SqlParameters[7].Value;
        flag = num == 0;
        if (flag)
        {
          info.DutyID = (int) SqlParameters[0].Value;
          info.DutyName = SqlParameters[2].Value == null ? "" : SqlParameters[2].Value.ToString();
          info.Right = (int) SqlParameters[3].Value;
          info.Level = (int) SqlParameters[4].Value;
        }
        switch (num)
        {
          case 2:
            msg = "ConsortiaBussiness.UpdateConsortiaDuty.Msg2";
            return flag;
          case 3:
          case 4:
            msg = "ConsortiaBussiness.UpdateConsortiaDuty.Msg3";
            return flag;
          case 5:
            msg = "ConsortiaBussiness.DeleteConsortiaDuty.Msg5";
            return flag;
          default:
            return flag;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpdateConsortiaIsBanChat(
      int banUserID,
      int consortiaID,
      int userID,
      bool isBanChat,
      ref int tempID,
      ref string tempName,
      ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[7]
        {
          new SqlParameter("@ID", (object) banUserID),
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@IsBanChat", (object) isBanChat),
          new SqlParameter("@TempID", (object) tempID),
          null,
          null
        };
        SqlParameters[4].Direction = ParameterDirection.InputOutput;
        SqlParameters[5] = new SqlParameter("@TempName", SqlDbType.NVarChar, 100);
        SqlParameters[5].Value = (object) tempName;
        SqlParameters[5].Direction = ParameterDirection.InputOutput;
        SqlParameters[6] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[6].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_ConsortiaIsBanChat_Update", SqlParameters);
        int num = (int) SqlParameters[6].Value;
        tempID = (int) SqlParameters[4].Value;
        tempName = SqlParameters[5].Value.ToString();
        flag = num == 0;
        if (num != 2)
        {
          if (num != 3)
            return flag;
          msg = "ConsortiaBussiness.UpdateConsortiaIsBanChat.Msg3";
          return flag;
        }
        msg = "ConsortiaBussiness.UpdateConsortiaIsBanChat.Msg2";
        return flag;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpdateConsortiaPlacard(
      int consortiaID,
      int userID,
      string placard,
      ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[4]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Placard", (object) placard),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[3].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_ConsortiaPlacard_Update", SqlParameters);
        int num = (int) SqlParameters[3].Value;
        flag = num == 0;
        if (num == 2)
          msg = "ConsortiaBussiness.UpdateConsortiaPlacard.Msg2";
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpdateConsortiaRiches(int consortiaID, int userID, int Riches, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[4]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Riches", (object) Riches),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[3].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_ConsortiaRiches_Update", SqlParameters);
        int num = (int) SqlParameters[3].Value;
        flag = num == 0;
        if (num == 2)
          msg = "ConsortiaBussiness.UpdateConsortiaRiches.Msg2";
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpdateConsortiaUserGrade(
      int id,
      int consortiaID,
      int userID,
      bool upGrade,
      ref string msg,
      ref ConsortiaDutyInfo info,
      ref string tempUserName)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[9]
        {
          new SqlParameter("@ID", (object) id),
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@UpGrade", (object) upGrade),
          new SqlParameter("@Result", SqlDbType.Int),
          null,
          null,
          null,
          null
        };
        SqlParameters[4].Direction = ParameterDirection.ReturnValue;
        SqlParameters[5] = new SqlParameter("@tempUserName", SqlDbType.NVarChar, 100);
        SqlParameters[5].Direction = ParameterDirection.InputOutput;
        SqlParameters[5].Value = (object) tempUserName;
        SqlParameters[6] = new SqlParameter("@tempDutyLevel", SqlDbType.Int);
        SqlParameters[6].Direction = ParameterDirection.InputOutput;
        SqlParameters[6].Value = (object) info.Level;
        SqlParameters[7] = new SqlParameter("@tempDutyName", SqlDbType.NVarChar, 100);
        SqlParameters[7].Direction = ParameterDirection.InputOutput;
        SqlParameters[7].Value = (object) "";
        SqlParameters[8] = new SqlParameter("@tempRight", SqlDbType.Int);
        SqlParameters[8].Direction = ParameterDirection.InputOutput;
        SqlParameters[8].Value = (object) info.Right;
        flag = this.db.RunProcedure("SP_ConsortiaUserGrade_Update", SqlParameters);
        int num = (int) SqlParameters[4].Value;
        flag = num == 0;
        if (flag)
        {
          tempUserName = SqlParameters[5].Value.ToString();
          info.Level = (int) SqlParameters[6].Value;
          info.DutyName = SqlParameters[7].Value.ToString();
          info.Right = (int) SqlParameters[8].Value;
        }
        switch (num)
        {
          case 2:
            msg = "ConsortiaBussiness.UpdateConsortiaUserGrade.Msg2";
            return flag;
          case 3:
            msg = upGrade ? "ConsortiaBussiness.UpdateConsortiaUserGrade.Msg3" : "ConsortiaBussiness.UpdateConsortiaUserGrade.Msg10";
            return flag;
          case 4:
            msg = "ConsortiaBussiness.UpdateConsortiaUserGrade.Msg4";
            return flag;
          case 5:
            msg = "ConsortiaBussiness.UpdateConsortiaUserGrade.Msg5";
            return flag;
          default:
            return flag;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpdateConsortiaUserRemark(
      int id,
      int consortiaID,
      int userID,
      string remark,
      ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[5]
        {
          new SqlParameter("@ID", (object) id),
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Remark", (object) remark),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[4].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_ConsortiaUserRemark_Update", SqlParameters);
        int num = (int) SqlParameters[4].Value;
        flag = num == 0;
        if (num == 2)
          msg = "ConsortiaBussiness.UpdateConsortiaUserRemark.Msg2";
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpdateConsotiaApplyState(int consortiaID, int userID, bool state, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[4]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@State", (object) state),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[3].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_Consortia_Apply_State", SqlParameters);
        int num = (int) SqlParameters[3].Value;
        flag = num == 0;
        if (num == 2)
          msg = "ConsortiaBussiness.UpdateConsotiaApplyState.Msg2";
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpGradeConsortia(int consortiaID, int userID, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[3]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[2].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_Consortia_UpGrade", SqlParameters);
        int num = (int) SqlParameters[2].Value;
        flag = num == 0;
        switch (num)
        {
          case 2:
            msg = "ConsortiaBussiness.UpGradeConsortia.Msg2";
            return flag;
          case 3:
            msg = "ConsortiaBussiness.UpGradeConsortia.Msg3";
            return flag;
          case 4:
            msg = "ConsortiaBussiness.UpGradeConsortia.Msg4";
            return flag;
          default:
            return flag;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpGradeShopConsortia(int consortiaID, int userID, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[3]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[2].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_Consortia_Shop_UpGrade", SqlParameters);
        int num = (int) SqlParameters[2].Value;
        flag = num == 0;
        switch (num)
        {
          case 2:
            msg = "ConsortiaBussiness.UpGradeShopConsortia.Msg2";
            return flag;
          case 3:
            msg = "ConsortiaBussiness.UpGradeShopConsortia.Msg3";
            return flag;
          case 4:
            msg = "ConsortiaBussiness.UpGradeShopConsortia.Msg4";
            return flag;
          default:
            return flag;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpGradeSkillConsortia(int consortiaID, int userID, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[3]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[2].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_Consortia_Skill_UpGrade", SqlParameters);
        int num = (int) SqlParameters[2].Value;
        flag = num == 0;
        switch (num)
        {
          case 2:
            msg = "ConsortiaBussiness.UpGradeSkillConsortia.Msg2";
            return flag;
          case 3:
            msg = "ConsortiaBussiness.UpGradeSkillConsortia.Msg3";
            return flag;
          case 4:
            msg = "ConsortiaBussiness.UpGradeSkillConsortia.Msg4";
            return flag;
          default:
            return flag;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpGradeSmithConsortia(int consortiaID, int userID, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[3]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[2].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_Consortia_Smith_UpGrade", SqlParameters);
        int num = (int) SqlParameters[2].Value;
        flag = num == 0;
        switch (num)
        {
          case 2:
            msg = "ConsortiaBussiness.UpGradeSmithConsortia.Msg2";
            return flag;
          case 3:
            msg = "ConsortiaBussiness.UpGradeSmithConsortia.Msg3";
            return flag;
          case 4:
            msg = "ConsortiaBussiness.UpGradeSmithConsortia.Msg4";
            return flag;
          default:
            return flag;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public bool UpGradeStoreConsortia(int consortiaID, int userID, ref string msg)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[3]
        {
          new SqlParameter("@ConsortiaID", (object) consortiaID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[2].Direction = ParameterDirection.ReturnValue;
        this.db.RunProcedure("SP_Consortia_Store_UpGrade", SqlParameters);
        int num = (int) SqlParameters[2].Value;
        flag = num == 0;
        switch (num)
        {
          case 2:
            msg = "ConsortiaBussiness.UpGradeStoreConsortia.Msg2";
            return flag;
          case 3:
            msg = "ConsortiaBussiness.UpGradeStoreConsortia.Msg3";
            return flag;
          case 4:
            msg = "ConsortiaBussiness.UpGradeStoreConsortia.Msg4";
            return flag;
          default:
            return flag;
        }
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return flag;
    }

    public ConsortiaInfo[] UpdateConsortiaFightPower()
    {
      List<ConsortiaInfo> consortiaInfoList = new List<ConsortiaInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_Consortia_Update_FightPower");
        while (ResultDataReader.Read())
          consortiaInfoList.Add(new ConsortiaInfo()
          {
            ConsortiaID = (int) ResultDataReader["ConsortiaID"],
            BuildDate = (DateTime) ResultDataReader["BuildDate"],
            CelebCount = (int) ResultDataReader["CelebCount"],
            ChairmanID = (int) ResultDataReader["ChairmanID"],
            ChairmanName = ResultDataReader["ChairmanName"].ToString(),
            ConsortiaName = ResultDataReader["ConsortiaName"].ToString(),
            CreatorID = (int) ResultDataReader["CreatorID"],
            CreatorName = ResultDataReader["CreatorName"].ToString(),
            Description = ResultDataReader["Description"].ToString(),
            Honor = (int) ResultDataReader["Honor"],
            IsExist = (bool) ResultDataReader["IsExist"],
            Level = (int) ResultDataReader["Level"],
            MaxCount = (int) ResultDataReader["MaxCount"],
            Placard = ResultDataReader["Placard"].ToString(),
            IP = ResultDataReader["IP"].ToString(),
            Port = (int) ResultDataReader["Port"],
            Repute = (int) ResultDataReader["Repute"],
            Count = (int) ResultDataReader["Count"],
            Riches = (int) ResultDataReader["Riches"],
            FightPower = (int) ResultDataReader["FightPower"],
            DeductDate = (DateTime) ResultDataReader["DeductDate"],
            AddDayHonor = (int) ResultDataReader["AddDayHonor"],
            AddDayRiches = (int) ResultDataReader["AddDayRiches"],
            AddWeekHonor = (int) ResultDataReader["AddWeekHonor"],
            AddWeekRiches = (int) ResultDataReader["AddWeekRiches"],
            LastDayRiches = (int) ResultDataReader["LastDayRiches"],
            OpenApply = (bool) ResultDataReader["OpenApply"],
            StoreLevel = (int) ResultDataReader["StoreLevel"],
            SmithLevel = (int) ResultDataReader["SmithLevel"],
            ShopLevel = (int) ResultDataReader["ShopLevel"],
            SkillLevel = (int) ResultDataReader["SkillLevel"],
            BadgeBuyTime = ResultDataReader["BadgeBuyTime"] == DBNull.Value ? "" : ResultDataReader["BadgeBuyTime"].ToString(),
            BadgeID = (int) ResultDataReader["BadgeID"],
            ValidDate = (int) ResultDataReader["ValidDate"]
          });
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
      return consortiaInfoList.ToArray();
    }

    public bool ConsortiaTaskUpdateDate(int consortiID, DateTime dateTask)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[3]
        {
          new SqlParameter("@ConsortiaID", (object) consortiID),
          new SqlParameter("@DateOpenTask", SqlDbType.DateTime),
          null
        };
        SqlParameters[1].Value = (object) dateTask;
        SqlParameters[2] = new SqlParameter("@Result", SqlDbType.Int);
        SqlParameters[2].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_Consortia_UpdateTimeOpenTask", SqlParameters);
        flag = (int) SqlParameters[2].Value == 0;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) nameof (ConsortiaTaskUpdateDate), ex);
      }
      return flag;
    }
  }
}
