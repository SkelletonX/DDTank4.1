using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class UserApprenticeshipInfoList : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = true;
			string value = "true!";
			int num = 0;
			XElement xElement = new XElement("Result");
			try
			{
				int num2 = int.Parse(context.Request["selfid"]);
				int num3 = int.Parse(context.Request["RelationshipID"]);
				if (num3 == 0)
				{
					num3 = num2;
				}
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					PlayerInfo userSingleByUserID = playerBussiness.GetUserSingleByUserID(num3);
					PlayerInfo userSingleByUserID2 = playerBussiness.GetUserSingleByUserID(num2);
					if (userSingleByUserID != null && userSingleByUserID2 != null)
					{
						if (userSingleByUserID2.masterID == userSingleByUserID.ID)
						{
							XElement content = FlashUtils.CreateUserApprenticeshipInfo(userSingleByUserID);
							xElement.Add(content);
						}
						foreach (KeyValuePair<int, string> item in userSingleByUserID.MasterOrApprenticesArr)
						{
							PlayerInfo userSingleByUserID3 = playerBussiness.GetUserSingleByUserID(item.Key);
							if (userSingleByUserID3 != null && userSingleByUserID3.ID != num2)
							{
								XElement content2 = FlashUtils.CreateUserApprenticeshipInfo(userSingleByUserID3);
								xElement.Add(content2);
							}
						}
					}
					flag = true;
					value = "Success!";
				}
			}
			catch (Exception message)
			{
				log.Error(message);
			}
			xElement.Add(new XAttribute("total", num));
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
