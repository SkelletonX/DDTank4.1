using Microsoft.QualityTools.Testing.Fakes.Instances;
using System;
using System.Diagnostics.Contracts;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	public abstract class ShimBase
	{
		private readonly Type targetType;

		public Type TargetType => targetType;

		protected ShimBase(Type targetType)
		{
			if ((object)targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (!ReflectionContract.IsClassOrValueType(targetType))
			{
				throw new ArgumentException(FakesFrameworkResources.MustBeAClassOrAValuetype, "targetType");
			}
			this.targetType = targetType;
			ShimRuntime.EnsureContext();
		}
	}
	[__DoNotInstrument]
	public abstract class ShimBase<T> : ShimBase, IInstanced<T>, IInstanced where T : class
	{
		private readonly bool isInstanceOwned;

		private T _instance;

		private ShimInstanceBehaviorClosure _instanceBehavior;

		public T Instance
		{
			get
			{
				using (ShimRuntime.AcquireProtectingThreadContext())
				{
					return GetInstance();
				}
			}
		}

		public bool IsInstanceOwned => isInstanceOwned;

		object IInstanced.Instance => Instance;

		public IShimBehavior InstanceBehavior
		{
			get
			{
				using (ShimRuntime.AcquireProtectingThreadContext())
				{
					return (_instanceBehavior == null) ? null : _instanceBehavior.ShimBehavior;
				}
			}
			set
			{
				using (ShimRuntime.AcquireProtectingThreadContext())
				{
					Type targetType = IsInstanceOwned ? null : base.TargetType;
					ShimRuntime.AttachBehavior(ref _instanceBehavior, GetInstance(), targetType, value);
				}
			}
		}

		protected ShimBase()
			: base(typeof(T))
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				isInstanceOwned = true;
				InstanceBehavior = ShimBehaviors.DefaultValue;
			}
		}

		protected ShimBase(T instance)
			: base(typeof(T))
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				isInstanceOwned = false;
				_instance = instance;
				InstanceBehavior = ShimBehaviors.Fallthrough;
			}
		}

		internal T GetInstance()
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				ShimRuntime.EnsureContext();
				if (_instance == null)
				{
					_instance = (T)ShimRuntime.CreateUninitializedInstance(typeof(T));
				}
				return _instance;
			}
		}

		public static implicit operator T(ShimBase<T> shim)
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				return (shim == null) ? null : shim.GetInstance();
			}
		}

		public void BehaveAsNotImplemented()
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				InstanceBehavior = ShimBehaviors.NotImplemented;
			}
		}

		public void BehaveAsDefaultValue()
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				InstanceBehavior = ShimBehaviors.DefaultValue;
			}
		}
	}
}
