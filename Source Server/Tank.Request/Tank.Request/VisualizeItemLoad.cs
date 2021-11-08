using Bussiness;
using log4net;
using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class VisualizeItemLoad : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string value = "Fail!";
			bool flag2 = bool.Parse(context.Request["sex"]);
			XElement xElement = new XElement("Result");
			try
			{
				string value2 = ConfigurationManager.AppSettings[flag2 ? "BoyVisualizeItem" : "GrilVisualizeItem"];
				xElement.Add(new XAttribute("content", value2));
				flag = true;
				value = "Success!";
			}
			catch (Exception exception)
			{
				log.Error("VisualizeItemLoad", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
