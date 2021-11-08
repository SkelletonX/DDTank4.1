using log4net.Core;

namespace log4net.Layout
{
	/// <summary>
	/// Extract the date from the <see cref="T:log4net.Core.LoggingEvent" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// Extract the date from the <see cref="T:log4net.Core.LoggingEvent" />
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class RawTimeStampLayout : IRawLayout
	{
		/// <summary>
		/// Gets the <see cref="P:log4net.Core.LoggingEvent.TimeStamp" /> as a <see cref="T:System.DateTime" />.
		/// </summary>
		/// <param name="loggingEvent">The event to format</param>
		/// <returns>returns the time stamp</returns>
		/// <remarks>
		/// <para>
		/// Gets the <see cref="P:log4net.Core.LoggingEvent.TimeStamp" /> as a <see cref="T:System.DateTime" />.
		/// </para>
		/// <para>
		/// The time stamp is in local time. To format the time stamp
		/// in universal time use <see cref="T:log4net.Layout.RawUtcTimeStampLayout" />.
		/// </para>
		/// </remarks>
		public virtual object Format(LoggingEvent loggingEvent)
		{
			return loggingEvent.TimeStamp;
		}
	}
}
