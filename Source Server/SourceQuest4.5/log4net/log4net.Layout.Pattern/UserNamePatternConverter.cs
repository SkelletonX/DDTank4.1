using log4net.Core;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Converter to include event user name
	/// </summary>
	/// <author>Douglas de la Torre</author>
	/// <author>Nicko Cadell</author>
	internal sealed class UserNamePatternConverter : PatternLayoutConverter
	{
		/// <summary>
		/// Convert the pattern to the rendered message
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(loggingEvent.UserName);
		}
	}
}
