using log4net.Core;
using log4net.Util;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Converter to include event NDC
	/// </summary>
	/// <remarks>
	/// <para>
	/// Outputs the value of the event property named <c>NDC</c>.
	/// </para>
	/// <para>
	/// The <see cref="T:log4net.Layout.Pattern.PropertyPatternConverter" /> should be used instead.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class NdcPatternConverter : PatternLayoutConverter
	{
		/// <summary>
		/// Write the event NDC to the output
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// As the thread context stacks are now stored in named event properties
		/// this converter simply looks up the value of the <c>NDC</c> property.
		/// </para>
		/// <para>
		/// The <see cref="T:log4net.Layout.Pattern.PropertyPatternConverter" /> should be used instead.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			PatternConverter.WriteObject(writer, loggingEvent.Repository, loggingEvent.LookupProperty("NDC"));
		}
	}
}
