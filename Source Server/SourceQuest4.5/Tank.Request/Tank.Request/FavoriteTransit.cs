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
	public class FavoriteTransit : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static string GetFavoriteUrl => ConfigurationManager.AppSettings["FavoriteUrl"];

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			try
			{
				string text = (context.Request["username"] == null) ? "" : HttpUtility.UrlDecode(context.Request["username"]);
				string text2 = (context.Request["site"] == null) ? "" : HttpUtility.UrlDecode(context.Request["site"]).ToLower();
				string text3 = string.Empty;
				if (!string.IsNullOrEmpty(text2))
				{
					text3 = ConfigurationManager.AppSettings[$"FavoriteUrl_{text2}"];
					int num = text.IndexOf('_');
					if (num != -1)
					{
						text = text.Substring(num + 1, text.Length - num - 1);
					}
				}
				if (string.IsNullOrEmpty(text3))
				{
					text3 = GetFavoriteUrl;
				}
				context.Response.Redirect(string.Format(text3, text, text2), endResponse: false);
			}
			catch (Exception exception)
			{
				log.Error("FavoriteTransit:", exception);
			}
		}
	}
}
