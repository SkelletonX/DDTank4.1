// Decompiled with JetBrains decompiler
// Type: Bussiness.MapBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Bussiness
{
  public class MapBussiness : BaseCrossBussiness
  {
    public MapInfo[] GetAllMap()
    {
      List<MapInfo> mapInfoList = new List<MapInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_Maps_All");
        while (ResultDataReader.Read())
        {
          MapInfo mapInfo = new MapInfo()
          {
            BackMusic = ResultDataReader["BackMusic"] == null ? "" : ResultDataReader["BackMusic"].ToString(),
            BackPic = ResultDataReader["BackPic"] == null ? "" : ResultDataReader["BackPic"].ToString(),
            BackroundHeight = (int) ResultDataReader["BackroundHeight"],
            BackroundWidht = (int) ResultDataReader["BackroundWidht"],
            DeadHeight = (int) ResultDataReader["DeadHeight"],
            DeadPic = ResultDataReader["DeadPic"] == null ? "" : ResultDataReader["DeadPic"].ToString(),
            DeadWidth = (int) ResultDataReader["DeadWidth"],
            Description = ResultDataReader["Description"] == null ? "" : ResultDataReader["Description"].ToString(),
            DragIndex = (int) ResultDataReader["DragIndex"],
            ForegroundHeight = (int) ResultDataReader["ForegroundHeight"],
            ForegroundWidth = (int) ResultDataReader["ForegroundWidth"],
            ForePic = ResultDataReader["ForePic"] == null ? "" : ResultDataReader["ForePic"].ToString(),
            ID = (int) ResultDataReader["ID"],
            Name = ResultDataReader["Name"] == null ? "" : ResultDataReader["Name"].ToString(),
            Pic = ResultDataReader["Pic"] == null ? "" : ResultDataReader["Pic"].ToString(),
            Remark = ResultDataReader["Remark"] == null ? "" : ResultDataReader["Remark"].ToString(),
            Weight = (int) ResultDataReader["Weight"],
            PosX = ResultDataReader["PosX"] == null ? "" : ResultDataReader["PosX"].ToString(),
            PosX1 = ResultDataReader["PosX1"] == null ? "" : ResultDataReader["PosX1"].ToString(),
            Type = (byte) (int) ResultDataReader["Type"]
          };
          mapInfoList.Add(mapInfo);
        }
      }
      catch (Exception ex)
      {
        if (this.log.IsErrorEnabled)
          this.log.Error((object) nameof (GetAllMap), ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return mapInfoList.ToArray();
    }

    public ServerMapInfo[] GetAllServerMap()
    {
      List<ServerMapInfo> serverMapInfoList = new List<ServerMapInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_Maps_Server_All");
        while (ResultDataReader.Read())
        {
          ServerMapInfo serverMapInfo = new ServerMapInfo()
          {
            ServerID = (int) ResultDataReader["ServerID"],
            OpenMap = ResultDataReader["OpenMap"].ToString(),
            IsSpecial = (int) ResultDataReader["IsSpecial"]
          };
          serverMapInfoList.Add(serverMapInfo);
        }
      }
      catch (Exception ex)
      {
        if (this.log.IsErrorEnabled)
          this.log.Error((object) "GetAllMapWeek", ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return serverMapInfoList.ToArray();
    }
  }
}
