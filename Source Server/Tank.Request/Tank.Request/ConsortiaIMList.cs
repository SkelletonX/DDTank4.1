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
	public class ConsortiaIMList : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string value = "Fail!";
			int total = 0;
			XElement xElement = new XElement("Result");
			try
			{
				int num = int.Parse(context.Request["id"]);
				using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
				{
					ConsortiaInfo consortiaSingle = consortiaBussiness.GetConsortiaSingle(num);
					if (consortiaSingle != null)
					{
						xElement.Add(new XAttribute("Level", consortiaSingle.Level));
						xElement.Add(new XAttribute("Repute", consortiaSingle.Repute));
					}
				}
				using (ConsortiaBussiness consortiaBussiness2 = new ConsortiaBussiness())
				{
					ConsortiaUserInfo[] consortiaUsersPage = consortiaBussiness2.GetConsortiaUsersPage(1, 1000, ref total, -1, num, -1, -1);
					foreach (ConsortiaUserInfo info in consortiaUsersPage)
					{
						xElement.Add(FlashUtils.CreateConsortiaIMInfo(info));
					}
					flag = true;
					value = "Success!";
				}
			}
			catch (Exception exception)
			{
				log.Error("ConsortiaIMList", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
