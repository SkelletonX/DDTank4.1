using System;

namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	public interface IStubObserver
	{
		void Enter(Type stubbedType, Delegate stubCall);

		void Enter(Type stubbedType, Delegate stubCall, object arg1);

		void Enter(Type stubbedType, Delegate stubCall, object arg1, object arg2);

		void Enter(Type stubbedType, Delegate stubCall, object arg1, object arg2, object arg3);

		void Enter(Type stubbedType, Delegate stubCall, params object[] args);
	}
}
