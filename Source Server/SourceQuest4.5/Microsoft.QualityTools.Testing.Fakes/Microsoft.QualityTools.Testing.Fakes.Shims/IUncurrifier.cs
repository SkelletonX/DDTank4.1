using System;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	public interface IUncurrifier
	{
		Delegate InnerDelegate
		{
			get;
		}
	}
	public interface IUncurrifier<TDelegate>
	{
		TDelegate InnerDelegate
		{
			get;
		}
	}
}
