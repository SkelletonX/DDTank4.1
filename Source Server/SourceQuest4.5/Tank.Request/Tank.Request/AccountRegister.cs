using Bussiness;
using log4net;
using System;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace Tank.Request
{
	public class AccountRegister
	{
		private static readonly ILog log;

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			XElement xElement = new XElement("Result");
			bool flag = false;
			try
			{
				string userName = HttpUtility.UrlDecode(context.Request["username"]);
				string nickName = HttpUtility.UrlDecode(context.Request["password"]);
				string password = HttpUtility.UrlDecode(context.Request["password"]);
				bool sex = false;
				int money = 100;
				int giftToken = 100;
				int gold = 100;
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					flag = playerBussiness.RegisterUser(userName, nickName, password, sex, money, giftToken, gold);
				}
			}
			catch (Exception exception)
			{
				log.Error("RegisterResult", exception);
			}
			finally
			{
				xElement.Add(new XAttribute("value", "vl"));
				xElement.Add(new XAttribute("message", flag));
				context.Response.ContentType = "text/plain";
				context.Response.Write(xElement.ToString(check: false));
			}
		}

		static AccountRegister()
		{
			log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}
	}
}
