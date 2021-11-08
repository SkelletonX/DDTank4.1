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
	public class LogTime : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			XElement xElement = new XElement("Result");
			int total = 0;
			try
			{
				int page = int.Parse(context.Request["page"]);
				int size = int.Parse(context.Request["size"]);
				int order = int.Parse(context.Request["order"]);
				int consortiaID = int.Parse(context.Request["consortiaID"]);
				int state = int.Parse(context.Request["state"]);
				string name = csFunction.ConvertSql(HttpUtility.UrlDecode((context.Request["name"] == null) ? "" : context.Request["name"]));
				using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
				{
					ConsortiaAllyInfo[] consortiaAllyPage = consortiaBussiness.GetConsortiaAllyPage(page, size, ref total, order, consortiaID, state, name);
					foreach (ConsortiaAllyInfo info in consortiaAllyPage)
					{
						xElement.Add(FlashUtils.CreateConsortiaAllyInfo(info));
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
