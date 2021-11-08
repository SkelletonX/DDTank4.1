using log4net.Core;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Converter to include event thread name
	/// </summary>
	/// <remarks>
	/// <para>
	/// Writes the <see cref="P:log4net.Core.LoggingEvent.ThreadName" /> to the output.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class ThreadPatternConverter : PatternLayoutConverter
	{
		/// <summary>
		/// Write the ThreadName to the output
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// Writes the <see cref="P:log4net.Core.LoggingEvent.ThreadName" /> to the <paramref name="writer" />.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(loggingEvent.ThreadName);
		}
	}
}
