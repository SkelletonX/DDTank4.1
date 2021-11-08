namespace Ajax
{
	internal class AsyncRequest
	{
		private AsyncRequestState _asyncRequestState;

		public AsyncRequest(AsyncRequestState ars)
		{
			_asyncRequestState = ars;
		}

		public void ProcessRequest()
		{
			AjaxRequestProcessor ajaxRequestProcessor = new AjaxRequestProcessor(_asyncRequestState._ctx);
			ajaxRequestProcessor.Run();
			_asyncRequestState.CompleteRequest();
		}
	}
}
