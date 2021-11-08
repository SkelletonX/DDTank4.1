using Bussiness.CenterService;
using log4net;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Tank.Request
{
	public class ExperienceRate : Page
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected HtmlForm form1;

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
				int serverId = int.Parse(Context.Request["serverId"]);
				if (ValidLoginIP(Context.Request.UserHostAddress))
				{
					using (CenterServiceClient centerServiceClient = new CenterServiceClient())
					{
						num = centerServiceClient.ExperienceRateUpdate(serverId);
					}
				}
			}
			catch (Exception exception)
			{
				log.Error("ExperienceRateUpdate:", exception);
			}
			base.Response.Write(num);
		}
	}
}
