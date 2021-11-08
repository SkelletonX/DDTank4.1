namespace Microsoft.QualityTools.Testing.Fakes.Instances
{
	public interface IInstanced
	{
		object Instance
		{
			get;
		}
	}
	public interface IInstanced<T> : IInstanced
	{
		new T Instance
		{
			get;
		}
	}
}
