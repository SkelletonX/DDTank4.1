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
	public class LoadUserEquip : IHttpHandler
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
				int userID = int.Parse(context.Request["ID"]);
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					PlayerInfo userSingleByUserID = playerBussiness.GetUserSingleByUserID(userID);
					xElement.Add(new XAttribute
						("Agility", userSingleByUserID.Agility), new XAttribute
						("Attack", userSingleByUserID.Attack), new XAttribute
						("Colors", userSingleByUserID.Colors), new XAttribute
						("Skin", userSingleByUserID.Skin), new XAttribute
						("Defence", userSingleByUserID.Defence), new XAttribute
						("GP", userSingleByUserID.GP), new XAttribute
						("Grade", userSingleByUserID.Grade), new XAttribute
						("Luck", userSingleByUserID.Luck), new XAttribute
						("Hide", userSingleByUserID.Hide), new XAttribute
						("Repute", userSingleByUserID.Repute), new XAttribute
						("Offer", userSingleByUserID.Offer), new XAttribute
						("NickName", userSingleByUserID.NickName), new XAttribute
						("ConsortiaName", userSingleByUserID.ConsortiaName), new XAttribute
						("ConsortiaID", userSingleByUserID.ConsortiaID), new XAttribute
						("ReputeOffer", userSingleByUserID.ReputeOffer), new XAttribute
						("ConsortiaHonor", userSingleByUserID.ConsortiaHonor), new XAttribute
						("ConsortiaLevel", userSingleByUserID.ConsortiaLevel), new XAttribute
						("ConsortiaRepute", userSingleByUserID.ConsortiaRepute), new XAttribute
						("WinCount", userSingleByUserID.Win), new XAttribute
						("TotalCount", userSingleByUserID.Total), new XAttribute
						("EscapeCount", userSingleByUserID.Escape), new XAttribute
						("Sex", userSingleByUserID.Sex), new XAttribute("Style", userSingleByUserID.Style), new XAttribute("FightPower", userSingleByUserID.FightPower));
					ItemInfo[] array = playerBussiness.GetUserEuqip(userID).ToArray();
					foreach (ItemInfo info in array)
					{
						xElement.Add(FlashUtils.CreateGoodsInfo(info));
					}
				}
				flag = true;
				value = "Success!";
			}
			catch (Exception exception)
			{
				log.Error("LoadUserEquip", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
