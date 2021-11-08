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
	public class IMRecentContactsList : IHttpHandler
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
				Convert.ToInt32(context.Request["recentContacts"]);
				Convert.ToInt32(context.Request["id"]);
				int userID = Convert.ToInt32(context.Request["selfid"]);
				_ = context.Request["key"];
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					FriendInfo[] friendsAll = playerBussiness.GetFriendsAll(userID);
					foreach (FriendInfo friendInfo in friendsAll)
					{
						XElement content = new XElement("Item", new XAttribute("ID", friendInfo.FriendID), new XAttribute("NickName", friendInfo.NickName), new XAttribute("LoginName", friendInfo.UserName), new XAttribute("Style", friendInfo.Style), new XAttribute("Sex", friendInfo.Sex == 1), new XAttribute("Colors", friendInfo.Colors), new XAttribute("Grade", friendInfo.Grade), new XAttribute("Hide", friendInfo.Hide), new XAttribute("ConsortiaName", friendInfo.ConsortiaName), new XAttribute("TotalCount", friendInfo.Total), new XAttribute("EscapeCount", friendInfo.Escape), new XAttribute("WinCount", friendInfo.Win), new XAttribute("Offer", friendInfo.Offer), new XAttribute("Relation", friendInfo.Relation), new XAttribute("Repute", friendInfo.Repute), new XAttribute("State", (friendInfo.State == 1) ? 1 : 0), new XAttribute("Nimbus", friendInfo.Nimbus), new XAttribute("DutyName", friendInfo.DutyName), new XAttribute("AchievementPoint", 0), new XAttribute("Rank", "Chiến sĩ siêu cấp"), new XAttribute("FightPower", 13528), new XAttribute("ApprenticeshipState", 0), new XAttribute("BBSFriends", false), new XAttribute("Birthday", DateTime.Now), new XAttribute("UserName", friendInfo.UserName), new XAttribute("IsMarried", false));
						xElement.Add(content);
					}
				}
				flag = true;
				value = "Success!";
			}
			catch (Exception exception)
			{
				log.Error("IMRecentContactsList", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}

		static IMRecentContactsList()
		{
			log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}
	}
}
