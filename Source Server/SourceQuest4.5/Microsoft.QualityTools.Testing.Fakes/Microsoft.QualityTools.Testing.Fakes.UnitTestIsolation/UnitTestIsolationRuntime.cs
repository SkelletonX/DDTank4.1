using Microsoft.QualityTools.Testing.Fakes.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;

namespace Microsoft.QualityTools.Testing.Fakes.UnitTestIsolation
{
	[DebuggerNonUserCode]
	public static class UnitTestIsolationRuntime
	{
		private static class Internal
		{
			public static readonly ReaderWriterLock DetoursLock = new ReaderWriterLock();
		}

		private struct DetourDispatcherKey
		{
			private readonly IntPtr methodHandle;

			private readonly IntPtr typeHandle;

			public DetourDispatcherKey(MethodBase method)
			{
				typeHandle = method.DeclaringType.TypeHandle.Value;
				methodHandle = method.MethodHandle.Value;
			}
		}

		private sealed class DetourCallbackData
		{
			public int ReferenceCount = 1;

			private List<KeyValuePair<object, MethodBase>> _methods;

			public void AddMethod(object _receiver, MethodBase method)
			{
				if (_methods == null)
				{
					_methods = new List<KeyValuePair<object, MethodBase>>();
				}
				_methods.Add(new KeyValuePair<object, MethodBase>(_receiver, method));
			}

			public bool TryGetMethods(out IEnumerable<KeyValuePair<object, MethodBase>> methods)
			{
				methods = _methods;
				return methods != null;
			}
		}

		private sealed class DetourDispatcher
		{
			private bool ignoreStaticFallback;

			private Delegate _methodDetour;

			private Dictionary<object, Delegate> _instanceDetours;

			public bool IsEmpty
			{
				get
				{
					if ((object)_methodDetour == null)
					{
						return _instanceDetours == null;
					}
					return false;
				}
			}

			public void AddDetour(object _receiver, Delegate _detour)
			{
				if (_receiver == null)
				{
					_methodDetour = _detour;
					if ((object)_detour == null)
					{
						ignoreStaticFallback = true;
					}
				}
				else
				{
					if (_instanceDetours == null)
					{
						_instanceDetours = new Dictionary<object, Delegate>(ObjectReferenceIdentity.Default);
					}
					_instanceDetours[_receiver] = _detour;
				}
			}

			[Pure]
			public bool HasNoFallbackOrMethod(object _receiver)
			{
				if (_receiver != null)
				{
					if (_instanceDetours != null && _instanceDetours.TryGetValue(_receiver, out Delegate value) && (object)value == null)
					{
						return false;
					}
				}
				else if (ignoreStaticFallback)
				{
					return false;
				}
				return (object)_methodDetour == null;
			}

			public Delegate GetDetour(object _receiver)
			{
				Delegate value = null;
				if (_receiver == null || _instanceDetours == null || !_instanceDetours.TryGetValue(_receiver, out value))
				{
					value = _methodDetour;
				}
				return value;
			}

			public bool RemoveDetour(object _receiver)
			{
				if (_receiver == null)
				{
					_methodDetour = null;
					return true;
				}
				if (_instanceDetours != null && _instanceDetours.Remove(_receiver))
				{
					if (_instanceDetours.Count == 0)
					{
						_instanceDetours = null;
					}
					return true;
				}
				return false;
			}

			public void ClearFallbacks()
			{
				ignoreStaticFallback = false;
				if (_instanceDetours != null)
				{
					List<object> list = null;
					foreach (KeyValuePair<object, Delegate> instanceDetour in _instanceDetours)
					{
						if ((object)instanceDetour.Value == null)
						{
							if (list == null)
							{
								list = new List<object>();
							}
							list.Add(instanceDetour.Key);
						}
					}
					if (list != null)
					{
						foreach (object item in list)
						{
							_instanceDetours.Remove(item);
						}
					}
				}
			}
		}

		private static IUnitTestIsolationInstrumentationProvider _instrumentationProvider;

		private static Dictionary<DetourDispatcherKey, DetourDispatcher> _dispatchers;

		private static Dictionary<DetourFactory, DetourCallbackData> _fallbackBehaviors;

		[ThreadStatic]
		private static object detourTarget;

		public static IUnitTestIsolationInstrumentationProvider InstrumentationProvider
		{
			get
			{
				return _instrumentationProvider;
			}
			[ReflectionPermission(SecurityAction.Demand, RestrictedMemberAccess = false)]
			set
			{
				if (_instrumentationProvider != value)
				{
					ClearDetours();
					_instrumentationProvider = value;
				}
			}
		}

		public static bool IsThreadProtected => _instrumentationProvider?.IsThreadProtected ?? true;

		public static object DetourTarget
		{
			get
			{
				return detourTarget;
			}
			set
			{
				detourTarget = value;
			}
		}

		public static event Action<MethodBase> AttachedUnsupportedMethod;

		public static event Action<MethodBase> AttachedMethod;

		public static event Action<MethodBase> DetachedMethod;

		public static event DetourCallback AtDetour;

		internal static void InitializeUnitTestIsolationInstrumentationProvider()
		{
			if (_instrumentationProvider == null)
			{
				IntelliTraceInstrumentationProvider intelliTraceInstrumentationProvider = new IntelliTraceInstrumentationProvider();
				intelliTraceInstrumentationProvider.Initialize();
				_instrumentationProvider = intelliTraceInstrumentationProvider;
			}
		}

		[DebuggerNonUserCode]
		public static IDisposable AcquireProtectingThreadContext()
		{
			return _instrumentationProvider?.AcquireProtectingThreadContext();
		}

		[Conditional("DEBUG")]
		[Pure]
		private static void AssertInvariant()
		{
		}

		[Pure]
		private static DetourDispatcherKey GetKey(MethodBase method)
		{
			return new DetourDispatcherKey(method);
		}

		[Pure]
		public static bool IsValidDetour(MethodBase method, Delegate detourDelegate)
		{
			if ((object)method == null)
			{
				return false;
			}
			if ((object)detourDelegate == null)
			{
				return false;
			}
			ParameterInfo[] parameters = method.GetParameters();
			int num = (!method.IsStatic) ? 1 : 0;
			Type type = detourDelegate.GetType();
			MethodInfo method2 = type.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
			ParameterInfo[] parameters2 = method2.GetParameters();
			if (parameters.Length + num != parameters2.Length)
			{
				return false;
			}
			if (num > 0 && !method.DeclaringType.IsAssignableFrom(parameters2[0].ParameterType))
			{
				return false;
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				if ((object)parameters[i].ParameterType != parameters2[i + num].ParameterType)
				{
					return false;
				}
			}
			if ((object)ReflectionHelper.GetReturnType(method) != method2.ReturnType)
			{
				return false;
			}
			return true;
		}

		public static void ClearDetours()
		{
			if (_dispatchers != null)
			{
				ProtectingClearDetours();
			}
		}

		private static void ProtectingClearDetours()
		{
			using (AcquireProtectingThreadContext())
			{
				Internal.DetoursLock.AcquireWriterLock(-1);
				try
				{
					_dispatchers = null;
					_fallbackBehaviors = null;
				}
				finally
				{
					Internal.DetoursLock.ReleaseWriterLock();
				}
			}
		}

		private static void AllocateDictionaries()
		{
			if (_dispatchers == null)
			{
				_dispatchers = new Dictionary<DetourDispatcherKey, DetourDispatcher>();
				_fallbackBehaviors = new Dictionary<DetourFactory, DetourCallbackData>();
			}
		}

		[ReflectionPermission(SecurityAction.Demand, RestrictedMemberAccess = false)]
		public static void AttachFallbackBehavior(DetourFactory fallbackBehavior)
		{
			if (fallbackBehavior == null)
			{
				throw new ArgumentNullException("fallbackBehavior");
			}
			using (AcquireProtectingThreadContext())
			{
				Internal.DetoursLock.AcquireWriterLock(-1);
				try
				{
					AllocateDictionaries();
					if (_fallbackBehaviors.TryGetValue(fallbackBehavior, out DetourCallbackData value))
					{
						value.ReferenceCount++;
					}
					else
					{
						_fallbackBehaviors.Add(fallbackBehavior, new DetourCallbackData());
						foreach (KeyValuePair<DetourDispatcherKey, DetourDispatcher> dispatcher in _dispatchers)
						{
							dispatcher.Value.ClearFallbacks();
						}
					}
				}
				finally
				{
					Internal.DetoursLock.ReleaseWriterLock();
				}
			}
		}

		public static void DetachFallbackBehavior(DetourFactory fallbackBehavior)
		{
			if (fallbackBehavior == null)
			{
				throw new ArgumentNullException("fallbackBehavior");
			}
			using (AcquireProtectingThreadContext())
			{
				Internal.DetoursLock.AcquireWriterLock(-1);
				try
				{
					AllocateDictionaries();
					if (_fallbackBehaviors.TryGetValue(fallbackBehavior, out DetourCallbackData value) && --value.ReferenceCount == 0)
					{
						if (value.TryGetMethods(out IEnumerable<KeyValuePair<object, MethodBase>> methods))
						{
							foreach (KeyValuePair<object, MethodBase> item in methods)
							{
								DetachDetour(item.Key, item.Value);
							}
						}
						_fallbackBehaviors.Remove(fallbackBehavior);
					}
				}
				finally
				{
					Internal.DetoursLock.ReleaseWriterLock();
				}
			}
		}

		private static void OnAttachedUnsupportedMethod(MethodBase method)
		{
			Action<MethodBase> attachedUnsupportedMethod = UnitTestIsolationRuntime.AttachedUnsupportedMethod;
			if (attachedUnsupportedMethod != null)
			{
				InvokeEvent(method, attachedUnsupportedMethod);
			}
		}

		private static void OnAttachedMethod(MethodBase method)
		{
			Action<MethodBase> attachedMethod = UnitTestIsolationRuntime.AttachedMethod;
			if (attachedMethod != null)
			{
				InvokeEvent(method, attachedMethod);
			}
		}

		private static void OnDetachedMethod(MethodBase method)
		{
			Action<MethodBase> detachedMethod = UnitTestIsolationRuntime.DetachedMethod;
			if (detachedMethod != null)
			{
				InvokeEvent(method, detachedMethod);
			}
		}

		private static void InvokeEvent<T>(T value, Action<T> eh)
		{
			Delegate[] invocationList = eh.GetInvocationList();
			Exception ex = null;
			for (int i = 0; i < invocationList.Length; i++)
			{
				Action<T> action = (Action<T>)invocationList[i];
				try
				{
					action(value);
				}
				catch (Exception ex2)
				{
					if (ex == null)
					{
						ex = ex2;
					}
				}
			}
			if (ex != null)
			{
				throw ex;
			}
		}

		private static void CheckInstrumentation(MethodBase method)
		{
			IUnitTestIsolationInstrumentationProvider unitTestIsolationInstrumentationProvider = CheckInstrumentationProvider();
			if (!unitTestIsolationInstrumentationProvider.CanDetour(method))
			{
				OnAttachedUnsupportedMethod(method);
				throw new UnitTestIsolationException(string.Format(CultureInfo.CurrentCulture, FakesFrameworkResources.CannotBeInstrumented, new object[2]
				{
					method.DeclaringType,
					method.Name
				}));
			}
		}

		internal static void EnsureInstrumentationProvider()
		{
			CheckInstrumentationProvider();
		}

		private static IUnitTestIsolationInstrumentationProvider CheckInstrumentationProvider()
		{
			IUnitTestIsolationInstrumentationProvider instrumentationProvider = _instrumentationProvider;
			if (instrumentationProvider == null)
			{
				throw new UnitTestIsolationException(FakesFrameworkResources.DetourInstrumentationProviderNotSet);
			}
			if (instrumentationProvider != null && !instrumentationProvider.IsDetoursEnabled())
			{
				throw new UnitTestIsolationException(FakesFrameworkResources.DetoursAreNotEnabled);
			}
			return instrumentationProvider;
		}

		[ReflectionPermission(SecurityAction.Demand, RestrictedMemberAccess = false)]
		public static void AttachDetour(object optionalReceiver, MethodBase method, Delegate detourDelegate)
		{
			using (AcquireProtectingThreadContext())
			{
				ValidateMethod(method);
				if (optionalReceiver != null && method.IsStatic)
				{
					throw new ArgumentException(FakesFrameworkResources.OptionalReceiverShouldBeNullForStaticMethods);
				}
				if ((object)detourDelegate == null)
				{
					throw new ArgumentNullException("detourDelegate");
				}
				if (!IsValidDetour(method, detourDelegate))
				{
					throw new ArgumentException(FakesFrameworkResources.IncompatibleMethodAndDetour);
				}
			}
			InternalAttachDetour(optionalReceiver, method, detourDelegate);
		}

		private static void ValidateMethod(MethodBase method)
		{
			if ((object)method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (method.IsAbstract)
			{
				throw new ArgumentException(FakesFrameworkResources.MustNotBeAbstract, "method");
			}
			if (method.ContainsGenericParameters)
			{
				throw new ArgumentException(FakesFrameworkResources.MustBeAFullyInstantiedMethod, "method");
			}
			if (method.CallingConvention == CallingConventions.VarArgs)
			{
				throw new ArgumentException(FakesFrameworkResources.MustNotUseTheVarargsCallingConvention, "method");
			}
			if (ReflectionHelper.IsExtern(method))
			{
				throw new ArgumentException(FakesFrameworkResources.MustNotBeExtern, "method");
			}
		}

		private static Delegate InternalAttachDetour(object optionalReceiver, MethodBase method, Delegate detourDelegate)
		{
			using (AcquireProtectingThreadContext())
			{
				CheckInstrumentation(method);
				Internal.DetoursLock.AcquireWriterLock(-1);
				try
				{
					AllocateDictionaries();
					DetourDispatcherKey key = GetKey(method);
					if (!_dispatchers.TryGetValue(key, out DetourDispatcher value))
					{
						value = (_dispatchers[key] = new DetourDispatcher());
					}
					value.AddDetour(optionalReceiver, detourDelegate);
				}
				finally
				{
					Internal.DetoursLock.ReleaseWriterLock();
				}
				OnAttachedMethod(method);
				return detourDelegate;
			}
		}

		public static void IgnoreDetour(MethodBase method)
		{
			using (AcquireProtectingThreadContext())
			{
				ValidateMethod(method);
				Internal.DetoursLock.AcquireWriterLock(-1);
				try
				{
					InternalIgnoreDetour(null, method);
				}
				finally
				{
					Internal.DetoursLock.ReleaseWriterLock();
				}
			}
		}

		private static void InternalIgnoreDetour(object _receiver, MethodBase method)
		{
			AllocateDictionaries();
			DetourDispatcherKey key = GetKey(method);
			if (!_dispatchers.TryGetValue(key, out DetourDispatcher value))
			{
				value = (_dispatchers[key] = new DetourDispatcher());
			}
			value.AddDetour(_receiver, null);
		}

		public static void DetachDetour(object optionalReceiver, MethodBase method)
		{
			using (AcquireProtectingThreadContext())
			{
				ValidateMethod(method);
				Internal.DetoursLock.AcquireWriterLock(-1);
				try
				{
					DetourDispatcherKey key = GetKey(method);
					if (_dispatchers != null && _dispatchers.TryGetValue(key, out DetourDispatcher value) && value.RemoveDetour(optionalReceiver) && value.IsEmpty)
					{
						_dispatchers.Remove(key);
						if (_dispatchers.Count == 0)
						{
							_dispatchers = null;
							_fallbackBehaviors = null;
						}
					}
				}
				finally
				{
					Internal.DetoursLock.ReleaseWriterLock();
				}
				OnDetachedMethod(method);
			}
		}

		[DebuggerNonUserCode]
		public static Delegate GetDetour(object optionalReceiver, MethodBase method)
		{
			if (_dispatchers == null || _instrumentationProvider == null)
			{
				return null;
			}
			using (AcquireProtectingThreadContext())
			{
				return InternalGetDetour(optionalReceiver, method);
			}
		}

		private static Delegate InternalGetDetour(object _receiver, MethodBase method)
		{
			Delegate @delegate = null;
			Dictionary<DetourDispatcherKey, DetourDispatcher> dispatchers = _dispatchers;
			if (dispatchers != null)
			{
				Internal.DetoursLock.AcquireReaderLock(-1);
				try
				{
					dispatchers = _dispatchers;
					if (dispatchers != null)
					{
						DetourDispatcherKey key = GetKey(method);
						if (dispatchers.TryGetValue(key, out DetourDispatcher value))
						{
							@delegate = value.GetDetour(_receiver);
						}
						else
						{
							value = null;
						}
						if ((object)@delegate == null && _fallbackBehaviors != null && (value == null || value.HasNoFallbackOrMethod(_receiver)))
						{
							@delegate = InternalGetFallbackDetour(_receiver, method);
						}
					}
				}
				finally
				{
					Internal.DetoursLock.ReleaseReaderLock();
				}
			}
			if ((object)@delegate != null)
			{
				UnitTestIsolationRuntime.AtDetour?.Invoke(_receiver, method, @delegate);
				DetourTarget = @delegate.Target;
			}
			return @delegate;
		}

		[DebuggerNonUserCode]
		public static Delegate GetDetourOrDefault(object optionalReceiver, MethodBase method)
		{
			if ((object)method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (_dispatchers == null || _instrumentationProvider == null)
			{
				return null;
			}
			using (AcquireProtectingThreadContext())
			{
				return InternalGetDetourOrDefault(optionalReceiver, method);
			}
		}

		private static Delegate InternalGetDetourOrDefault(object optionalReceiver, MethodBase method)
		{
			Delegate result = null;
			Dictionary<DetourDispatcherKey, DetourDispatcher> dispatchers = _dispatchers;
			if (dispatchers != null)
			{
				Internal.DetoursLock.AcquireReaderLock(-1);
				try
				{
					dispatchers = _dispatchers;
					if (dispatchers == null)
					{
						return result;
					}
					DetourDispatcherKey key = GetKey(method);
					if (dispatchers.TryGetValue(key, out DetourDispatcher value))
					{
						return value.GetDetour(optionalReceiver);
					}
					return result;
				}
				finally
				{
					Internal.DetoursLock.ReleaseReaderLock();
				}
			}
			return result;
		}

		[DebuggerNonUserCode]
		private static Delegate InternalGetFallbackDetour(object _receiver, MethodBase method)
		{
			Delegate result = null;
			DetourCallbackData detourCallbackData = null;
			Delegate @delegate = null;
			foreach (KeyValuePair<DetourFactory, DetourCallbackData> fallbackBehavior in _fallbackBehaviors)
			{
				@delegate = fallbackBehavior.Key(_receiver, method);
				if ((object)@delegate != null)
				{
					detourCallbackData = fallbackBehavior.Value;
					break;
				}
			}
			LockCookie lockCookie = Internal.DetoursLock.UpgradeToWriterLock(-1);
			try
			{
				if (detourCallbackData != null)
				{
					detourCallbackData.AddMethod(_receiver, method);
					return InternalAttachDetour(_receiver, method, @delegate);
				}
				InternalIgnoreDetour(_receiver, method);
				return result;
			}
			finally
			{
				Internal.DetoursLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}
	}
}
