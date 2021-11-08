using Microsoft.QualityTools.Testing.Fakes.Shims;
using System;
using System.Diagnostics;

namespace Microsoft.QualityTools.Testing.Fakes
{
	[__DoNotInstrument]
	[DebuggerNonUserCode]
	public static class ShimsContext
	{
		public static IDisposable Create()
		{
			return ShimRuntime.CreateContext();
		}

		public static void Reset()
		{
			ShimRuntime.EnsureContext();
			ShimRuntime.Clear();
		}

		public static void ExecuteWithoutShims(FakesDelegates.Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				action();
			}
		}

		public static T ExecuteWithoutShims<T>(FakesDelegates.Func<T> func)
		{
			if (func == null)
			{
				throw new ArgumentNullException("func");
			}
			using (ShimRuntime.AcquireProtectingThreadContext())
			{
				return func();
			}
		}
	}
}
