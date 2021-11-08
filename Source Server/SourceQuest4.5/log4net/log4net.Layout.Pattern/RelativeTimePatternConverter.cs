using log4net.Core;
using System;
using System.Globalization;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Converter to output the relative time of the event
	/// </summary>
	/// <remarks>
	/// <para>
	/// Converter to output the time of the event relative to the start of the program.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class RelativeTimePatternConverter : PatternLayoutConverter
	{
		/// <summary>
		/// Write the relative time to the output
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// Writes out the relative time of the event in milliseconds.
		/// That is the number of milliseconds between the event <see cref="P:log4net.Core.LoggingEvent.TimeStamp" />
		/// and the <see cref="P:log4net.Core.LoggingEvent.StartTime" />.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(TimeDifferenceInMillis(LoggingEvent.StartTime, loggingEvent.TimeStamp).ToString(NumberFormatInfo.InvariantInfo));
		}

		/// <summary>
		/// Helper method to get the time difference between two DateTime objects
		/// </summary>
		/// <param name="start">start time (in the current local time zone)</param>
		/// <param name="end">end time (in the current local time zone)</param>
		/// <returns>the time difference in milliseconds</returns>
		private static long TimeDifferenceInMillis(DateTime start, DateTime end)
		{
			return (long)(end.ToUniversalTime() - start.ToUniversalTime()).TotalMilliseconds;
		}
	}
}
