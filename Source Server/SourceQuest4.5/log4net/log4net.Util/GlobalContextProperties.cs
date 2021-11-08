namespace log4net.Util
{
	/// <summary>
	/// Implementation of Properties collection for the <see cref="T:log4net.GlobalContext" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class implements a properties collection that is thread safe and supports both
	/// storing properties and capturing a read only copy of the current propertied.
	/// </para>
	/// <para>
	/// This class is optimized to the scenario where the properties are read frequently
	/// and are modified infrequently.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public sealed class GlobalContextProperties : ContextPropertiesBase
	{
		/// <summary>
		/// The read only copy of the properties.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This variable is declared <c>volatile</c> to prevent the compiler and JIT from
		/// reordering reads and writes of this thread performed on different threads.
		/// </para>
		/// </remarks>
		private volatile ReadOnlyPropertiesDictionary m_readOnlyProperties = new ReadOnlyPropertiesDictionary();

		/// <summary>
		/// Lock object used to synchronize updates within this instance
		/// </summary>
		private readonly object m_syncRoot = new object();

		/// <summary>
		/// Gets or sets the value of a property
		/// </summary>
		/// <value>
		/// The value for the property with the specified key
		/// </value>
		/// <remarks>
		/// <para>
		/// Reading the value for a key is faster than setting the value.
		/// When the value is written a new read only copy of 
		/// the properties is created.
		/// </para>
		/// </remarks>
		public override object this[string key]
		{
			get
			{
				return m_readOnlyProperties[key];
			}
			set
			{
				lock (m_syncRoot)
				{
					PropertiesDictionary propertiesDictionary = new PropertiesDictionary(m_readOnlyProperties);
					propertiesDictionary[key] = value;
					m_readOnlyProperties = new ReadOnlyPropertiesDictionary(propertiesDictionary);
				}
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.GlobalContextProperties" /> class.
		/// </para>
		/// </remarks>
		internal GlobalContextProperties()
		{
		}

		/// <summary>
		/// Remove a property from the global context
		/// </summary>
		/// <param name="key">the key for the entry to remove</param>
		/// <remarks>
		/// <para>
		/// Removing an entry from the global context properties is relatively expensive compared
		/// with reading a value. 
		/// </para>
		/// </remarks>
		public void Remove(string key)
		{
			lock (m_syncRoot)
			{
				if (m_readOnlyProperties.Contains(key))
				{
					PropertiesDictionary propertiesDictionary = new PropertiesDictionary(m_readOnlyProperties);
					propertiesDictionary.Remove(key);
					m_readOnlyProperties = new ReadOnlyPropertiesDictionary(propertiesDictionary);
				}
			}
		}

		/// <summary>
		/// Clear the global context properties
		/// </summary>
		public void Clear()
		{
			lock (m_syncRoot)
			{
				m_readOnlyProperties = new ReadOnlyPropertiesDictionary();
			}
		}

		/// <summary>
		/// Get a readonly immutable copy of the properties
		/// </summary>
		/// <returns>the current global context properties</returns>
		/// <remarks>
		/// <para>
		/// This implementation is fast because the GlobalContextProperties class
		/// stores a readonly copy of the properties.
		/// </para>
		/// </remarks>
		internal ReadOnlyPropertiesDictionary GetReadOnlyProperties()
		{
			return m_readOnlyProperties;
		}
	}
}
