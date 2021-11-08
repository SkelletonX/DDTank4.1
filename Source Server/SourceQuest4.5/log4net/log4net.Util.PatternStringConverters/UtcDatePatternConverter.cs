using System;
using System.IO;

namespace log4net.Util.PatternStringConverters
{
	/// <summary>
	/// Write the UTC date time to the output
	/// </summary>
	/// <remarks>
	/// <para>
	/// Date pattern converter, uses a <see cref="T:log4net.DateFormatter.IDateFormatter" /> to format 
	/// the current date and time in Universal time.
	/// </para>
	/// <para>
	/// See the <see cref="T:log4net.Util.PatternStringConverters.DatePatternConverter" /> for details on the date pattern syntax.
	/// </para>
	/// </remarks>
	/// <seealso cref="T:log4net.Util.PatternStringConverters.DatePatternConverter" />
	/// <author>Nicko Cadell</author>
	internal class UtcDatePatternConverter : DatePatternConverter
	{
		/// <summary>
		/// Write the current date and time to the output
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="state">null, state is not set</param>
		/// <remarks>
		/// <para>
		/// Pass the current date and time to the <see cref="T:log4net.DateFormatter.IDateFormatter" />
		/// for it to render it to the writer.
		/// </para>
		/// <para>
		/// The date is in Universal time when it is rendered.
		/// </para>
		/// </remarks>
		/// <seealso cref="T:log4net.Util.PatternStringConverters.DatePatternConverter" />
		protected override void Convert(TextWriter writer, object state)
		{
			try
			{
				m_dateFormatter.FormatDate(DateTime.UtcNow, writer);
			}
			catch (Exception exception)
			{
				LogLog.Error("UtcDatePatternConverter: Error occurred while converting date.", exception);
			}
		}
	}
}
