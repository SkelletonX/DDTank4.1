using System;
using System.Diagnostics;

namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	[Serializable]
	[DebuggerDisplay("{Value}")]
	[DebuggerNonUserCode]
	[__Instrument]
	public sealed class StubValueHolder<T>
	{
		public T Value;

		public StubValueHolder(T value)
		{
			Value = value;
		}

		public FakesDelegates.Func<T> GetGetter()
		{
			return GetValue;
		}

		public FakesDelegates.Action<T> GetSetter()
		{
			return SetValue;
		}

		[DebuggerHidden]
		internal T GetValue()
		{
			return Value;
		}

		[DebuggerHidden]
		internal void SetValue(T value)
		{
			Value = value;
		}
	}
}
