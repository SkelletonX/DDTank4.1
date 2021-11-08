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
	public class LoadUserMail : IHttpHandler
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
				int num = int.Parse(context.Request.QueryString["selfid"]);
				if (num != 0)
				{
					using (PlayerBussiness playerBussiness = new PlayerBussiness())
					{
						MailInfo[] mailByUserID = playerBussiness.GetMailByUserID(num);
						foreach (MailInfo mailInfo in mailByUserID)
						{
							XElement xElement2 = new XElement("Item", new XAttribute("ID", mailInfo.ID), new XAttribute("Title", mailInfo.Title), new XAttribute("Content", mailInfo.Content), new XAttribute("Sender", mailInfo.Sender), new XAttribute("SendTime", mailInfo.SendTime.ToString("yyyy-MM-dd HH:mm:ss")), new XAttribute("Gold", mailInfo.Gold), new XAttribute("Money", mailInfo.Money), new XAttribute("Annex1ID", (mailInfo.Annex1 == null) ? "" : mailInfo.Annex1), new XAttribute("Annex2ID", (mailInfo.Annex2 == null) ? "" : mailInfo.Annex2), new XAttribute("Annex3ID", (mailInfo.Annex3 == null) ? "" : mailInfo.Annex3), new XAttribute("Annex4ID", (mailInfo.Annex4 == null) ? "" : mailInfo.Annex4), new XAttribute("Annex5ID", (mailInfo.Annex5 == null) ? "" : mailInfo.Annex5), new XAttribute("Type", mailInfo.Type), new XAttribute("ValidDate", mailInfo.ValidDate), new XAttribute("IsRead", mailInfo.IsRead));
							AddAnnex(xElement2, mailInfo.Annex1);
							AddAnnex(xElement2, mailInfo.Annex2);
							AddAnnex(xElement2, mailInfo.Annex3);
							AddAnnex(xElement2, mailInfo.Annex4);
							AddAnnex(xElement2, mailInfo.Annex5);
							xElement.Add(xElement2);
						}
					}
					flag = true;
					value = "Success!";
				}
			}
			catch (Exception exception)
			{
				log.Error("LoadUserMail", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.BinaryWrite(StaticFunction.Compress(xElement.ToString(check: false)));
		}

		public static void AddAnnex(XElement node, string value)
		{
			using (PlayerBussiness playerBussiness = new PlayerBussiness())
			{
				if (!string.IsNullOrEmpty(value))
				{
					ItemInfo userItemSingle = playerBussiness.GetUserItemSingle(int.Parse(value));
					if (userItemSingle != null)
					{
						node.Add(FlashUtils.CreateGoodsInfo(userItemSingle));
					}
				}
			}
		}
	}
}
