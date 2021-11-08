using MS.Web;
using System.Web;
using System.Web.Caching;

namespace Ajax
{
	internal class PageHandlerFactory : IHttpHandlerFactory
	{
		public void RemovedCallback(string k, object v, CacheItemRemovedReason r)
		{
		}

		public void ReleaseHandler(IHttpHandler handler)
		{
		}

		public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
		{
			Request request = new Request(context, requestType, url);
			if (request.Extension == Utility.HandlerExtension)
			{
				if (context.Request["_session"] != null)
				{
					if (context.Request["_session"] == "rw")
					{
						return new AjaxHandlerSessionStateRW();
					}
					if (context.Request["_session"] == "r")
					{
						return new AjaxHandlerSessionStateR();
					}
					if (context.Request["_session"] == "no")
					{
						return new AjaxHandler();
					}
				}
				if (context.Session != null && !context.Session.IsCookieless)
				{
					return new AjaxHandler();
				}
				return new AjaxHandlerSessionStateR();
			}
			throw new HttpException(403, "The type of page you have requested is not served because it has been explicitly forbidden. The extension '." + request.Extension + "' may be incorrect. Please review the URL below and make sure that it is spelled correctly.");
		}
	}
}
