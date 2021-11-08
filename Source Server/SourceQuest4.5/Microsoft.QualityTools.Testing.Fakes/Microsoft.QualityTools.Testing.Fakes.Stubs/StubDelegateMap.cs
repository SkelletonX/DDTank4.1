using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	[__Instrument]
	[DebuggerNonUserCode]
	public struct StubDelegateMap
	{
		private enum State
		{
			Empty,
			One,
			Dictionary
		}

		private readonly object items;

		private readonly State state;

		private StubDelegateMap(State state, object items)
		{
			this.state = state;
			this.items = items;
		}

		public bool TryGetValue<TDelegate>(out TDelegate @delegate)
		{
			switch (state)
			{
			case State.One:
				if ((object)typeof(TDelegate) == items.GetType())
				{
					@delegate = (TDelegate)items;
					return true;
				}
				break;
			case State.Dictionary:
			{
				Dictionary<object, Delegate> dictionary = (Dictionary<object, Delegate>)items;
				if (dictionary.TryGetValue(typeof(TDelegate), out Delegate value))
				{
					@delegate = (TDelegate)(object)value;
					return true;
				}
				break;
			}
			}
			@delegate = default(TDelegate);
			return false;
		}

		private static StubDelegateMap One(Delegate @delegate)
		{
			if ((object)@delegate == null)
			{
				throw new ArgumentNullException("delegate");
			}
			return new StubDelegateMap(State.One, @delegate);
		}

		public static StubDelegateMap Concat(StubDelegateMap dictionary, Delegate @delegate)
		{
			if ((object)@delegate == null)
			{
				throw new ArgumentNullException("delegate");
			}
			switch (dictionary.state)
			{
			case State.Empty:
				return One(@delegate);
			case State.One:
			{
				Dictionary<object, Delegate> dictionary4 = new Dictionary<object, Delegate>(2);
				dictionary4.Add(dictionary.items.GetType(), (Delegate)dictionary.items);
				dictionary4.Add(@delegate.GetType(), @delegate);
				return new StubDelegateMap(State.Dictionary, dictionary4);
			}
			default:
			{
				Dictionary<object, Delegate> dictionary2 = (Dictionary<object, Delegate>)dictionary.items;
				Dictionary<object, Delegate> dictionary3 = new Dictionary<object, Delegate>(dictionary2);
				dictionary3.Add(@delegate.GetType(), @delegate);
				return new StubDelegateMap(State.Dictionary, dictionary3);
			}
			}
		}
	}
}
