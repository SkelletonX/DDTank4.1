using System;
using System.Threading;
using System.Web;
using System.Web.SessionState;

namespace Ajax
{
	internal class AjaxAsyncHandler : IRequiresSessionState, IHttpAsyncHandler, IHttpHandler
	{
		public bool IsReusable => false;

		public void ProcessRequest(HttpContext ctx)
		{
		}

		public IAsyncResult BeginProcessRequest(HttpContext ctx, AsyncCallback cb, object obj)
		{
			AsyncRequestState asyncRequestState = new AsyncRequestState(ctx, cb, obj);
			AsyncRequest @object = new AsyncRequest(asyncRequestState);
			ThreadStart start = @object.ProcessRequest;
			Thread thread = new Thread(start);
			thread.Start();
			return asyncRequestState;
		}

		public void EndProcessRequest(IAsyncResult ar)
		{
			AsyncRequestState asyncRequestState = ar as AsyncRequestState;
		}
	}
}
