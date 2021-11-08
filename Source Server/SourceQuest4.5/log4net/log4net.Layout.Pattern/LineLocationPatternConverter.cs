using log4net.Core;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Write the caller location line number to the output
	/// </summary>
	/// <remarks>
	/// <para>
	/// Writes the value of the <see cref="P:log4net.Core.LocationInfo.LineNumber" /> for
	/// the event to the output writer.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class LineLocationPatternConverter : PatternLayoutConverter
	{
		/// <summary>
		/// Write the caller location line number to the output
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// Writes the value of the <see cref="P:log4net.Core.LocationInfo.LineNumber" /> for
		/// the <paramref name="loggingEvent" /> to the output <paramref name="writer" />.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(loggingEvent.LocationInformation.LineNumber);
		}
	}
}
