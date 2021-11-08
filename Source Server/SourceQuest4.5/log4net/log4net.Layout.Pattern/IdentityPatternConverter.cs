using log4net.Core;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Writes the event identity to the output
	/// </summary>
	/// <remarks>
	/// <para>
	/// Writes the value of the <see cref="P:log4net.Core.LoggingEvent.Identity" /> to
	/// the output writer.
	/// </para>
	/// </remarks>
	/// <author>Daniel Cazzulino</author>
	/// <author>Nicko Cadell</author>
	internal sealed class IdentityPatternConverter : PatternLayoutConverter
	{
		/// <summary>
		/// Writes the event identity to the output
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// Writes the value of the <paramref name="loggingEvent" /> 
		/// <see cref="P:log4net.Core.LoggingEvent.Identity" /> to
		/// the output <paramref name="writer" />.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(loggingEvent.Identity);
		}
	}
}
