using Bussiness;
using log4net;
using log4net.Config;
using System;
using System.Reflection;
using System.Web;

namespace Tank.Request
{
	public class Global : HttpApplication
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected void Application_Start(object sender, EventArgs e)
		{
			LanguageMgr.Setup(base.Server.MapPath("~"));
			XmlConfigurator.Configure();
			StaticsMgr.Setup();
			PlayerManager.Setup();
		}

		protected void Session_Start(object sender, EventArgs e)
		{
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{
		}

		protected void Application_Error(object sender, EventArgs e)
		{
		}

		protected void Session_End(object sender, EventArgs e)
		{
		}

		protected void Application_End(object sender, EventArgs e)
		{
			StaticsMgr.Stop();
		}
	}
}
