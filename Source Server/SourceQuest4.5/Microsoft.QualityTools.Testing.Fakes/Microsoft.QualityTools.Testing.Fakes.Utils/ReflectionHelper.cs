using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.QualityTools.Testing.Fakes.Utils
{
	internal static class ReflectionHelper
	{
		private struct ModuleToken : IEquatable<ModuleToken>
		{
			public readonly Module Module;

			public readonly int Token;

			public ModuleToken(Module module, int token)
			{
				Module = module;
				Token = token;
			}

			public override int GetHashCode()
			{
				return Token;
			}

			public bool Equals(ModuleToken other)
			{
				if ((object)Module == other.Module)
				{
					return Token == other.Token;
				}
				return false;
			}
		}

		[Pure]
		public static MethodBase GetMethodDefinition(MethodBase methodBase)
		{
			MethodInfo methodInfo = methodBase as MethodInfo;
			if ((object)methodInfo != null && methodInfo.IsGenericMethod)
			{
				return methodInfo.GetGenericMethodDefinition();
			}
			return methodBase;
		}

		[Pure]
		public static Type GetReturnType(MethodBase method)
		{
			MethodInfo methodInfo = method as MethodInfo;
			if ((object)methodInfo != null)
			{
				return methodInfo.ReturnType;
			}
			return typeof(void);
		}

		[Pure]
		public static TypeSpec GetTypeSpec(Type parameterType)
		{
			if (parameterType.IsArray)
			{
				if (parameterType.GetArrayRank() > 1)
				{
					return TypeSpec.MdArray;
				}
				return TypeSpec.SzArray;
			}
			if (parameterType.IsValueType)
			{
				return TypeSpec.ValueType;
			}
			if (parameterType.IsGenericParameter)
			{
				return TypeSpec.GenericParameter;
			}
			if (parameterType.IsPointer)
			{
				return TypeSpec.Pointer;
			}
			if (parameterType.IsByRef)
			{
				return TypeSpec.ManagedPointer;
			}
			if (parameterType.IsClass || parameterType.IsInterface)
			{
				return TypeSpec.Class;
			}
			return TypeSpec.Class;
		}

		[Pure]
		public static Type GetTypeDefinition(Type type)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition();
			}
			return type;
		}

		public static MethodInfo Instantiate(MethodInfo methodDefinition, Type[] genericMethodArguments)
		{
			if (genericMethodArguments.Length == 0)
			{
				return methodDefinition;
			}
			return methodDefinition.MakeGenericMethod(genericMethodArguments);
		}

		public static MethodInfo Instantiate(MethodInfo methodDefinition, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			if (genericTypeArguments.Length == 0)
			{
				if (genericMethodArguments.Length == 0)
				{
					return methodDefinition;
				}
				return methodDefinition.MakeGenericMethod(genericMethodArguments);
			}
			Type type = methodDefinition.DeclaringType.MakeGenericType(genericTypeArguments);
			BindingFlags bindingAttr = (BindingFlags)((methodDefinition.IsStatic ? 8 : 4) | (methodDefinition.IsPublic ? 16 : 32));
			MethodInfo method = type.GetMethod(methodDefinition.Name, bindingAttr);
			if (genericMethodArguments.Length == 0)
			{
				return method;
			}
			return method.MakeGenericMethod(genericMethodArguments);
		}

		public static Type[] GetGenericTypeArguments(Type type)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericArguments();
			}
			return Type.EmptyTypes;
		}

		[Pure]
		public static bool TryGetDeclaringType(MethodBase method, out Type declaringType)
		{
			declaringType = method.DeclaringType;
			return (object)declaringType != null;
		}

		[Pure]
		public static bool TryGetMethod(Type type, string name, BindingFlags flags, Type[] parameterTypes, out MethodBase method)
		{
			try
			{
				if (name == ".ctor" || name == ".cctor")
				{
					method = type.GetConstructor(flags, null, parameterTypes, null);
				}
				else
				{
					method = type.GetMethod(name, flags, null, parameterTypes, null);
				}
			}
			catch (AmbiguousMatchException)
			{
				method = null;
			}
			return (object)method != null;
		}

		private static MethodInfo GetRootDefinition(MethodInfo methodInfo)
		{
			bool wasBaseDefinitionAlready = false;
			do
			{
				methodInfo = GetBaseDefinition(methodInfo, out wasBaseDefinitionAlready);
			}
			while (!wasBaseDefinitionAlready);
			return methodInfo;
		}

		private static MethodInfo GetBaseDefinition(MethodInfo methodInfo, out bool wasBaseDefinitionAlready)
		{
			if (methodInfo.DeclaringType.IsValueType)
			{
				methodInfo = RebuildMethodInfo(methodInfo);
			}
			MethodInfo baseDefinition = GetBaseDefinition(methodInfo);
			if ((object)baseDefinition != null && GetMetadataTokenOrZero(baseDefinition) == GetMetadataTokenOrZero(methodInfo) && (object)baseDefinition.Module == methodInfo.Module)
			{
				wasBaseDefinitionAlready = true;
				return baseDefinition;
			}
			wasBaseDefinitionAlready = false;
			return baseDefinition;
		}

		private static MethodInfo GetBaseDefinition(MethodInfo methodInfo)
		{
			try
			{
				return methodInfo.GetBaseDefinition();
			}
			catch (ArgumentException)
			{
				return GetBaseDefinition(RebuildMethodInfo(methodInfo));
			}
			catch (Exception)
			{
				return GetBaseDefinition(RebuildMethodInfo(methodInfo));
			}
		}

		private static MethodInfo RebuildMethodInfo(MethodInfo methodInfo)
		{
			Type declaringType = methodInfo.DeclaringType;
			int metadataTokenOrZero = GetMetadataTokenOrZero(methodInfo);
			if (metadataTokenOrZero != 0)
			{
				methodInfo = (MethodInfo)methodInfo.Module.ResolveMethod(metadataTokenOrZero, ((object)declaringType == null || !ReflectionContract.IsGenericType(declaringType)) ? Type.EmptyTypes : methodInfo.GetGenericArguments(), (!methodInfo.IsGenericMethod) ? Type.EmptyTypes : methodInfo.GetGenericArguments());
			}
			return methodInfo;
		}

		public static int GetMetadataTokenOrZero(MemberInfo memberInfo)
		{
			try
			{
				return memberInfo.MetadataToken;
			}
			catch (InvalidOperationException)
			{
				return 0;
			}
		}

		public static MethodInfo VTableLookup(Type targetType, MethodInfo runtimeMethod)
		{
			MethodInfo methodInfo = (MethodInfo)GetMethodDefinition(runtimeMethod);
			MethodInfo methodInfo2 = vTableLookup(targetType, methodInfo);
			if ((object)methodInfo != runtimeMethod)
			{
				methodInfo2 = methodInfo2.MakeGenericMethod(runtimeMethod.GetGenericArguments());
			}
			return methodInfo2;
		}

		private static MethodInfo vTableLookup(Type me, MethodInfo method)
		{
			Type declaringType = method.DeclaringType;
			if (ReflectionContract.IsInterface(declaringType))
			{
				Dictionary<int, MethodInfo> dictionary = ComputeInterfaceMapping(declaringType, me);
				if (dictionary == null)
				{
					return method;
				}
				int metadataToken = method.MetadataToken;
				return dictionary[metadataToken];
			}
			Dictionary<ModuleToken, MethodInfo> dictionary2 = new Dictionary<ModuleToken, MethodInfo>();
			Type type = me;
			while ((object)type != null)
			{
				MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (MethodInfo methodInfo in methods)
				{
					if (methodInfo.IsVirtual)
					{
						MethodInfo rootDefinition = GetRootDefinition(methodInfo);
						ModuleToken key = new ModuleToken(rootDefinition.Module, rootDefinition.MetadataToken);
						if (!dictionary2.ContainsKey(key))
						{
							dictionary2[key] = methodInfo;
						}
					}
				}
				type = type.BaseType;
			}
			MethodInfo rootDefinition2 = GetRootDefinition(method);
			ModuleToken key2 = new ModuleToken(rootDefinition2.Module, rootDefinition2.MetadataToken);
			return dictionary2[key2];
		}

		private static Dictionary<int, MethodInfo> ComputeInterfaceMapping(Type declaringType, Type me)
		{
			try
			{
				InterfaceMapping interfaceMap = me.GetInterfaceMap(declaringType);
				Dictionary<int, MethodInfo> dictionary = new Dictionary<int, MethodInfo>();
				for (int i = 0; i < interfaceMap.InterfaceMethods.Length; i++)
				{
					MethodInfo methodInfo = interfaceMap.InterfaceMethods[i];
					int metadataToken = methodInfo.MetadataToken;
					dictionary[metadataToken] = interfaceMap.TargetMethods[i];
				}
				return dictionary;
			}
			catch (ArgumentException)
			{
				return null;
			}
		}

		[Pure]
		public static bool IsExtern(MethodBase method)
		{
			MethodAttributes attributes = method.Attributes;
			if ((attributes & (MethodAttributes.PinvokeImpl | MethodAttributes.UnmanagedExport)) != 0)
			{
				return true;
			}
			MethodImplAttributes methodImplementationFlags = method.GetMethodImplementationFlags();
			if ((methodImplementationFlags & (MethodImplAttributes)4101) != 0)
			{
				return true;
			}
			MethodImplAttribute methodImplAttribute = Attribute.GetCustomAttribute(method, typeof(MethodImplAttribute)) as MethodImplAttribute;
			if (methodImplAttribute != null && (methodImplAttribute.Value & (MethodImplOptions.Unmanaged | MethodImplOptions.InternalCall)) != 0)
			{
				return true;
			}
			return false;
		}
	}
}
