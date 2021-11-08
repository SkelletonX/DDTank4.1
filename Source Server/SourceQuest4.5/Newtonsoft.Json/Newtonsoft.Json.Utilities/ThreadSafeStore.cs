using System;
using System.Collections.Generic;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	internal class ThreadSafeStore<TKey, TValue>
	{
		private readonly object _lock = new object();

		private Dictionary<TKey, TValue> _store;

		private readonly Func<TKey, TValue> _creator;

		public ThreadSafeStore(Func<TKey, TValue> creator)
		{
			if (creator == null)
			{
				throw new ArgumentNullException("creator");
			}
			_creator = creator;
			_store = new Dictionary<TKey, TValue>();
		}

		public TValue Get(TKey key)
		{
			if (!_store.TryGetValue(key, out TValue value))
			{
				return AddValue(key);
			}
			return value;
		}

		private TValue AddValue(TKey key)
		{
			TValue val = _creator(key);
			lock (_lock)
			{
				if (_store == null)
				{
					_store = new Dictionary<TKey, TValue>();
					_store[key] = val;
				}
				else
				{
					if (_store.TryGetValue(key, out TValue value))
					{
						return value;
					}
					Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(_store);
					dictionary[key] = val;
					Thread.MemoryBarrier();
					_store = dictionary;
				}
				return val;
			}
		}
	}
}
