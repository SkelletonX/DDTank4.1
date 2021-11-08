using System;
using System.Reflection;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	public interface IShimBehavior
	{
		bool TryGetShimMethod(MethodBase method, out Delegate shim);
	}
}
