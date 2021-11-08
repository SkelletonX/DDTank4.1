using log4net.Core;
using log4net.Util;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Property pattern converter
	/// </summary>
	/// <remarks>
	/// <para>
	/// Writes out the value of a named property. The property name
	/// should be set in the <see cref="P:log4net.Util.PatternConverter.Option" />
	/// property.
	/// </para>
	/// <para>
	/// If the <see cref="P:log4net.Util.PatternConverter.Option" /> is set to <c>null</c>
	/// then all the properties are written as key value pairs.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class PropertyPatternConverter : PatternLayoutConverter
	{
		/// <summary>
		/// Write the property value to the output
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// Writes out the value of a named property. The property name
		/// should be set in the <see cref="P:log4net.Util.PatternConverter.Option" />
		/// property.
		/// </para>
		/// <para>
		/// If the <see cref="P:log4net.Util.PatternConverter.Option" /> is set to <c>null</c>
		/// then all the properties are written as key value pairs.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			if (Option != null)
			{
				PatternConverter.WriteObject(writer, loggingEvent.Repository, loggingEvent.LookupProperty(Option));
			}
			else
			{
				PatternConverter.WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
			}
		}
	}
}
