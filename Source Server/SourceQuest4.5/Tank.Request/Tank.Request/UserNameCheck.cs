using Bussiness;
using Bussiness.Interface;
using log4net;
using System;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace Tank.Request
{
	public class UserNameCheck : Page
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected void Page_Load(object sender, EventArgs e)
		{
			int num = 1;
			try
			{
				string text = HttpUtility.UrlDecode(base.Request["username"]);
				string site = (base.Request["site"] == null) ? "" : HttpUtility.UrlDecode(base.Request["site"]);
				if (!string.IsNullOrEmpty(text))
				{
					string nameBySite = BaseInterface.GetNameBySite(text, site);
					using (PlayerBussiness playerBussiness = new PlayerBussiness())
					{
						num = ((playerBussiness.GetUserSingleByUserName(nameBySite) == null) ? 2 : 0);
					}
				}
			}
			catch (Exception exception)
			{
				log.Error("UserNameCheck:", exception);
			}
			base.Response.Write(num);
		}
	}
}
