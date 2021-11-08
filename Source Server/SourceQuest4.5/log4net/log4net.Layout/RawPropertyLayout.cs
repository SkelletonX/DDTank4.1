using log4net.Core;

namespace log4net.Layout
{
	/// <summary>
	/// Extract the value of a property from the <see cref="T:log4net.Core.LoggingEvent" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// Extract the value of a property from the <see cref="T:log4net.Core.LoggingEvent" />
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public class RawPropertyLayout : IRawLayout
	{
		private string m_key;

		/// <summary>
		/// The name of the value to lookup in the LoggingEvent Properties collection.
		/// </summary>
		/// <value>
		/// Value to lookup in the LoggingEvent Properties collection
		/// </value>
		/// <remarks>
		/// <para>
		/// String name of the property to lookup in the <see cref="T:log4net.Core.LoggingEvent" />.
		/// </para>
		/// </remarks>
		public string Key
		{
			get
			{
				return m_key;
			}
			set
			{
				m_key = value;
			}
		}

		/// <summary>
		/// Lookup the property for <see cref="P:log4net.Layout.RawPropertyLayout.Key" />
		/// </summary>
		/// <param name="loggingEvent">The event to format</param>
		/// <returns>returns property value</returns>
		/// <remarks>
		/// <para>
		/// Looks up and returns the object value of the property
		/// named <see cref="P:log4net.Layout.RawPropertyLayout.Key" />. If there is no property defined
		/// with than name then <c>null</c> will be returned.
		/// </para>
		/// </remarks>
		public virtual object Format(LoggingEvent loggingEvent)
		{
			return loggingEvent.LookupProperty(m_key);
		}
	}
}
