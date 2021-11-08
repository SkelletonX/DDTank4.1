#define TRACE
using Microsoft.QualityTools.Testing.Fakes.Engine;
using Microsoft.QualityTools.Testing.Fakes.Instances;
using Microsoft.QualityTools.Testing.Fakes.UnitTestIsolation;
using Microsoft.QualityTools.Testing.Fakes.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	[__DoNotInstrument]
	[DebuggerNonUserCode]
	public static class ShimRuntime
	{
		[__DoNotInstrument]
		[DebuggerNonUserCode]
		private sealed class Context : IDisposable
		{
			private bool disposed;

			public Context()
			{
				Interlocked.Increment(ref gcontext);
			}

			public void Dispose()
			{
				if (!disposed)
				{
					disposed = true;
					using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
					{
						Interlocked.Decrement(ref gcontext);
						Clear();
					}
				}
			}
		}

		internal static class Metadata
		{
			public static readonly MethodInfo BindArrayMethodDefinition = typeof(BindRuntime).GetMethod("BindArray", BindingFlags.Static | BindingFlags.Public);
		}

		[DebuggerNonUserCode]
		[__Instrument]
		private static class BindRuntime
		{
			[DebuggerDisplay("{Array}")]
			[__Instrument]
			[DebuggerNonUserCode]
			public sealed class InstrumentedArray<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable, ICloneable
			{
				public readonly T[] Array;

				T IList<T>.this[int index]
				{
					get
					{
						return Array[index];
					}
					set
					{
						Array[index] = value;
					}
				}

				int ICollection<T>.Count
				{
					get
					{
						ICollection<T> array = Array;
						return array.Count;
					}
				}

				bool ICollection<T>.IsReadOnly
				{
					get
					{
						ICollection<T> array = Array;
						return array.IsReadOnly;
					}
				}

				int ICollection.Count
				{
					get
					{
						ICollection array = Array;
						return array.Count;
					}
				}

				bool ICollection.IsSynchronized
				{
					get
					{
						ICollection array = Array;
						return array.IsSynchronized;
					}
				}

				object ICollection.SyncRoot
				{
					get
					{
						ICollection array = Array;
						return array.SyncRoot;
					}
				}

				bool IList.IsFixedSize
				{
					get
					{
						IList array = Array;
						return array.IsFixedSize;
					}
				}

				bool IList.IsReadOnly
				{
					get
					{
						IList array = Array;
						return array.IsReadOnly;
					}
				}

				object IList.this[int index]
				{
					get
					{
						IList array = Array;
						return array[index];
					}
					set
					{
						IList array = Array;
						array[index] = value;
					}
				}

				public InstrumentedArray(T[] array)
				{
					Array = array;
				}

				int IList<T>.IndexOf(T item)
				{
					IList<T> array = Array;
					return array.IndexOf(item);
				}

				void IList<T>.Insert(int index, T item)
				{
					IList<T> array = Array;
					array.Insert(index, item);
				}

				void IList<T>.RemoveAt(int index)
				{
					IList<T> array = Array;
					array.RemoveAt(index);
				}

				void ICollection<T>.Add(T item)
				{
					ICollection<T> array = Array;
					array.Add(item);
				}

				void ICollection<T>.Clear()
				{
					ICollection<T> array = Array;
					array.Clear();
				}

				bool ICollection<T>.Contains(T item)
				{
					ICollection<T> array = Array;
					return array.Contains(item);
				}

				void ICollection<T>.CopyTo(T[] array, int arrayIndex)
				{
					ICollection<T> array2 = Array;
					array2.CopyTo(array, arrayIndex);
				}

				bool ICollection<T>.Remove(T item)
				{
					ICollection<T> array = Array;
					return array.Remove(item);
				}

				IEnumerator<T> IEnumerable<T>.GetEnumerator()
				{
					IEnumerable<T> array = Array;
					return array.GetEnumerator();
				}

				IEnumerator IEnumerable.GetEnumerator()
				{
					IEnumerable array = Array;
					return array.GetEnumerator();
				}

				object ICloneable.Clone()
				{
					ICloneable array = Array;
					return new InstrumentedArray<T>((T[])array.Clone());
				}

				void ICollection.CopyTo(Array array, int index)
				{
					ICollection array2 = Array;
					array2.CopyTo(array, index);
				}

				int IList.Add(object value)
				{
					IList array = Array;
					return array.Add(value);
				}

				void IList.Clear()
				{
					IList array = Array;
					array.Clear();
				}

				bool IList.Contains(object value)
				{
					IList array = Array;
					return array.Contains(value);
				}

				int IList.IndexOf(object value)
				{
					IList array = Array;
					return array.IndexOf(value);
				}

				void IList.Insert(int index, object value)
				{
					IList array = Array;
					array.Insert(index, value);
				}

				void IList.Remove(object value)
				{
					IList array = Array;
					array.Remove(value);
				}

				void IList.RemoveAt(int index)
				{
					IList array = Array;
					array.RemoveAt(index);
				}
			}

			public static void BindArray<TShimmed, TShim, TBound, T>(TShim shim, TBound bound) where TShimmed : class, TBound where TShim : ShimBase<TShimmed> where TBound : class
			{
				T[] array = (T[])(object)bound;
				TBound bound2 = (TBound)(object)new InstrumentedArray<T>(array);
				Bind<TShimmed, TShim, TBound>(shim, bound2);
			}
		}

		private static class BindCompiler
		{
			private struct TypePair : IEquatable<TypePair>
			{
				private const int FNV1_prime_32 = 16777619;

				public readonly Type Left;

				public readonly Type Right;

				public TypePair(Type left, Type right)
				{
					Left = left;
					Right = right;
				}

				public bool Equals(TypePair other)
				{
					if ((object)other.Left == Left)
					{
						return (object)other.Right == Right;
					}
					return false;
				}

				public override int GetHashCode()
				{
					return (Left.GetHashCode() * 16777619) ^ Right.GetHashCode();
				}
			}

			private static readonly RuntimeDelegateTypePool delegates = new RuntimeDelegateTypePool();

			private static readonly Dictionary<TypePair, FakesDelegates.Func<Delegate, Delegate>> uncurrifiers = new Dictionary<TypePair, FakesDelegates.Func<Delegate, Delegate>>();

			private static readonly Dictionary<Assembly, object> delegateAssemblies = new Dictionary<Assembly, object>();

			public static void ClearCache()
			{
				uncurrifiers.Clear();
			}

			public static void Bind<TShimmed>(Type shimType, Type shimmedType, Type boundType, ShimBase<TShimmed> shim, object bound) where TShimmed : class
			{
				Type typeDefinition = ReflectionHelper.GetTypeDefinition(shimType);
				if ((object)typeDefinition != null)
				{
					RegisterDelegates(typeDefinition.Module.Assembly);
				}
				bound.GetType();
				Type[] interfaces = boundType.GetInterfaces();
				foreach (Type boundType2 in interfaces)
				{
					InternalBindInterface(shimType, shimmedType, boundType2, shim, bound);
				}
				InternalBindInterface(shimType, shimmedType, boundType, shim, bound);
			}

			private static void RegisterDelegates(Assembly assembly)
			{
				if (delegateAssemblies.ContainsKey(assembly))
				{
					return;
				}
				delegateAssemblies[assembly] = null;
				object[] customAttributes = assembly.GetCustomAttributes(typeof(FakesDelegatesTypeAttribute), inherit: false);
				for (int i = 0; i < customAttributes.Length; i++)
				{
					FakesDelegatesTypeAttribute fakesDelegatesTypeAttribute = (FakesDelegatesTypeAttribute)customAttributes[i];
					Type[] nestedTypes = fakesDelegatesTypeAttribute.HolderType.GetNestedTypes();
					foreach (Type type in nestedTypes)
					{
						if (ReflectionContract.IsDelegate(type))
						{
							delegates.AddDelegate(type);
						}
					}
				}
			}

			private static void InternalBindInterface<TShimmed>(Type shimType, Type shimmedType, Type boundType, ShimBase<TShimmed> shim, object bound) where TShimmed : class
			{
				Type type = bound.GetType();
				MethodInfo[] methods = boundType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
				int num = 0;
				MethodInfo methodInfo;
				while (true)
				{
					if (num < methods.Length)
					{
						methodInfo = methods[num];
						if (methodInfo.IsGenericMethod)
						{
							throw new ShimMethodNotBindableException(string.Format(CultureInfo.InvariantCulture, "{0} is not bindable", new object[1]
							{
								methodInfo
							}));
						}
						MethodInfo method = ReflectionHelper.VTableLookup(shimmedType, methodInfo);
						if (delegates.TryGetDelegateType(boundType, methodInfo, useThisParameter: false, out Type delegateType, out MethodInfo _) != DelegateInstantiationType.Success || !TryResolveImplementationMethod(type, methodInfo, out MethodInfo runtimeImplementationMethod))
						{
							break;
						}
						Delegate @delegate = Delegate.CreateDelegate(delegateType, bound, runtimeImplementationMethod, throwOnBindFailure: true);
						Delegate optionalStub = UnCurryDelegate(@delegate, shimmedType);
						SetShimMethod(optionalStub, shim.Instance, method);
						num++;
						continue;
					}
					return;
				}
				throw new ShimMethodNotBindableException(string.Format(CultureInfo.InvariantCulture, "{0} is not bindable", new object[1]
				{
					methodInfo
				}));
			}

			private static Delegate UnCurryDelegate(Delegate @delegate, Type receiverType)
			{
				Type type = @delegate.GetType();
				TypePair key = new TypePair(type, receiverType);
				if (!uncurrifiers.TryGetValue(key, out FakesDelegates.Func<Delegate, Delegate> value))
				{
					value = (uncurrifiers[key] = CreateUnCurrifier(@delegate, receiverType));
				}
				return value(@delegate);
			}

			private static FakesDelegates.Func<Delegate, Delegate> CreateUnCurrifier(Delegate @delegate, Type receiverType)
			{
				Type type = @delegate.GetType();
				if (delegates.TryGetUncurryMethod(receiverType, type, out MethodInfo uncurrifyMethod))
				{
					return (Delegate d) => (Delegate)uncurrifyMethod.Invoke(null, new object[1]
					{
						d
					});
				}
				return null;
			}

			private static bool TryResolveImplementationMethod(Type implementationType, MethodInfo boundMethod, out MethodInfo runtimeImplementationMethod)
			{
				runtimeImplementationMethod = ReflectionHelper.VTableLookup(implementationType, boundMethod);
				return (object)runtimeImplementationMethod != null;
			}
		}

		private static readonly object syncLock;

		private static readonly List<FakesDelegates.Action> stateCleaners;

		private static int gcontext;

		public static bool HasContext
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				return gcontext > 0;
			}
		}

		public static bool IsThreadProtected => UnitTestIsolationRuntime.IsThreadProtected;

		public static event EventHandler<ShimMethodShimmedEventArgs> ShimmedMethod;

		static ShimRuntime()
		{
			syncLock = new object();
			stateCleaners = new List<FakesDelegates.Action>();
			UnitTestIsolationRuntime.AttachedUnsupportedMethod += RaiseShimNotSupported;
		}

		[DebuggerNonUserCode]
		private static void RaiseShimNotSupported(MethodBase method)
		{
			throw new ShimNotSupportedException(((object)method.DeclaringType != null) ? method.DeclaringType.ToString() : method.Module.ToString());
		}

		public static IDisposable AcquireProtectingThreadContext()
		{
			return UnitTestIsolationRuntime.AcquireProtectingThreadContext();
		}

		internal static void EnsureContext()
		{
			if (!HasContext)
			{
				throw new ShimInvalidOperationException(FakesFrameworkResources.ShimsContextNotCreated);
			}
			UnitTestIsolationRuntime.EnsureInstrumentationProvider();
		}

		internal static IDisposable CreateContext()
		{
			try
			{
				UnitTestIsolationRuntime.InitializeUnitTestIsolationInstrumentationProvider();
				UnitTestIsolationRuntime.EnsureInstrumentationProvider();
				using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
				{
					return new Context();
				}
			}
			catch (Exception data)
			{
				FakesTraceSources.Runtime.TraceData(TraceEventType.Error, 0, data);
				throw;
			}
		}

		public static object CreateUninitializedInstance(Type runtimeType)
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				if ((object)runtimeType == null)
				{
					throw new ArgumentNullException("runtimeType");
				}
				if (runtimeType.IsAbstract)
				{
					throw new ShimInvalidOperationException(FakesFrameworkResources.CannotInstantiateAbstractType + runtimeType.ToString());
				}
				object uninitializedObject = FormatterServices.GetUninitializedObject(runtimeType);
				GC.SuppressFinalize(uninitializedObject);
				return uninitializedObject;
			}
		}

		public static T CreateUninitializedInstance<T>() where T : class
		{
			return (T)CreateUninitializedInstance(typeof(T));
		}

		public static void RegisterStateCleaner(FakesDelegates.Action stateCleaner)
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				if (stateCleaner == null)
				{
					throw new ArgumentNullException("stateCleaner");
				}
				lock (syncLock)
				{
					stateCleaners.Add(stateCleaner);
				}
			}
		}

		public static void SetShimPublicInstance(Delegate optionalStub, Type receiverType, object optionalReceiver, string name, Type returnType, params Type[] parameterTypes)
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				SetShim(optionalStub, receiverType, optionalReceiver, name, ShimBinding.PublicInstance, returnType, parameterTypes);
			}
		}

		public static void SetShimNonPublicInstance(Delegate optionalStub, Type receiverType, object optionalReceiver, string name, Type returnType, params Type[] parameterTypes)
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				SetShim(optionalStub, receiverType, optionalReceiver, name, ShimBinding.NonPublicInstance, returnType, parameterTypes);
			}
		}

		public static void SetShimPublicStatic(Delegate optionalStub, Type receiverType, string name, Type returnType, params Type[] parameterTypes)
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				SetShim(optionalStub, receiverType, null, name, ShimBinding.PublicStatic, returnType, parameterTypes);
			}
		}

		public static void SetShimNonPublicStatic(Delegate optionalStub, Type receiverType, string name, Type returnType, params Type[] parameterTypes)
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				SetShim(optionalStub, receiverType, null, name, ShimBinding.NonPublicStatic, returnType, parameterTypes);
			}
		}

		private static void SetShim(Delegate optionalStub, Type receiverType, object optionalReceiver, string name, ShimBinding flags, Type returnType, params Type[] parameterTypes)
		{
			if (TryResolveMethod(name, flags, receiverType, returnType, parameterTypes, out MethodBase method))
			{
				SetShimMethod(optionalStub, optionalReceiver, method);
				return;
			}
			throw new ShimOutdatedException("could not resolve method " + name);
		}

		private static bool TryResolveMethod(string name, ShimBinding flags, Type receiverTypeEx, Type returnTypeEx, Type[] parameterTypeExs, out MethodBase method)
		{
			if (!ReflectionHelper.TryGetMethod(receiverTypeEx, name, (BindingFlags)flags, parameterTypeExs, out method))
			{
				Type typeDefinition = ReflectionHelper.GetTypeDefinition(receiverTypeEx);
				MethodInfo[] methods = typeDefinition.GetMethods((BindingFlags)flags);
				foreach (MethodInfo methodInfo in methods)
				{
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (!methodInfo.IsGenericMethod && methodInfo.Name == name && parameters.Length == parameterTypeExs.Length)
					{
						method = ReflectionHelper.Instantiate(methodInfo, ReflectionHelper.GetGenericTypeArguments(receiverTypeEx), Type.EmptyTypes);
						Type[] right = Array.ConvertAll(parameters, (ParameterInfo p) => p.ParameterType);
						if (returnTypeEx.Equals(ReflectionHelper.GetReturnType(method)) && AreEqual(parameterTypeExs, right))
						{
							break;
						}
					}
				}
			}
			return (object)method != null;
		}

		private static bool AreEqual(Type[] left, Type[] right)
		{
			if (left.Length != right.Length)
			{
				return false;
			}
			for (int i = 0; i < left.Length; i++)
			{
				if (!left[i].Equals(right[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static void SetGenericShimPublicInstance(Delegate optionalStub, Type receiverType, object optionalReceiver, string name, Type[] genericMethodArguments, Type returnType, params Type[] parameterTypes)
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				SetGenericShim(optionalStub, receiverType, optionalReceiver, name, ShimBinding.PublicInstance, genericMethodArguments, returnType, parameterTypes);
			}
		}

		public static void SetGenericShimPublicStatic(Delegate optionalStub, Type receiverType, string name, Type[] genericMethodArguments, Type returnType, params Type[] parameterTypes)
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				SetGenericShim(optionalStub, receiverType, null, name, ShimBinding.PublicStatic, genericMethodArguments, returnType, parameterTypes);
			}
		}

		public static void SetGenericShimNonPublicInstance(Delegate optionalStub, Type receiverType, object optionalReceiver, string name, Type[] genericMethodArguments, Type returnType, params Type[] parameterTypes)
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				SetGenericShim(optionalStub, receiverType, optionalReceiver, name, ShimBinding.NonPublicInstance, genericMethodArguments, returnType, parameterTypes);
			}
		}

		public static void SetGenericShimNonPublicStatic(Delegate optionalStub, Type receiverType, string name, Type[] genericMethodArguments, Type returnType, params Type[] parameterTypes)
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				SetGenericShim(optionalStub, receiverType, null, name, ShimBinding.NonPublicStatic, genericMethodArguments, returnType, parameterTypes);
			}
		}

		private static void SetGenericShim(Delegate optionalStub, Type receiverType, object optionalReceiver, string name, ShimBinding flags, Type[] genericMethodArguments, Type returnType, params Type[] parameterTypes)
		{
			int num = genericMethodArguments.Length;
			MethodInfo[] methods = receiverType.GetMethods((BindingFlags)flags);
			foreach (MethodInfo methodInfo in methods)
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (methodInfo.Name == name && parameters.Length == parameterTypes.Length && methodInfo.GetGenericArguments().Length == num)
				{
					MethodInfo methodInfo2 = ReflectionHelper.Instantiate(methodInfo, genericMethodArguments);
					Type[] right = Array.ConvertAll(methodInfo2.GetParameters(), (ParameterInfo p) => p.ParameterType);
					if (returnType.Equals(ReflectionHelper.GetReturnType(methodInfo2)) && AreEqual(parameterTypes, right))
					{
						SetShimMethod(optionalStub, optionalReceiver, methodInfo2);
						return;
					}
				}
			}
			throw new ShimOutdatedException("could not resolve method " + name);
		}

		public static void SetShim(Delegate optionalStub, object optionalReceiver, MethodBase method)
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				ValidateReceiverAndMethod(optionalReceiver, method);
				SetShimMethod(optionalStub, optionalReceiver, method);
			}
		}

		public static Delegate GetShim(object optionalReceiver, MethodBase method)
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				ValidateReceiverAndMethod(optionalReceiver, method);
				return GetShimMethod(optionalReceiver, method);
			}
		}

		private static void ValidateReceiverAndMethod(object optionalReceiver, MethodBase method)
		{
			if ((object)method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (method.IsAbstract)
			{
				throw new ArgumentException(FakesFrameworkResources.MethodIsAbstract, "method");
			}
			if (method.IsGenericMethodDefinition)
			{
				throw new ArgumentException(FakesFrameworkResources.MethodMustBeAFullyInstantiated, "method");
			}
			if (method.IsStatic && method.IsConstructor)
			{
				throw new ArgumentException(FakesFrameworkResources.MethodIsAStaticConstructor, "method");
			}
			if (optionalReceiver != null && method.IsStatic)
			{
				throw new ArgumentException(FakesFrameworkResources.StaticMethodCannotBeDispatchedToInstances, "method");
			}
			Type declaringType = method.DeclaringType;
			if ((object)declaringType != null && declaringType.IsGenericTypeDefinition)
			{
				throw new ArgumentException(FakesFrameworkResources.MethodMustBeAFullyInstantiated, "method");
			}
		}

		private static void SetShimMethod(Delegate optionalStub, object optionalReceiver, MethodBase method)
		{
			EnsureContext();
			OnShimmedMethod(optionalStub, optionalReceiver, method);
			if ((object)optionalStub == null)
			{
				UnitTestIsolationRuntime.DetachDetour(optionalReceiver, method);
			}
			else
			{
				UnitTestIsolationRuntime.AttachDetour(optionalReceiver, method, optionalStub);
			}
		}

		private static Delegate GetShimMethod(object _receiver, MethodBase method)
		{
			EnsureContext();
			Delegate detourOrDefault = UnitTestIsolationRuntime.GetDetourOrDefault(_receiver, method);
			IUncurrifier uncurrifier = detourOrDefault as IUncurrifier;
			if (uncurrifier != null)
			{
				return uncurrifier.InnerDelegate;
			}
			return detourOrDefault;
		}

		private static void OnShimmedMethod(Delegate _stub, object _receiver, MethodBase method)
		{
			ShimRuntime.ShimmedMethod?.Invoke(null, new ShimMethodShimmedEventArgs(_stub, _receiver, method));
		}

		public static void ClearCache()
		{
			BindCompiler.ClearCache();
		}

		public static void Clear()
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				lock (syncLock)
				{
					UnitTestIsolationRuntime.ClearDetours();
					InstancedPool.Clear();
					ShimBehaviors.Clear();
					CleanAndResetState();
				}
			}
		}

		private static void CleanAndResetState()
		{
			foreach (FakesDelegates.Action stateCleaner in stateCleaners)
			{
				stateCleaner();
			}
			stateCleaners.Clear();
		}

		public static void Bind<TShimmed, TShim, TBound>(TShim shim, TBound bound) where TShimmed : class, TBound where TShim : ShimBase<TShimmed> where TBound : class
		{
			MethodInfo methodInfo = null;
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				if (shim == null)
				{
					throw new ArgumentNullException("shim");
				}
				if (bound == null)
				{
					throw new ArgumentNullException("bound");
				}
				Type type = bound.GetType();
				if (ReflectionHelper.GetTypeSpec(type) == TypeSpec.SzArray)
				{
					Type typeFromHandle = typeof(TShim);
					Type typeFromHandle2 = typeof(TShimmed);
					Type typeFromHandle3 = typeof(TBound);
					methodInfo = ReflectionHelper.Instantiate(Metadata.BindArrayMethodDefinition, new Type[4]
					{
						typeFromHandle2,
						typeFromHandle,
						typeFromHandle3,
						type.GetElementType()
					});
				}
			}
			if ((object)methodInfo != null)
			{
				methodInfo.Invoke(null, new object[2]
				{
					shim,
					bound
				});
			}
			else
			{
				BindNonArray<TShimmed, TShim, TBound>(shim, bound);
			}
		}

		private static void BindNonArray<TShimmed, TShim, TBound>(TShim shim, TBound bound) where TShimmed : class, TBound where TShim : ShimBase<TShimmed> where TBound : class
		{
			using (UnitTestIsolationRuntime.AcquireProtectingThreadContext())
			{
				Type typeFromHandle = typeof(TShim);
				Type typeFromHandle2 = typeof(TShimmed);
				Type typeFromHandle3 = typeof(TBound);
				BindCompiler.Bind(typeFromHandle, typeFromHandle2, typeFromHandle3, shim, bound);
			}
		}

		internal static void AttachBehavior(ref ShimInstanceBehaviorClosure _behavior, object receiver, Type _targetType, IShimBehavior shimBehavior)
		{
			IShimBehavior shimBehavior2 = (_behavior == null) ? null : _behavior.ShimBehavior;
			if (_behavior == null || !_behavior.TryGetTargetType(out Type targetType))
			{
				targetType = null;
			}
			if (shimBehavior2 != shimBehavior || (object)targetType != _targetType)
			{
				if (_behavior != null)
				{
					UnitTestIsolationRuntime.DetachFallbackBehavior(_behavior.GetBehavior);
				}
				if (shimBehavior == null)
				{
					_behavior = null;
					return;
				}
				_behavior = new ShimInstanceBehaviorClosure(receiver, _targetType, shimBehavior);
				UnitTestIsolationRuntime.AttachFallbackBehavior(_behavior.GetBehavior);
			}
		}
	}
}
