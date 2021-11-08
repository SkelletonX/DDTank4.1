using Bussiness.Interface;
using log4net;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace Tank.Request
{
	public class CreateLogin : Page
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static string GetLoginIP => ConfigurationManager.AppSettings["LoginIP"];

		public static bool ValidLoginIP(string ip)
		{
			string getLoginIP = GetLoginIP;
			if (!string.IsNullOrEmpty(getLoginIP) && !getLoginIP.Split('|').Contains(ip))
			{
				return false;
			}
			return true;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			int result = 1;
			try
			{
				string content = HttpUtility.UrlDecode(base.Request["content"]);
				string site = (base.Request["site"] == null) ? "" : HttpUtility.UrlDecode(base.Request["site"]).ToLower();
				string[] array = BaseInterface.CreateInterface().UnEncryptLogin(content, ref result, site);
				if (array.Length > 3)
				{
					string text = array[0].Trim().ToLower();
					string text2 = array[1].Trim().ToLower();
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						PlayerManager.Add(BaseInterface.GetNameBySite(text, site), text2);
						result = 0;
					}
					else
					{
						result = -91010;
					}
				}
				else
				{
					result = -1900;
				}
			}
			catch (Exception exception)
			{
				log.Error("CreateLogin:", exception);
			}
			base.Response.Write(result);
		}
	}
}
