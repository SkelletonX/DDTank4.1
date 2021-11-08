using System.Web.SessionState;

namespace Ajax
{
	internal class AjaxAsyncHandlerSessionStateR : AjaxAsyncHandler, IReadOnlySessionState, IRequiresSessionState
	{
		internal AjaxAsyncHandlerSessionStateR()
		{
		}
	}
}
