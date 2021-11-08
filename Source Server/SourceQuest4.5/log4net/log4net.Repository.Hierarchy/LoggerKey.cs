namespace log4net.Repository.Hierarchy
{
	/// <summary>
	/// Used internally to accelerate hash table searches.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Internal class used to improve performance of 
	/// string keyed hashtables.
	/// </para>
	/// <para>
	/// The hashcode of the string is cached for reuse.
	/// The string is stored as an interned value.
	/// When comparing two <see cref="T:log4net.Repository.Hierarchy.LoggerKey" /> objects for equality 
	/// the reference equality of the interned strings is compared.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	internal sealed class LoggerKey
	{
		private readonly string m_name;

		private readonly int m_hashCache;

		/// <summary>
		/// Construct key with string name
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Repository.Hierarchy.LoggerKey" /> class 
		/// with the specified name.
		/// </para>
		/// <para>
		/// Stores the hashcode of the string and interns
		/// the string key to optimize comparisons.
		/// </para>
		/// <note>
		/// The Compact Framework 1.0 the <see cref="M:System.String.Intern(System.String)" />
		/// method does not work. On the Compact Framework
		/// the string keys are not interned nor are they
		/// compared by reference.
		/// </note>
		/// </remarks>
		/// <param name="name">The name of the logger.</param>
		internal LoggerKey(string name)
		{
			m_name = string.Intern(name);
			m_hashCache = name.GetHashCode();
		}

		/// <summary>
		/// Returns a hash code for the current instance.
		/// </summary>
		/// <returns>A hash code for the current instance.</returns>
		/// <remarks>
		/// <para>
		/// Returns the cached hashcode.
		/// </para>
		/// </remarks>
		public override int GetHashCode()
		{
			return m_hashCache;
		}

		/// <summary>
		/// Determines whether two <see cref="T:log4net.Repository.Hierarchy.LoggerKey" /> instances 
		/// are equal.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:log4net.Repository.Hierarchy.LoggerKey" />.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:log4net.Repository.Hierarchy.LoggerKey" />; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Compares the references of the interned strings.
		/// </para>
		/// </remarks>
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			LoggerKey loggerKey = obj as LoggerKey;
			if (loggerKey != null)
			{
				return (object)m_name == loggerKey.m_name;
			}
			return false;
		}
	}
}
