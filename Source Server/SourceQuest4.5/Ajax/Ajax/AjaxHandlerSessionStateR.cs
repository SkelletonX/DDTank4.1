using System.Web.SessionState;

namespace Ajax
{
	internal class AjaxHandlerSessionStateR : AjaxHandler, IReadOnlySessionState, IRequiresSessionState
	{
		internal AjaxHandlerSessionStateR()
		{
		}
	}
}
