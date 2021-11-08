using log4net.Core;
using System.Globalization;
using System.IO;

namespace log4net.Layout
{
	/// <summary>
	/// Adapts any <see cref="T:log4net.Layout.ILayout" /> to a <see cref="T:log4net.Layout.IRawLayout" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// Where an <see cref="T:log4net.Layout.IRawLayout" /> is required this adapter
	/// allows a <see cref="T:log4net.Layout.ILayout" /> to be specified.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class Layout2RawLayoutAdapter : IRawLayout
	{
		/// <summary>
		/// The layout to adapt
		/// </summary>
		private ILayout m_layout;

		/// <summary>
		/// Construct a new adapter
		/// </summary>
		/// <param name="layout">the layout to adapt</param>
		/// <remarks>
		/// <para>
		/// Create the adapter for the specified <paramref name="layout" />.
		/// </para>
		/// </remarks>
		public Layout2RawLayoutAdapter(ILayout layout)
		{
			m_layout = layout;
		}

		/// <summary>
		/// Format the logging event as an object.
		/// </summary>
		/// <param name="loggingEvent">The event to format</param>
		/// <returns>returns the formatted event</returns>
		/// <remarks>
		/// <para>
		/// Format the logging event as an object.
		/// </para>
		/// <para>
		/// Uses the <see cref="T:log4net.Layout.ILayout" /> object supplied to 
		/// the constructor to perform the formatting.
		/// </para>
		/// </remarks>
		public virtual object Format(LoggingEvent loggingEvent)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			m_layout.Format(stringWriter, loggingEvent);
			return stringWriter.ToString();
		}
	}
}
