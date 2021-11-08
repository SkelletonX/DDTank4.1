using Bussiness;
using Bussiness.Interface;
using Game.Base;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class Login : IHttpHandler, IRequiresSessionState
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string message = LanguageMgr.GetTranslation("Tank.Request.Login.Fail1");
			bool isError = false;
			XElement xElement = new XElement("Result");
			string text = context.Request["p"];
			try
			{
				BaseInterface baseInterface = BaseInterface.CreateInterface();
				string site = (context.Request["site"] == null) ? "" : HttpUtility.UrlDecode(context.Request["site"]);
				string userHostAddress = context.Request.UserHostAddress;
				if (!string.IsNullOrEmpty(text))
				{
					byte[] array = CryptoHelper.RsaDecryt2(StaticFunction.RsaCryptor, text);
					string[] array2 = Encoding.UTF8.GetString(array, 7, array.Length - 7).Split(',');
					if (array2.Length == 4)
					{
						string text2 = array2[0];
						string text3 = array2[1];
						string text4 = array2[2];
						string nickname = array2[3];
						if (PlayerManager.Login(text2, text3))
						{
							int isFirst = 0;
							bool isActive = false;
							bool byUserIsFirst = PlayerManager.GetByUserIsFirst(text2);
							PlayerInfo playerInfo = baseInterface.CreateLogin(text2, text4, int.Parse(ConfigurationManager.AppSettings["ServerID"]), ref message, ref isFirst, userHostAddress, ref isError, byUserIsFirst, ref isActive, site, nickname);
							if (isActive)
							{
								StaticsMgr.RegCountAdd();
							}
							if (playerInfo != null && !isError)
							{
								if (isFirst == 0)
								{
									PlayerManager.Update(text2, text4);
								}
								else
								{
									PlayerManager.Remove(text2);
								}
								string value = string.IsNullOrEmpty(playerInfo.Style) ? ",,,,,,,," : playerInfo.Style;
								playerInfo.Colors = (string.IsNullOrEmpty(playerInfo.Colors) ? ",,,,,,,," : playerInfo.Colors);
								XElement content = 
									new XElement("Item",
									new XAttribute("ID", playerInfo.ID), 
									new XAttribute("IsFirst", isFirst), 
									new XAttribute("NickName", playerInfo.NickName),
									new XAttribute("Date", ""), 
									new XAttribute("IsConsortia", 0),
									new XAttribute("ConsortiaID", playerInfo.ConsortiaID), 
									new XAttribute("Sex", playerInfo.Sex), 
									new XAttribute("WinCount", playerInfo.Win),
									new XAttribute("TotalCount", playerInfo.Total),
									new XAttribute("EscapeCount", playerInfo.Escape), 
									new XAttribute("DutyName", (playerInfo.DutyName == null) ? "" : playerInfo.DutyName), 
									new XAttribute("GP", playerInfo.GP), 
									new XAttribute("Honor", ""), 
									new XAttribute("Style", value), 
									new XAttribute("Gold", playerInfo.Gold), 
									new XAttribute("Colors", (playerInfo.Colors == null) ? "" : playerInfo.Colors), 
									new XAttribute("Attack", playerInfo.Attack), 
									new XAttribute("Defence", playerInfo.Defence),
									new XAttribute("Agility", playerInfo.Agility),
									new XAttribute("Luck", playerInfo.Luck), 
									new XAttribute("Grade", playerInfo.Grade), 
									new XAttribute("Hide", playerInfo.Hide),
									new XAttribute("Repute", playerInfo.Repute), 
									new XAttribute("ConsortiaName", (playerInfo.ConsortiaName == null) ? "" : playerInfo.ConsortiaName), 
									new XAttribute("Offer", playerInfo.Offer),
									new XAttribute("Skin", (playerInfo.Skin == null) ? "" : playerInfo.Skin), 
									new XAttribute("ReputeOffer", playerInfo.ReputeOffer), 
									new XAttribute("ConsortiaHonor", playerInfo.ConsortiaHonor), 
									new XAttribute("ConsortiaLevel", playerInfo.ConsortiaLevel), 
									new XAttribute("ConsortiaRepute", playerInfo.ConsortiaRepute), 
									new XAttribute("Money", playerInfo.Money + playerInfo.MoneyLock), 
									new XAttribute("AntiAddiction", playerInfo.AntiAddiction), 
									new XAttribute("IsMarried", playerInfo.IsMarried), 
									new XAttribute("SpouseID", playerInfo.SpouseID), 
									new XAttribute("SpouseName", (playerInfo.SpouseName == null) ? "" : playerInfo.SpouseName), 
									new XAttribute("MarryInfoID", playerInfo.MarryInfoID), 
									new XAttribute("IsCreatedMarryRoom", playerInfo.IsCreatedMarryRoom), 
									new XAttribute("IsGotRing", playerInfo.IsGotRing), 
									new XAttribute("LoginName", (playerInfo.UserName == null) ? "" : playerInfo.UserName), 
									new XAttribute("Nimbus", playerInfo.Nimbus),
									new XAttribute("FightPower", playerInfo.FightPower),
									new XAttribute("AnswerSite", playerInfo.AnswerSite),
									new XAttribute("WeaklessGuildProgressStr", (playerInfo.WeaklessGuildProgressStr == null) ? "" : playerInfo.WeaklessGuildProgressStr),
									new XAttribute("IsOldPlayer", false));
								xElement.Add(content);
								flag = true;
								message = LanguageMgr.GetTranslation("Tank.Request.Login.Success");
							}
							else
							{
								PlayerManager.Remove(text2);
							}
						}
						else
						{
							log.Error("name:" + text2 + "-pwd:" + text3);
							message = LanguageMgr.GetTranslation("BaseInterface.LoginAndUpdate.Try");
						}
					}
				}
			}
			catch (Exception ex)
			{
				byte[] array3 = Convert.FromBase64String(text);
				log.Error("User Login error: (--" + StaticFunction.RsaCryptor.KeySize + "--)" + ex.ToString());
				log.Error("--dataarray: " + Marshal.ToHexDump("fuckingbitch " + array3.Length, array3));
				flag = false;
				message = LanguageMgr.GetTranslation("Tank.Request.Login.Fail2");
			}
			finally
			{
				xElement.Add(new XAttribute("value", flag));
				xElement.Add(new XAttribute("message", message));
				context.Response.ContentType = "text/plain";
				context.Response.Write(xElement.ToString(check: false));
			}
		}
	}
}
