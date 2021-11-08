using System;
using System.Collections;

namespace log4net.Util
{
	/// <summary>
	/// An always empty <see cref="T:System.Collections.IDictionary" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A singleton implementation of the <see cref="T:System.Collections.IDictionary" />
	/// interface that always represents an empty collection.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	[Serializable]
	public sealed class EmptyDictionary : IDictionary, ICollection, IEnumerable
	{
		/// <summary>
		/// The singleton instance of the empty dictionary.
		/// </summary>
		private static readonly EmptyDictionary s_instance = new EmptyDictionary();

		/// <summary>
		/// Gets the singleton instance of the <see cref="T:log4net.Util.EmptyDictionary" />.
		/// </summary>
		/// <returns>The singleton instance of the <see cref="T:log4net.Util.EmptyDictionary" />.</returns>
		/// <remarks>
		/// <para>
		/// Gets the singleton instance of the <see cref="T:log4net.Util.EmptyDictionary" />.
		/// </para>
		/// </remarks>
		public static EmptyDictionary Instance => s_instance;

		/// <summary>
		/// Gets a value indicating if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread-safe).
		/// </summary>
		/// <value>
		/// <b>true</b> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread-safe); otherwise, <b>false</b>.
		/// </value>
		/// <remarks>
		/// <para>
		/// For the <see cref="T:log4net.Util.EmptyCollection" /> this property is always <b>true</b>.
		/// </para>
		/// </remarks>
		public bool IsSynchronized => true;

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />
		/// </summary>
		/// <value>
		/// The number of elements contained in the <see cref="T:System.Collections.ICollection" />.
		/// </value>
		/// <remarks>
		/// <para>
		/// As the collection is empty the <see cref="P:log4net.Util.EmptyDictionary.Count" /> is always <c>0</c>.
		/// </para>
		/// </remarks>
		public int Count => 0;

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
		/// </summary>
		/// <value>
		/// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
		/// </value>
		/// <remarks>
		/// <para>
		/// As the collection is empty and thread safe and synchronized this instance is also
		/// the <see cref="P:log4net.Util.EmptyDictionary.SyncRoot" /> object.
		/// </para>
		/// </remarks>
		public object SyncRoot => this;

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:log4net.Util.EmptyDictionary" /> has a fixed size.
		/// </summary>
		/// <value><c>true</c></value>
		/// <remarks>
		/// <para>
		/// As the collection is empty <see cref="P:log4net.Util.EmptyDictionary.IsFixedSize" /> always returns <c>true</c>.
		/// </para>
		/// </remarks>
		public bool IsFixedSize => true;

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:log4net.Util.EmptyDictionary" /> is read-only.
		/// </summary>
		/// <value><c>true</c></value>
		/// <remarks>
		/// <para>
		/// As the collection is empty <see cref="P:log4net.Util.EmptyDictionary.IsReadOnly" /> always returns <c>true</c>.
		/// </para>
		/// </remarks>
		public bool IsReadOnly => true;

		/// <summary>
		/// Gets an <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:log4net.Util.EmptyDictionary" />.
		/// </summary>
		/// <value>An <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:log4net.Util.EmptyDictionary" />.</value>
		/// <remarks>
		/// <para>
		/// As the collection is empty a <see cref="T:log4net.Util.EmptyCollection" /> is returned.
		/// </para>
		/// </remarks>
		public ICollection Keys => EmptyCollection.Instance;

		/// <summary>
		/// Gets an <see cref="T:System.Collections.ICollection" /> containing the values of the <see cref="T:log4net.Util.EmptyDictionary" />.
		/// </summary>
		/// <value>An <see cref="T:System.Collections.ICollection" /> containing the values of the <see cref="T:log4net.Util.EmptyDictionary" />.</value>
		/// <remarks>
		/// <para>
		/// As the collection is empty a <see cref="T:log4net.Util.EmptyCollection" /> is returned.
		/// </para>
		/// </remarks>
		public ICollection Values => EmptyCollection.Instance;

		/// <summary>
		/// Gets or sets the element with the specified key.
		/// </summary>
		/// <param name="key">The key of the element to get or set.</param>
		/// <value><c>null</c></value>
		/// <remarks>
		/// <para>
		/// As the collection is empty no values can be looked up or stored. 
		/// If the index getter is called then <c>null</c> is returned.
		/// A <see cref="T:System.InvalidOperationException" /> is thrown if the setter is called.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">This dictionary is always empty and cannot be modified.</exception>
		public object this[object key]
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Util.EmptyDictionary" /> class. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// Uses a private access modifier to enforce the singleton pattern.
		/// </para>
		/// </remarks>
		private EmptyDictionary()
		{
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an 
		/// <see cref="T:System.Array" />, starting at a particular Array index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> 
		/// that is the destination of the elements copied from 
		/// <see cref="T:System.Collections.ICollection" />. The Array must have zero-based 
		/// indexing.</param>
		/// <param name="index">The zero-based index in array at which 
		/// copying begins.</param>
		/// <remarks>
		/// <para>
		/// As the collection is empty no values are copied into the array.
		/// </para>
		/// </remarks>
		public void CopyTo(Array array, int index)
		{
		}

		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator" /> that can be used to 
		/// iterate through the collection.
		/// </returns>
		/// <remarks>
		/// <para>
		/// As the collection is empty a <see cref="T:log4net.Util.NullEnumerator" /> is returned.
		/// </para>
		/// </remarks>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return NullEnumerator.Instance;
		}

		/// <summary>
		/// Adds an element with the provided key and value to the 
		/// <see cref="T:log4net.Util.EmptyDictionary" />.
		/// </summary>
		/// <param name="key">The <see cref="T:System.Object" /> to use as the key of the element to add.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to use as the value of the element to add.</param>
		/// <remarks>
		/// <para>
		/// As the collection is empty no new values can be added. A <see cref="T:System.InvalidOperationException" />
		/// is thrown if this method is called.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">This dictionary is always empty and cannot be modified.</exception>
		public void Add(object key, object value)
		{
			throw new InvalidOperationException();
		}

		/// <summary>
		/// Removes all elements from the <see cref="T:log4net.Util.EmptyDictionary" />.
		/// </summary>
		/// <remarks>
		/// <para>
		/// As the collection is empty no values can be removed. A <see cref="T:System.InvalidOperationException" />
		/// is thrown if this method is called.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">This dictionary is always empty and cannot be modified.</exception>
		public void Clear()
		{
			throw new InvalidOperationException();
		}

		/// <summary>
		/// Determines whether the <see cref="T:log4net.Util.EmptyDictionary" /> contains an element 
		/// with the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the <see cref="T:log4net.Util.EmptyDictionary" />.</param>
		/// <returns><c>false</c></returns>
		/// <remarks>
		/// <para>
		/// As the collection is empty the <see cref="M:log4net.Util.EmptyDictionary.Contains(System.Object)" /> method always returns <c>false</c>.
		/// </para>
		/// </remarks>
		public bool Contains(object key)
		{
			return false;
		}

		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator" /> that can be used to 
		/// iterate through the collection.
		/// </returns>
		/// <remarks>
		/// <para>
		/// As the collection is empty a <see cref="T:log4net.Util.NullEnumerator" /> is returned.
		/// </para>
		/// </remarks>
		public IDictionaryEnumerator GetEnumerator()
		{
			return NullDictionaryEnumerator.Instance;
		}

		/// <summary>
		/// Removes the element with the specified key from the <see cref="T:log4net.Util.EmptyDictionary" />.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <remarks>
		/// <para>
		/// As the collection is empty no values can be removed. A <see cref="T:System.InvalidOperationException" />
		/// is thrown if this method is called.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">This dictionary is always empty and cannot be modified.</exception>
		public void Remove(object key)
		{
			throw new InvalidOperationException();
		}
	}
}
