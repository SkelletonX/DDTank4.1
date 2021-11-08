using System;
using System.Collections;

namespace log4net.Util
{
	/// <summary>
	/// An always empty <see cref="T:System.Collections.ICollection" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A singleton implementation of the <see cref="T:System.Collections.ICollection" />
	/// interface that always represents an empty collection.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	[Serializable]
	public sealed class EmptyCollection : ICollection, IEnumerable
	{
		/// <summary>
		/// The singleton instance of the empty collection.
		/// </summary>
		private static readonly EmptyCollection s_instance = new EmptyCollection();

		/// <summary>
		/// Gets the singleton instance of the empty collection.
		/// </summary>
		/// <returns>The singleton instance of the empty collection.</returns>
		/// <remarks>
		/// <para>
		/// Gets the singleton instance of the empty collection.
		/// </para>
		/// </remarks>
		public static EmptyCollection Instance => s_instance;

		/// <summary>
		/// Gets a value indicating if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread-safe).
		/// </summary>
		/// <value>
		/// <b>true</b> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread-safe); otherwise, <b>false</b>.
		/// </value>
		/// <remarks>
		/// <para>
		/// For the <see cref="T:log4net.Util.EmptyCollection" /> this property is always <c>true</c>.
		/// </para>
		/// </remarks>
		public bool IsSynchronized => true;

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />.
		/// </summary>
		/// <value>
		/// The number of elements contained in the <see cref="T:System.Collections.ICollection" />.
		/// </value>
		/// <remarks>
		/// <para>
		/// As the collection is empty the <see cref="P:log4net.Util.EmptyCollection.Count" /> is always <c>0</c>.
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
		/// the <see cref="P:log4net.Util.EmptyCollection.SyncRoot" /> object.
		/// </para>
		/// </remarks>
		public object SyncRoot => this;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Util.EmptyCollection" /> class. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// Uses a private access modifier to enforce the singleton pattern.
		/// </para>
		/// </remarks>
		private EmptyCollection()
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
		public IEnumerator GetEnumerator()
		{
			return NullEnumerator.Instance;
		}
	}
}
