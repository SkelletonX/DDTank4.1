using log4net.Core;
using System.Globalization;

namespace log4net.Util.PatternStringConverters
{
	/// <summary>
	/// Writes a newline to the output
	/// </summary>
	/// <remarks>
	/// <para>
	/// Writes the system dependent line terminator to the output.
	/// This behavior can be overridden by setting the <see cref="P:log4net.Util.PatternConverter.Option" />:
	/// </para>
	/// <list type="definition">
	///   <listheader>
	///     <term>Option Value</term>
	///     <description>Output</description>
	///   </listheader>
	///   <item>
	///     <term>DOS</term>
	///     <description>DOS or Windows line terminator <c>"\r\n"</c></description>
	///   </item>
	///   <item>
	///     <term>UNIX</term>
	///     <description>UNIX line terminator <c>"\n"</c></description>
	///   </item>
	/// </list>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class NewLinePatternConverter : LiteralPatternConverter, IOptionHandler
	{
		/// <summary>
		/// Initialize the converter
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Util.PatternStringConverters.NewLinePatternConverter.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Util.PatternStringConverters.NewLinePatternConverter.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Util.PatternStringConverters.NewLinePatternConverter.ActivateOptions" /> must be called again.
		/// </para>
		/// </remarks>
		public void ActivateOptions()
		{
			if (string.Compare(Option, "DOS", ignoreCase: true, CultureInfo.InvariantCulture) == 0)
			{
				Option = "\r\n";
			}
			else if (string.Compare(Option, "UNIX", ignoreCase: true, CultureInfo.InvariantCulture) == 0)
			{
				Option = "\n";
			}
			else
			{
				Option = SystemInfo.NewLine;
			}
		}
	}
}
