namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	public interface IStub
	{
		IStubBehavior InstanceBehavior
		{
			get;
		}
	}
	public interface IStub<T> : IStub, IStubObservable where T : class
	{
	}
}
