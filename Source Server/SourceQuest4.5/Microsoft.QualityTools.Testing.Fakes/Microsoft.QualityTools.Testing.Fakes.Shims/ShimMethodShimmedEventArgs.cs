using System;
using System.Reflection;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	public sealed class ShimMethodShimmedEventArgs : EventArgs
	{
		private readonly Delegate _shim;

		private readonly object _receiver;

		private readonly MethodBase method;

		public Delegate Shim => _shim;

		public object Receiver => _receiver;

		public MethodBase Method => method;

		internal ShimMethodShimmedEventArgs(Delegate optionalShim, object optionalReceiver, MethodBase method)
		{
			_shim = optionalShim;
			_receiver = optionalReceiver;
			this.method = method;
		}
	}
}
