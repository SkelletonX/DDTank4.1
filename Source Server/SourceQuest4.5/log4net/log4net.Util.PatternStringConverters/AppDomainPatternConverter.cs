using System.IO;

namespace log4net.Util.PatternStringConverters
{
	/// <summary>
	/// Write the name of the current AppDomain to the output
	/// </summary>
	/// <remarks>
	/// <para>
	/// Write the name of the current AppDomain to the output writer
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class AppDomainPatternConverter : PatternConverter
	{
		/// <summary>
		/// Write the name of the current AppDomain to the output
		/// </summary>
		/// <param name="writer">the writer to write to</param>
		/// <param name="state">null, state is not set</param>
		/// <remarks>
		/// <para>
		/// Writes name of the current AppDomain to the output <paramref name="writer" />.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, object state)
		{
			writer.Write(SystemInfo.ApplicationFriendlyName);
		}
	}
}
