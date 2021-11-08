#define TRACE
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Microsoft.QualityTools.Testing.Fakes.UnitTestIsolation
{
	internal sealed class IntelliTraceInstrumentationProvider : IUnitTestIsolationInstrumentationProvider
	{
		private sealed class ProtectingContext : IDisposable
		{
			[ThreadStatic]
			private static int instanceCount;

			public static bool IsThreadProtected => instanceCount > 0;

			public ProtectingContext()
			{
				instanceCount++;
			}

			public void Dispose()
			{
				instanceCount--;
			}
		}

		private static IntPtr detourProviderAddress = typeof(IntelliTraceInstrumentationProvider).GetMethod("DetourProvider", BindingFlags.Static | BindingFlags.NonPublic).MethodHandle.GetFunctionPointer();

		private IntPtr profilerModule;

		private NativeSetDetourProvider setDetourProvider;

		private NativeCanDetour canDetour;

		private bool enabled;

		private RegistryKey classesRootKey = Registry.ClassesRoot;

		public bool IsThreadProtected => ProtectingContext.IsThreadProtected;

		internal RegistryKey ClassesRootKey
		{
			get
			{
				return classesRootKey;
			}
			set
			{
				classesRootKey = value;
			}
		}

		public void Initialize()
		{
			string text = ResolveProfilerPath();
			profilerModule = LoadProfilerModule(text);
			setDetourProvider = LibraryMethods.GetFunction<NativeSetDetourProvider>(profilerModule, "SetDetourProvider");
			canDetour = LibraryMethods.GetFunction<NativeCanDetour>(profilerModule, "CanDetour");
			if (setDetourProvider(detourProviderAddress) != 0)
			{
				throw new UnitTestIsolationException(string.Format(CultureInfo.CurrentCulture, FakesFrameworkResources.FailedToSetDetourProvider, new object[1]
				{
					text
				}));
			}
			enabled = true;
		}

		public bool IsDetoursEnabled()
		{
			return enabled;
		}

		public bool CanDetour(MethodBase method)
		{
			if ((object)method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (!IsDetoursEnabled())
			{
				throw new InvalidOperationException();
			}
			return canDetour(method.Module.ModuleVersionId, method.MetadataToken);
		}

		public IDisposable AcquireProtectingThreadContext()
		{
			return new ProtectingContext();
		}

		private string ResolveProfilerPath()
		{
			string environmentVariable = Environment.GetEnvironmentVariable("COR_PROFILER_PATH", EnvironmentVariableTarget.Process);
			if (!string.IsNullOrEmpty(environmentVariable))
			{
				FakesTraceSources.Runtime.TraceInformation(FakesFrameworkResources.ProfilerPathTraceMessage, "COR_PROFILER_PATH", environmentVariable);
				return environmentVariable;
			}
			string environmentVariable2 = Environment.GetEnvironmentVariable("COR_PROFILER", EnvironmentVariableTarget.Process);
			if (!string.IsNullOrEmpty(environmentVariable2))
			{
				FakesTraceSources.Runtime.TraceInformation(FakesFrameworkResources.ProfilerClassIdTraceMessage, "COR_PROFILER", environmentVariable2);
				string text = "CLSID\\" + environmentVariable2 + "\\InprocServer32";
				SYSTEM_INFO SystemInfo = default(SYSTEM_INFO);
				NativeMethods.GetSystemInfo(ref SystemInfo);
				if (IntPtr.Size == 4 && SystemInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE.PROCESSOR_ARCHITECTURE_AMD64)
				{
					text = "Wow6432Node\\" + text;
				}
				FakesTraceSources.Runtime.TraceInformation(FakesFrameworkResources.ProfilerRegistryPathTraceMessage, text);
				using (RegistryKey registryKey = classesRootKey.OpenSubKey(text))
				{
					if (registryKey == null)
					{
						throw new UnitTestIsolationException(string.Format(CultureInfo.CurrentCulture, FakesFrameworkResources.FailedToOpenProfilerRegistryKey, new object[1]
						{
							text
						}));
					}
					return (string)registryKey.GetValue(null);
				}
			}
			throw new UnitTestIsolationException(FakesFrameworkResources.FailedToResolveProfilerPath);
		}

		private static IntPtr LoadProfilerModule(string profilerPath)
		{
			try
			{
				return LibraryMethods.GetModuleHandle(profilerPath);
			}
			catch (Win32Exception ex)
			{
				throw new UnitTestIsolationException(string.Format(CultureInfo.CurrentCulture, FakesFrameworkResources.FailedToGetProfilerModuleHandle, new object[2]
				{
					profilerPath,
					ex.Message
				}), ex);
			}
		}

		internal static void DetourProvider(object receiver, RuntimeMethodHandle methodHandle, RuntimeTypeHandle declaringTypeHandle, RuntimeTypeHandle[] genericMethodTypeArgumentHandles, out object detourDelegate, out IntPtr detourPointer)
		{
			detourDelegate = null;
			detourPointer = IntPtr.Zero;
			if (!ProtectingContext.IsThreadProtected)
			{
				using (new ProtectingContext())
				{
					MethodBase methodBase = MethodBase.GetMethodFromHandle(methodHandle, declaringTypeHandle);
					if (methodBase.IsGenericMethodDefinition)
					{
						Type[] array = new Type[genericMethodTypeArgumentHandles.Length];
						for (int i = 0; i < genericMethodTypeArgumentHandles.Length; i++)
						{
							array[i] = Type.GetTypeFromHandle(genericMethodTypeArgumentHandles[i]);
						}
						methodBase = ((MethodInfo)methodBase).MakeGenericMethod(array);
					}
					detourDelegate = UnitTestIsolationRuntime.GetDetour(receiver, methodBase);
					if (detourDelegate != null)
					{
						detourPointer = detourDelegate.GetType().GetMethod("Invoke").MethodHandle.GetFunctionPointer();
					}
				}
			}
		}
	}
}
