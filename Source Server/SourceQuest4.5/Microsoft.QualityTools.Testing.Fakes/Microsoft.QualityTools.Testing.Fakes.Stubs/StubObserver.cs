using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	[DebuggerNonUserCode]
	public sealed class StubObserver : IStubObserver
	{
		private readonly List<StubObservedCall> calls = new List<StubObservedCall>();

		public void Clear()
		{
			lock (calls)
			{
				calls.Clear();
			}
		}

		public StubObservedCall[] GetCalls()
		{
			lock (calls)
			{
				return calls.ToArray();
			}
		}

		void IStubObserver.Enter(Type stubbedType, Delegate stubCall)
		{
			if ((object)stubCall != null)
			{
				EnterSynchronized(stubbedType, stubCall, null);
			}
		}

		void IStubObserver.Enter(Type stubbedType, Delegate stubCall, object arg1)
		{
			if ((object)stubCall != null)
			{
				EnterSynchronized(stubbedType, stubCall, new object[1]
				{
					arg1
				});
			}
		}

		void IStubObserver.Enter(Type stubbedType, Delegate stubCall, object arg1, object arg2)
		{
			if ((object)stubCall != null)
			{
				EnterSynchronized(stubbedType, stubCall, new object[2]
				{
					arg1,
					arg2
				});
			}
		}

		void IStubObserver.Enter(Type stubbedType, Delegate stubCall, object arg1, object arg2, object arg3)
		{
			if ((object)stubCall != null)
			{
				EnterSynchronized(stubbedType, stubCall, new object[3]
				{
					arg1,
					arg2,
					arg3
				});
			}
		}

		void IStubObserver.Enter(Type stubbedType, Delegate stubCall, params object[] args)
		{
			if ((object)stubCall != null)
			{
				EnterSynchronized(stubbedType, stubCall, args);
			}
		}

		private void EnterSynchronized(Type stubbedType, Delegate stubCall, object[] args)
		{
			lock (calls)
			{
				calls.Add(new StubObservedCall(stubbedType, stubCall, args));
			}
		}
	}
}
