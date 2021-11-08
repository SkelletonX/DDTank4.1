using Bussiness;
using Bussiness.CenterService;
using Bussiness.Interface;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace Tank.Request
{
	public class ChargeMoney : Page
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static string GetChargeIP => ConfigurationSettings.AppSettings["ChargeIP"];

		protected void Page_Load(object sender, EventArgs e)
		{
			int result = 1;
			try
			{
				string userHostAddress = Context.Request.UserHostAddress;
				if (ValidLoginIP(userHostAddress))
				{
					string content = HttpUtility.UrlDecode(base.Request["content"]);
					string site = (base.Request["site"] == null) ? "" : HttpUtility.UrlDecode(base.Request["site"]).ToLower();
					int userID = Convert.ToInt32(HttpUtility.UrlDecode(base.Request["nickname"]));
					string[] array = BaseInterface.CreateInterface().UnEncryptCharge(content, ref result, site);
					if (array.Length > 5)
					{
						string chargeID = array[0];
						string text = array[1].Trim();
						int num = int.Parse(array[2]);
						string text2 = array[3];
						decimal needMoney = decimal.Parse(array[4]);
						if (!string.IsNullOrEmpty(text))
						{
							string nameBySite = BaseInterface.GetNameBySite(text, site);
							if (num > 0)
							{
								using (PlayerBussiness playerBussiness = new PlayerBussiness())
								{
									int userID2 = 0;
									DateTime now = DateTime.Now;
									if (playerBussiness.AddChargeMoney(chargeID, nameBySite, num, text2, needMoney, ref userID2, ref result, now, userHostAddress, userID))
									{
										result = 0;
										using (CenterServiceClient centerServiceClient = new CenterServiceClient())
										{
											centerServiceClient.ChargeMoney(userID2, chargeID);
											using (PlayerBussiness playerBussiness2 = new PlayerBussiness())
											{
												PlayerInfo userSingleByUserID = playerBussiness2.GetUserSingleByUserID(userID2);
												if (userSingleByUserID != null)
												{
													StaticsMgr.Log(now, nameBySite, userSingleByUserID.Sex, num, text2, needMoney);
												}
												else
												{
													StaticsMgr.Log(now, nameBySite, sex: true, num, text2, needMoney);
													log.Error("ChargeMoney_StaticsMgr:Player is null!");
												}
											}
										}
									}
								}
							}
							else
							{
								result = 3;
							}
						}
						else
						{
							result = 2;
						}
					}
				}
				else
				{
					result = 5;
				}
			}
			catch (Exception exception)
			{
				log.Error("ChargeMoney:", exception);
			}
			base.Response.Write(result + Context.Request.UserHostAddress);
		}

		public static bool ValidLoginIP(string ip)
		{
			string getChargeIP = GetChargeIP;
			int num = string.IsNullOrEmpty(getChargeIP) ? 1 : (getChargeIP.Split('|').Contains(ip) ? 1 : 0);
			return num != 0;
		}
	}
}
