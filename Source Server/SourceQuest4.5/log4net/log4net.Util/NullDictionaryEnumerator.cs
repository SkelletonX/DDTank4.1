using System;
using System.Collections;

namespace log4net.Util
{
	/// <summary>
	/// An always empty <see cref="T:System.Collections.IDictionaryEnumerator" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A singleton implementation of the <see cref="T:System.Collections.IDictionaryEnumerator" /> over a collection
	/// that is empty and not modifiable.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public sealed class NullDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		/// <summary>
		/// The singleton instance of the <see cref="T:log4net.Util.NullDictionaryEnumerator" />.
		/// </summary>
		private static readonly NullDictionaryEnumerator s_instance = new NullDictionaryEnumerator();

		/// <summary>
		/// Gets the singleton instance of the <see cref="T:log4net.Util.NullDictionaryEnumerator" />.
		/// </summary>
		/// <returns>The singleton instance of the <see cref="T:log4net.Util.NullDictionaryEnumerator" />.</returns>
		/// <remarks>
		/// <para>
		/// Gets the singleton instance of the <see cref="T:log4net.Util.NullDictionaryEnumerator" />.
		/// </para>
		/// </remarks>
		public static NullDictionaryEnumerator Instance => s_instance;

		/// <summary>
		/// Gets the current object from the enumerator.
		/// </summary>
		/// <remarks>
		/// Throws an <see cref="T:System.InvalidOperationException" /> because the 
		/// <see cref="T:log4net.Util.NullDictionaryEnumerator" /> never has a current value.
		/// </remarks>
		/// <remarks>
		/// <para>
		/// As the enumerator is over an empty collection its <see cref="P:log4net.Util.NullDictionaryEnumerator.Current" />
		/// value cannot be moved over a valid position, therefore <see cref="P:log4net.Util.NullDictionaryEnumerator.Current" />
		/// will throw an <see cref="T:System.InvalidOperationException" />.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">The collection is empty and <see cref="P:log4net.Util.NullDictionaryEnumerator.Current" /> 
		/// cannot be positioned over a valid location.</exception>
		public object Current
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// Gets the current key from the enumerator.
		/// </summary>
		/// <remarks>
		/// Throws an exception because the <see cref="T:log4net.Util.NullDictionaryEnumerator" />
		/// never has a current value.
		/// </remarks>
		/// <remarks>
		/// <para>
		/// As the enumerator is over an empty collection its <see cref="P:log4net.Util.NullDictionaryEnumerator.Current" />
		/// value cannot be moved over a valid position, therefore <see cref="P:log4net.Util.NullDictionaryEnumerator.Key" />
		/// will throw an <see cref="T:System.InvalidOperationException" />.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">The collection is empty and <see cref="P:log4net.Util.NullDictionaryEnumerator.Current" /> 
		/// cannot be positioned over a valid location.</exception>
		public object Key
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// Gets the current value from the enumerator.
		/// </summary>
		/// <value>The current value from the enumerator.</value>
		/// <remarks>
		/// Throws an <see cref="T:System.InvalidOperationException" /> because the 
		/// <see cref="T:log4net.Util.NullDictionaryEnumerator" /> never has a current value.
		/// </remarks>
		/// <remarks>
		/// <para>
		/// As the enumerator is over an empty collection its <see cref="P:log4net.Util.NullDictionaryEnumerator.Current" />
		/// value cannot be moved over a valid position, therefore <see cref="P:log4net.Util.NullDictionaryEnumerator.Value" />
		/// will throw an <see cref="T:System.InvalidOperationException" />.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">The collection is empty and <see cref="P:log4net.Util.NullDictionaryEnumerator.Current" /> 
		/// cannot be positioned over a valid location.</exception>
		public object Value
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// Gets the current entry from the enumerator.
		/// </summary>
		/// <remarks>
		/// Throws an <see cref="T:System.InvalidOperationException" /> because the 
		/// <see cref="T:log4net.Util.NullDictionaryEnumerator" /> never has a current entry.
		/// </remarks>
		/// <remarks>
		/// <para>
		/// As the enumerator is over an empty collection its <see cref="P:log4net.Util.NullDictionaryEnumerator.Current" />
		/// value cannot be moved over a valid position, therefore <see cref="P:log4net.Util.NullDictionaryEnumerator.Entry" />
		/// will throw an <see cref="T:System.InvalidOperationException" />.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">The collection is empty and <see cref="P:log4net.Util.NullDictionaryEnumerator.Current" /> 
		/// cannot be positioned over a valid location.</exception>
		public DictionaryEntry Entry
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Util.NullDictionaryEnumerator" /> class. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// Uses a private access modifier to enforce the singleton pattern.
		/// </para>
		/// </remarks>
		private NullDictionaryEnumerator()
		{
		}

		/// <summary>
		/// Test if the enumerator can advance, if so advance.
		/// </summary>
		/// <returns><c>false</c> as the <see cref="T:log4net.Util.NullDictionaryEnumerator" /> cannot advance.</returns>
		/// <remarks>
		/// <para>
		/// As the enumerator is over an empty collection its <see cref="P:log4net.Util.NullDictionaryEnumerator.Current" />
		/// value cannot be moved over a valid position, therefore <see cref="M:log4net.Util.NullDictionaryEnumerator.MoveNext" />
		/// will always return <c>false</c>.
		/// </para>
		/// </remarks>
		public bool MoveNext()
		{
			return false;
		}

		/// <summary>
		/// Resets the enumerator back to the start.
		/// </summary>
		/// <remarks>
		/// <para>
		/// As the enumerator is over an empty collection <see cref="M:log4net.Util.NullDictionaryEnumerator.Reset" /> does nothing.
		/// </para>
		/// </remarks>
		public void Reset()
		{
		}
	}
}
