using Microsoft.QualityTools.Testing.Fakes.Shims;
using Microsoft.QualityTools.Testing.Fakes.Utils;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	[DebuggerNonUserCode]
	public static class StubRuntime
	{
		public static Delegate BindProperty(Type delegateType, object target, Type type, string propertyName, bool isGetter, Type returnType)
		{
			return BindAccessor(delegateType, target, type, propertyName, isGetter, returnType, Type.EmptyTypes);
		}

		public static Delegate BindIndexer(Type delegateType, object target, Type type, string propertyName, bool isGetter, Type returnType, Type argumentType)
		{
			return BindAccessor(delegateType, target, type, propertyName, isGetter, returnType, new Type[1]
			{
				argumentType
			});
		}

		public static Delegate BindIndexer(Type delegateType, object target, Type type, string propertyName, bool isGetter, Type returnType, params Type[] argumentTypes)
		{
			return BindAccessor(delegateType, target, type, propertyName, isGetter, returnType, argumentTypes);
		}

		private static Delegate BindAccessor(Type delegateType, object target, Type type, string propertyName, bool getter, Type returnType, Type[] argumentTypes)
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				if ((object)delegateType == null)
				{
					throw new ArgumentNullException("delegateType");
				}
				if (target == null)
				{
					throw new ArgumentNullException("target");
				}
				if ((object)type == null)
				{
					throw new ArgumentNullException("type");
				}
				if (propertyName == null)
				{
					throw new ArgumentNullException("propertyName");
				}
				if ((object)returnType == null)
				{
					throw new ArgumentNullException("returnType");
				}
				if (argumentTypes == null)
				{
					throw new ArgumentNullException("argumentTypes");
				}
				MethodInfo methodInfo = ResolveAccessor(type, propertyName, getter, returnType, argumentTypes);
				if ((object)methodInfo == null)
				{
					return null;
				}
				return Delegate.CreateDelegate(delegateType, target, methodInfo);
			}
		}

		private static MethodInfo ResolveAccessor(Type type, string propertyName, bool getter, Type returnType, Type[] argumentTypes)
		{
			BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			PropertyInfo property;
			try
			{
				property = type.GetProperty(propertyName, bindingAttr, null, returnType, argumentTypes, null);
				if ((object)property == null)
				{
					return null;
				}
			}
			catch (AmbiguousMatchException)
			{
				return null;
			}
			if (getter)
			{
				if (!property.CanRead)
				{
					return null;
				}
				return property.GetGetMethod();
			}
			if (!property.CanWrite)
			{
				return null;
			}
			return property.GetSetMethod();
		}

		public static MethodInfo ResolveStubbedMethod(Type stubbedType, Delegate stubCall)
		{
			if ((object)stubbedType == null)
			{
				throw new ArgumentNullException("stubbedType");
			}
			if ((object)stubCall == null)
			{
				throw new ArgumentNullException("stubCall");
			}
			if (stubCall.Target == null)
			{
				throw new ArgumentException(FakesFrameworkResources.DelegateTargetIsANullReference, "stubCall");
			}
			if (stubbedType.IsInterface)
			{
				return ResolveStubbedInterfaceMethod(stubbedType, stubCall);
			}
			return ResolveStubbedBaseMethod(stubbedType, stubCall);
		}

		private static MethodInfo ResolveStubbedBaseMethod(Type stubbedType, Delegate stubCall)
		{
			object target = stubCall.Target;
			target.GetType();
			MethodInfo method = stubCall.Method;
			MethodInfo methodInfo = (MethodInfo)ReflectionHelper.GetMethodDefinition(method);
			MethodInfo methodInfo2 = methodInfo.GetBaseDefinition();
			if (method.IsGenericMethod)
			{
				methodInfo2 = ReflectionHelper.Instantiate(methodInfo2, method.GetGenericArguments());
			}
			return methodInfo2;
		}

		private static MethodInfo ResolveStubbedInterfaceMethod(Type stubbedType, Delegate stubCall)
		{
			object target = stubCall.Target;
			Type type = target.GetType();
			MethodInfo method = stubCall.Method;
			MethodInfo methodInfo = (MethodInfo)ReflectionHelper.GetMethodDefinition(method);
			InterfaceMapping interfaceMap = type.GetInterfaceMap(stubbedType);
			MethodInfo[] targetMethods = interfaceMap.TargetMethods;
			MethodInfo[] interfaceMethods = interfaceMap.InterfaceMethods;
			for (int i = 0; i < targetMethods.Length; i++)
			{
				MethodInfo methodInfo2 = targetMethods[i];
				if ((object)methodInfo2 == methodInfo)
				{
					MethodInfo methodInfo3 = interfaceMethods[i];
					if (method.IsGenericMethod)
					{
						methodInfo3 = ReflectionHelper.Instantiate(methodInfo3, method.GetGenericArguments());
					}
					return methodInfo3;
				}
			}
			return null;
		}
	}
}
