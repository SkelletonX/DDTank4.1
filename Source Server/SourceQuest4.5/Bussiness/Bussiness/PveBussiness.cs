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
			List<PveInfo> list = new List<PveInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				db.GetReader(ref resultDataReader, "SP_PveInfos_All");
				while (resultDataReader.Read())
				{
					PveInfo item = new PveInfo
					{
						ID = (int)resultDataReader["Id"],
						Name = ((resultDataReader["Name"] == null) ? "" : resultDataReader["Name"].ToString()),
						Type = (int)resultDataReader["Type"],
						LevelLimits = (int)resultDataReader["LevelLimits"],
						SimpleTemplateIds = ((resultDataReader["SimpleTemplateIds"] == null) ? "" : resultDataReader["SimpleTemplateIds"].ToString()),
						NormalTemplateIds = ((resultDataReader["NormalTemplateIds"] == null) ? "" : resultDataReader["NormalTemplateIds"].ToString()),
						HardTemplateIds = ((resultDataReader["HardTemplateIds"] == null) ? "" : resultDataReader["HardTemplateIds"].ToString()),
						TerrorTemplateIds = ((resultDataReader["TerrorTemplateIds"] == null) ? "" : resultDataReader["TerrorTemplateIds"].ToString()),
						Pic = ((resultDataReader["Pic"] == null) ? "" : resultDataReader["Pic"].ToString()),
						Description = ((resultDataReader["Description"] == null) ? "" : resultDataReader["Description"].ToString()),
						Ordering = (int)resultDataReader["Ordering"],
						AdviceTips = ((resultDataReader["AdviceTips"] == null) ? "" : resultDataReader["AdviceTips"].ToString()),
						SimpleGameScript = (resultDataReader["SimpleGameScript"] as string),
						NormalGameScript = (resultDataReader["NormalGameScript"] as string),
						HardGameScript = (resultDataReader["HardGameScript"] as string),
						TerrorGameScript = (resultDataReader["TerrorGameScript"] as string)
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("GetAllPveInfos", exception);
				}
			}
			finally
			{
				if (resultDataReader != null && !resultDataReader.IsClosed)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}
	}
}
