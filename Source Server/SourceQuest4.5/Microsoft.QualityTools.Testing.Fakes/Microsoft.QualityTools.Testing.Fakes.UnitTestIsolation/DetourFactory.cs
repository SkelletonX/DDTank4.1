using System;
using System.Reflection;

namespace Microsoft.QualityTools.Testing.Fakes.UnitTestIsolation
{
	public delegate Delegate DetourFactory(object optionalReceiver, MethodBase method);
}
