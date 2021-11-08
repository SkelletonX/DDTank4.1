using System;
using System.Diagnostics;

namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	[Serializable]
	[__Instrument]
	[DebuggerNonUserCode]
	public abstract class StubBase : IStub
	{
		private IStubBehavior _instanceBehavior;

		private IStubObserver _instanceObserver;

		public IStubBehavior InstanceBehavior
		{
			get
			{
				return StubBehaviors.GetValueOrCurrent(_instanceBehavior);
			}
			set
			{
				_instanceBehavior = value;
			}
		}

		public IStubObserver InstanceObserver
		{
			get
			{
				return StubObservers.GetValueOrCurrent(_instanceObserver);
			}
			set
			{
				_instanceObserver = value;
			}
		}

		public void BehaveAsNotImplemented()
		{
			InstanceBehavior = StubBehaviors.NotImplemented;
		}

		public void BehaveAsDefaultValue()
		{
			InstanceBehavior = StubBehaviors.DefaultValue;
		}
	}
	[Serializable]
	[__Instrument]
	[DebuggerNonUserCode]
	public abstract class StubBase<T> : StubBase, IStub<T>, IStub, IStubObservable where T : class
	{
	}
}
