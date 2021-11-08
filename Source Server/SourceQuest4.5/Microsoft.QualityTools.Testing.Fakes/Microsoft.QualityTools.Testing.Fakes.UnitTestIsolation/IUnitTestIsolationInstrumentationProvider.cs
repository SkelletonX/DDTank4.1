using System;
using System.Reflection;

namespace Microsoft.QualityTools.Testing.Fakes.UnitTestIsolation
{
	public interface IUnitTestIsolationInstrumentationProvider
	{
		bool IsThreadProtected
		{
			get;
		}

		bool IsDetoursEnabled();

		bool CanDetour(MethodBase method);

		IDisposable AcquireProtectingThreadContext();
	}
}
