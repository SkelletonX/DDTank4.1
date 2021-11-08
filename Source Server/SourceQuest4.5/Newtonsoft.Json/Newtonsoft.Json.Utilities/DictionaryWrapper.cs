using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	internal class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IWrappedDictionary, IDictionary, ICollection, IEnumerable
	{
		private struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : IDictionaryEnumerator, IEnumerator
		{
			private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;

			public DictionaryEntry Entry => (DictionaryEntry)Current;

			public object Key => Entry.Key;

			public object Value => Entry.Value;

			public object Current => new DictionaryEntry(_e.Current.Key, _e.Current.Value);

			public DictionaryEnumerator(IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				ValidationUtils.ArgumentNotNull(e, "e");
				_e = e;
			}

			public bool MoveNext()
			{
				return _e.MoveNext();
			}

			public void Reset()
			{
				_e.Reset();
			}
		}

		private readonly IDictionary _dictionary;

		private readonly IDictionary<TKey, TValue> _genericDictionary;

		private object _syncRoot;

		public ICollection<TKey> Keys
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary.Keys.Cast<TKey>().ToList();
				}
				return _genericDictionary.Keys;
			}
		}

		public ICollection<TValue> Values
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary.Values.Cast<TValue>().ToList();
				}
				return _genericDictionary.Values;
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				if (_dictionary != null)
				{
					return (TValue)_dictionary[key];
				}
				return _genericDictionary[key];
			}
			set
			{
				if (_dictionary != null)
				{
					_dictionary[key] = value;
				}
				else
				{
					_genericDictionary[key] = value;
				}
			}
		}

		public int Count
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary.Count;
				}
				return _genericDictionary.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary.IsReadOnly;
				}
				return _genericDictionary.IsReadOnly;
			}
		}

		object IDictionary.this[object key]
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary[key];
				}
				return _genericDictionary[(TKey)key];
			}
			set
			{
				if (_dictionary != null)
				{
					_dictionary[key] = value;
				}
				else
				{
					_genericDictionary[(TKey)key] = (TValue)value;
				}
			}
		}

		bool IDictionary.IsFixedSize
		{
			get
			{
				if (_genericDictionary != null)
				{
					return false;
				}
				return _dictionary.IsFixedSize;
			}
		}

		ICollection IDictionary.Keys
		{
			get
			{
				if (_genericDictionary != null)
				{
					return _genericDictionary.Keys.ToList();
				}
				return _dictionary.Keys;
			}
		}

		ICollection IDictionary.Values
		{
			get
			{
				if (_genericDictionary != null)
				{
					return _genericDictionary.Values.ToList();
				}
				return _dictionary.Values;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary.IsSynchronized;
				}
				return false;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				if (_syncRoot == null)
				{
					Interlocked.CompareExchange(ref _syncRoot, new object(), null);
				}
				return _syncRoot;
			}
		}

		public object UnderlyingDictionary
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary;
				}
				return _genericDictionary;
			}
		}

		public DictionaryWrapper(IDictionary dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			_dictionary = dictionary;
		}

		public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			_genericDictionary = dictionary;
		}

		public void Add(TKey key, TValue value)
		{
			if (_dictionary != null)
			{
				_dictionary.Add(key, value);
				return;
			}
			if (_genericDictionary != null)
			{
				_genericDictionary.Add(key, value);
				return;
			}
			throw new NotSupportedException();
		}

		public bool ContainsKey(TKey key)
		{
			if (_dictionary != null)
			{
				return _dictionary.Contains(key);
			}
			return _genericDictionary.ContainsKey(key);
		}

		public bool Remove(TKey key)
		{
			if (_dictionary != null)
			{
				if (_dictionary.Contains(key))
				{
					_dictionary.Remove(key);
					return true;
				}
				return false;
			}
			return _genericDictionary.Remove(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			if (_dictionary != null)
			{
				if (!_dictionary.Contains(key))
				{
					value = default(TValue);
					return false;
				}
				value = (TValue)_dictionary[key];
				return true;
			}
			return _genericDictionary.TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			if (_dictionary != null)
			{
				((IList)_dictionary).Add(item);
			}
			else if (_genericDictionary != null)
			{
				_genericDictionary.Add(item);
			}
		}

		public void Clear()
		{
			if (_dictionary != null)
			{
				_dictionary.Clear();
			}
			else
			{
				_genericDictionary.Clear();
			}
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			if (_dictionary != null)
			{
				return ((IList)_dictionary).Contains(item);
			}
			return _genericDictionary.Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (_dictionary != null)
			{
				foreach (DictionaryEntry item in _dictionary)
				{
					array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey)item.Key, (TValue)item.Value);
				}
			}
			else
			{
				_genericDictionary.CopyTo(array, arrayIndex);
			}
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (_dictionary != null)
			{
				if (_dictionary.Contains(item.Key))
				{
					object objA = _dictionary[item.Key];
					if (object.Equals(objA, item.Value))
					{
						_dictionary.Remove(item.Key);
						return true;
					}
					return false;
				}
				return true;
			}
			return _genericDictionary.Remove(item);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (_dictionary != null)
			{
				return (from DictionaryEntry de in _dictionary
					select new KeyValuePair<TKey, TValue>((TKey)de.Key, (TValue)de.Value)).GetEnumerator();
			}
			return _genericDictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		void IDictionary.Add(object key, object value)
		{
			if (_dictionary != null)
			{
				_dictionary.Add(key, value);
			}
			else
			{
				_genericDictionary.Add((TKey)key, (TValue)value);
			}
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			if (_dictionary != null)
			{
				return _dictionary.GetEnumerator();
			}
			return new DictionaryEnumerator<TKey, TValue>(_genericDictionary.GetEnumerator());
		}

		bool IDictionary.Contains(object key)
		{
			if (_genericDictionary != null)
			{
				return _genericDictionary.ContainsKey((TKey)key);
			}
			return _dictionary.Contains(key);
		}

		public void Remove(object key)
		{
			if (_dictionary != null)
			{
				_dictionary.Remove(key);
			}
			else
			{
				_genericDictionary.Remove((TKey)key);
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			if (_dictionary != null)
			{
				_dictionary.CopyTo(array, index);
			}
			else
			{
				_genericDictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
			}
		}
	}
}
