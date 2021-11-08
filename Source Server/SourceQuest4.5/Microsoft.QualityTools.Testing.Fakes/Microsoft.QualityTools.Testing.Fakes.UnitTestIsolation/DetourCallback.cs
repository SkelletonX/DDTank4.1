using System;
using System.Reflection;

namespace Microsoft.QualityTools.Testing.Fakes.UnitTestIsolation
{
	public delegate void DetourCallback(object optionalReceiver, MethodBase method, Delegate detour);
}
