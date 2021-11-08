namespace log4net
{
	/// <summary>
	/// Implementation of Mapped Diagnostic Contexts.
	/// </summary>
	/// <remarks>
	/// <note>
	/// <para>
	/// The MDC is deprecated and has been replaced by the <see cref="P:log4net.ThreadContext.Properties" />.
	/// The current MDC implementation forwards to the <c>ThreadContext.Properties</c>.
	/// </para>
	/// </note>
	/// <para>
	/// The MDC class is similar to the <see cref="T:log4net.NDC" /> class except that it is
	/// based on a map instead of a stack. It provides <i>mapped
	/// diagnostic contexts</i>. A <i>Mapped Diagnostic Context</i>, or
	/// MDC in short, is an instrument for distinguishing interleaved log
	/// output from different sources. Log output is typically interleaved
	/// when a server handles multiple clients near-simultaneously.
	/// </para>
	/// <para>
	/// The MDC is managed on a per thread basis.
	/// </para>
	/// </remarks>
	/// <threadsafety static="true" instance="true" />
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public sealed class MDC
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.MDC" /> class. 
		/// </summary>
		/// <remarks>
		/// Uses a private access modifier to prevent instantiation of this class.
		/// </remarks>
		private MDC()
		{
		}

		/// <summary>
		/// Gets the context value identified by the <paramref name="key" /> parameter.
		/// </summary>
		/// <param name="key">The key to lookup in the MDC.</param>
		/// <returns>The string value held for the key, or a <c>null</c> reference if no corresponding value is found.</returns>
		/// <remarks>
		/// <note>
		/// <para>
		/// The MDC is deprecated and has been replaced by the <see cref="P:log4net.ThreadContext.Properties" />.
		/// The current MDC implementation forwards to the <c>ThreadContext.Properties</c>.
		/// </para>
		/// </note>
		/// <para>
		/// If the <paramref name="key" /> parameter does not look up to a
		/// previously defined context then <c>null</c> will be returned.
		/// </para>
		/// </remarks>
		public static string Get(string key)
		{
			return ThreadContext.Properties[key]?.ToString();
		}

		/// <summary>
		/// Add an entry to the MDC
		/// </summary>
		/// <param name="key">The key to store the value under.</param>
		/// <param name="value">The value to store.</param>
		/// <remarks>
		/// <note>
		/// <para>
		/// The MDC is deprecated and has been replaced by the <see cref="P:log4net.ThreadContext.Properties" />.
		/// The current MDC implementation forwards to the <c>ThreadContext.Properties</c>.
		/// </para>
		/// </note>
		/// <para>
		/// Puts a context value (the <paramref name="val" /> parameter) as identified
		/// with the <paramref name="key" /> parameter into the current thread's
		/// context map.
		/// </para>
		/// <para>
		/// If a value is already defined for the <paramref name="key" />
		/// specified then the value will be replaced. If the <paramref name="val" /> 
		/// is specified as <c>null</c> then the key value mapping will be removed.
		/// </para>
		/// </remarks>
		public static void Set(string key, string value)
		{
			ThreadContext.Properties[key] = value;
		}

		/// <summary>
		/// Removes the key value mapping for the key specified.
		/// </summary>
		/// <param name="key">The key to remove.</param>
		/// <remarks>
		/// <note>
		/// <para>
		/// The MDC is deprecated and has been replaced by the <see cref="P:log4net.ThreadContext.Properties" />.
		/// The current MDC implementation forwards to the <c>ThreadContext.Properties</c>.
		/// </para>
		/// </note>
		/// <para>
		/// Remove the specified entry from this thread's MDC
		/// </para>
		/// </remarks>
		public static void Remove(string key)
		{
			ThreadContext.Properties.Remove(key);
		}

		/// <summary>
		/// Clear all entries in the MDC
		/// </summary>
		/// <remarks>
		/// <note>
		/// <para>
		/// The MDC is deprecated and has been replaced by the <see cref="P:log4net.ThreadContext.Properties" />.
		/// The current MDC implementation forwards to the <c>ThreadContext.Properties</c>.
		/// </para>
		/// </note>
		/// <para>
		/// Remove all the entries from this thread's MDC
		/// </para>
		/// </remarks>
		public static void Clear()
		{
			ThreadContext.Properties.Clear();
		}
	}
}
