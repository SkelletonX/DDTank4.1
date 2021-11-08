using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class LoginSelectList : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string value = "Fail!";
			XElement xElement = new XElement("Result");
			try
			{
				string userName = HttpUtility.UrlDecode(context.Request["username"]);
				HttpUtility.UrlDecode(context.Request["password"]);
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					PlayerInfo[] userLoginList = playerBussiness.GetUserLoginList(userName);
					if (userLoginList.Length != 0)
					{
						PlayerInfo[] array = userLoginList;
						foreach (PlayerInfo playerInfo in array)
						{
							if (!string.IsNullOrEmpty(playerInfo.NickName))
							{
								xElement.Add(FlashUtils.CreateUserLoginList(playerInfo));
							}
						}
						flag = true;
						value = "Success!";
					}
				}
			}
			catch (Exception exception)
			{
				log.Error("LoginSelectList", exception);
			}
			finally
			{
				xElement.Add(new XAttribute("value", flag));
				xElement.Add(new XAttribute("message", value));
				context.Response.ContentType = "text/plain";
				context.Response.Write(xElement.ToString(check: false));
			}
		}
	}
}
