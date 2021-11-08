using System;
using System.Globalization;
using System.IO;

namespace log4net.DateFormatter
{
	/// <summary>
	/// Formats the <see cref="T:System.DateTime" /> using the <see cref="M:System.DateTime.ToString(System.String,System.IFormatProvider)" /> method.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Formats the <see cref="T:System.DateTime" /> using the <see cref="T:System.DateTime" /> <see cref="M:System.DateTime.ToString(System.String,System.IFormatProvider)" /> method.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class SimpleDateFormatter : IDateFormatter
	{
		/// <summary>
		/// The format string used to format the <see cref="T:System.DateTime" />.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The format string must be compatible with the options
		/// that can be supplied to <see cref="M:System.DateTime.ToString(System.String,System.IFormatProvider)" />.
		/// </para>
		/// </remarks>
		private readonly string m_formatString;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.DateFormatter.SimpleDateFormatter" /> class 
		/// with the specified format string.
		/// </para>
		/// <para>
		/// The format string must be compatible with the options
		/// that can be supplied to <see cref="M:System.DateTime.ToString(System.String,System.IFormatProvider)" />.
		/// </para>
		/// </remarks>
		public SimpleDateFormatter(string format)
		{
			m_formatString = format;
		}

		/// <summary>
		/// Formats the date using <see cref="M:System.DateTime.ToString(System.String,System.IFormatProvider)" />.
		/// </summary>
		/// <param name="dateToFormat">The date to convert to a string.</param>
		/// <param name="writer">The writer to write to.</param>
		/// <remarks>
		/// <para>
		/// Uses the date format string supplied to the constructor to call
		/// the <see cref="M:System.DateTime.ToString(System.String,System.IFormatProvider)" /> method to format the date.
		/// </para>
		/// </remarks>
		public virtual void FormatDate(DateTime dateToFormat, TextWriter writer)
		{
			writer.Write(dateToFormat.ToString(m_formatString, DateTimeFormatInfo.InvariantInfo));
		}
	}
}
