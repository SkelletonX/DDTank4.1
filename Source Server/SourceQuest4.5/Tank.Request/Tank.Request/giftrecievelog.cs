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
	public class giftrecievelog : IHttpHandler
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
				_ = context.Request["key"];
				int.Parse(context.Request["selfid"]);
				int userid = int.Parse(context.Request["userID"]);
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					UserGiftInfo[] allUserGifts = playerBussiness.GetAllUserGifts(userid, isReceive: true);
					if (allUserGifts != null)
					{
						UserGiftInfo[] array = allUserGifts;
						foreach (UserGiftInfo userGiftInfo in array)
						{
							XElement content = new XElement("Item", new XAttribute("playerID", userGiftInfo.ReceiverID), new XAttribute("TemplateID", userGiftInfo.TemplateID), new XAttribute("count", userGiftInfo.Count));
							xElement.Add(content);
						}
					}
				}
				flag = true;
				value = "Success!";
			}
			catch (Exception exception)
			{
				log.Error("giftrecievelog", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.BinaryWrite(StaticFunction.Compress(xElement.ToString(check: false)));
		}
	}
}
