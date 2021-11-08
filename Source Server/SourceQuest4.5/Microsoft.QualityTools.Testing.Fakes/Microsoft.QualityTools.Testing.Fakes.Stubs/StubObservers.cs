using System.Diagnostics;

namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	[DebuggerNonUserCode]
	public static class StubObservers
	{
		private static IStubObserver _current;

		public static IStubObserver Current
		{
			get
			{
				return _current;
			}
			set
			{
				_current = value;
			}
		}

		public static IStubObserver GetValueOrCurrent(IStubObserver observer)
		{
			if (observer == null)
			{
				return _current;
			}
			return observer;
		}
	}
}
