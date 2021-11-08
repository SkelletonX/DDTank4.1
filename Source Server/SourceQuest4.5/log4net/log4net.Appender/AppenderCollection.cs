using log4net.Util;
using System;
using System.Collections;

namespace log4net.Appender
{
	/// <summary>
	/// A strongly-typed collection of <see cref="T:log4net.Appender.IAppender" /> objects.
	/// </summary>
	/// <author>Nicko Cadell</author>
	public class AppenderCollection : IList, ICollection, IEnumerable, ICloneable
	{
		/// <summary>
		/// Supports type-safe iteration over a <see cref="T:log4net.Appender.AppenderCollection" />.
		/// </summary>
		/// <exclude />
		public interface IAppenderCollectionEnumerator
		{
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			IAppender Current
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
		/// Supports simple iteration over a <see cref="T:log4net.Appender.AppenderCollection" />.
		/// </summary>
		/// <exclude />
		private sealed class Enumerator : IEnumerator, IAppenderCollectionEnumerator
		{
			private readonly AppenderCollection m_collection;

			private int m_index;

			private int m_version;

			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public IAppender Current => m_collection[m_index];

			object IEnumerator.Current => Current;

			/// <summary>
			/// Initializes a new instance of the <c>Enumerator</c> class.
			/// </summary>
			/// <param name="tc"></param>
			internal Enumerator(AppenderCollection tc)
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
		private sealed class ReadOnlyAppenderCollection : AppenderCollection, ICollection, IEnumerable
		{
			private readonly AppenderCollection m_collection;

			public override int Count => m_collection.Count;

			public override bool IsSynchronized => m_collection.IsSynchronized;

			public override object SyncRoot => m_collection.SyncRoot;

			public override IAppender this[int i]
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

			internal ReadOnlyAppenderCollection(AppenderCollection list)
				: base(Tag.Default)
			{
				m_collection = list;
			}

			public override void CopyTo(IAppender[] array)
			{
				m_collection.CopyTo(array);
			}

			public override void CopyTo(IAppender[] array, int start)
			{
				m_collection.CopyTo(array, start);
			}

			void ICollection.CopyTo(Array array, int start)
			{
				((ICollection)m_collection).CopyTo(array, start);
			}

			public override int Add(IAppender x)
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}

			public override void Clear()
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}

			public override bool Contains(IAppender x)
			{
				return m_collection.Contains(x);
			}

			public override int IndexOf(IAppender x)
			{
				return m_collection.IndexOf(x);
			}

			public override void Insert(int pos, IAppender x)
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}

			public override void Remove(IAppender x)
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}

			public override void RemoveAt(int pos)
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}

			public override IAppenderCollectionEnumerator GetEnumerator()
			{
				return m_collection.GetEnumerator();
			}

			public override int AddRange(AppenderCollection x)
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}

			public override int AddRange(IAppender[] x)
			{
				throw new NotSupportedException("This is a Read Only Collection and can not be modified");
			}
		}

		private const int DEFAULT_CAPACITY = 16;

		private IAppender[] m_array;

		private int m_count = 0;

		private int m_version = 0;

		/// <summary>
		/// An empty readonly static AppenderCollection
		/// </summary>
		public static readonly AppenderCollection EmptyCollection = ReadOnly(new AppenderCollection(0));

		/// <summary>
		/// Gets the number of elements actually contained in the <c>AppenderCollection</c>.
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
		public virtual object SyncRoot => m_array.SyncRoot;

		/// <summary>
		/// Gets or sets the <see cref="T:log4net.Appender.IAppender" /> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<para><paramref name="index" /> is less than zero</para>
		/// 	<para>-or-</para>
		/// 	<para><paramref name="index" /> is equal to or greater than <see cref="P:log4net.Appender.AppenderCollection.Count" />.</para>
		/// </exception>
		public virtual IAppender this[int index]
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
		/// <value>true if the collection has a fixed size; otherwise, false. The default is false</value>
		public virtual bool IsFixedSize => false;

		/// <summary>
		/// Gets a value indicating whether the IList is read-only.
		/// </summary>
		/// <value>true if the collection is read-only; otherwise, false. The default is false</value>
		public virtual bool IsReadOnly => false;

		/// <summary>
		/// Gets or sets the number of elements the <c>AppenderCollection</c> can contain.
		/// </summary>
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
						IAppender[] array = new IAppender[value];
						Array.Copy(m_array, 0, array, 0, m_count);
						m_array = array;
					}
					else
					{
						m_array = new IAppender[16];
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
				this[i] = (IAppender)value;
			}
		}

		/// <summary>
		/// Creates a read-only wrapper for a <c>AppenderCollection</c> instance.
		/// </summary>
		/// <param name="list">list to create a readonly wrapper arround</param>
		/// <returns>
		/// An <c>AppenderCollection</c> wrapper that is read-only.
		/// </returns>
		public static AppenderCollection ReadOnly(AppenderCollection list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ReadOnlyAppenderCollection(list);
		}

		/// <summary>
		/// Initializes a new instance of the <c>AppenderCollection</c> class
		/// that is empty and has the default initial capacity.
		/// </summary>
		public AppenderCollection()
		{
			m_array = new IAppender[16];
		}

		/// <summary>
		/// Initializes a new instance of the <c>AppenderCollection</c> class
		/// that has the specified initial capacity.
		/// </summary>
		/// <param name="capacity">
		/// The number of elements that the new <c>AppenderCollection</c> is initially capable of storing.
		/// </param>
		public AppenderCollection(int capacity)
		{
			m_array = new IAppender[capacity];
		}

		/// <summary>
		/// Initializes a new instance of the <c>AppenderCollection</c> class
		/// that contains elements copied from the specified <c>AppenderCollection</c>.
		/// </summary>
		/// <param name="c">The <c>AppenderCollection</c> whose elements are copied to the new collection.</param>
		public AppenderCollection(AppenderCollection c)
		{
			m_array = new IAppender[c.Count];
			AddRange(c);
		}

		/// <summary>
		/// Initializes a new instance of the <c>AppenderCollection</c> class
		/// that contains elements copied from the specified <see cref="T:log4net.Appender.IAppender" /> array.
		/// </summary>
		/// <param name="a">The <see cref="T:log4net.Appender.IAppender" /> array whose elements are copied to the new list.</param>
		public AppenderCollection(IAppender[] a)
		{
			m_array = new IAppender[a.Length];
			AddRange(a);
		}

		/// <summary>
		/// Initializes a new instance of the <c>AppenderCollection</c> class
		/// that contains elements copied from the specified <see cref="T:log4net.Appender.IAppender" /> collection.
		/// </summary>
		/// <param name="col">The <see cref="T:log4net.Appender.IAppender" /> collection whose elements are copied to the new list.</param>
		public AppenderCollection(ICollection col)
		{
			m_array = new IAppender[col.Count];
			AddRange(col);
		}

		/// <summary>
		/// Allow subclasses to avoid our default constructors
		/// </summary>
		/// <param name="tag"></param>
		/// <exclude />
		protected internal AppenderCollection(Tag tag)
		{
			m_array = null;
		}

		/// <summary>
		/// Copies the entire <c>AppenderCollection</c> to a one-dimensional
		/// <see cref="T:log4net.Appender.IAppender" /> array.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:log4net.Appender.IAppender" /> array to copy to.</param>
		public virtual void CopyTo(IAppender[] array)
		{
			CopyTo(array, 0);
		}

		/// <summary>
		/// Copies the entire <c>AppenderCollection</c> to a one-dimensional
		/// <see cref="T:log4net.Appender.IAppender" /> array, starting at the specified index of the target array.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:log4net.Appender.IAppender" /> array to copy to.</param>
		/// <param name="start">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		public virtual void CopyTo(IAppender[] array, int start)
		{
			if (m_count > array.GetUpperBound(0) + 1 - start)
			{
				throw new ArgumentException("Destination array was not long enough.");
			}
			Array.Copy(m_array, 0, array, start, m_count);
		}

		/// <summary>
		/// Adds a <see cref="T:log4net.Appender.IAppender" /> to the end of the <c>AppenderCollection</c>.
		/// </summary>
		/// <param name="item">The <see cref="T:log4net.Appender.IAppender" /> to be added to the end of the <c>AppenderCollection</c>.</param>
		/// <returns>The index at which the value has been added.</returns>
		public virtual int Add(IAppender item)
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
		/// Removes all elements from the <c>AppenderCollection</c>.
		/// </summary>
		public virtual void Clear()
		{
			m_version++;
			m_array = new IAppender[16];
			m_count = 0;
		}

		/// <summary>
		/// Creates a shallow copy of the <see cref="T:log4net.Appender.AppenderCollection" />.
		/// </summary>
		/// <returns>A new <see cref="T:log4net.Appender.AppenderCollection" /> with a shallow copy of the collection data.</returns>
		public virtual object Clone()
		{
			AppenderCollection appenderCollection = new AppenderCollection(m_count);
			Array.Copy(m_array, 0, appenderCollection.m_array, 0, m_count);
			appenderCollection.m_count = m_count;
			appenderCollection.m_version = m_version;
			return appenderCollection;
		}

		/// <summary>
		/// Determines whether a given <see cref="T:log4net.Appender.IAppender" /> is in the <c>AppenderCollection</c>.
		/// </summary>
		/// <param name="item">The <see cref="T:log4net.Appender.IAppender" /> to check for.</param>
		/// <returns><c>true</c> if <paramref name="item" /> is found in the <c>AppenderCollection</c>; otherwise, <c>false</c>.</returns>
		public virtual bool Contains(IAppender item)
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
		/// Returns the zero-based index of the first occurrence of a <see cref="T:log4net.Appender.IAppender" />
		/// in the <c>AppenderCollection</c>.
		/// </summary>
		/// <param name="item">The <see cref="T:log4net.Appender.IAppender" /> to locate in the <c>AppenderCollection</c>.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of <paramref name="item" /> 
		/// in the entire <c>AppenderCollection</c>, if found; otherwise, -1.
		/// </returns>
		public virtual int IndexOf(IAppender item)
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
		/// Inserts an element into the <c>AppenderCollection</c> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
		/// <param name="item">The <see cref="T:log4net.Appender.IAppender" /> to insert.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <para><paramref name="index" /> is less than zero</para>
		/// <para>-or-</para>
		/// <para><paramref name="index" /> is equal to or greater than <see cref="P:log4net.Appender.AppenderCollection.Count" />.</para>
		/// </exception>
		public virtual void Insert(int index, IAppender item)
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
		/// Removes the first occurrence of a specific <see cref="T:log4net.Appender.IAppender" /> from the <c>AppenderCollection</c>.
		/// </summary>
		/// <param name="item">The <see cref="T:log4net.Appender.IAppender" /> to remove from the <c>AppenderCollection</c>.</param>
		/// <exception cref="T:System.ArgumentException">
		/// The specified <see cref="T:log4net.Appender.IAppender" /> was not found in the <c>AppenderCollection</c>.
		/// </exception>
		public virtual void Remove(IAppender item)
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
		/// Removes the element at the specified index of the <c>AppenderCollection</c>.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <para><paramref name="index" /> is less than zero</para>
		/// <para>-or-</para>
		/// <para><paramref name="index" /> is equal to or greater than <see cref="P:log4net.Appender.AppenderCollection.Count" />.</para>
		/// </exception>
		public virtual void RemoveAt(int index)
		{
			ValidateIndex(index);
			m_count--;
			if (index < m_count)
			{
				Array.Copy(m_array, index + 1, m_array, index, m_count - index);
			}
			IAppender[] sourceArray = new IAppender[1];
			Array.Copy(sourceArray, 0, m_array, m_count, 1);
			m_version++;
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the <c>AppenderCollection</c>.
		/// </summary>
		/// <returns>An <see cref="T:log4net.Appender.AppenderCollection.Enumerator" /> for the entire <c>AppenderCollection</c>.</returns>
		public virtual IAppenderCollectionEnumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// Adds the elements of another <c>AppenderCollection</c> to the current <c>AppenderCollection</c>.
		/// </summary>
		/// <param name="x">The <c>AppenderCollection</c> whose elements should be added to the end of the current <c>AppenderCollection</c>.</param>
		/// <returns>The new <see cref="P:log4net.Appender.AppenderCollection.Count" /> of the <c>AppenderCollection</c>.</returns>
		public virtual int AddRange(AppenderCollection x)
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
		/// Adds the elements of a <see cref="T:log4net.Appender.IAppender" /> array to the current <c>AppenderCollection</c>.
		/// </summary>
		/// <param name="x">The <see cref="T:log4net.Appender.IAppender" /> array whose elements should be added to the end of the <c>AppenderCollection</c>.</param>
		/// <returns>The new <see cref="P:log4net.Appender.AppenderCollection.Count" /> of the <c>AppenderCollection</c>.</returns>
		public virtual int AddRange(IAppender[] x)
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
		/// Adds the elements of a <see cref="T:log4net.Appender.IAppender" /> collection to the current <c>AppenderCollection</c>.
		/// </summary>
		/// <param name="col">The <see cref="T:log4net.Appender.IAppender" /> collection whose elements should be added to the end of the <c>AppenderCollection</c>.</param>
		/// <returns>The new <see cref="P:log4net.Appender.AppenderCollection.Count" /> of the <c>AppenderCollection</c>.</returns>
		public virtual int AddRange(ICollection col)
		{
			if (m_count + col.Count >= m_array.Length)
			{
				EnsureCapacity(m_count + col.Count);
			}
			foreach (object item in col)
			{
				Add((IAppender)item);
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

		/// <summary>
		/// Return the collection elements as an array
		/// </summary>
		/// <returns>the array</returns>
		public virtual IAppender[] ToArray()
		{
			IAppender[] array = new IAppender[m_count];
			if (m_count > 0)
			{
				Array.Copy(m_array, 0, array, 0, m_count);
			}
			return array;
		}

		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <para><paramref name="index" /> is less than zero</para>
		/// <para>-or-</para>
		/// <para><paramref name="index" /> is equal to or greater than <see cref="P:log4net.Appender.AppenderCollection.Count" />.</para>
		/// </exception>
		private void ValidateIndex(int i)
		{
			ValidateIndex(i, allowEqualEnd: false);
		}

		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <para><paramref name="index" /> is less than zero</para>
		/// <para>-or-</para>
		/// <para><paramref name="index" /> is equal to or greater than <see cref="P:log4net.Appender.AppenderCollection.Count" />.</para>
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
			if (m_count > 0)
			{
				Array.Copy(m_array, 0, array, start, m_count);
			}
		}

		int IList.Add(object x)
		{
			return Add((IAppender)x);
		}

		bool IList.Contains(object x)
		{
			return Contains((IAppender)x);
		}

		int IList.IndexOf(object x)
		{
			return IndexOf((IAppender)x);
		}

		void IList.Insert(int pos, object x)
		{
			Insert(pos, (IAppender)x);
		}

		void IList.Remove(object x)
		{
			Remove((IAppender)x);
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
