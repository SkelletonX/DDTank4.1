namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	public interface IPartialStub : IStub
	{
		bool CallBase
		{
			get;
			set;
		}
	}
}
