using System;
using System.Diagnostics;
using System.Reflection;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	[__DoNotInstrument]
	[DebuggerNonUserCode]
	internal sealed class ShimInstanceBehaviorClosure
	{
		public readonly object Receiver;

		public readonly IShimBehavior ShimBehavior;

		private readonly Type _targetType;

		public ShimInstanceBehaviorClosure(object receiver, Type _targetType, IShimBehavior shimBehavior)
		{
			Receiver = receiver;
			this._targetType = _targetType;
			ShimBehavior = shimBehavior;
		}

		public bool TryGetTargetType(out Type targetType)
		{
			targetType = _targetType;
			return (object)targetType != null;
		}

		public Delegate GetBehavior(object _receiver, MethodBase method)
		{
			if (object.ReferenceEquals(_receiver, Receiver) && ((object)_targetType == null || (object)_receiver.GetType() == _targetType) && ShimBehavior.TryGetShimMethod(method, out Delegate shim))
			{
				return shim;
			}
			return null;
		}
	}
}
