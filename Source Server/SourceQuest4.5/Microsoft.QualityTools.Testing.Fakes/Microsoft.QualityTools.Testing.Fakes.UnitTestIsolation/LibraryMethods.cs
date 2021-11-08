using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.QualityTools.Testing.Fakes.UnitTestIsolation
{
	internal static class LibraryMethods
	{
		public static IntPtr GetModuleHandle(string fileName)
		{
			IntPtr moduleHandleW = NativeMethods.GetModuleHandleW(fileName);
			if (moduleHandleW == IntPtr.Zero)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			return moduleHandleW;
		}

		public static IntPtr GetProcAddress(IntPtr hModule, string functionName)
		{
			IntPtr procAddress = NativeMethods.GetProcAddress(hModule, functionName);
			if (procAddress == IntPtr.Zero)
			{
				string moduleFileName = GetModuleFileName(hModule);
				throw new UnitTestIsolationException(string.Format(CultureInfo.CurrentCulture, FakesFrameworkResources.FailedToGetFunctionAddress, new object[2]
				{
					functionName,
					moduleFileName
				}));
			}
			return procAddress;
		}

		public static T GetFunction<T>(IntPtr hModule, string functionName) where T : class
		{
			IntPtr procAddress = GetProcAddress(hModule, functionName);
			object delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer(procAddress, typeof(T));
			return (T)delegateForFunctionPointer;
		}

		public static string GetModuleFileName(IntPtr hModule)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			if (NativeMethods.GetModuleFileName(hModule, stringBuilder, stringBuilder.Capacity) == 0)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			return stringBuilder.ToString();
		}
	}
}
