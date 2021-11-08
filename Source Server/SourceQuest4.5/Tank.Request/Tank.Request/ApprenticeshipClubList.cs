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
	public class ApprenticeshipClubList : IHttpHandler
	{
		private static readonly ILog log;

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool resultValue = true;
			string value = "true!";
			bool flag = false;
			bool flag2 = false;
			int total = 0;
			XElement xElement = new XElement("Result");
			try
			{
				int page = int.Parse(context.Request["page"]);
				int.Parse(context.Request["selfid"]);
				bool.Parse(context.Request["isReturnSelf"]);
				string text = (context.Request["name"] == null) ? "" : context.Request["name"];
				bool flag3 = bool.Parse(context.Request["appshipStateType"]);
				bool flag4 = bool.Parse(context.Request["requestType"]);
				int size = flag4 ? 9 : 3;
				int where = (!flag3) ? 1 : 2;
				int order = (!flag3) ? 8 : 10;
				int userID = -1;
				if (!flag4 && !flag3)
				{
					where = 3;
					order = 9;
				}
				else if (!flag4 && flag3)
				{
					where = 4;
					order = 9;
				}
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					if (text != null && text.Length > 0)
					{
						userID = (playerBussiness.GetUserSingleByNickName(text)?.ID ?? 0);
					}
					PlayerInfo[] playerPage = playerBussiness.GetPlayerPage(page, size, ref total, order, where, userID, "", ref resultValue);
					for (int i = 0; i < playerPage.Length; i++)
					{
						XElement content = FlashUtils.CreateApprenticeShipInfo(playerPage[i]);
						xElement.Add(content);
					}
					resultValue = true;
					value = "Success!";
				}
			}
			catch (Exception message)
			{
				log.Error(message);
			}
			xElement.Add(new XAttribute("total", total));
			xElement.Add(new XAttribute("value", resultValue));
			xElement.Add(new XAttribute("message", value));
			xElement.Add(new XAttribute("isPlayerRegeisted", flag));
			xElement.Add(new XAttribute("isSelfPublishEquip", flag2));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}

		static ApprenticeshipClubList()
		{
			log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}
	}
}
