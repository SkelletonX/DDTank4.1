using Microsoft.QualityTools.Testing.Fakes.Shims;
using System;
using System.Diagnostics;

namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	[DebuggerNonUserCode]
	public static class StubBehaviors
	{
		[Serializable]
		[DebuggerNonUserCode]
		[__Instrument]
		private class DefaultValueStub : IStubBehavior
		{
			public TResult Result<TBehaved, TResult>(TBehaved me, string name) where TBehaved : IStub
			{
				return default(TResult);
			}

			public void VoidResult<TBehaved>(TBehaved me, string name) where TBehaved : IStub
			{
			}

			public void ValueAtReturn<TBehaved, TResult>(TBehaved me, string name, out TResult value) where TBehaved : IStub
			{
				value = default(TResult);
			}

			public void ValueAtEnterAndReturn<TBehaved, TResult>(TBehaved stub, string name, ref TResult value) where TBehaved : IStub
			{
				value = default(TResult);
			}

			public bool TryGetValue<TValue>(object name, out TValue value)
			{
				value = default(TValue);
				return true;
			}
		}

		[Serializable]
		[DebuggerNonUserCode]
		[__Instrument]
		private class NotImplementedStub : IStubBehavior
		{
			public TResult Result<TBehaved, TResult>(TBehaved me, string name) where TBehaved : IStub
			{
				throw new NotImplementedException();
			}

			public void VoidResult<TBehaved>(TBehaved me, string name) where TBehaved : IStub
			{
				throw new NotImplementedException();
			}

			public void ValueAtReturn<TBehaved, TResult>(TBehaved me, string name, out TResult value) where TBehaved : IStub
			{
				throw new NotImplementedException();
			}

			public void ValueAtEnterAndReturn<TBehaved, TResult>(TBehaved stub, string name, ref TResult value) where TBehaved : IStub
			{
				throw new NotImplementedException();
			}

			public bool TryGetValue<TValue>(object name, out TValue value)
			{
				value = default(TValue);
				return false;
			}
		}

		private sealed class CurrentProxyBehavior : IStubBehavior
		{
			public static readonly CurrentProxyBehavior Instance = new CurrentProxyBehavior();

			private CurrentProxyBehavior()
			{
			}

			public bool TryGetValue<TValue>(object name, out TValue value)
			{
				return Current.TryGetValue(name, out value);
			}

			public TResult Result<TBehaved, TResult>(TBehaved target, string name) where TBehaved : IStub
			{
				return Current.Result<TBehaved, TResult>(target, name);
			}

			public void ValueAtReturn<TBehaved, TValue>(TBehaved target, string name, out TValue value) where TBehaved : IStub
			{
				Current.ValueAtReturn(target, name, out value);
			}

			public void ValueAtEnterAndReturn<TBehaved, TValue>(TBehaved target, string name, ref TValue value) where TBehaved : IStub
			{
				Current.ValueAtEnterAndReturn(target, name, ref value);
			}

			public void VoidResult<TBehaved>(TBehaved target, string name) where TBehaved : IStub
			{
				Current.VoidResult(target, name);
			}
		}

		private static IStubBehavior @default;

		private static IStubBehavior notImplementedStub;

		private static IStubBehavior current = DefaultValue;

		public static IStubBehavior DefaultValue
		{
			get
			{
				if (@default == null)
				{
					using (ShimRuntime.AcquireProtectingThreadContext())
					{
						@default = new DefaultValueStub();
					}
				}
				return @default;
			}
		}

		public static IStubBehavior NotImplemented
		{
			get
			{
				if (notImplementedStub == null)
				{
					using (ShimRuntime.AcquireProtectingThreadContext())
					{
						notImplementedStub = new NotImplementedStub();
					}
				}
				return notImplementedStub;
			}
		}

		public static IStubBehavior Current
		{
			get
			{
				using (ShimRuntime.AcquireProtectingThreadContext())
				{
					return current;
				}
			}
			set
			{
				using (ShimRuntime.AcquireProtectingThreadContext())
				{
					if (value == null)
					{
						throw new ArgumentNullException("value");
					}
					current = value;
				}
			}
		}

		public static IStubBehavior CurrentProxy => CurrentProxyBehavior.Instance;

		public static void BehaveAsNotImplemented()
		{
			Current = NotImplemented;
		}

		public static IStubBehavior GetValueOrCurrent(IStubBehavior behavior)
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				return (behavior != null) ? behavior : Current;
			}
		}
	}
}
