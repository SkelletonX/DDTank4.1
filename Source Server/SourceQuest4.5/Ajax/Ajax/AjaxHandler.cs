using MS.Web;
using System.Web;

namespace Ajax
{
	internal class AjaxHandler : PageHandler
	{
		internal AjaxHandler()
		{
		}

		public override void ProcessRequest(HttpContext context)
		{
			AjaxRequestProcessor ajaxRequestProcessor = new AjaxRequestProcessor(context);
			ajaxRequestProcessor.Run();
		}
	}
}
