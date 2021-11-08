using System;
using System.Globalization;
using System.Text;

namespace log4net.DateFormatter
{
	/// <summary>
	/// Formats a <see cref="T:System.DateTime" /> as <c>"dd MMM yyyy HH:mm:ss,fff"</c>
	/// </summary>
	/// <remarks>
	/// <para>
	/// Formats a <see cref="T:System.DateTime" /> in the format 
	/// <c>"dd MMM yyyy HH:mm:ss,fff"</c> for example, 
	/// <c>"06 Nov 1994 15:49:37,459"</c>.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	/// <author>Angelika Schnagl</author>
	public class DateTimeDateFormatter : AbsoluteTimeDateFormatter
	{
		/// <summary>
		/// The format info for the invariant culture.
		/// </summary>
		private readonly DateTimeFormatInfo m_dateTimeFormatInfo;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.DateFormatter.DateTimeDateFormatter" /> class.
		/// </para>
		/// </remarks>
		public DateTimeDateFormatter()
		{
			m_dateTimeFormatInfo = DateTimeFormatInfo.InvariantInfo;
		}

		/// <summary>
		/// Formats the date without the milliseconds part
		/// </summary>
		/// <param name="dateToFormat">The date to format.</param>
		/// <param name="buffer">The string builder to write to.</param>
		/// <remarks>
		/// <para>
		/// Formats a DateTime in the format <c>"dd MMM yyyy HH:mm:ss"</c>
		/// for example, <c>"06 Nov 1994 15:49:37"</c>.
		/// </para>
		/// <para>
		/// The base class will append the <c>",fff"</c> milliseconds section.
		/// This method will only be called at most once per second.
		/// </para>
		/// </remarks>
		protected override void FormatDateWithoutMillis(DateTime dateToFormat, StringBuilder buffer)
		{
			int day = dateToFormat.Day;
			if (day < 10)
			{
				buffer.Append('0');
			}
			buffer.Append(day);
			buffer.Append(' ');
			buffer.Append(m_dateTimeFormatInfo.GetAbbreviatedMonthName(dateToFormat.Month));
			buffer.Append(' ');
			buffer.Append(dateToFormat.Year);
			buffer.Append(' ');
			base.FormatDateWithoutMillis(dateToFormat, buffer);
		}
	}
}
