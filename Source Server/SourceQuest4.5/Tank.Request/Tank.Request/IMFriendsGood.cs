using Bussiness;
using log4net;
using System;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class IMFriendsGood : IHttpHandler
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
				string userName = context.Request["UserName"];
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					ArrayList friendsGood = playerBussiness.GetFriendsGood(userName);
					for (int i = 0; i < friendsGood.Count; i++)
					{
						XElement content = new XElement("Item", new XAttribute("UserName", friendsGood[i].ToString()));
						xElement.Add(content);
					}
				}
				flag = true;
				value = "Success!";
			}
			catch (Exception exception)
			{
				log.Error("IMFriendsGood", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
