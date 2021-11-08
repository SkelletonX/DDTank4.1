using log4net.Core;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Converter for logger name
	/// </summary>
	/// <remarks>
	/// <para>
	/// Outputs the <see cref="P:log4net.Core.LoggingEvent.LoggerName" /> of the event.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class LoggerPatternConverter : NamedPatternConverter
	{
		/// <summary>
		/// Gets the fully qualified name of the logger
		/// </summary>
		/// <param name="loggingEvent">the event being logged</param>
		/// <returns>The fully qualified logger name</returns>
		/// <remarks>
		/// <para>
		/// Returns the <see cref="P:log4net.Core.LoggingEvent.LoggerName" /> of the <paramref name="loggingEvent" />.
		/// </para>
		/// </remarks>
		protected override string GetFullyQualifiedName(LoggingEvent loggingEvent)
		{
			return loggingEvent.LoggerName;
		}
	}
}
