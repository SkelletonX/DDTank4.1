using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.QualityTools.Testing.Fakes.UnitTestIsolation
{
	internal static class NativeMethods
	{
		[DllImport("kernel32.dll")]
		public static extern void GetSystemInfo(ref SYSTEM_INFO SystemInfo);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetModuleHandleW([In] [MarshalAs(UnmanagedType.LPWStr)] string fileName);

		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, [In] [MarshalAs(UnmanagedType.LPStr)] string procedureName);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern uint GetModuleFileName(IntPtr hModule, [Out] StringBuilder lpFilename, [In] [MarshalAs(UnmanagedType.U4)] int nSize);
	}
}
