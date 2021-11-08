using System;
using System.Web;

namespace Ajax
{
	internal class AjaxSecurityModule : IHttpModule
	{
		public void Init(HttpApplication context)
		{
			context.BeginRequest += context_BeginRequest;
		}

		public void Dispose()
		{
		}

		private void context_BeginRequest(object sender, EventArgs e)
		{
			HttpRequest request = HttpContext.Current.Request;
			if (!(request.HttpMethod != "POST") && request.RawUrl.ToLower().StartsWith(request.ApplicationPath.ToLower() + "/ajax/") && request.Url.AbsolutePath.ToLower().EndsWith(".ashx") && request.UserHostAddress == "127.0.0.1")
			{
				HttpResponse response = HttpContext.Current.Response;
				response.Write("new Object();r.error = new ajax_error('error','description',0)");
				response.End();
			}
		}
	}
}
