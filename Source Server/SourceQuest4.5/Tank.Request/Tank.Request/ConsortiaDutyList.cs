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
	public class ConsortiaDutyList : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string value = "Fail!";
			XElement xElement = new XElement("Result");
			int total = 0;
			try
			{
				int page = int.Parse(context.Request["page"]);
				int size = int.Parse(context.Request["size"]);
				int order = int.Parse(context.Request["order"]);
				int consortiaID = int.Parse(context.Request["consortiaID"]);
				int dutyID = int.Parse(context.Request["dutyID"]);
				using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
				{
					ConsortiaDutyInfo[] consortiaDutyPage = consortiaBussiness.GetConsortiaDutyPage(page, size, ref total, order, consortiaID, dutyID);
					foreach (ConsortiaDutyInfo info in consortiaDutyPage)
					{
						xElement.Add(FlashUtils.CreateConsortiaDutyInfo(info));
					}
					flag = true;
					value = "Success!";
				}
			}
			catch (Exception exception)
			{
				log.Error("ConsortiaDutyList", exception);
			}
			xElement.Add(new XAttribute("total", total));
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
