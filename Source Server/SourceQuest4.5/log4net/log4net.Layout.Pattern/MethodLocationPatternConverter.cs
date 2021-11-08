using log4net.Core;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Write the method name to the output
	/// </summary>
	/// <remarks>
	/// <para>
	/// Writes the caller location <see cref="P:log4net.Core.LocationInfo.MethodName" /> to
	/// the output.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class MethodLocationPatternConverter : PatternLayoutConverter
	{
		/// <summary>
		/// Write the method name to the output
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// Writes the caller location <see cref="P:log4net.Core.LocationInfo.MethodName" /> to
		/// the output.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(loggingEvent.LocationInformation.MethodName);
		}
	}
}
