namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	public interface IStubObservable
	{
		IStubObserver InstanceObserver
		{
			get;
			set;
		}
	}
}
