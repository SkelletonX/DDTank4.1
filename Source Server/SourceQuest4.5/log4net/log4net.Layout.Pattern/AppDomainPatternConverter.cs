using log4net.Core;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Write the event appdomain name to the output
	/// </summary>
	/// <remarks>
	/// <para>
	/// Writes the <see cref="P:log4net.Core.LoggingEvent.Domain" /> to the output writer.
	/// </para>
	/// </remarks>
	/// <author>Daniel Cazzulino</author>
	/// <author>Nicko Cadell</author>
	internal sealed class AppDomainPatternConverter : PatternLayoutConverter
	{
		/// <summary>
		/// Write the event appdomain name to the output
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// Writes the <see cref="P:log4net.Core.LoggingEvent.Domain" /> to the output <paramref name="writer" />.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(loggingEvent.Domain);
		}
	}
}
