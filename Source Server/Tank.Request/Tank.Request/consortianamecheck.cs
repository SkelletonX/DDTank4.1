using Bussiness;
using log4net;
using System;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class consortianamecheck : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			LanguageMgr.Setup(ConfigurationManager.AppSettings["ReqPath"]);
			bool flag = false;
			string value = "O nome já foi usado.";
			XElement xElement = new XElement("Result");
			try
			{
				string text = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["NickName"]));
				if (Encoding.Default.GetByteCount(text) <= 20)
				{
					if (!string.IsNullOrEmpty(text))
					{
						using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
						{
							if (consortiaBussiness.GetConsortiaSingleByName(text) == null)
							{
								flag = true;
								value = "Sucesso! O nome pode ser utilizado.";
							}
						}
					}
				}
				else
				{
					value = "O nome da associação é muito longo";
				}
			}
			catch (Exception exception)
			{
				log.Error("NickNameCheck", exception);
				flag = false;
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
