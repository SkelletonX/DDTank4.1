using log4net.Core;
using log4net.DateFormatter;
using log4net.Util;
using System;
using System.Globalization;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Date pattern converter, uses a <see cref="T:log4net.DateFormatter.IDateFormatter" /> to format 
	/// the date of a <see cref="T:log4net.Core.LoggingEvent" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Render the <see cref="P:log4net.Core.LoggingEvent.TimeStamp" /> to the writer as a string.
	/// </para>
	/// <para>
	/// The value of the <see cref="P:log4net.Util.PatternConverter.Option" /> determines 
	/// the formatting of the date. The following values are allowed:
	/// <list type="definition">
	/// 	<listheader>
	/// 		<term>Option value</term>
	/// 		<description>Output</description>
	/// 	</listheader>
	/// 	<item>
	/// 		<term>ISO8601</term>
	/// 		<description>
	/// 		Uses the <see cref="T:log4net.DateFormatter.Iso8601DateFormatter" /> formatter. 
	/// 		Formats using the <c>"yyyy-MM-dd HH:mm:ss,fff"</c> pattern.
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<term>DATE</term>
	/// 		<description>
	/// 		Uses the <see cref="T:log4net.DateFormatter.DateTimeDateFormatter" /> formatter. 
	/// 		Formats using the <c>"dd MMM yyyy HH:mm:ss,fff"</c> for example, <c>"06 Nov 1994 15:49:37,459"</c>.
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<term>ABSOLUTE</term>
	/// 		<description>
	/// 		Uses the <see cref="T:log4net.DateFormatter.AbsoluteTimeDateFormatter" /> formatter. 
	/// 		Formats using the <c>"HH:mm:ss,yyyy"</c> for example, <c>"15:49:37,459"</c>.
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<term>other</term>
	/// 		<description>
	/// 		Any other pattern string uses the <see cref="T:log4net.DateFormatter.SimpleDateFormatter" /> formatter. 
	/// 		This formatter passes the pattern string to the <see cref="T:System.DateTime" /> 
	/// 		<see cref="M:System.DateTime.ToString(System.String)" /> method.
	/// 		For details on valid patterns see 
	/// 		<a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemglobalizationdatetimeformatinfoclasstopic.asp">DateTimeFormatInfo Class</a>.
	/// 		</description>
	/// 	</item>
	/// </list>
	/// </para>
	/// <para>
	/// The <see cref="P:log4net.Core.LoggingEvent.TimeStamp" /> is in the local time zone and is rendered in that zone.
	/// To output the time in Universal time see <see cref="T:log4net.Layout.Pattern.UtcDatePatternConverter" />.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal class DatePatternConverter : PatternLayoutConverter, IOptionHandler
	{
		/// <summary>
		/// The <see cref="T:log4net.DateFormatter.IDateFormatter" /> used to render the date to a string
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="T:log4net.DateFormatter.IDateFormatter" /> used to render the date to a string
		/// </para>
		/// </remarks>
		protected IDateFormatter m_dateFormatter;

		/// <summary>
		/// Initialize the converter pattern based on the <see cref="P:log4net.Util.PatternConverter.Option" /> property.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Layout.Pattern.DatePatternConverter.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Layout.Pattern.DatePatternConverter.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Layout.Pattern.DatePatternConverter.ActivateOptions" /> must be called again.
		/// </para>
		/// </remarks>
		public void ActivateOptions()
		{
			string text = Option;
			if (text == null)
			{
				text = "ISO8601";
			}
			if (string.Compare(text, "ISO8601", ignoreCase: true, CultureInfo.InvariantCulture) == 0)
			{
				m_dateFormatter = new Iso8601DateFormatter();
			}
			else if (string.Compare(text, "ABSOLUTE", ignoreCase: true, CultureInfo.InvariantCulture) == 0)
			{
				m_dateFormatter = new AbsoluteTimeDateFormatter();
			}
			else if (string.Compare(text, "DATE", ignoreCase: true, CultureInfo.InvariantCulture) == 0)
			{
				m_dateFormatter = new DateTimeDateFormatter();
			}
			else
			{
				try
				{
					m_dateFormatter = new SimpleDateFormatter(text);
				}
				catch (Exception exception)
				{
					LogLog.Error("DatePatternConverter: Could not instantiate SimpleDateFormatter with [" + text + "]", exception);
					m_dateFormatter = new Iso8601DateFormatter();
				}
			}
		}

		/// <summary>
		/// Convert the pattern into the rendered message
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// Pass the <see cref="P:log4net.Core.LoggingEvent.TimeStamp" /> to the <see cref="T:log4net.DateFormatter.IDateFormatter" />
		/// for it to render it to the writer.
		/// </para>
		/// <para>
		/// The <see cref="P:log4net.Core.LoggingEvent.TimeStamp" /> passed is in the local time zone.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			try
			{
				m_dateFormatter.FormatDate(loggingEvent.TimeStamp, writer);
			}
			catch (Exception exception)
			{
				LogLog.Error("DatePatternConverter: Error occurred while converting date.", exception);
			}
		}
	}
}
