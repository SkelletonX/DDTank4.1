using System;
using System.Threading;
using System.Web;

namespace Ajax
{
	internal class AsyncRequestState : IAsyncResult
	{
		internal HttpContext _ctx;

		internal AsyncCallback _cb;

		internal object _extraData;

		private bool _isCompleted = false;

		private ManualResetEvent _callCompleteEvent = null;

		public object AsyncState => _extraData;

		public bool CompletedSynchronously => false;

		public bool IsCompleted => _isCompleted;

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				lock (this)
				{
					if (_callCompleteEvent == null)
					{
						_callCompleteEvent = new ManualResetEvent(initialState: false);
					}
					return _callCompleteEvent;
				}
			}
		}

		public AsyncRequestState(HttpContext ctx, AsyncCallback cb, object extraData)
		{
			_ctx = ctx;
			_cb = cb;
			_extraData = extraData;
		}

		internal void CompleteRequest()
		{
			_isCompleted = true;
			lock (this)
			{
				if (_callCompleteEvent != null)
				{
					_callCompleteEvent.Set();
				}
			}
			if (_cb != null)
			{
				_cb(this);
			}
		}
	}
}
