using Microsoft.QualityTools.Testing.Fakes.Shims;
using Microsoft.QualityTools.Testing.Fakes.UnitTestIsolation;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.QualityTools.Testing.Fakes.Instances
{
	public static class InstancedPool
	{
		private sealed class InstanceDictionary : Dictionary<object, IInstanced>
		{
			public InstanceDictionary()
				: base(ObjectReferenceIdentity.Default)
			{
			}
		}

		private static readonly object syncLock = new object();

		private static readonly InstanceDictionary instances = new InstanceDictionary();

		public static void Clear()
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				lock (syncLock)
				{
					instances.Clear();
				}
			}
		}

		public static void RegisterInstanced(IInstanced instanced)
		{
			if (instanced == null)
			{
				throw new ArgumentNullException("instanced");
			}
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				object instance = instanced.Instance;
				if (instance == null)
				{
					throw new ArgumentOutOfRangeException("instanced");
				}
				lock (syncLock)
				{
					instances[instance] = instanced;
				}
			}
		}

		public static object GetInstancedOrSelf(object optionalInstance)
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				if (optionalInstance != null)
				{
					lock (syncLock)
					{
						if (TryGetInstancedUnlocked(optionalInstance, out IInstanced instanced))
						{
							return instanced;
						}
						return optionalInstance;
					}
				}
				return null;
			}
		}

		public static bool TryGetInstanced<T>(object instance, out T instanced) where T : class, IInstanced
		{
			if (!TryGetInstanced(instance, out IInstanced instanced2))
			{
				instanced2 = null;
			}
			instanced = (instanced2 as T);
			return instanced != null;
		}

		public static bool TryGetInstanced(object instance, out IInstanced instanced)
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				if (instance != null)
				{
					lock (syncLock)
					{
						return TryGetInstancedUnlocked(instance, out instanced);
					}
				}
				instanced = null;
				return false;
			}
		}

		private static bool TryGetInstancedUnlocked(object instance, out IInstanced instanced)
		{
			return instances.TryGetValue(instance, out instanced);
		}

		[DebuggerHidden]
		public static TResult CastAsInstance<T, TResult>(T target) where T : IInstanced
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				if (target == null)
				{
					return default(TResult);
				}
				return (TResult)target.Instance;
			}
		}

		[DebuggerHidden]
		public static TResult CastAsInstanced<T, TResult>(T target) where TResult : class, IInstanced
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				if (target == null)
				{
					return null;
				}
				if (!TryGetInstanced(target, out TResult instanced))
				{
					return null;
				}
				return instanced;
			}
		}

		public static object[] GetInstancedObjects()
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				lock (syncLock)
				{
					return new List<object>(instances.Keys).ToArray();
				}
			}
		}
	}
}
