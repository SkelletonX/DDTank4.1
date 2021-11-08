using log4net.Core;
using log4net.Util;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Converter to output and truncate <c>'.'</c> separated strings
	/// </summary>
	/// <remarks>
	/// <para>
	/// This abstract class supports truncating a <c>'.'</c> separated string
	/// to show a specified number of elements from the right hand side.
	/// This is used to truncate class names that are fully qualified.
	/// </para>
	/// <para>
	/// Subclasses should override the <see cref="M:log4net.Layout.Pattern.NamedPatternConverter.GetFullyQualifiedName(log4net.Core.LoggingEvent)" /> method to
	/// return the fully qualified string.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal abstract class NamedPatternConverter : PatternLayoutConverter, IOptionHandler
	{
		protected int m_precision = 0;

		/// <summary>
		/// Initialize the converter 
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Layout.Pattern.NamedPatternConverter.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Layout.Pattern.NamedPatternConverter.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Layout.Pattern.NamedPatternConverter.ActivateOptions" /> must be called again.
		/// </para>
		/// </remarks>
		public void ActivateOptions()
		{
			m_precision = 0;
			if (Option == null)
			{
				return;
			}
			string text = Option.Trim();
			if (text.Length <= 0)
			{
				return;
			}
			if (SystemInfo.TryParse(text, out int val))
			{
				if (val <= 0)
				{
					LogLog.Error("NamedPatternConverter: Precision option (" + text + ") isn't a positive integer.");
				}
				else
				{
					m_precision = val;
				}
			}
			else
			{
				LogLog.Error("NamedPatternConverter: Precision option \"" + text + "\" not a decimal integer.");
			}
		}

		/// <summary>
		/// Get the fully qualified string data
		/// </summary>
		/// <param name="loggingEvent">the event being logged</param>
		/// <returns>the fully qualified name</returns>
		/// <remarks>
		/// <para>
		/// Overridden by subclasses to get the fully qualified name before the
		/// precision is applied to it.
		/// </para>
		/// <para>
		/// Return the fully qualified <c>'.'</c> (dot/period) separated string.
		/// </para>
		/// </remarks>
		protected abstract string GetFullyQualifiedName(LoggingEvent loggingEvent);

		/// <summary>
		/// Convert the pattern to the rendered message
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// Render the <see cref="M:log4net.Layout.Pattern.NamedPatternConverter.GetFullyQualifiedName(log4net.Core.LoggingEvent)" /> to the precision
		/// specified by the <see cref="P:log4net.Util.PatternConverter.Option" /> property.
		/// </remarks>
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			string fullyQualifiedName = GetFullyQualifiedName(loggingEvent);
			if (m_precision <= 0)
			{
				writer.Write(fullyQualifiedName);
				return;
			}
			int length = fullyQualifiedName.Length;
			int num = length - 1;
			for (int num2 = m_precision; num2 > 0; num2--)
			{
				num = fullyQualifiedName.LastIndexOf('.', num - 1);
				if (num == -1)
				{
					writer.Write(fullyQualifiedName);
					return;
				}
			}
			writer.Write(fullyQualifiedName.Substring(num + 1, length - num - 1));
		}
	}
}
