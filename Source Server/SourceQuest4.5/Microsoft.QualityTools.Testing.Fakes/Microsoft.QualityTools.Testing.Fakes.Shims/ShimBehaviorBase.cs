using Microsoft.QualityTools.Testing.Fakes.Engine;
using Microsoft.QualityTools.Testing.Fakes.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	public abstract class ShimBehaviorBase : IShimBehavior
	{
		private static class Metadata
		{
			public const string DelegateName = "Invoke";

			public static readonly ConstructorInfo CompilerGeneratedAttributeCtor = typeof(CompilerGeneratedAttribute).GetConstructor(Type.EmptyTypes);

			public static readonly ConstructorInfo __InstrumentAttributeCtor = typeof(__InstrumentAttribute).GetConstructor(Type.EmptyTypes);
		}

		private readonly string typeNameSuffix;

		private readonly Dictionary<Type, Delegate> delegates;

		private readonly AssemblyBuilder assembly;

		private readonly ModuleBuilder module;

		private static readonly RuntimeDelegateTypePool delegatePool = new RuntimeDelegateTypePool();

		protected ShimBehaviorBase(string assemblyName, string typeNameSuffix)
		{
			delegates = new Dictionary<Type, Delegate>();
			AssemblyName assemblyName2 = new AssemblyName(assemblyName);
			assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName2, AssemblyBuilderAccess.Run);
			module = assembly.DefineDynamicModule(assemblyName2.Name + ".dll", emitSymbolInfo: false);
			this.typeNameSuffix = typeNameSuffix;
		}

		protected TDelegate CreateDelegate<TDelegate>()
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				if (!TryCreateDelegate(typeof(TDelegate), out Delegate @delegate))
				{
					throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "could not generate a delegate for {0}", new object[1]
					{
						typeof(TDelegate)
					}));
				}
				return (TDelegate)(object)@delegate;
			}
		}

		protected bool TryCreateDelegate(Type delegateType, out Delegate @delegate)
		{
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				lock (this)
				{
					@delegate = null;
					return (object)delegateType != null && ReflectionContract.IsDelegate(delegateType) && TryCreateDelegateProtected(delegateType, out @delegate);
				}
			}
		}

		private bool TryCreateDelegateProtected(Type delegateType, out Delegate @delegate)
		{
			if (!delegates.TryGetValue(delegateType, out @delegate))
			{
				try
				{
					@delegate = CreateDelegateUncached(delegateType);
				}
				catch (Exception)
				{
					@delegate = null;
				}
				delegates[delegateType] = @delegate;
			}
			return (object)@delegate != null;
		}

		private Delegate CreateDelegateUncached(Type delegateType)
		{
			MethodInfo method = delegateType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
			string text = Guid.NewGuid().ToString();
			string text2 = "$" + delegateType.Name + typeNameSuffix + text;
			if (text2.Length >= 1024)
			{
				text2 = string.Format(CultureInfo.InvariantCulture, text2.Substring(0, 100), new object[1]
				{
					text
				});
			}
			TypeBuilder typeBuilder = module.DefineType(text2, TypeAttributes.Abstract | TypeAttributes.Sealed);
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(Metadata.CompilerGeneratedAttributeCtor, new object[0]));
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(Metadata.__InstrumentAttributeCtor, new object[0]));
			Type returnType = method.ReturnType;
			ParameterInfo[] parameters = method.GetParameters();
			Type[] parameterTypes = Array.ConvertAll(parameters, (ParameterInfo p) => p.ParameterType);
			MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, method.ReturnType, parameterTypes);
			if ((object)returnType != typeof(void))
			{
				methodBuilder.DefineParameter(0, method.ReturnParameter.Attributes, method.ReturnParameter.Name);
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				methodBuilder.DefineParameter(i + 1, parameterInfo.Attributes, parameterInfo.Name);
			}
			EmitDelegateBody(returnType, parameters, methodBuilder);
			Type type = typeBuilder.CreateType();
			MethodInfo method2 = type.GetMethod("Invoke", BindingFlags.Static | BindingFlags.Public);
			return Delegate.CreateDelegate(delegateType, method2);
		}

		protected abstract void EmitDelegateBody(Type delegateReturnType, ParameterInfo[] delegateParameters, MethodBuilder methodBuilder);

		public bool TryGetShimMethod(MethodBase method, out Delegate shim)
		{
			if ((object)method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (ReflectionHelper.TryGetDeclaringType(method, out Type declaringType) && delegatePool.TryGetDelegateType(declaringType, method, useThisParameter: true, out Type delegateType, out MethodInfo _) == DelegateInstantiationType.Success && TryCreateDelegate(delegateType, out Delegate @delegate))
			{
				shim = @delegate;
				return (object)shim != null;
			}
			shim = null;
			return false;
		}
	}
}
