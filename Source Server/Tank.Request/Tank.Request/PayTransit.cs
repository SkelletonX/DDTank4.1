using log4net;
using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Services;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class PayTransit : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private string site = "";

		public string PayURL => ConfigurationManager.AppSettings["PayURL_" + site];

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			string text = "";
			string text2 = string.Empty;
			try
			{
				if (!string.IsNullOrEmpty(context.Request["username"]))
				{
					text = HttpUtility.UrlDecode(context.Request["username"].Trim());
				}
				site = ((context.Request["site"] == null) ? "" : HttpUtility.UrlDecode(context.Request["site"]).ToLower());
				if (!string.IsNullOrEmpty(site))
				{
					text2 = PayURL;
					int num = text.IndexOf('_');
					if (num != -1)
					{
						text = text.Substring(num + 1, text.Length - num - 1);
					}
				}
				if (string.IsNullOrEmpty(text2))
				{
					text2 = ConfigurationManager.AppSettings["PayURL"];
				}
				context.Response.Redirect(string.Format(text2, text, site), endResponse: false);
			}
			catch (Exception exception)
			{
				log.Error("PayTransit:", exception);
			}
		}
	}
}
