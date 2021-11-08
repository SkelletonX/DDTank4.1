using log4net.Core;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Writes the event message to the output
	/// </summary>
	/// <remarks>
	/// <para>
	/// Uses the <see cref="M:log4net.Core.LoggingEvent.WriteRenderedMessage(System.IO.TextWriter)" /> method
	/// to write out the event message.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class MessagePatternConverter : PatternLayoutConverter
	{
		/// <summary>
		/// Writes the event message to the output
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// Uses the <see cref="M:log4net.Core.LoggingEvent.WriteRenderedMessage(System.IO.TextWriter)" /> method
		/// to write out the event message.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			loggingEvent.WriteRenderedMessage(writer);
		}
	}
}
