using log4net.Core;
using log4net.Util;
using System;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Write the TimeStamp to the output
	/// </summary>
	/// <remarks>
	/// <para>
	/// Date pattern converter, uses a <see cref="T:log4net.DateFormatter.IDateFormatter" /> to format 
	/// the date of a <see cref="T:log4net.Core.LoggingEvent" />.
	/// </para>
	/// <para>
	/// Uses a <see cref="T:log4net.DateFormatter.IDateFormatter" /> to format the <see cref="P:log4net.Core.LoggingEvent.TimeStamp" /> 
	/// in Universal time.
	/// </para>
	/// <para>
	/// See the <see cref="T:log4net.Layout.Pattern.DatePatternConverter" /> for details on the date pattern syntax.
	/// </para>
	/// </remarks>
	/// <seealso cref="T:log4net.Layout.Pattern.DatePatternConverter" />
	/// <author>Nicko Cadell</author>
	internal class UtcDatePatternConverter : DatePatternConverter
	{
		/// <summary>
		/// Write the TimeStamp to the output
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// Pass the <see cref="P:log4net.Core.LoggingEvent.TimeStamp" /> to the <see cref="T:log4net.DateFormatter.IDateFormatter" />
		/// for it to render it to the writer.
		/// </para>
		/// <para>
		/// The <see cref="P:log4net.Core.LoggingEvent.TimeStamp" /> passed is in the local time zone, this is converted
		/// to Universal time before it is rendered.
		/// </para>
		/// </remarks>
		/// <seealso cref="T:log4net.Layout.Pattern.DatePatternConverter" />
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			try
			{
				m_dateFormatter.FormatDate(loggingEvent.TimeStamp.ToUniversalTime(), writer);
			}
			catch (Exception exception)
			{
				LogLog.Error("UtcDatePatternConverter: Error occurred while converting date.", exception);
			}
		}
	}
}
