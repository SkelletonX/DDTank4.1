using Bussiness;
using Bussiness.CenterService;
using log4net;
using Road.Flash;
using System;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Tank.Request
{
	public class ActivePullDown : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			LanguageMgr.Setup(ConfigurationManager.AppSettings["ReqPath"]);
			int num = Convert.ToInt32(context.Request["selfid"]);
			int activeID = Convert.ToInt32(context.Request["activeID"]);
			_ = context.Request["key"];
			string text = context.Request["activeKey"];
			bool flag = false;
			string msg = "ActivePullDownHandler.Fail";
			string awardID = "";
			XElement xElement = new XElement("Result");
			if (text != "")
			{
				byte[] array = CryptoHelper.RsaDecryt2(StaticFunction.RsaCryptor, text);
				awardID = Encoding.UTF8.GetString(array, 0, array.Length);
			}
			try
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					if (playerBussiness.PullDown(activeID, awardID, num, ref msg) == 0)
					{
						using (CenterServiceClient centerServiceClient = new CenterServiceClient())
						{
							centerServiceClient.MailNotice(num);
						}
					}
				}
				flag = true;
				msg = LanguageMgr.GetTranslation(msg);
			}
			catch (Exception exception)
			{
				log.Error("ActivePullDown", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", msg));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
