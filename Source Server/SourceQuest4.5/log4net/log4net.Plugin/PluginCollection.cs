using log4net.Util;
using System;
using System.Collections;

namespace log4net.Plugin
{
	/// <summary>
	/// A strongly-typed collection of <see cref="T:log4net.Plugin.IPlugin" /> objects.
	/// </summary>
	/// <author>Nicko Cadell</author>
	public class PluginCollection : IList, ICollection, IEnumerable, ICloneable
	{
		/// <summary>
		/// Supports type-safe iteration over a <see cref="T:log4net.Plugin.PluginCollection" />.
		/// </summary>
		/// <exclude />
		public interface IPluginCollectionEnumerator
		{
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			IPlugin Current
			{
				get;
			}

			/// <summary>
			/// Advances the enumerator to the next element in the collection.
			/// </summary>
			/// <returns>
			/// <c>true</c> if the enumerator was successfully advanced to the next element; 
			/// <c>false</c> if the enumerator has passed the end of the collection.
			/// </returns>
			/// <exception cref="T:System.InvalidOperationException">
			/// The collection was modified after the enumerator was created.
			/// </exception>
			bool MoveNext();

			/// <summary>
			/// Sets the enumerator to its initial position, before the first element in the collection.
			/// </summary>
			void Reset();
		}

		/// <summary>
		/// Type visible only to our subclasses
		/// Used to access protected constructor
		/// </summary>
		/// <exclude />
		protected internal enum Tag
		{
			/// <summary>
			/// A value
			/// </summary>
			Default
		}

		/// <summary>
		/// Supports simple iteration over a <see cref="T:log4net.Plugin.PluginCollection" />.
		/// </summary>
		/// <exclude />
		private sealed class Enumerator : IEnumerator, IPluginCollectionEnumerator
		{
			private readonly PluginCollection m_collection;

			private int m_index;

			private int m_version;

			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			/// <value>
			/// The current element in the collection.
			/// </value>
			public IPlugin Current => m_collection[m_index];

			object IEnumerator.Current => Current;

			/// <summary>
			/// Initializes a new instance of the <c>Enumerator</c> class.
			/// </summary>
			/// <param name="tc"></param>
			internal Enumerator(PluginCollection tc)
			{
				m_collection = tc;
				m_index = -1;
				m_version = tc.m_version;
			}

			/// <summary>
			/// Advances the enumerator to the next element in the collection.
			/// </summary>
			/// <returns>
			/// <c>true</c> if the enumerator was successfully advanced to the next element; 
			/// <c>false</c> if the enumerator has passed the end of the collection.
			/// </returns>
			/// <exception cref="T:System.InvalidOperationException">
			/// The collection was modified after the enumerator was created.
			/// </exception>
			public bool MoveNext()
			{
				if (m_version != m_collection.m_version)
				{
					throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
				}
				m_index++;
				return m_index < m_collection.Count;
			}

			/// <summary>
			/// Sets the enumerator to its initial position, before the first element in the collection.
			/// </summary>
			public void Reset()
			{
				m_index = -1;
			}
		}

		/// <exclude />
		private sealed class ReadOnlyPluginCollection : PluginCollection
		{
			private readonly PluginCollection m_collection;

			public override int Count => m_collection.Count;

			public override bool IsSynchronized => m_collection.IsSynchronized;

			public override object SyncRoot => m_collection.SyncRoot;

			public override IPlugin this[int i]
			{
				get
				{
					return m_collection[i];
				}
				set
				{
					throw new NotSupportedException("This is a Read Only Collection and can not be modified");
				}
			}

			public override bool IsFixedSize => true;

			public override bool IsReadOnly => true;

			public override int Capacity
			{
				get
				{
					return m_collection.Capacity;
				}
				set
				{
					throw new NotSupportedException("This is a Read Only Collection and can not be modified");
				}
			}

			internal ReadOnlyPluginCollection(PluginCollection list)
				: base(Tag.Default)
			{
				m_collection = list;
			}

			public override void CopyTo(IPlugin[] array)
			{
				m_collection.CopyTo(array);
			}

			public override void CopyTo(IPlugin[] array, int start)
			{
				m_collection.CopyTo(array, start);
			}

			public override int Add(IPlugin x)
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}

			public override void Clear()
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}

			public override bool Contains(IPlugin x)
			{
				return m_collection.Contains(x);
			}

			public override int IndexOf(IPlugin x)
			{
				return m_collection.IndexOf(x);
			}

			public override void Insert(int pos, IPlugin x)
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}

			public override void Remove(IPlugin x)
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}

			public override void RemoveAt(int pos)
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}

			public override IPluginCollectionEnumerator GetEnumerator()
			{
				return m_collection.GetEnumerator();
			}

			public override int AddRange(PluginCollection x)
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}

			public override int AddRange(IPlugin[] x)
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}
		}

		private const int DEFAULT_CAPACITY = 16;

		private IPlugin[] m_array;

		private int m_count = 0;

		private int m_version = 0;

		/// <summary>
		/// Gets the number of elements actually contained in the <c>PluginCollection</c>.
		/// </summary>
		public virtual int Count => m_count;

		/// <summary>
		/// Gets a value indicating whether access to the collection is synchronized (thread-safe).
		/// </summary>
		/// <returns>true if access to the ICollection is synchronized (thread-safe); otherwise, false.</returns>
		public virtual bool IsSynchronized => m_array.IsSynchronized;

		/// <summary>
		/// Gets an object that can be used to synchronize access to the collection.
		/// </summary>
		/// <value>
		/// An object that can be used to synchronize access to the collection.
		/// </value>
		public virtual object SyncRoot => m_array.SyncRoot;

		/// <summary>
		/// Gets or sets the <see cref="T:log4net.Plugin.IPlugin" /> at the specified index.
		/// </summary>
		/// <value>
		/// The <see cref="T:log4net.Plugin.IPlugin" /> at the specified index.
		/// </value>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <para><paramref name="index" /> is less than zero.</para>
		/// <para>-or-</para>
		/// <para><paramref name="index" /> is equal to or greater than <see cref="P:log4net.Plugin.PluginCollection.Count" />.</para>
		/// </exception>
		public virtual IPlugin this[int index]
		{
			get
			{
				ValidateIndex(index);
				return m_array[index];
			}
			set
			{
				ValidateIndex(index);
				m_version++;
				m_array[index] = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the collection has a fixed size.
		/// </summary>
		/// <value><c>true</c> if the collection has a fixed size; otherwise, <c>false</c>. The default is <c>false</c>.</value>
		public virtual bool IsFixedSize => false;

		/// <summary>
		/// Gets a value indicating whether the IList is read-only.
		/// </summary>
		/// <value><c>true</c> if the collection is read-only; otherwise, <c>false</c>. The default is <c>false</c>.</value>
		public virtual bool IsReadOnly => false;

		/// <summary>
		/// Gets or sets the number of elements the <c>PluginCollection</c> can contain.
		/// </summary>
		/// <value>
		/// The number of elements the <c>PluginCollection</c> can contain.
		/// </value>
		public virtual int Capacity
		{
			get
			{
				return m_array.Length;
			}
			set
			{
				if (value < m_count)
				{
					value = m_count;
				}
				if (value != m_array.Length)
				{
					if (value > 0)
					{
						IPlugin[] array = new IPlugin[value];
						Array.Copy(m_array, 0, array, 0, m_count);
						m_array = array;
					}
					else
					{
						m_array = new IPlugin[16];
					}
				}
			}
		}

		object IList.this[int i]
		{
			get
			{
				return this[i];
			}
			set
			{
				this[i] = (IPlugin)value;
			}
		}

		/// <summary>
		/// Creates a read-only wrapper for a <c>PluginCollection</c> instance.
		/// </summary>
		/// <param name="list">list to create a readonly wrapper arround</param>
		/// <returns>
		/// A <c>PluginCollection</c> wrapper that is read-only.
		/// </returns>
		public static PluginCollection ReadOnly(PluginCollection list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ReadOnlyPluginCollection(list);
		}

		/// <summary>
		/// Initializes a new instance of the <c>PluginCollection</c> class
		/// that is empty and has the default initial capacity.
		/// </summary>
		public PluginCollection()
		{
			m_array = new IPlugin[16];
		}

		/// <summary>
		/// Initializes a new instance of the <c>PluginCollection</c> class
		/// that has the specified initial capacity.
		/// </summary>
		/// <param name="capacity">
		/// The number of elements that the new <c>PluginCollection</c> is initially capable of storing.
		/// </param>
		public PluginCollection(int capacity)
		{
			m_array = new IPlugin[capacity];
		}

		/// <summary>
		/// Initializes a new instance of the <c>PluginCollection</c> class
		/// that contains elements copied from the specified <c>PluginCollection</c>.
		/// </summary>
		/// <param name="c">The <c>PluginCollection</c> whose elements are copied to the new collection.</param>
		public PluginCollection(PluginCollection c)
		{
			m_array = new IPlugin[c.Count];
			AddRange(c);
		}

		/// <summary>
		/// Initializes a new instance of the <c>PluginCollection</c> class
		/// that contains elements copied from the specified <see cref="T:log4net.Plugin.IPlugin" /> array.
		/// </summary>
		/// <param name="a">The <see cref="T:log4net.Plugin.IPlugin" /> array whose elements are copied to the new list.</param>
		public PluginCollection(IPlugin[] a)
		{
			m_array = new IPlugin[a.Length];
			AddRange(a);
		}

		/// <summary>
		/// Initializes a new instance of the <c>PluginCollection</c> class
		/// that contains elements copied from the specified <see cref="T:log4net.Plugin.IPlugin" /> collection.
		/// </summary>
		/// <param name="col">The <see cref="T:log4net.Plugin.IPlugin" /> collection whose elements are copied to the new list.</param>
		public PluginCollection(ICollection col)
		{
			m_array = new IPlugin[col.Count];
			AddRange(col);
		}

		/// <summary>
		/// Allow subclasses to avoid our default constructors
		/// </summary>
		/// <param name="tag"></param>
		/// <exclude />
		protected internal PluginCollection(Tag tag)
		{
			m_array = null;
		}

		/// <summary>
		/// Copies the entire <c>PluginCollection</c> to a one-dimensional
		/// <see cref="T:log4net.Plugin.IPlugin" /> array.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:log4net.Plugin.IPlugin" /> array to copy to.</param>
		public virtual void CopyTo(IPlugin[] array)
		{
			CopyTo(array, 0);
		}

		/// <summary>
		/// Copies the entire <c>PluginCollection</c> to a one-dimensional
		/// <see cref="T:log4net.Plugin.IPlugin" /> array, starting at the specified index of the target array.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:log4net.Plugin.IPlugin" /> array to copy to.</param>
		/// <param name="start">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		public virtual void CopyTo(IPlugin[] array, int start)
		{
			if (m_count > array.GetUpperBound(0) + 1 - start)
			{
				throw new ArgumentException("Destination array was not long enough.");
			}
			Array.Copy(m_array, 0, array, start, m_count);
		}

		/// <summary>
		/// Adds a <see cref="T:log4net.Plugin.IPlugin" /> to the end of the <c>PluginCollection</c>.
		/// </summary>
		/// <param name="item">The <see cref="T:log4net.Plugin.IPlugin" /> to be added to the end of the <c>PluginCollection</c>.</param>
		/// <returns>The index at which the value has been added.</returns>
		public virtual int Add(IPlugin item)
		{
			if (m_count == m_array.Length)
			{
				EnsureCapacity(m_count + 1);
			}
			m_array[m_count] = item;
			m_version++;
			return m_count++;
		}

		/// <summary>
		/// Removes all elements from the <c>PluginCollection</c>.
		/// </summary>
		public virtual void Clear()
		{
			m_version++;
			m_array = new IPlugin[16];
			m_count = 0;
		}

		/// <summary>
		/// Creates a shallow copy of the <see cref="T:log4net.Plugin.PluginCollection" />.
		/// </summary>
		/// <returns>A new <see cref="T:log4net.Plugin.PluginCollection" /> with a shallow copy of the collection data.</returns>
		public virtual object Clone()
		{
			PluginCollection pluginCollection = new PluginCollection(m_count);
			Array.Copy(m_array, 0, pluginCollection.m_array, 0, m_count);
			pluginCollection.m_count = m_count;
			pluginCollection.m_version = m_version;
			return pluginCollection;
		}

		/// <summary>
		/// Determines whether a given <see cref="T:log4net.Plugin.IPlugin" /> is in the <c>PluginCollection</c>.
		/// </summary>
		/// <param name="item">The <see cref="T:log4net.Plugin.IPlugin" /> to check for.</param>
		/// <returns><c>true</c> if <paramref name="item" /> is found in the <c>PluginCollection</c>; otherwise, <c>false</c>.</returns>
		public virtual bool Contains(IPlugin item)
		{
			for (int i = 0; i != m_count; i++)
			{
				if (m_array[i].Equals(item))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns the zero-based index of the first occurrence of a <see cref="T:log4net.Plugin.IPlugin" />
		/// in the <c>PluginCollection</c>.
		/// </summary>
		/// <param name="item">The <see cref="T:log4net.Plugin.IPlugin" /> to locate in the <c>PluginCollection</c>.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of <paramref name="item" /> 
		/// in the entire <c>PluginCollection</c>, if found; otherwise, -1.
		/// </returns>
		public virtual int IndexOf(IPlugin item)
		{
			for (int i = 0; i != m_count; i++)
			{
				if (m_array[i].Equals(item))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Inserts an element into the <c>PluginCollection</c> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
		/// <param name="item">The <see cref="T:log4net.Plugin.IPlugin" /> to insert.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <para><paramref name="index" /> is less than zero</para>
		/// <para>-or-</para>
		/// <para><paramref name="index" /> is equal to or greater than <see cref="P:log4net.Plugin.PluginCollection.Count" />.</para>
		/// </exception>
		public virtual void Insert(int index, IPlugin item)
		{
			ValidateIndex(index, allowEqualEnd: true);
			if (m_count == m_array.Length)
			{
				EnsureCapacity(m_count + 1);
			}
			if (index < m_count)
			{
				Array.Copy(m_array, index, m_array, index + 1, m_count - index);
			}
			m_array[index] = item;
			m_count++;
			m_version++;
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="T:log4net.Plugin.IPlugin" /> from the <c>PluginCollection</c>.
		/// </summary>
		/// <param name="item">The <see cref="T:log4net.Plugin.IPlugin" /> to remove from the <c>PluginCollection</c>.</param>
		/// <exception cref="T:System.ArgumentException">
		/// The specified <see cref="T:log4net.Plugin.IPlugin" /> was not found in the <c>PluginCollection</c>.
		/// </exception>
		public virtual void Remove(IPlugin item)
		{
			int num = IndexOf(item);
			if (num < 0)
			{
				throw new ArgumentException("Cannot remove the specified item because it was not found in the specified Collection.");
			}
			m_version++;
			RemoveAt(num);
		}

		/// <summary>
		/// Removes the element at the specified index of the <c>PluginCollection</c>.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <para><paramref name="index" /> is less than zero.</para>
		/// <para>-or-</para>
		/// <para><paramref name="index" /> is equal to or greater than <see cref="P:log4net.Plugin.PluginCollection.Count" />.</para>
		/// </exception>
		public virtual void RemoveAt(int index)
		{
			ValidateIndex(index);
			m_count--;
			if (index < m_count)
			{
				Array.Copy(m_array, index + 1, m_array, index, m_count - index);
			}
			IPlugin[] sourceArray = new IPlugin[1];
			Array.Copy(sourceArray, 0, m_array, m_count, 1);
			m_version++;
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the <c>PluginCollection</c>.
		/// </summary>
		/// <returns>An <see cref="T:log4net.Plugin.PluginCollection.Enumerator" /> for the entire <c>PluginCollection</c>.</returns>
		public virtual IPluginCollectionEnumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// Adds the elements of another <c>PluginCollection</c> to the current <c>PluginCollection</c>.
		/// </summary>
		/// <param name="x">The <c>PluginCollection</c> whose elements should be added to the end of the current <c>PluginCollection</c>.</param>
		/// <returns>The new <see cref="P:log4net.Plugin.PluginCollection.Count" /> of the <c>PluginCollection</c>.</returns>
		public virtual int AddRange(PluginCollection x)
		{
			if (m_count + x.Count >= m_array.Length)
			{
				EnsureCapacity(m_count + x.Count);
			}
			Array.Copy(x.m_array, 0, m_array, m_count, x.Count);
			m_count += x.Count;
			m_version++;
			return m_count;
		}

		/// <summary>
		/// Adds the elements of a <see cref="T:log4net.Plugin.IPlugin" /> array to the current <c>PluginCollection</c>.
		/// </summary>
		/// <param name="x">The <see cref="T:log4net.Plugin.IPlugin" /> array whose elements should be added to the end of the <c>PluginCollection</c>.</param>
		/// <returns>The new <see cref="P:log4net.Plugin.PluginCollection.Count" /> of the <c>PluginCollection</c>.</returns>
		public virtual int AddRange(IPlugin[] x)
		{
			if (m_count + x.Length >= m_array.Length)
			{
				EnsureCapacity(m_count + x.Length);
			}
			Array.Copy(x, 0, m_array, m_count, x.Length);
			m_count += x.Length;
			m_version++;
			return m_count;
		}

		/// <summary>
		/// Adds the elements of a <see cref="T:log4net.Plugin.IPlugin" /> collection to the current <c>PluginCollection</c>.
		/// </summary>
		/// <param name="col">The <see cref="T:log4net.Plugin.IPlugin" /> collection whose elements should be added to the end of the <c>PluginCollection</c>.</param>
		/// <returns>The new <see cref="P:log4net.Plugin.PluginCollection.Count" /> of the <c>PluginCollection</c>.</returns>
		public virtual int AddRange(ICollection col)
		{
			if (m_count + col.Count >= m_array.Length)
			{
				EnsureCapacity(m_count + col.Count);
			}
			foreach (object item in col)
			{
				Add((IPlugin)item);
			}
			return m_count;
		}

		/// <summary>
		/// Sets the capacity to the actual number of elements.
		/// </summary>
		public virtual void TrimToSize()
		{
			Capacity = m_count;
		}

		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <para><paramref name="index" /> is less than zero.</para>
		/// <para>-or-</para>
		/// <para><paramref name="index" /> is equal to or greater than <see cref="P:log4net.Plugin.PluginCollection.Count" />.</para>
		/// </exception>
		private void ValidateIndex(int i)
		{
			ValidateIndex(i, allowEqualEnd: false);
		}

		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <para><paramref name="index" /> is less than zero.</para>
		/// <para>-or-</para>
		/// <para><paramref name="index" /> is equal to or greater than <see cref="P:log4net.Plugin.PluginCollection.Count" />.</para>
		/// </exception>
		private void ValidateIndex(int i, bool allowEqualEnd)
		{
			int num = allowEqualEnd ? m_count : (m_count - 1);
			if (i < 0 || i > num)
			{
				throw SystemInfo.CreateArgumentOutOfRangeException("i", i, "Index was out of range. Must be non-negative and less than the size of the collection. [" + i + "] Specified argument was out of the range of valid values.");
			}
		}

		private void EnsureCapacity(int min)
		{
			int num = (m_array.Length == 0) ? 16 : (m_array.Length * 2);
			if (num < min)
			{
				num = min;
			}
			Capacity = num;
		}

		void ICollection.CopyTo(Array array, int start)
		{
			Array.Copy(m_array, 0, array, start, m_count);
		}

		int IList.Add(object x)
		{
			return Add((IPlugin)x);
		}

		bool IList.Contains(object x)
		{
			return Contains((IPlugin)x);
		}

		int IList.IndexOf(object x)
		{
			return IndexOf((IPlugin)x);
		}

		void IList.Insert(int pos, object x)
		{
			Insert(pos, (IPlugin)x);
		}

		void IList.Remove(object x)
		{
			Remove((IPlugin)x);
		}

		void IList.RemoveAt(int pos)
		{
			RemoveAt(pos);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator)GetEnumerator();
		}
	}
}
