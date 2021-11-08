using System;
using System.Runtime.InteropServices;

namespace Microsoft.QualityTools.Testing.Fakes.UnitTestIsolation
{
	internal delegate bool NativeCanDetour([In] [MarshalAs(UnmanagedType.LPStruct)] Guid mvid, int methodDef);
}
