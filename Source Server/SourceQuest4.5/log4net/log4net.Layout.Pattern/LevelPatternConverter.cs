using log4net.Core;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Write the event level to the output
	/// </summary>
	/// <remarks>
	/// <para>
	/// Writes the display name of the event <see cref="P:log4net.Core.LoggingEvent.Level" />
	/// to the writer.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class LevelPatternConverter : PatternLayoutConverter
	{
		/// <summary>
		/// Write the event level to the output
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// Writes the <see cref="P:log4net.Core.Level.DisplayName" /> of the <paramref name="loggingEvent" /> <see cref="P:log4net.Core.LoggingEvent.Level" />
		/// to the <paramref name="writer" />.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(loggingEvent.Level.DisplayName);
		}
	}
}
