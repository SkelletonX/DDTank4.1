using log4net.Core;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Pattern converter for the class name
	/// </summary>
	/// <remarks>
	/// <para>
	/// Outputs the <see cref="P:log4net.Core.LocationInfo.ClassName" /> of the event.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class TypeNamePatternConverter : NamedPatternConverter
	{
		/// <summary>
		/// Gets the fully qualified name of the class
		/// </summary>
		/// <param name="loggingEvent">the event being logged</param>
		/// <returns>The fully qualified type name for the caller location</returns>
		/// <remarks>
		/// <para>
		/// Returns the <see cref="P:log4net.Core.LocationInfo.ClassName" /> of the <paramref name="loggingEvent" />.
		/// </para>
		/// </remarks>
		protected override string GetFullyQualifiedName(LoggingEvent loggingEvent)
		{
			return loggingEvent.LocationInformation.ClassName;
		}
	}
}
