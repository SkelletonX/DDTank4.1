using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	internal static class FSharpUtils
	{
		public const string FSharpSetTypeName = "FSharpSet`1";

		public const string FSharpListTypeName = "FSharpList`1";

		public const string FSharpMapTypeName = "FSharpMap`2";

		private static readonly object Lock = new object();

		private static bool _initialized;

		private static MethodInfo _ofSeq;

		private static Type _mapType;

		public static Assembly FSharpCoreAssembly
		{
			get;
			private set;
		}

		public static MethodCall<object, object> IsUnion
		{
			get;
			private set;
		}

		public static MethodCall<object, object> GetUnionFields
		{
			get;
			private set;
		}

		public static MethodCall<object, object> GetUnionCases
		{
			get;
			private set;
		}

		public static MethodCall<object, object> MakeUnion
		{
			get;
			private set;
		}

		public static Func<object, object> GetUnionCaseInfoName
		{
			get;
			private set;
		}

		public static Func<object, object> GetUnionCaseInfo
		{
			get;
			private set;
		}

		public static Func<object, object> GetUnionCaseFields
		{
			get;
			private set;
		}

		public static MethodCall<object, object> GetUnionCaseInfoFields
		{
			get;
			private set;
		}

		public static void EnsureInitialized(Assembly fsharpCoreAssembly)
		{
			if (!_initialized)
			{
				lock (Lock)
				{
					if (!_initialized)
					{
						FSharpCoreAssembly = fsharpCoreAssembly;
						Type type = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpType");
						MethodInfo method = type.GetMethod("IsUnion", BindingFlags.Static | BindingFlags.Public);
						IsUnion = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
						MethodInfo method2 = type.GetMethod("GetUnionCases", BindingFlags.Static | BindingFlags.Public);
						GetUnionCases = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method2);
						Type type2 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpValue");
						MethodInfo method3 = type2.GetMethod("GetUnionFields", BindingFlags.Static | BindingFlags.Public);
						GetUnionFields = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method3);
						GetUnionCaseInfo = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(method3.ReturnType.GetProperty("Item1"));
						GetUnionCaseFields = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(method3.ReturnType.GetProperty("Item2"));
						MethodInfo method4 = type2.GetMethod("MakeUnion", BindingFlags.Static | BindingFlags.Public);
						MakeUnion = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method4);
						Type type3 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.UnionCaseInfo");
						GetUnionCaseInfoName = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Name"));
						GetUnionCaseInfoFields = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(type3.GetMethod("GetFields"));
						Type type4 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.ListModule");
						_ofSeq = type4.GetMethod("OfSeq");
						_mapType = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.FSharpMap`2");
						Thread.MemoryBarrier();
						_initialized = true;
					}
				}
			}
		}

		public static ObjectConstructor<object> CreateSeq(Type t)
		{
			MethodInfo method = _ofSeq.MakeGenericMethod(t);
			return JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(method);
		}

		public static ObjectConstructor<object> CreateMap(Type keyType, Type valueType)
		{
			MethodInfo method = typeof(FSharpUtils).GetMethod("BuildMapCreator");
			MethodInfo methodInfo = method.MakeGenericMethod(keyType, valueType);
			return (ObjectConstructor<object>)methodInfo.Invoke(null, null);
		}

		public static ObjectConstructor<object> BuildMapCreator<TKey, TValue>()
		{
			Type type = _mapType.MakeGenericType(typeof(TKey), typeof(TValue));
			ConstructorInfo constructor = type.GetConstructor(new Type[1]
			{
				typeof(IEnumerable<Tuple<TKey, TValue>>)
			});
			ObjectConstructor<object> ctorDelegate = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(constructor);
			return delegate(object[] args)
			{
				IEnumerable<KeyValuePair<TKey, TValue>> source = (IEnumerable<KeyValuePair<TKey, TValue>>)args[0];
				IEnumerable<Tuple<TKey, TValue>> enumerable = source.Select((KeyValuePair<TKey, TValue> kv) => new Tuple<TKey, TValue>(kv.Key, kv.Value));
				return ctorDelegate(enumerable);
			};
		}
	}
}
