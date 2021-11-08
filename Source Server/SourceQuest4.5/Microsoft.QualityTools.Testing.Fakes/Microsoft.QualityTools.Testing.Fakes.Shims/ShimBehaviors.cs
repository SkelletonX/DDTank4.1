using Microsoft.QualityTools.Testing.Fakes.UnitTestIsolation;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	public static class ShimBehaviors
	{
		private sealed class DefaultValueBehavior : ShimBehaviorBase
		{
			public DefaultValueBehavior()
				: base("Microsoft.QualityTools.Testing.Fakes.DelegateProxies.DefaultValues", "DefaultValue")
			{
			}

			protected override void EmitDelegateBody(Type delegateReturnType, ParameterInfo[] delegateParameters, MethodBuilder methodBuilder)
			{
				ILGenerator iLGenerator = methodBuilder.GetILGenerator();
				foreach (ParameterInfo parameterInfo in delegateParameters)
				{
					if (parameterInfo.IsOut)
					{
						iLGenerator.Emit(OpCodes.Ldarg, parameterInfo.Position);
						iLGenerator.Emit(OpCodes.Initobj, parameterInfo.ParameterType);
					}
				}
				if ((object)delegateReturnType != null && (object)delegateReturnType != typeof(void))
				{
					if (!delegateReturnType.IsValueType)
					{
						iLGenerator.Emit(OpCodes.Ldnull);
					}
					else
					{
						LocalBuilder local = iLGenerator.DeclareLocal(delegateReturnType);
						iLGenerator.Emit(OpCodes.Ldloca, local);
						iLGenerator.Emit(OpCodes.Initobj, delegateReturnType);
						iLGenerator.Emit(OpCodes.Ldloc, local);
					}
				}
				iLGenerator.Emit(OpCodes.Ret);
			}
		}

		private sealed class NotImplementedBehavior : ShimBehaviorBase
		{
			private static class Metadata
			{
				public static readonly ConstructorInfo ShimNotImplementedExceptionCtor = typeof(ShimNotImplementedException).GetConstructor(Type.EmptyTypes);
			}

			public NotImplementedBehavior()
				: base("Microsoft.QualityTools.Testing.Fakes.DelegateProxies.NotImplementeds", "NotImplemented")
			{
			}

			protected override void EmitDelegateBody(Type delegateReturnType, ParameterInfo[] delegateParameters, MethodBuilder methodBuilder)
			{
				ILGenerator iLGenerator = methodBuilder.GetILGenerator();
				iLGenerator.Emit(OpCodes.Newobj, Metadata.ShimNotImplementedExceptionCtor);
				iLGenerator.Emit(OpCodes.Throw);
			}
		}

		private sealed class FallthroughBehavior : IShimBehavior
		{
			public bool TryGetShimMethod(MethodBase method, out Delegate shim)
			{
				shim = null;
				return false;
			}
		}

		private sealed class CurrentProxyShimBehavior : IShimBehavior
		{
			public static readonly IShimBehavior Instance;

			private CurrentProxyShimBehavior()
			{
			}

			static CurrentProxyShimBehavior()
			{
				using (ShimRuntime.AcquireProtectingThreadContext())
				{
					Instance = new CurrentProxyShimBehavior();
				}
			}

			public bool TryGetShimMethod(MethodBase method, out Delegate shim)
			{
				return Current.TryGetShimMethod(method, out shim);
			}
		}

		[__DoNotInstrument]
		[DebuggerNonUserCode]
		private sealed class ShimTypeDefinitionBehaviorClosure : IDisposable
		{
			private readonly Type typeDefinition;

			private readonly IShimBehavior shimBehavior;

			public ShimTypeDefinitionBehaviorClosure(Type typeDefinition, IShimBehavior shimBehavior)
			{
				this.typeDefinition = typeDefinition;
				this.shimBehavior = shimBehavior;
				UnitTestIsolationRuntime.AttachFallbackBehavior(GetBehavior);
			}

			public Delegate GetBehavior(object _receiver, MethodBase method)
			{
				using (ShimRuntime.AcquireProtectingThreadContext())
				{
					if ((object)method == null)
					{
						throw new ArgumentNullException("method");
					}
					Type declaringType;
					Type type;
					if ((object)(declaringType = method.DeclaringType) != null && (object)(type = (declaringType.IsGenericType ? declaringType.GetGenericTypeDefinition() : declaringType)) != null && (object)type == typeDefinition && shimBehavior.TryGetShimMethod(method, out Delegate shim))
					{
						return shim;
					}
					return null;
				}
			}

			public void Dispose()
			{
				using (ShimRuntime.AcquireProtectingThreadContext())
				{
					UnitTestIsolationRuntime.DetachFallbackBehavior(GetBehavior);
				}
			}
		}

		private static IShimBehavior _defaultValue;

		private static IShimBehavior _notImplemented;

		private static IShimBehavior _fallthrough;

		private static IShimBehavior current;

		public static IShimBehavior DefaultValue
		{
			get
			{
				if (_defaultValue == null)
				{
					using (ShimRuntime.AcquireProtectingThreadContext())
					{
						_defaultValue = new DefaultValueBehavior();
					}
				}
				return _defaultValue;
			}
		}

		public static IShimBehavior NotImplemented
		{
			get
			{
				if (_notImplemented == null)
				{
					using (ShimRuntime.AcquireProtectingThreadContext())
					{
						_notImplemented = new NotImplementedBehavior();
					}
				}
				return _notImplemented;
			}
		}

		public static IShimBehavior Fallthrough
		{
			get
			{
				if (_fallthrough == null)
				{
					using (ShimRuntime.AcquireProtectingThreadContext())
					{
						_fallthrough = new FallthroughBehavior();
					}
				}
				return _fallthrough;
			}
		}

		public static IShimBehavior Current
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
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				using (ShimRuntime.AcquireProtectingThreadContext())
				{
					current = value;
				}
			}
		}

		public static IShimBehavior CurrentProxy => CurrentProxyShimBehavior.Instance;

		static ShimBehaviors()
		{
			Clear();
		}

		public static void Clear()
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				current = NotImplemented;
			}
		}

		public static void BehaveAsNotImplemented()
		{
			Current = NotImplemented;
		}

		public static void BehaveAsNotImplemented(Type type)
		{
			AttachToType(type, NotImplemented);
		}

		public static void BehaveAsDefaultValue()
		{
			Current = DefaultValue;
		}

		public static void BehaveAsDefaultValue(Type type)
		{
			AttachToType(type, DefaultValue);
		}

		public static void BehaveAsFallthrough()
		{
			Current = Fallthrough;
		}

		public static void BehaveAsFallthrough(Type type)
		{
			AttachToType(type, Fallthrough);
		}

		public static IDisposable AttachToType(Type type, IShimBehavior behavior)
		{
			if ((object)type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (behavior == null)
			{
				throw new ArgumentNullException("behavior");
			}
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				return new ShimTypeDefinitionBehaviorClosure(type, behavior);
			}
		}
	}
}
