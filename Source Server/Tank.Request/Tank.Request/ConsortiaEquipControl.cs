using Bussiness;
using log4net;
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
	public class ConsortiaEquipControl : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			bool flag = false;
			string value = "Fail!";
			XElement xElement = new XElement("Result");
			int num = 0;
			try
			{
				int consortiaID = int.Parse(context.Request["consortiaID"]);
				using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
				{
					for (int i = 1; i < 3; i++)
					{
						for (int j = 1; j < 11; j++)
						{
							ConsortiaEquipControlInfo consortiaEuqipRiches = consortiaBussiness.GetConsortiaEuqipRiches(consortiaID, j, i);
							if (consortiaEuqipRiches != null)
							{
								xElement.Add(new XElement("Item", new XAttribute("type", consortiaEuqipRiches.Type), new XAttribute("level", consortiaEuqipRiches.Level), new XAttribute("riches", consortiaEuqipRiches.Riches)));
								num++;
							}
						}
					}
					flag = true;
					value = "Success!";
				}
			}
			catch (Exception exception)
			{
				log.Error("ConsortiaEventList", exception);
			}
			xElement.Add(new XAttribute("total", num));
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
