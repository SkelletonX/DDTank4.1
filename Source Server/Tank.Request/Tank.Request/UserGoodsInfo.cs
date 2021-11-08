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
	public class UserGoodsInfo : IHttpHandler
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
				int itemID = int.Parse(context.Request.Params["ID"]);
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					ItemInfo userItemSingle = playerBussiness.GetUserItemSingle(itemID);
					xElement.Add(FlashUtils.CreateGoodsInfo(userItemSingle));
				}
				flag = true;
				value = "Success!";
			}
			catch (Exception exception)
			{
				log.Error("UserGoodsInfo", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
