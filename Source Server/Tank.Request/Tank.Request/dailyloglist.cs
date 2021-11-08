using Bussiness;
using log4net;
using SqlDataProvider.BaseClass;
using SqlDataProvider.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[WebService(Namespace = "http://tempuri.org/")]
	public class dailyloglist : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected Sql_DbObject db = new Sql_DbObject("AppConfig", "conString");

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string value = "Fail!";
			XElement xElement = new XElement("Result");
			try
			{
				_ = context.Request["key"];
				int userID = int.Parse(context.Request["selfid"]);
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					DailyLogListInfo dailyLogListInfo = playerBussiness.GetDailyLogListSingle(userID);
					if (dailyLogListInfo == null)
					{
						dailyLogListInfo = new DailyLogListInfo
						{
							UserID = userID,
							DayLog = "",
							UserAwardLog = 0,
							LastDate = DateTime.Now
						};
					}
					string text = dailyLogListInfo.DayLog;
					int num = dailyLogListInfo.UserAwardLog;
					DateTime lastDate = dailyLogListInfo.LastDate;
					char[] separator = new char[1]
					{
						','
					};
					int num2 = text.Split(separator).Length;
					int month = DateTime.Now.Month;
					int year = DateTime.Now.Year;
					int day = DateTime.Now.Day;
					int num3 = DateTime.DaysInMonth(year, month);
					if (month != lastDate.Month || year != lastDate.Year)
					{
						text = "";
						num = 0;
						lastDate = DateTime.Now;
					}
					if (num2 < num3)
					{
						if (string.IsNullOrEmpty(text) && num2 > 1)
						{
							text = "False";
						}
						for (int i = num2; i < day - 1; i++)
						{
							text += ",False";
						}
					}
					dailyLogListInfo.DayLog = text;
					dailyLogListInfo.UserAwardLog = num;
					dailyLogListInfo.LastDate = lastDate;
					playerBussiness.UpdateDailyLogList(dailyLogListInfo);
					XElement content = new XElement("DailyLogList", new XAttribute("UserAwardLog", num), new XAttribute("DayLog", text), new XAttribute("luckyNum", 0), new XAttribute("myLuckyNum", 0));
					xElement.Add(content);
				}
				flag = true;
				value = "Success!";
			}
			catch (Exception exception)
			{
				log.Error("dailyloglist", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			xElement.Add(new XAttribute("nowDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			context.Response.ContentType = "text/plain";
			context.Response.BinaryWrite(StaticFunction.Compress(xElement.ToString(check: false)));
		}

		public bool UpdateDailyLogList(DailyLogListInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] array = new SqlParameter[5]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@UserAwardLog", info.UserAwardLog),
					new SqlParameter("@DayLog", info.DayLog),
					new SqlParameter("@LastDate", info.LastDate),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				array[4].Direction = ParameterDirection.ReturnValue;
				result = db.RunProcedure("SP_DailyLogList_Update", array);
				return result;
			}
			catch (Exception exception)
			{
				if (!log.IsErrorEnabled)
				{
					return result;
				}
				log.Error("SP_DailyLogList_Update", exception);
				return result;
			}
		}
	}
}
