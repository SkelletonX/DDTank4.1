using log4net;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.UI;

namespace Tank.Request
{
	public class KitoffUser : Page
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
			bool flag = false;
			try
			{
				ValidLoginIP(Context.Request.UserHostAddress);
			}
			catch (Exception exception)
			{
				log.Error("GetAdminIP:", exception);
			}
			base.Response.Write(flag);
		}
	}
}
