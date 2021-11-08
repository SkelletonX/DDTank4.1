using Bussiness.CenterService;
using log4net;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.UI;

namespace Tank.Request
{
	public class AASGetState : Page
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static string GetAdminIP => ConfigurationManager.AppSettings["AdminIP"];

		public static bool ValidLoginIP(string ip)
		{
			string getAdminIP = GetAdminIP;
			if (!string.IsNullOrEmpty(getAdminIP) && !getAdminIP.Split('|').Contains(ip))
			{
				return false;
			}
			return true;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			int num = 2;
			try
			{
				if (ValidLoginIP(Context.Request.UserHostAddress))
				{
					using (CenterServiceClient centerServiceClient = new CenterServiceClient())
					{
						num = centerServiceClient.AASGetState();
					}
				}
			}
			catch (Exception exception)
			{
				log.Error("ASSGetState:", exception);
			}
			base.Response.Write(num);
		}
	}
}
