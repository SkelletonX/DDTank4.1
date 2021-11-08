using Bussiness.CenterService;
using log4net;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace Tank.Request
{
	public class SystemNotice : Page
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static string GetChargeIP => ConfigurationManager.AppSettings["AdminIP"];

		public static bool ValidLoginIP(string ip)
		{
			string getChargeIP = GetChargeIP;
			if (!string.IsNullOrEmpty(getChargeIP) && !getChargeIP.Split('|').Contains(ip))
			{
				return false;
			}
			return true;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			int num = 1;
			try
			{
				if (ValidLoginIP(Context.Request.UserHostAddress))
				{
					string text = HttpUtility.UrlDecode(base.Request["content"]);
					if (!string.IsNullOrEmpty(text))
					{
						using (CenterServiceClient centerServiceClient = new CenterServiceClient())
						{
							if (centerServiceClient.SystemNotice(text))
							{
								num = 0;
							}
						}
					}
				}
				else
				{
					num = 2;
				}
			}
			catch (Exception exception)
			{
				log.Error("SystemNotice:", exception);
			}
			base.Response.Write(num);
		}
	}
}
