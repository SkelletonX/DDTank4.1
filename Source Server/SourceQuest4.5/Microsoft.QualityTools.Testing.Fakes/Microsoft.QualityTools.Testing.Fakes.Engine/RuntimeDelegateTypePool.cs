using Microsoft.QualityTools.Testing.Fakes.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.QualityTools.Testing.Fakes.Engine
{
	internal sealed class RuntimeDelegateTypePool
	{
		[StructLayout(LayoutKind.Auto)]
		private struct SignatureKey : IEquatable<SignatureKey>
		{
			private const int FNV1_prime_32 = 16777619;

			private readonly bool hasResult;

			private readonly byte[] parameterSpecs;

			public SignatureKey(MethodBase invokeMethod, bool useThisParameter)
			{
				hasResult = ((object)ReflectionHelper.GetReturnType(invokeMethod) != typeof(void));
				ParameterInfo[] parameters = invokeMethod.GetParameters();
				int num = 0;
				if (useThisParameter && !invokeMethod.IsStatic)
				{
					num = 1;
				}
				parameterSpecs = new byte[parameters.Length + num];
				if (num > 0)
				{
					parameterSpecs[0] = Encode(isByRef: false, isOut: false);
				}
				for (int i = 0; i < parameters.Length; i++)
				{
					ParameterInfo parameterInfo = parameters[i];
					bool isByRef = parameterInfo.ParameterType.IsByRef;
					bool isOut = parameterInfo.IsOut;
					parameterSpecs[i + num] = Encode(isByRef, isOut);
				}
			}

			private static byte Encode(bool isByRef, bool isOut)
			{
				return (byte)((isByRef ? 1 : 0) | (isOut ? 4 : 2));
			}

			public bool Equals(SignatureKey other)
			{
				if (other.hasResult != hasResult || other.parameterSpecs.Length != parameterSpecs.Length)
				{
					return false;
				}
				byte[] array = other.parameterSpecs;
				byte[] array2 = parameterSpecs;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != array2[i])
					{
						return false;
					}
				}
				return true;
			}

			private static int Fold(int hash, byte value)
			{
				return (hash * 16777619) ^ value;
			}

			public override int GetHashCode()
			{
				int num = hasResult ? 1 : 0;
				for (int i = 0; i < parameterSpecs.Length; i++)
				{
					num = Fold(num, parameterSpecs[i]);
				}
				return num;
			}
		}

		private readonly Dictionary<SignatureKey, MethodInfo> delegates = new Dictionary<SignatureKey, MethodInfo>();

		private readonly Dictionary<Type, MethodInfo> uncurryMethods = new Dictionary<Type, MethodInfo>();

		public RuntimeDelegateTypePool()
		{
			Type typeFromHandle = typeof(FakesDelegates);
			Type[] nestedTypes = typeFromHandle.GetNestedTypes(BindingFlags.Public);
			foreach (Type type in nestedTypes)
			{
				if (ReflectionContract.IsDelegate(type))
				{
					AddDelegate(type);
				}
			}
			Type typeFromHandle2 = typeof(FakesExtensions);
			MethodInfo[] methods = typeFromHandle2.GetMethods(BindingFlags.Static | BindingFlags.Public);
			foreach (MethodInfo methodInfo in methods)
			{
				if (methodInfo.Name == "Uncurrify")
				{
					AddUncurryMethod(methodInfo);
				}
			}
		}

		public bool AddDelegate(Type td)
		{
			MethodInfo method = td.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
			return AddInvokeMethod(method);
		}

		public bool AddInvokeMethod(MethodInfo invokeMethod)
		{
			SignatureKey key = new SignatureKey(invokeMethod, useThisParameter: false);
			if (!delegates.ContainsKey(key))
			{
				delegates.Add(key, invokeMethod);
				return true;
			}
			return false;
		}

		public DelegateInstantiationType TryGetDelegateType(Type declaringType, MethodBase method, bool useThisParameter, out Type delegateType, out MethodInfo invokeDelegateMethod)
		{
			MethodBase methodDefinition = ReflectionHelper.GetMethodDefinition(method);
			if ((object)methodDefinition == null)
			{
				delegateType = null;
				invokeDelegateMethod = null;
				return DelegateInstantiationType.NotSupported;
			}
			SignatureKey key = new SignatureKey(methodDefinition, useThisParameter);
			if (delegates.TryGetValue(key, out MethodInfo value))
			{
				if (TryCreateMatch(declaringType, method, useThisParameter, value, out delegateType, out invokeDelegateMethod))
				{
					return DelegateInstantiationType.Success;
				}
				delegateType = null;
				invokeDelegateMethod = null;
				return DelegateInstantiationType.NotSupported;
			}
			delegateType = null;
			invokeDelegateMethod = null;
			return DelegateInstantiationType.NotFound;
		}

		private static bool TryCreateMatch(Type declaringType, MethodBase method, bool useThisParameter, MethodInfo invokeDefinition, out Type delegateType, out MethodInfo invokeMethod)
		{
			MethodBase methodDefinition = ReflectionHelper.GetMethodDefinition(method);
			if ((object)methodDefinition == null)
			{
				delegateType = null;
				invokeMethod = null;
				return false;
			}
			Type returnType = ReflectionHelper.GetReturnType(method);
			bool flag = (object)returnType == typeof(void);
			if (!IsTypeSupported(returnType) || flag == ReflectionHelper.GetReturnType(invokeDefinition).IsGenericParameter)
			{
				delegateType = null;
				invokeMethod = null;
				return false;
			}
			int num = (useThisParameter && !method.IsStatic) ? 1 : 0;
			ParameterInfo[] parameters = method.GetParameters();
			ParameterInfo[] parameters2 = invokeDefinition.GetParameters();
			if (parameters2.Length != parameters.Length + num)
			{
				delegateType = null;
				invokeMethod = null;
				return false;
			}
			Type[] array = Array.ConvertAll(parameters, (ParameterInfo p) => p.ParameterType);
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo delegateParameter = parameters2[i + num];
				ParameterInfo parameter = parameters[i];
				Type parameterType = array[i];
				if (!IsParameterSupported(delegateParameter, parameter, parameterType))
				{
					delegateType = null;
					invokeMethod = null;
					return false;
				}
			}
			List<Type> list = new List<Type>(1 + parameters.Length + num);
			if (num > 0)
			{
				list.Add(GetInstantiationType(declaringType));
			}
			for (int j = 0; j < parameters.Length; j++)
			{
				list.Add(GetInstantiationType(array[j]));
			}
			if (!flag)
			{
				list.Add(GetInstantiationType(returnType));
			}
			invokeMethod = ReflectionHelper.Instantiate(invokeDefinition, list.ToArray(), Type.EmptyTypes);
			delegateType = invokeMethod.DeclaringType;
			return true;
		}

		private static Type GetInstantiationType(Type type)
		{
			switch (ReflectionHelper.GetTypeSpec(type))
			{
			case TypeSpec.SzArray:
			case TypeSpec.MdArray:
			case TypeSpec.ValueType:
			case TypeSpec.Class:
			case TypeSpec.GenericParameter:
				return type;
			case TypeSpec.Pointer:
			case TypeSpec.ManagedPointer:
				return type.GetElementType();
			default:
				return type;
			}
		}

		private static bool IsParameterSupported(ParameterInfo delegateParameter, ParameterInfo parameter, Type parameterType)
		{
			if (delegateParameter.IsOut != parameter.IsOut)
			{
				return false;
			}
			TypeSpec typeSpec = ReflectionHelper.GetTypeSpec(parameterType);
			if (ReflectionHelper.GetTypeSpec(delegateParameter.ParameterType) == TypeSpec.ManagedPointer != (typeSpec == TypeSpec.ManagedPointer))
			{
				return false;
			}
			switch (typeSpec)
			{
			case TypeSpec.ValueType:
			case TypeSpec.Class:
				return IsTypeSupported(parameterType);
			case TypeSpec.ManagedPointer:
				return IsTypeSupported(parameterType.GetElementType());
			case TypeSpec.SzArray:
			case TypeSpec.MdArray:
				return IsTypeSupported(parameterType.GetElementType());
			case TypeSpec.GenericParameter:
				return true;
			default:
				return false;
			}
		}

		private static bool IsTypeSupported(Type parameterType)
		{
			switch (ReflectionHelper.GetTypeSpec(parameterType))
			{
			case TypeSpec.ValueType:
			case TypeSpec.Class:
			case TypeSpec.GenericParameter:
				if ((object)parameterType == typeof(TypedReference))
				{
					return false;
				}
				return true;
			case TypeSpec.SzArray:
			case TypeSpec.MdArray:
				return IsTypeSupported(parameterType.GetElementType());
			default:
				return false;
			}
		}

		public void AddUncurryMethod(MethodInfo method)
		{
			ParameterInfo[] parameters = method.GetParameters();
			Type parameterType = parameters[0].ParameterType;
			Type typeDefinition = ReflectionHelper.GetTypeDefinition(parameterType);
			uncurryMethods.Add(typeDefinition, method);
		}

		public bool TryGetUncurryMethod(Type receiverType, Type delegateType, out MethodInfo uncurryMethod)
		{
			Type typeDefinition = ReflectionHelper.GetTypeDefinition(delegateType);
			if ((object)typeDefinition != null && uncurryMethods.TryGetValue(typeDefinition, out MethodInfo value))
			{
				Type[] genericArguments = delegateType.GetGenericArguments();
				Type[] array = new Type[1 + genericArguments.Length];
				array[0] = receiverType;
				genericArguments.CopyTo(array, 1);
				uncurryMethod = ReflectionHelper.Instantiate(value, array);
				return true;
			}
			uncurryMethod = null;
			return false;
		}
	}
}
