using System;
using System.Runtime.CompilerServices;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	[__Instrument]
	[CompilerGenerated]
	public abstract class Uncurrifier<TDelegate> : IUncurrifier<TDelegate>, IUncurrifier
	{
		public readonly TDelegate InnerDelegate;

		TDelegate IUncurrifier<TDelegate>.InnerDelegate => InnerDelegate;

		Delegate IUncurrifier.InnerDelegate => (Delegate)(object)InnerDelegate;

		protected Uncurrifier(TDelegate innerDelegate)
		{
			InnerDelegate = innerDelegate;
		}
	}
}
