using System.Collections;

namespace log4net.Util
{
	/// <summary>
	/// This class aggregates several PropertiesDictionary collections together.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Provides a dictionary style lookup over an ordered list of
	/// <see cref="T:log4net.Util.PropertiesDictionary" /> collections.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public sealed class CompositeProperties
	{
		private PropertiesDictionary m_flattened = null;

		private ArrayList m_nestedProperties = new ArrayList();

		/// <summary>
		/// Gets the value of a property
		/// </summary>
		/// <value>
		/// The value for the property with the specified key
		/// </value>
		/// <remarks>
		/// <para>
		/// Looks up the value for the <paramref name="key" /> specified.
		/// The <see cref="T:log4net.Util.PropertiesDictionary" /> collections are searched
		/// in the order in which they were added to this collection. The value
		/// returned is the value held by the first collection that contains
		/// the specified key.
		/// </para>
		/// <para>
		/// If none of the collections contain the specified key then
		/// <c>null</c> is returned.
		/// </para>
		/// </remarks>
		public object this[string key]
		{
			get
			{
				if (m_flattened != null)
				{
					return m_flattened[key];
				}
				foreach (ReadOnlyPropertiesDictionary nestedProperty in m_nestedProperties)
				{
					if (nestedProperty.Contains(key))
					{
						return nestedProperty[key];
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.CompositeProperties" /> class.
		/// </para>
		/// </remarks>
		internal CompositeProperties()
		{
		}

		/// <summary>
		/// Add a Properties Dictionary to this composite collection
		/// </summary>
		/// <param name="properties">the properties to add</param>
		/// <remarks>
		/// <para>
		/// Properties dictionaries added first take precedence over dictionaries added
		/// later.
		/// </para>
		/// </remarks>
		public void Add(ReadOnlyPropertiesDictionary properties)
		{
			m_flattened = null;
			m_nestedProperties.Add(properties);
		}

		/// <summary>
		/// Flatten this composite collection into a single properties dictionary
		/// </summary>
		/// <returns>the flattened dictionary</returns>
		/// <remarks>
		/// <para>
		/// Reduces the collection of ordered dictionaries to a single dictionary
		/// containing the resultant values for the keys.
		/// </para>
		/// </remarks>
		public PropertiesDictionary Flatten()
		{
			if (m_flattened == null)
			{
				m_flattened = new PropertiesDictionary();
				int num = m_nestedProperties.Count;
				while (--num >= 0)
				{
					ReadOnlyPropertiesDictionary readOnlyPropertiesDictionary = (ReadOnlyPropertiesDictionary)m_nestedProperties[num];
					foreach (DictionaryEntry item in (IEnumerable)readOnlyPropertiesDictionary)
					{
						m_flattened[(string)item.Key] = item.Value;
					}
				}
			}
			return m_flattened;
		}
	}
}
