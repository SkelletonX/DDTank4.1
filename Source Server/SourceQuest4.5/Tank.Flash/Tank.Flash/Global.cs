using System;
using System.Web;

namespace Tank.Flash
{
	public class Global : HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
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
			if (base.Session["Loading"] != null)
			{
				LoadingManager.LoadingCount--;
			}
		}

		protected void Application_End(object sender, EventArgs e)
		{
		}
	}
}
