using System;
using System.Collections;

namespace log4net.Util
{
	/// <summary>
	/// An always empty <see cref="T:System.Collections.IEnumerator" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A singleton implementation of the <see cref="T:System.Collections.IEnumerator" /> over a collection
	/// that is empty and not modifiable.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public sealed class NullEnumerator : IEnumerator
	{
		/// <summary>
		/// The singleton instance of the <see cref="T:log4net.Util.NullEnumerator" />.
		/// </summary>
		private static readonly NullEnumerator s_instance = new NullEnumerator();

		/// <summary>
		/// Get the singleton instance of the <see cref="T:log4net.Util.NullEnumerator" />.
		/// </summary>
		/// <returns>The singleton instance of the <see cref="T:log4net.Util.NullEnumerator" />.</returns>
		/// <remarks>
		/// <para>
		/// Gets the singleton instance of the <see cref="T:log4net.Util.NullEnumerator" />.
		/// </para>
		/// </remarks>
		public static NullEnumerator Instance => s_instance;

		/// <summary>
		/// Gets the current object from the enumerator.
		/// </summary>
		/// <remarks>
		/// Throws an <see cref="T:System.InvalidOperationException" /> because the 
		/// <see cref="T:log4net.Util.NullDictionaryEnumerator" /> never has a current value.
		/// </remarks>
		/// <remarks>
		/// <para>
		/// As the enumerator is over an empty collection its <see cref="P:log4net.Util.NullEnumerator.Current" />
		/// value cannot be moved over a valid position, therefore <see cref="P:log4net.Util.NullEnumerator.Current" />
		/// will throw an <see cref="T:System.InvalidOperationException" />.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">The collection is empty and <see cref="P:log4net.Util.NullEnumerator.Current" /> 
		/// cannot be positioned over a valid location.</exception>
		public object Current
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Util.NullEnumerator" /> class. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// Uses a private access modifier to enforce the singleton pattern.
		/// </para>
		/// </remarks>
		private NullEnumerator()
		{
		}

		/// <summary>
		/// Test if the enumerator can advance, if so advance
		/// </summary>
		/// <returns><c>false</c> as the <see cref="T:log4net.Util.NullEnumerator" /> cannot advance.</returns>
		/// <remarks>
		/// <para>
		/// As the enumerator is over an empty collection its <see cref="P:log4net.Util.NullEnumerator.Current" />
		/// value cannot be moved over a valid position, therefore <see cref="M:log4net.Util.NullEnumerator.MoveNext" />
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
		/// As the enumerator is over an empty collection <see cref="M:log4net.Util.NullEnumerator.Reset" /> does nothing.
		/// </para>
		/// </remarks>
		public void Reset()
		{
		}
	}
}
