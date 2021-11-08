namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	public interface IStubBehavior
	{
		bool TryGetValue<TValue>(object name, out TValue value);

		TResult Result<TStub, TResult>(TStub target, string name) where TStub : IStub;

		void ValueAtReturn<TStub, TValue>(TStub target, string name, out TValue value) where TStub : IStub;

		void ValueAtEnterAndReturn<TStub, TValue>(TStub target, string name, ref TValue value) where TStub : IStub;

		void VoidResult<TStub>(TStub target, string name) where TStub : IStub;
	}
}
