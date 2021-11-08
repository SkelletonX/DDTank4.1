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
	public class AuctionPageList : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string value = "Fail!";
			int total = 0;
			XElement xElement = new XElement("Result");
			try
			{
				int page = int.Parse(context.Request["page"]);
				string name = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["name"]));
				int type = int.Parse(context.Request["type"]);
				int pay = int.Parse(context.Request["pay"]);
				int userID = int.Parse(context.Request["userID"]);
				int buyID = int.Parse(context.Request["buyID"]);
				int order = int.Parse(context.Request["order"]);
				bool sort = bool.Parse(context.Request["sort"]);
				string text = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["Auctions"]));
				string string_ = string.IsNullOrEmpty(text) ? "0" : text;
				int size = 50;
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					AuctionInfo[] auctionPage = playerBussiness.GetAuctionPage(page, name, type, pay, ref total, userID, buyID, order, sort, size, string_);
					foreach (AuctionInfo auctionInfo in auctionPage)
					{
						XElement xElement2 = FlashUtils.CreateAuctionInfo(auctionInfo);
						using (PlayerBussiness playerBussiness2 = new PlayerBussiness())
						{
							ItemInfo userItemSingle = playerBussiness2.GetUserItemSingle(auctionInfo.ItemID);
							if (userItemSingle != null)
							{
								xElement2.Add(FlashUtils.CreateGoodsInfo(userItemSingle));
							}
							xElement.Add(xElement2);
						}
					}
					flag = true;
					value = "Success!";
				}
			}
			catch (Exception exception)
			{
				log.Error("AuctionPageList", exception);
			}
			xElement.Add(new XAttribute("total", total));
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
