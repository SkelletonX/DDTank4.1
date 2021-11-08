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
			List<ActiveInfo> list = new List<ActiveInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				db.GetReader(ref resultDataReader, "SP_Active_All");
				while (resultDataReader.Read())
				{
					list.Add(InitActiveInfo(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Init", exception);
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

		public ActiveConvertItemInfo[] GetSingleActiveConvertItems(int activeID)
		{
			List<ActiveConvertItemInfo> list = new List<ActiveConvertItemInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[1]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = activeID;
				db.GetReader(ref resultDataReader, "SP_Active_Convert_Item_Info_Single", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add(InitActiveConvertItemInfo(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Init", exception);
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

		public ActiveInfo GetSingleActives(int activeID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[1]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = activeID;
				db.GetReader(ref resultDataReader, "SP_Active_Single", sqlParameters);
				if (resultDataReader.Read())
				{
					return InitActiveInfo(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Init", exception);
				}
			}
			finally
			{
				if (resultDataReader != null && !resultDataReader.IsClosed)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		public ActiveConvertItemInfo InitActiveConvertItemInfo(SqlDataReader reader)
		{
			return new ActiveConvertItemInfo
			{
				ID = (int)reader["ID"],
				ActiveID = (int)reader["ActiveID"],
				TemplateID = (int)reader["TemplateID"],
				ItemType = (int)reader["ItemType"],
				ItemCount = (int)reader["ItemCount"],
				LimitValue = (int)reader["LimitValue"],
				IsBind = (bool)reader["IsBind"],
				ValidDate = (int)reader["ValidDate"]
			};
		}

		public ActiveInfo InitActiveInfo(SqlDataReader reader)
		{
			ActiveInfo info = new ActiveInfo
			{
				ActiveID = (int)reader["ActiveID"],
				Description = ((reader["Description"] == null) ? "" : reader["Description"].ToString()),
				Content = ((reader["Content"] == null) ? "" : reader["Content"].ToString()),
				AwardContent = ((reader["AwardContent"] == null) ? "" : reader["AwardContent"].ToString()),
				HasKey = (int)reader["HasKey"]
			};
			if (!string.IsNullOrEmpty(reader["EndDate"].ToString()))
			{
				info.EndDate = (DateTime)reader["EndDate"];
			}
			info.IsOnly = (int)reader["IsOnly"];
			info.StartDate = (DateTime)reader["StartDate"];
			info.Title = reader["Title"].ToString();
			info.Type = (int)reader["Type"];
			info.ActiveType = (int)reader["ActiveType"];
			info.ActionTimeContent = ((reader["ActionTimeContent"] == null) ? "" : reader["ActionTimeContent"].ToString());
			info.IsAdvance = (bool)reader["IsAdvance"];
			info.GoodsExchangeTypes = ((reader["GoodsExchangeTypes"] == null) ? "" : reader["GoodsExchangeTypes"].ToString());
			info.GoodsExchangeNum = ((reader["GoodsExchangeNum"] == null) ? "" : reader["GoodsExchangeNum"].ToString());
			info.limitType = ((reader["limitType"] == null) ? "" : reader["limitType"].ToString());
			info.limitValue = ((reader["limitValue"] == null) ? "" : reader["limitValue"].ToString());
			info.IsShow = (bool)reader["IsShow"];
			info.IconID = (int)reader["IconID"];
			return info;
		}
	}
}
