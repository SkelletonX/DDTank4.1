// Decompiled with JetBrains decompiler
// Type: Bussiness.PveBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Bussiness
{
  public class PveBussiness : BaseCrossBussiness
  {
    public PveInfo[] GetAllPveInfos()
    {
      List<PveInfo> pveInfoList = new List<PveInfo>();
      SqlDataReader ResultDataReader = (SqlDataReader) null;
      try
      {
        this.db.GetReader(ref ResultDataReader, "SP_PveInfos_All");
        while (ResultDataReader.Read())
        {
          PveInfo pveInfo = new PveInfo()
          {
            ID = (int) ResultDataReader["Id"],
            Name = ResultDataReader["Name"] == null ? "" : ResultDataReader["Name"].ToString(),
            Type = (int) ResultDataReader["Type"],
            LevelLimits = (int) ResultDataReader["LevelLimits"],
            SimpleTemplateIds = ResultDataReader["SimpleTemplateIds"] == null ? "" : ResultDataReader["SimpleTemplateIds"].ToString(),
            NormalTemplateIds = ResultDataReader["NormalTemplateIds"] == null ? "" : ResultDataReader["NormalTemplateIds"].ToString(),
            HardTemplateIds = ResultDataReader["HardTemplateIds"] == null ? "" : ResultDataReader["HardTemplateIds"].ToString(),
            TerrorTemplateIds = ResultDataReader["TerrorTemplateIds"] == null ? "" : ResultDataReader["TerrorTemplateIds"].ToString(),
            Pic = ResultDataReader["Pic"] == null ? "" : ResultDataReader["Pic"].ToString(),
            Description = ResultDataReader["Description"] == null ? "" : ResultDataReader["Description"].ToString(),
            Ordering = (int) ResultDataReader["Ordering"],
            AdviceTips = ResultDataReader["AdviceTips"] == null ? "" : ResultDataReader["AdviceTips"].ToString(),
            SimpleGameScript = ResultDataReader["SimpleGameScript"] as string,
            NormalGameScript = ResultDataReader["NormalGameScript"] as string,
            HardGameScript = ResultDataReader["HardGameScript"] as string,
            TerrorGameScript = ResultDataReader["TerrorGameScript"] as string
          };
          pveInfoList.Add(pveInfo);
        }
      }
      catch (Exception ex)
      {
        if (this.log.IsErrorEnabled)
          this.log.Error((object) nameof (GetAllPveInfos), ex);
      }
      finally
      {
        if (ResultDataReader != null && !ResultDataReader.IsClosed)
          ResultDataReader.Close();
      }
      return pveInfoList.ToArray();
    }
  }
}
