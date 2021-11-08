using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class IMListLoad : IHttpHandler, IRequiresSessionState
	{
		private static readonly ILog log;

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string value = "Fail!";
			XElement xElement = new XElement("Result");
			try
			{
				int userID = int.Parse(context.Request["id"]);
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					FriendInfo[] friendsAll = playerBussiness.GetFriendsAll(userID);
					XElement content = new XElement("customList", new XAttribute("ID", 0), new XAttribute("Name", "好友"));
					xElement.Add(content);
					content = new XElement("customList", new XAttribute("ID", 1), new XAttribute("Name", "黑名单"));
					xElement.Add(content);
					content = new XElement("customList", new XAttribute("ID", 10), new XAttribute("Name", ""));
					xElement.Add(content);
					content = new XElement("customList", new XAttribute("ID", 11), new XAttribute("Name", ""));
					xElement.Add(content);
					content = new XElement("customList", new XAttribute("ID", 12), new XAttribute("Name", ""));
					xElement.Add(content);
					content = new XElement("customList", new XAttribute("ID", 13), new XAttribute("Name", ""));
					xElement.Add(content);
					content = new XElement("customList", new XAttribute("ID", 14), new XAttribute("Name", ""));
					xElement.Add(content);
					content = new XElement("customList", new XAttribute("ID", 15), new XAttribute("Name", ""));
					content = new XElement("customList", new XAttribute("ID", 16), new XAttribute("Name", ""));
					content = new XElement("customList", new XAttribute("ID", 17), new XAttribute("Name", ""));
					content = new XElement("customList", new XAttribute("ID", 18), new XAttribute("Name", ""));
					content = new XElement("customList", new XAttribute("ID", 19), new XAttribute("Name", ""));
					xElement.Add(content);
					FriendInfo[] array = friendsAll;
					foreach (FriendInfo friendInfo in array)
					{
						XElement content2 = new XElement("Item", new XAttribute("ID", friendInfo.FriendID), new XAttribute("NickName", friendInfo.NickName), new XAttribute("Birthday", DateTime.Now), new XAttribute("ApprenticeshipState", friendInfo.apprenticeshipState), new XAttribute("LoginName", friendInfo.UserName), new XAttribute("Style", friendInfo.Style), new XAttribute("Sex", friendInfo.Sex == 1), new XAttribute("Colors", friendInfo.Colors), new XAttribute("Grade", friendInfo.Grade), new XAttribute("Hide", friendInfo.Hide), new XAttribute("ConsortiaName", friendInfo.ConsortiaName), new XAttribute("TotalCount", friendInfo.Total), new XAttribute("EscapeCount", friendInfo.Escape), new XAttribute("WinCount", friendInfo.Win), new XAttribute("Offer", friendInfo.Offer), new XAttribute("Relation", friendInfo.Relation), new XAttribute("Repute", friendInfo.Repute), new XAttribute("State", (friendInfo.State == 1) ? 1 : 0), new XAttribute("Nimbus", friendInfo.Nimbus), new XAttribute("DutyName", friendInfo.DutyName));
						xElement.Add(content2);
					}
				}
				flag = true;
				value = "Success!";
			}
			catch (Exception exception)
			{
				log.Error("IMListLoad", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}

		static IMListLoad()
		{
			log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}
	}
}
